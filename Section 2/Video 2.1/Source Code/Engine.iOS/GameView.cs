using CoreAnimation;
using CoreGraphics;
using Engine.Shared.Base;
using Engine.Shared.Touch;
using Foundation;
using ObjCRuntime;
using OpenGLES;
using OpenTK;
using OpenTK.Platform.iPhoneOS;
using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;

namespace Engine.iOS
{
    /// <summary> The main game view </summary>
    public class GameView : iPhoneOSGameView
    {
        /// <summary> The instance of the game </summary>
        private BaseGame _GameInstance;

        [Export("layerClass")]
        public static Class GetLayerClass()
        {
            return iPhoneOSGameView.GetLayerClass();
        }

        [Export("initWithFrame:")]
        public GameView(CGRect frame) : base(frame)
        {
            LayerColorFormat = EAGLColorFormat.RGBA8;
            UserInteractionEnabled = true;
            MultipleTouchEnabled = true;
            ContentScaleFactor = UIScreen.MainScreen.Scale;
        }

        /// <summary> Sets the game instance </summary>
        /// <param name="gameInstance"></param>
        public void SetGameInstance(BaseGame gameInstance)
        {
            _GameInstance = gameInstance;
        }

        /// <summary> Creates the OpenGL ES Context </summary>
        protected override void CreateFrameBuffer()
        {
            try
            {
                ContextRenderingApi = EAGLRenderingAPI.OpenGLES3;
                base.CreateFrameBuffer();
            }
            catch (Exception e)
            {
                throw e;
            }
            _GameInstance.LoadContent();
        }

        /// <summary> Configures the layer by making it opaque </summary>
        /// <param name="eaglLayer"></param>
        protected override void ConfigureLayer(CAEAGLLayer eaglLayer)
        {
            eaglLayer.Opaque = true;
        }

        /// <summary> Will update all objects in our game </summary>
        /// <param name="e"></param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            _GameInstance.Update();
        }

        /// <summary> Renders our game </summary>
        /// <param name="e"></param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            _GameInstance.Draw();
            SwapBuffers();
        }

        /// <summary> Adds the touches to the TouchManager </summary>
        /// <param name="touches"></param>
        /// <param name="type"></param>
        private void AddTouches(IEnumerable<UITouch> touches, TouchEvent.Type type)
        {
            foreach (UITouch touch in touches)
            {
                CGPoint point = touch.LocationInView(touch.View);
                Vector2 position = new Vector2((Single)point.X * Renderer.Instance.ScreenScale.X, (Single)point.Y * Renderer.Instance.ScreenScale.Y) * (Single)ContentScaleFactor;
                TouchManager.Instance.AddEvent(new TouchEvent(touch.Handle.ToInt32(), position, type));
            }

        }

        /// <summary> Called when the touch has begun </summary>
        /// <param name="touches"></param>
        /// <param name="evt"></param>
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            AddTouches(touches.Cast<UITouch>(), TouchEvent.Type.PRESS);
        }

        /// <summary> Called when the touch has been moved </summary>
        /// <param name="touches"></param>
        /// <param name="evt"></param>
        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);
            AddTouches(touches.Cast<UITouch>(), TouchEvent.Type.MOVE);
        }

        /// <summary> Called when the touch is complete </summary>
        /// <param name="touches"></param>
        /// <param name="evt"></param>
        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
            AddTouches(touches.Cast<UITouch>(), TouchEvent.Type.RELEASE);
        }

        /// <summary> Called when a touch has been cancelled </summary>
        /// <param name="touches"></param>
        /// <param name="evt"></param>
        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);
            AddTouches(touches.Cast<UITouch>(), TouchEvent.Type.RELEASE);
        }
    }
}
