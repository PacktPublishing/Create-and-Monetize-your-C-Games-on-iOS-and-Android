using System;
using OpenTK.Graphics;
using Android.Content;
using OpenTK.Platform.Android;
using Engine.Shared.Base;
using OpenTK;

namespace Engine.Android
{
    /// <summary> Our view for our game </summary>
    public class CustomGLView : AndroidGameView
    {
        /// <summary> The instance of our game </summary>
        protected readonly BaseGame _GameInstance;
        
        /// <summary> Creates the view </summary>
        /// <param name="context"></param>
        public CustomGLView(Context context, BaseGame gameInstance) : base(context)
        {
            _GameInstance = gameInstance;
        }

        /// <summary> Called when the view is loaded - it starts running the game </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Run(60);
        }

        /// <summary> Creates the frame buffer </summary>
        protected override void CreateFrameBuffer()
        {
            try
            {
                GLContextVersion = GLContextVersion.Gles2_0;
                base.CreateFrameBuffer();
            }
            catch (Exception e)
            {
                throw e;
            }
            _GameInstance.LoadContent();
        }

        /// <summary> Will update all objects in our game </summary>
        /// <param name="e"></param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            _GameInstance.Update();
        }

        /// <summary> Called every frame to render all objects </summary>
        /// <param name="e"></param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            _GameInstance.Draw();
            SwapBuffers();
        }

    }
}