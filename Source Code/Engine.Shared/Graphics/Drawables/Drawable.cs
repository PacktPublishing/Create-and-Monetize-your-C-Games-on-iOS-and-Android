using OpenTK;
using OpenTK.Graphics.ES30;
using System;
using System.Collections.Generic;
using Boolean = System.Boolean;

namespace Engine.Shared.Graphics.Drawables
{
    /// <summary> An object that can be drawn on screen </summary>
    public abstract class Drawable
    {
        /// <summary> The vertices position, UV and colour in single form </summary>
        private Single[] _VertexArray;
        /// <summary> The visibility of the sprite </summary>
        protected Boolean _Visible;
        /// <summary> The canvas the drawable should be drawn on </summary>
        protected readonly Canvas _Canvas;
        /// <summary> The Z order of the drawable </summary>
        protected Int32 _ZOrder;
        /// <summary> Whether or not the Z order of the drawable has changed </summary>
        protected Boolean _ZOrderChanged;
        /// <summary> Whether or not the vertices needs updating </summary>
        protected Boolean _VerticesShouldUpdate;
        /// <summary> Whether or not the indices needs updating </summary>
        protected Boolean _IndicesShouldUpdate;
        /// <summary> The texture for the drawable </summary>
        protected Texture _Texture;
        /// <summary> The width of the drawable </summary>
        protected Single _Width;
        /// <summary> The height of the drawable </summary>
        protected Single _Height;
        /// <summary> The WVP matrix </summary>
        protected Matrix4 _WVPMatrix;
        /// <summary> The colour of the drawable </summary>
        protected Vector4 _Colour;

        /// <summary> The World matrix for the drawable </summary>
        public Matrix4 WorldMatrix { get; protected set; }
        /// <summary> The WVP matrix for the drawable </summary>
        public Matrix4 WVPMatrix
        {
            get { return _WVPMatrix; }
            protected set
            {
                _WVPMatrix = value;
            }
        }
        /// <summary> The array of vertices for the drawable </summary>
        public Single[] VertexArray => _VertexArray;
        /// <summary> Whether or not the WVP matrix is dirty and needs updating </summary>
        public Boolean WVPMatrixInvalid { get; protected set; }
        /// <summary> The vertices for the drawable </summary>
        public List<Vertex> Vertices { get; protected set; }
        /// <summary> The indices for the drawable </summary>
        public List<UInt32> Indices { get; protected set; }
        /// <summary> Whether or not the Z order has changed </summary>
        public Boolean ZOrderChanged => _ZOrderChanged;
        /// <summary> The colour tint of the drawable </summary>
        public Vector4 Colour
        {
            get { return _Colour; }
            set
            {
                _Colour = value;
                _VerticesShouldUpdate = true;
            }
        }
        /// <summary> Whether or not the drawable is visible </summary>
        public virtual Boolean Visible
        {
            get { return _Visible && ParentVisible; }
            set
            {
                _Visible = value;
            }
        }
        /// <summary> Whether or not the parent is visible </summary>
        public Boolean ParentVisible { get; set; }
        /// <summary> The width of the sprite </summary>
        public Single Width
        {
            get { return _Width; }
            set
            {
                _Width = value;
                _VerticesShouldUpdate = true;
            }
        }
        /// <summary> The height of the sprite </summary>
        public Single Height
        {
            get { return _Height; }
            set
            {
                _Height = value;
                _VerticesShouldUpdate = true;
            }
        }
        /// <summary> Whether or not the vertices are invalid </summary>
        public Boolean VerticesInvalid => _VerticesShouldUpdate;
        /// <summary> Whether or not the vertices are invalid </summary>
        public Boolean IndicesInvalid => _IndicesShouldUpdate;
        /// <summary> The Z order of the sprite - determines the draw order for the sprite </summary>
        public Int32 ZOrder
        {
            get { return _ZOrder; }
            set
            {
                if (_ZOrder == value) return;
                _ZOrder = value;
                _ZOrderChanged = true;
            }
        }
        /// <summary> The canvas the drawable should be drawn on </summary>
        /// <param name="canvas"></param>
        public Drawable(Canvas canvas, Int32 zOrder, Texture texture)
        {
            canvas.RegisterDrawable(this);
            _Canvas = canvas;
            ZOrder = zOrder;
            _Colour = new Vector4(1, 1, 1, 1f);
            _Texture = texture;
            _Width = _Texture.Width;
            _Height = _Texture.Height;
            WVPMatrixInvalid = true;
            _VerticesShouldUpdate = true;
            _IndicesShouldUpdate = true;
            ParentVisible = true;
        }

        /// <summary> Creates a drawable without a texture </summary>
        /// <param name="canvas"></param>
        public Drawable(Canvas canvas)
        {
            canvas.RegisterDrawable(this);
            _Canvas = canvas;
            _Colour = new Vector4(1, 1, 1, 1);
            WVPMatrixInvalid = true;
            _VerticesShouldUpdate = true;
            _IndicesShouldUpdate = true;
            ParentVisible = true;
        }

        /// <summary> Resets the flags on the drawable </summary>
        public virtual void ResetFlags()
        {
            _ZOrderChanged = false;
        }

        /// <summary> Updates the vertices in the drawable </summary>
        public void UpdateVertices()
        {
            Vertices = GenerateVertices();
            UpdateMatrices(_Canvas.Camera.ViewProjectionMatrix);
            UpdateVertexArray();
            _VerticesShouldUpdate = false;
        }

        /// <summary> Updates the indices on the drawable </summary>
        /// <param name="offset"></param>
        public void UpdateIndices(UInt32 offset)
        {
            Indices = GenerateIndices(offset);
            _IndicesShouldUpdate = false;
        }

        /// <summary> Generates the vertices for the drawable </summary>
        /// <returns></returns>
        public abstract List<Vertex> GenerateVertices();

        /// <summary> Generates the indices for the drawable </summary>
        /// <returns></returns>
        public abstract List<UInt32> GenerateIndices(UInt32 offset);

        /// <summary> Calculates the World & WorldViewProjection matrix for the drawable </summary>
        /// <param name="viewProjectionMatrix"></param>
        public void UpdateMatrices(Matrix4 viewProjectionMatrix)
        {
            _WVPMatrix = UpdateWVP(viewProjectionMatrix);
            WVPMatrixInvalid = false;
        }

        /// <summary> Updates the WVP matrix </summary>
        /// <param name="vpMatrix"></param>
        public abstract Matrix4 UpdateWVP(Matrix4 vpMatrix);

        /// <summary> Updates the vertex array  </summary>
        protected void UpdateVertexArray()
        {
            _VertexArray = new Single[Vertices.Count * 9];
            Int32 index = 0;
            for (Int32 i = 0; i < Vertices.Count; i++)
            {
                _VertexArray[index++] = Vertices[i].Position.X;
                _VertexArray[index++] = Vertices[i].Position.Y;
                _VertexArray[index++] = Vertices[i].Position.Z;
                _VertexArray[index++] = Vertices[i].Uv.X;
                _VertexArray[index++] = Vertices[i].Uv.Y;
                _VertexArray[index++] = Vertices[i].Colour.X;
                _VertexArray[index++] = Vertices[i].Colour.Y;
                _VertexArray[index++] = Vertices[i].Colour.Z;
                _VertexArray[index++] = Vertices[i].Colour.W;
            }
        }

        /// <summary> Draws the drawable </summary>
        public void Draw(ref Int32 indexOffset, ref Int32 lastGlid)
        {
            if (!Visible)
            {
                indexOffset += Indices.Count;
                return;
            }
            GL.UniformMatrix4(_Canvas.Shader.Uniforms[Shader.WVP_UNIFORM], 1, false, ref _WVPMatrix.Row0.X);

            if (lastGlid != _Texture.GLID)
            {
#if __ANDROID__
                GL.ActiveTexture(All.Texture0);
                GL.BindTexture(All.Texture2D, _Texture.GLID);
#elif __IOS__
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, _Texture.GLID);
#endif
                GL.Uniform1(_Canvas.Shader.Uniforms[Shader.TEXTURE_UNIFORM], 0);
            }

#if __ANDROID__
            GL.DrawElements(All.Triangles, Indices.Count, All.UnsignedInt, (IntPtr)(indexOffset * sizeof(UInt32)));
#elif __IOS__
            GL.DrawElements(BeginMode.Triangles, Indices.Count, DrawElementsType.UnsignedInt, (IntPtr)(indexOffset * sizeof(UInt32)));
#endif

            indexOffset += Indices.Count;

            if (lastGlid != _Texture.GLID)
            {
                lastGlid = _Texture.GLID;
            }
        }

        /// <summary> Disposes of the drawable </summary>
        public virtual void Dispose()
        {
            _Canvas.UnregisterDrawable(this);
            _Texture = null;
        }
    }
}