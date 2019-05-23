using Engine.Shared.Base;
using Engine.Shared.Graphics;
using Engine.Shared.Graphics.Drawables;
using Engine.Shared.State;
using Game.Shared.Base;
using Game.Shared.Scenes;
using Game.Shared.States;
using OpenTK;
using System;

namespace Game.Shared
{
    /// <summary> The instance of our game </summary>
    public class ZippyGame : BaseGame
    {
        /// <summary> The target resolution for the game </summary>
        public override Vector2 InitialResolution => new Vector2(1920, 1080);
        /// <summary> The camera for the game </summary>
        public static LookAtCamera Camera;
        /// <summary> The canvas for the game graphics </summary>
        public static Canvas MainCanvas;
        /// <summary> The canvas for the UI graphics </summary>
        public static Canvas UICanvas;

        /// <summary> Creates our game </summary>
        public ZippyGame()
        {

        }

        /// <summary> Loads the content for the game </summary>
        public override void LoadContent()
        {
            Camera = new LookAtCamera(Vector2.Zero, Renderer.Instance.TargetDimensions);
            MainCanvas = new Canvas(Camera, 0, new Shader("Content/Shaders/VertexShader.txt", "Content/Shaders/FragmentShader.txt"));
            UICanvas = new Canvas(new Camera(Vector2.Zero, Renderer.Instance.TargetDimensions), 1, new Shader("Content/Shaders/VertexShader.txt", "Content/Shaders/FragmentShader.txt"));
            StateManager.Instance.StartState(new MenuState());
        }

        /// <summary> Calculates the extra offset based on the difference in height between the target and the actual resolution </summary>
        /// <param name="heightDifference"></param>
        /// <returns></returns>
        protected override Vector2 CalculateExtraOffset(Single heightDifference)
        {
            return new Vector2(0, 0);
        }
    }

}
