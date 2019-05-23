using System;
using Android.App;
using Android.OS;
using Android.Views;
using Engine.Shared.Base;
using Android.Graphics;
using OpenTK;
using Engine.Shared.Touch;

namespace Engine.Android
{
    public abstract class GameActivity : Activity
    {
        /// <summary> The View for our game </summary>
        protected CustomGLView _View;
        /// <summary> The instance of the game </summary>
        protected BaseGame _GameInstance;

        /// <summary> The instance of the game activity </summary>
        public static GameActivity Instance { get; private set; }

        /// <summary> Creates the activity </summary>
        /// <param name="gameInstance"></param>
        protected GameActivity(BaseGame gameInstance)
        {
            _GameInstance = gameInstance;
            Instance = this;
        }

        /// <summary> Called when the activity is created - it creates the view and the game </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            _View = new CustomGLView(this, _GameInstance);
            Point point = new Point();
            WindowManager.DefaultDisplay.GetRealSize(point);
            _GameInstance.Init(new Vector2(point.X, point.Y));

            SetContentView(_View);
        }

        /// <summary> Called when the activity is paused - it pauses the view </summary>
        protected override void OnPause()
        {
            base.OnPause();
            _View.Pause();
            _GameInstance.Pause();
        }

        /// <summary> Called when the activity is resumed - it resumes the view </summary>
        protected override void OnResume()
        {
            base.OnResume();
            _View.Resume();
            _GameInstance.Resume();
            Window.DecorView.SystemUiVisibility |= (StatusBarVisibility)(Int32)
                (SystemUiFlags.HideNavigation
                | SystemUiFlags.Fullscreen
                | SystemUiFlags.LayoutFullscreen
                | SystemUiFlags.ImmersiveSticky
                | SystemUiFlags.LayoutStable);
        }
        
        /// <summary> Called when the activity has been touched </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool OnTouchEvent(MotionEvent e)
        {
            Int32 id = e.GetPointerId(e.ActionIndex);

            Vector2 position = new Vector2(e.GetX(e.ActionIndex), e.GetY(e.ActionIndex));
            position = new Vector2(position.X * Renderer.Instance.ScreenScale.X, position.Y * Renderer.Instance.ScreenScale.Y) + Renderer.Instance.ViewOffset;

            switch (e.ActionMasked)
            {
                case MotionEventActions.Down:
                case MotionEventActions.PointerDown:
                    TouchManager.Instance.AddEvent(new TouchEvent(id, position, TouchEvent.Type.PRESS));
                    break;

                case MotionEventActions.Up:
                case MotionEventActions.PointerUp:
                case MotionEventActions.Cancel:
                case MotionEventActions.Outside:
                    TouchManager.Instance.AddEvent(new TouchEvent(id, position, TouchEvent.Type.RELEASE));
                    break;

                case MotionEventActions.Move:
                    for (Int32 i = 0; i < e.PointerCount; i++)
                    {
                        id = e.GetPointerId(i);
                        position = new Vector2(e.GetX(i) * Renderer.Instance.ScreenScale.X, e.GetY(i) * Renderer.Instance.ScreenScale.Y) + Renderer.Instance.ViewOffset;
                        TouchManager.Instance.AddEvent(new TouchEvent(id, position, TouchEvent.Type.MOVE));
                    }
                    break;

            }

            return base.OnTouchEvent(e);
        }
    }
}
