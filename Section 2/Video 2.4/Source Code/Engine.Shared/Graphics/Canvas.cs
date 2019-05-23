using Engine.Shared.Base;
using Engine.Shared.Graphics.Drawables;
using OpenTK.Graphics.ES30;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Shared.Graphics
{
    /// <summary> The canvas will draw all the objects using the camera attached</summary>
    public class Canvas : IDisposable
    {
        /// <summary> The Vertex Buffer Object </summary>
        private Int32 _VBO;
        /// <summary> The Index Buffer Object </summary>
        private Int32 _EBO;
        /// <summary> Whether or not the list of drawables has changed </summary>
        private System.Boolean _ListChanged;
        /// <summary> The vertices position, UV and colour in single form </summary>
        private Single[] _VertexArray;
        /// <summary> The indices for the drawables </summary>
        private UInt32[] _IndicesArray;
        /// <summary> The Z order of the canvas </summary>
        private Int32 _ZOrder;

        /// <summary> The camera that draws the objects on the canvas </summary>
        protected readonly Camera _Camera;
        /// <summary> The shader for the canvas </summary>
        protected Shader _Shader;
        /// <summary> A list of all the drawables to be drawn on this canvas </summary>
        protected List<Drawable> _Drawables = new List<Drawable>();

        /// <summary> The shader used in the canvas </summary>
        public Shader Shader => _Shader;
        /// <summary> The camera that draws the objects on the canvas </summary>
        public Camera Camera => _Camera;
        /// <summary> The Z order of the canvas </summary>
        public Int32 ZOrder
        {
            get => _ZOrder;
            set
            {
                _ZOrder = value;
                ZOrderChanged = true;
            }
        }
        /// <summary> Whether or not the Z order has changed </summary>
        public System.Boolean ZOrderChanged { get; set; }

        /// <summary> Creates the canvas </summary>
        /// <param name="camera"></param>
        public Canvas(Camera camera, Int32 zOrder, Shader shader)
        {
            _Camera = camera;
            ZOrder = zOrder;
            Renderer.Instance.AddCanvas(this);
            _Shader = shader;
            _Shader.Compile();

            _VertexArray = new Single[0];
            _IndicesArray = new UInt32[0];
            GL.GenBuffers(1, out _VBO);
            GL.GenBuffers(1, out _EBO);
        }

        /// <summary> Registers the drawable to be drawn </summary>
        /// <param name="drawable"></param>
        public void RegisterDrawable(Drawable drawable)
        {
            _Drawables.Add(drawable);
            _ListChanged = true;
        }

        /// <summary> Unregisters the drawable from the list of drawables </summary>
        /// <param name="drawable"></param>
        public void UnregisterDrawable(Drawable drawable)
        {
            _Drawables.Remove(drawable);
            _ListChanged = true;
        }

        /// <summary> Updates the canvas </summary>
        public void Update()
        {
            if (_Drawables.Any(s => s.ZOrderChanged))
            {
                _Drawables = _Drawables.OrderBy(s => s.ZOrder).ToList();
                _ListChanged = true;
            }
            if (_ListChanged || _Drawables.Any(s => s.VerticesInvalid || s.IndicesInvalid)) UpdateBufferData();
        }

        /// <summary> Draws the objects on the canvas </summary>
        public void Draw()
        {
            GL.UseProgram(_Shader.Program);

#if __ANDROID__
            GL.BindBuffer(All.ArrayBuffer, _VBO);
#elif __IOS__
            GL.BindBuffer(BufferTarget.ArrayBuffer, _VBO);
#endif
            SetAttribPointer(Shader.POSITION_ATTRIBUTE, 3, IntPtr.Zero);
            GL.EnableVertexAttribArray(Shader.POSITION_ATTRIBUTE);
            SetAttribPointer(Shader.UV_ATTRIBUTE, 2, new IntPtr(sizeof(Single) * 3));
            GL.EnableVertexAttribArray(Shader.UV_ATTRIBUTE);
            SetAttribPointer(Shader.COLOUR_ATTRIBUTE, 4, new IntPtr(sizeof(Single) * 5));
            GL.EnableVertexAttribArray(Shader.COLOUR_ATTRIBUTE);

#if __ANDROID__
            GL.BindBuffer(All.ArrayBuffer, 0);
            GL.BindBuffer(All.ElementArrayBuffer, _EBO);
#elif __IOS__
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _EBO);
#endif

            System.Boolean viewChanged = _Camera.MatrixInvalid;
            if (viewChanged) _Camera.UpdateViewProjectionMatrix();

            Int32 indexCount = 0;
            Int32 lastGlid = -1;
            foreach (Drawable drawable in _Drawables)
            {
                if (viewChanged || drawable.WVPMatrixInvalid) drawable.UpdateMatrices(_Camera.ViewProjectionMatrix);
                drawable.Draw(ref indexCount, ref lastGlid);
            }

            GL.DisableVertexAttribArray(Shader.POSITION_ATTRIBUTE);
            GL.DisableVertexAttribArray(Shader.UV_ATTRIBUTE);
            GL.DisableVertexAttribArray(Shader.COLOUR_ATTRIBUTE);
            
#if __ANDROID__
            GL.BindBuffer(All.ElementArrayBuffer, 0);
            GL.BindTexture(All.Texture2D, 0);
#elif __IOS__
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
#endif

            GL.UseProgram(0);
        }

        /// <summary> Sets the attribute pointer </summary>
        /// <param name="attribute">The attribute location</param>
        /// <param name="size">The size of the attribute</param>
        /// <param name="start">The start position for the pointer</param>
        private void SetAttribPointer(Int32 attribute, Int32 size, IntPtr start)
        {
#if __ANDROID__
            GL.VertexAttribPointer(attribute, size, All.Float, false, sizeof(Single) * 9, start);
#elif __IOS__
            GL.VertexAttribPointer(attribute, size, VertexAttribPointerType.Float, false, sizeof(Single) * 9, start);
#endif
        }

            /// <summary> Updates the buffer data </summary>
        protected void UpdateBufferData()
        {
            List<Single> vertexData = new List<Single>();
            List<UInt32> indexData = new List<UInt32>();
            UInt32 offset = 0;
            System.Boolean indicesInvalid = _Drawables.Any(s => s.IndicesInvalid);
            foreach (Drawable drawable in _Drawables)
            {
                if (drawable.VerticesInvalid) drawable.UpdateVertices();
                if (indicesInvalid || _ListChanged) drawable.UpdateIndices(offset);
                offset += (UInt32)drawable.Vertices.Count;
                vertexData.AddRange(drawable.VertexArray);
                indexData.AddRange(drawable.Indices);
            }

            Single[] vertexDataArray = vertexData.ToArray();
            UInt32[] indicesArray = indexData.ToArray();
#if __ANDROID__
            GL.BindBuffer(All.ArrayBuffer, _VBO);
            if (vertexDataArray.Length != _VertexArray.Length)
            {
                GL.BufferData(All.ArrayBuffer, (IntPtr)(vertexDataArray.Length * sizeof(Single)), vertexDataArray, All.DynamicDraw);
            }
            else
            {
                GL.BufferSubData(All.ArrayBuffer, IntPtr.Zero, (IntPtr)(vertexDataArray.Length * sizeof(Single)), vertexDataArray);
            }
            GL.BindBuffer(All.ArrayBuffer, 0);
            GL.BindBuffer(All.ElementArrayBuffer, _EBO);
            if (indicesArray.Length != _IndicesArray.Length)
            {
                GL.BufferData(All.ElementArrayBuffer, (IntPtr)(indicesArray.Length * sizeof(UInt32)), indicesArray, All.DynamicDraw);
            }
            else
            {
                GL.BufferSubData(All.ElementArrayBuffer, IntPtr.Zero, (IntPtr)(indicesArray.Length * sizeof(UInt32)), indicesArray);
            }
            GL.BindBuffer(All.ElementArrayBuffer, 0);
#elif __IOS__
            GL.BindBuffer(BufferTarget.ArrayBuffer, _VBO);
            if (vertexDataArray.Length != _VertexArray.Length)
            {
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexDataArray.Length * sizeof(Single)), vertexDataArray, BufferUsage.DynamicDraw);
            }
            else
            {
                GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, (IntPtr)(vertexDataArray.Length * sizeof(Single)), vertexDataArray);
            }
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _EBO);
            if (indicesArray.Length != _IndicesArray.Length)
            {
                GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indicesArray.Length * sizeof(UInt32)), indicesArray, BufferUsage.DynamicDraw);
            }
            else
            {
                GL.BufferSubData(BufferTarget.ElementArrayBuffer, IntPtr.Zero, (IntPtr)(indicesArray.Length * sizeof(UInt32)), indicesArray);
            }
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
#endif
            _VertexArray = vertexDataArray;
            _IndicesArray = indicesArray;
            _ListChanged = false;
            foreach (Drawable drawable in _Drawables.Where(s => s.ZOrderChanged)) drawable.ResetFlags();
        }

        /// <summary> Disposes of the canvas </summary>
        public void Dispose()
        {
            Renderer.Instance.RemoveCanvas(this);
            foreach (Drawable drawable in _Drawables) drawable.Dispose();
            _Drawables.Clear();
            _Shader.Dispose();
            GL.DeleteBuffers(1, ref _VBO);
            GL.DeleteBuffers(1, ref _EBO);
        }
    }
}
