using Engine.Shared.Graphics;
using OpenTK;
using OpenTK.Graphics.ES30;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Shared.Base
{
    /// <summary> The renderer will draw all our objects that need to be drawn </summary>
    public class Renderer
    {
        /// <summary> The target dimensions of the display </summary>
        private Vector2 _TargetDimensions;
        /// <summary> The offset of the viewport </summary>
        private Vector2 _ViewOffset;
        /// <summary> The viewport dimensions that have been scaled up/down </summary>
        private Vector2 _ScaledDimensions;

        /// <summary> The instance of the renderer </summary>
        private static Renderer _Instance;
        /// <summary> The canvas that contain all the drawables to be drawn </summary>
        private readonly List<Canvas> _Canvases = new List<Canvas>();

        /// <summary> The instance of the renderer </summary>
        public static Renderer Instance => _Instance ?? (_Instance = new Renderer());
        /// <summary> The target dimensions of the display </summary>
        public Vector2 TargetDimensions => _TargetDimensions;
        /// <summary> The dimensions scaled to fit the display </summary>
        public Vector2 ScaledDimensions => _ScaledDimensions;
        /// <summary> The view offset </summary>
        public Vector2 ViewOffset => _ViewOffset;
        /// <summary> The scale of the dimensions </summary>
        public Vector2 ScreenScale { get; private set; }

        /// <summary> The constructor of the renderer </summary>
        private Renderer()
        {
        }
        
        /// <summary> Creates the renderer </summary>
        /// <param name="targetDimensions"></param>
        internal void Init(Vector2 targetDimensions, Vector2 deviceDimensions, Vector2 viewportOffset)
        {
            _TargetDimensions = targetDimensions;
            ScreenScale = new Vector2(targetDimensions.X / deviceDimensions.X, targetDimensions.Y / deviceDimensions.Y);
            _ScaledDimensions = deviceDimensions;
            _ViewOffset = viewportOffset;
        }

        /// <summary> This will draw all objects for our game </summary>
        internal void Draw()
        {
#if __ANDROID__
            GL.Enable(All.Blend);
            GL.BlendFunc(All.SrcAlpha, All.OneMinusSrcAlpha);
#elif __IOS__
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
#endif

            GL.Viewport((Int32)_ViewOffset.X, (Int32)_ViewOffset.Y, (Int32)_ScaledDimensions.X, (Int32)_ScaledDimensions.Y);
            GL.ClearColor(0f, 0.4f, 0f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            foreach (Canvas canvas in _Canvases.ToList())
            {
                canvas.Draw();
            }
        }

        /// <summary> Updates the canvases in the renderer </summary>
        internal void Update()
        {
            if (_Canvases.Any(s => s.ZOrderChanged))
            {
                _Canvases.OrderBy(s => s.ZOrder);
                foreach (Canvas canvas in _Canvases.ToList()) canvas.ZOrderChanged = false;
            }

            foreach (Canvas canvas in _Canvases.ToList())
            {
                canvas.Update();
            }
        }

        /// <summary> Adds the canvas to the list of canvases </summary>
        /// <param name="canvasToAdd"></param>
        internal void AddCanvas(Canvas canvasToAdd)
        {
            _Canvases.Add(canvasToAdd);
        }

        /// <summary> Removes the canvas from the list of canvases </summary>
        /// <param name="canvas"></param>
        internal void RemoveCanvas(Canvas canvas)
        {
            _Canvases.Remove(canvas);
        }
    }
}
