using Engine.Shared.Base;
using Engine.Shared.Graphics;
using Engine.Shared.Graphics.Drawables;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Game.Shared.Base;
using Game.Shared.Characters;
using Game.Shared.Characters.Enemies;
using Game.Shared.Level;
using Game.Shared.Objects;
using Game.Shared.Objects.Collectibles;
using Game.Shared.Objects.UI;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Globalization;
using XVector2 = Microsoft.Xna.Framework.Vector2;

namespace Game.Shared.Scenes
{
    /// <summary> The main scene for the game </summary>
    public class GameScene : Scene
    {
        /// <summary> The background for the scene </summary>
        private readonly Sprite _Background;
        /// <summary> The collectibles in the scene </summary>
        private readonly List<Collectible> _Collectibles = new List<Collectible>();
        /// <summary> The enemies in the scene </summary>
        private readonly List<Enemy> _Enemies = new List<Enemy>();
        /// <summary> The platforms in the game </summary>
        private readonly List<Platform> _Platforms = new List<Platform>();
        /// <summary> The drawables for the current level </summary>
        private readonly List<Drawable> _LevelDrawables = new List<Drawable>();
        /// <summary> The text for the score </summary>
        private readonly TextDisplay _ScoreText;
        /// <summary> The image used to display the number of lives </summary>
        private readonly Sprite _LifeImage;
        /// <summary> The text which displays the number of lives </summary>
        private readonly TextDisplay _LifeText;
        /// <summary> The background for the health bar </summary>
        private readonly Sprite _HealthBg;
        /// <summary> The main health bar </summary>
        private readonly HealthBar _HealthBar;
        /// <summary> The Zippy character </summary>
        private Zippy _Zippy;
        /// <summary> The number of coins the player has collected </summary>
        private Int32 _Coins;
        /// <summary> The current score of the player </summary>
        private Int64 _TotalScore;
        /// <summary> The current level's score </summary>
        private Int64 _LevelScore;
        /// <summary> The instance of the game scene </summary>
        private static GameScene _Instance;
        /// <summary> The collider for when the player reaches the left of the screen </summary>
        private readonly Body _LeftCollider;
        /// <summary> The collider for when the player reaches the right of the screen </summary>
        private readonly Body _RightCollider;

        /// <summary> The instance of the game scene </summary>
        public static GameScene Instance => _Instance ?? (_Instance = new GameScene());
        /// <summary> The Zippy character </summary>
        public Zippy Zippy => _Zippy;
        /// <summary> The action fired when the level is complete </summary>
        public Action LevelComplete;
        /// <summary> The action fired when the player dies </summary>
        public Action OnDeath;
        /// <summary> The player's total score </summary>
        public Int64 TotalScore => _TotalScore;

        /// <summary> Creates the game scene </summary>
        public GameScene()
        {
            _ScoreText = new TextDisplay(ZippyGame.UICanvas, ZOrders.SCORE_TEXT, Texture.GetTexture("Content/Graphics/UI/kromasky.png"), Constants.KROMASKY_CHARACTERS, 32, 32)
            {
                Position = new Vector2(-Renderer.Instance.TargetDimensions.X / 2 + 50, Renderer.Instance.TargetDimensions.Y / 2 - 60),
                Visible = true,
                Text = "0"
            };
            AddDrawable(_ScoreText);
            
            _HealthBg = new Sprite(ZippyGame.UICanvas, ZOrders.HEALTH_BG, Texture.GetTexture("Content/Graphics/UI/HealthBg.png"))
            {
                Position = new Vector2(Renderer.Instance.TargetDimensions.X / 2 - 180, Renderer.Instance.TargetDimensions.Y / 2 - 60),
                Visible = true
            };
            AddDrawable(_HealthBg);
            _HealthBar = new HealthBar(_HealthBg.Position);
            AddDrawable(_HealthBar);

            _Zippy = new Zippy(_HealthBar)
            {
                Visible = true,
                Active = false
            };

            _LifeImage = new Sprite(ZippyGame.UICanvas, ZOrders.SCORE_TEXT, Texture.GetTexture("Content/Graphics/UI/LivesIcon.png"))
            {
                Position = new Vector2(Renderer.Instance.TargetDimensions.X / 2 - 150, Renderer.Instance.TargetDimensions.Y / 2 - 110),
                Visible = true
            };
            AddDrawable(_LifeImage);
            _LifeText = new TextDisplay(ZippyGame.UICanvas, ZOrders.SCORE_TEXT, Texture.GetTexture("Content/Graphics/UI/kromasky.png"), Constants.KROMASKY_CHARACTERS, 32, 32)
            {
                Position = new Vector2(Renderer.Instance.TargetDimensions.X / 2 - 100, Renderer.Instance.TargetDimensions.Y / 2 - 110),
                Visible = true,
                Text = $"x{_Zippy.Lives}"
            };
            AddDrawable(_LifeText);

            _LeftCollider = PhysicsWorld.Instance.CreateRectangle(new XVector2(-Single.MaxValue, 0), new XVector2(20, Renderer.Instance.TargetDimensions.Y), 10);
            _LeftCollider.BodyType = BodyType.Static;
            _LeftCollider.CollisionCategories = Constants.LIMIT_CATEGORY;
            _RightCollider = PhysicsWorld.Instance.CreateRectangle(new XVector2(Single.MaxValue, 0), new XVector2(20, Renderer.Instance.TargetDimensions.Y), 10);
            _RightCollider.BodyType = BodyType.Static;
            _RightCollider.CollisionCategories = Constants.LIMIT_CATEGORY;
        }

        /// <summary> Initialises the game and the elements </summary>
        public void InitialiseGame()
        {
            _Zippy.Reset();
            _LifeText.Text = $"x{_Zippy.Lives}";
            _TotalScore = 0;
            _ScoreText.Text = "0";
        }

        /// <summary> Starts the game </summary>
        public void Start()
        {
            _Zippy.Active = true;
            foreach (Enemy enemy in _Enemies)
            {
                enemy.Active = true;
                enemy.Visible = true;
            }
            foreach (Collectible collectible in _Collectibles)
            {
                collectible.Visible = true;
            }
            foreach (Platform platform in _Platforms)
            {
                platform.Visible = true;
                platform.Enabled = true;
            }
            foreach (Drawable drawable in _LevelDrawables)
            {
                drawable.Visible = true;
            }
            ZippyGame.Camera.Active = true;
            ZippyGame.Camera.Reset();

            _LevelScore = 0;
            PhysicsWorld.Instance.Enabled = true;
            _ScoreText.Text = $"{_TotalScore}";
            _Zippy.Initialise();
        }

        /// <summary> Stops the game  </summary>
        public void Stop()
        {
            _Zippy.Active = false;
            foreach (Enemy enemy in _Enemies) enemy.Active = false;
            PhysicsWorld.Instance.Enabled = false;
            _Zippy.Stop();
        }

        /// <summary> Loads the current level </summary>
        public void LoadLevel()
        {
            DisposeElements();
            String[] data = LevelController.Instance.CurrentLevel.Data;

            Vector2 playerStart = Vector2.Zero;

            foreach (String element in data)
            {
                if (String.IsNullOrEmpty(element) || element.Equals("\r")) continue;
                String[] elementData = element.Split(',');
                switch (elementData[0])
                {
                    case "Sprite":
                        _LevelDrawables.Add(new Sprite(ZippyGame.MainCanvas, elementData));
                        break;

                    case "TextDisplay":
                        _LevelDrawables.Add(new TextDisplay(ZippyGame.MainCanvas, elementData));
                        break;

                    case "AnimatedSprite":
                        _LevelDrawables.Add(new AnimatedSprite(ZippyGame.MainCanvas, elementData));
                        break;

                    case "PlayerStart":
                        String[] position = elementData[1].Split('|');
                        XVector2 pos = new XVector2(Single.Parse(position[0], CultureInfo.InvariantCulture), Single.Parse(position[1], CultureInfo.InvariantCulture));
                        _Zippy.SetStartPosition(pos);
                        break;

                    case "CameraLimits":
                        ZippyGame.Camera.SetLimits(elementData);
                        _LeftCollider.Position = ConvertUnits.ToSimUnits(new XVector2(ZippyGame.Camera.TopLeft.X - (Renderer.Instance.TargetDimensions.X / 2 - 10), ZippyGame.Camera.TopLeft.Y));
                        _RightCollider.Position = ConvertUnits.ToSimUnits(new XVector2(ZippyGame.Camera.BottomRight.X + (Renderer.Instance.TargetDimensions.X / 2 + 10), ZippyGame.Camera.BottomRight.Y));
                        break;

                    default:
                        if (!SceneTypeMap.TypeExists(elementData[0])) throw new KeyNotFoundException($"The given type {elementData[0]} is not in the type map");
                        Object obj = Activator.CreateInstance(SceneTypeMap.GetType(elementData[0]), new Object[] { elementData });
                        if (obj is Enemy) _Enemies.Add((Enemy)obj);
                        else if (obj is Collectible) _Collectibles.Add((Collectible)obj);
                        else if (obj is Platform) _Platforms.Add((Platform)obj);
                        break;

                }
            }
        }

        public override void Update(TimeSpan timeSinceUpdate)
        {
        }

        /// <summary> Increments the number of coins the player has </summary>
        public void IncrementCoins(Coin coin)
        {
            _Coins++;
            _Collectibles.Remove(coin);
            _LevelScore += 100;
            _ScoreText.Text = $"{_TotalScore + _LevelScore}";
        }

        /// <summary> Updates the text for the lives </summary>
        public void UpdateLivesText()
        {
            _LifeText.Text = $"x{_Zippy.Lives}";
        }

        /// <summary> Called when an enemy is defeated - increases the score </summary>
        public void OnEnemyDefeated(Enemy enemy)
        {
            _LevelScore += 500;
            _ScoreText.Text = $"{_TotalScore + _LevelScore}";
            _Enemies.Remove(enemy);
        }

        /// <summary> Called when the level is complete </summary>
        public void OnComplete()
        {
            _TotalScore += 1000;
            _TotalScore += _LevelScore;
            _ScoreText.Text = $"{_TotalScore}";
            LevelComplete?.Invoke();
        }

        /// <summary> Disposes of the elements in the scene </summary>
        private void DisposeElements()
        {
            foreach (Collectible collectible in _Collectibles) collectible.Dispose();
            _Collectibles.Clear();

            foreach (Enemy enemy in _Enemies) enemy.Dispose();
            _Enemies.Clear();

            foreach (Platform platform in _Platforms) platform.Dispose();
            _Platforms.Clear();

            foreach (Drawable drawable in _LevelDrawables) drawable.Dispose();
            _LevelDrawables.Clear();
        }
    }
}
