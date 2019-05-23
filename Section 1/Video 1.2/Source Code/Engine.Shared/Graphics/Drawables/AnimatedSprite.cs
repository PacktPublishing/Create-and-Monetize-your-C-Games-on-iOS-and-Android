using Engine.Shared.Base;
using Engine.Shared.Interfaces;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Engine.Shared.Graphics.Drawables
{
    /// <summary> An animated sprite </summary>
    public class AnimatedSprite : Sprite, IUpdatable
    {
        /// <summary> The behaviour to perform when the end of the animation is reached </summary>
        public enum EndBehaviour
        {
            LOOP,
            STOP,
            REVERSE
        }

        /// <summary> The number of frames per second </summary>
        protected Single _Fps;
        /// <summary> The top left and bottom right coordinates for each frame </summary>
        protected Vector4[] _Uvs;
        /// <summary> How much time has elapsed </summary>
        protected TimeSpan _ElapsedTime;
        /// <summary> The amount of time before changing FPS </summary>
        protected TimeSpan _ChangeTime;
        /// <summary> The current frame of the sprite </summary>
        protected Int32 _CurrentFrame;
        /// <summary> The behaviour to perform at the end of the animation </summary>
        protected EndBehaviour _EndBehaviour;

        /// <summary> The number of frames per second </summary>
        public Single Fps
        {
            get { return _Fps; }
            set
            {
                _Fps = value;
                _ChangeTime = TimeSpan.FromSeconds(1 / Math.Abs(value));
            }
        }
        /// <summary> Whether or not the sprite is playing </summary>
        public Boolean Playing { get; set; }
        /// <summary> The current frame of the sprite </summary>
        public Int32 CurrentFrame
        {
            get { return _CurrentFrame; }
            set
            {
                _CurrentFrame = value;
                _VerticesShouldUpdate = true;
            }
        }
        /// <summary> The behaviour to perform at the end of the animation </summary>
        public EndBehaviour AnimEndBehaviour
        {
            get { return _EndBehaviour; }
            set { _EndBehaviour = value; }
        }
        /// <summary> The action to call when the action is complete </summary>
        public Action<AnimatedSprite> OnComplete;

        /// <summary> Creates the animated sprite </summary>
        /// <param name="canvas"></param>
        /// <param name="zOrder"></param>
        /// <param name="texture"></param>
        public AnimatedSprite(Canvas canvas, Int32 zOrder, Texture texture, Int32 imageWidth, Int32 imageHeight, Int32 numFrames, Single fps)
            : base(canvas, zOrder, texture)
        {
            _Width = imageWidth;
            _Height = imageHeight;
            _Uvs = new Vector4[numFrames];
            CreateUvs(numFrames);
            _VerticesShouldUpdate = true;
            Fps = fps;
            _EndBehaviour = EndBehaviour.LOOP;
            UpdateManager.Instance.AddUpdatable(this);
        }

        /// <summary> Creates the animated sprite from CSV data </summary>
        /// <param name="canvas"></param>
        /// <param name="data"></param>
        public AnimatedSprite(Canvas canvas, String[] data)
            : base(canvas, data)
        {
            Int32 numFrames = -1;
            foreach (String stringData in data)
            {
                String[] splitData = stringData.Split('|');
                switch (splitData[0])
                {
                    case "ImageSize":
                        _Width = Single.Parse(splitData[1], CultureInfo.InvariantCulture);
                        _Height = Single.Parse(splitData[2], CultureInfo.InvariantCulture);
                        break;
                    case "Fps":
                        Fps = Single.Parse(splitData[1], CultureInfo.InvariantCulture);
                        break;
                    case "NumberOfFrames":
                        numFrames = Int32.Parse(splitData[1]);
                        break;
                    case "EndBehaviour":
                        _EndBehaviour = (EndBehaviour)Enum.Parse(typeof(EndBehaviour), splitData[1]);
                        break;
                    case "Playing":
                        Playing = Boolean.Parse(splitData[1]);
                        break;
                    case "CurrentFrame":
                        CurrentFrame = Int32.Parse(splitData[1]);
                        break;
                }
            }
            if (numFrames == -1) throw new ArgumentOutOfRangeException(nameof(numFrames), "The AnimatedSprite needs to define the number of frames");
            _Uvs = new Vector4[numFrames];
            CreateUvs(numFrames);
            _VerticesShouldUpdate = true;
            UpdateManager.Instance.AddUpdatable(this);
        }

        /// <summary> Creates the UVs for each frame </summary>
        private void CreateUvs(Int32 numFrames)
        {
            Single currentX = 0;
            Single currentY = 0;
            Single frameWidth = (Single)_Width / _Texture.Width;
            Single frameHeight = (Single)_Height / _Texture.Height;
            for (Int32 i = 0; i < numFrames; i++)
            {
                Single xProportion = currentX / _Texture.Width;
                Single yProportion = currentY / _Texture.Height;
                _Uvs[i] = new Vector4(xProportion, yProportion, xProportion + frameWidth, yProportion + frameHeight);
                currentX += _Width;
                if (currentX >= _Texture.Width)
                {
                    currentX = 0;
                    currentY += _Height;
                }
            }
        }

        /// <summary> Generates the vertices for the sprite </summary>
        /// <returns></returns>
        public override List<Vertex> GenerateVertices()
        {
            if (_Uvs == null) return base.GenerateVertices();
            return new List<Vertex>
            {
                new Vertex(new Vector3(0, 0, 0f), new Vector2(_Uvs[_CurrentFrame].X, _Uvs[_CurrentFrame].W), _Colour),
                new Vertex(new Vector3(_Width, 0, 0f), new Vector2(_Uvs[_CurrentFrame].Z, _Uvs[_CurrentFrame].W), _Colour),
                new Vertex(new Vector3(_Width, _Height, 0f), new Vector2(_Uvs[_CurrentFrame].Z, _Uvs[_CurrentFrame].Y), _Colour),
                new Vertex(new Vector3(0, _Height, 0f), new Vector2(_Uvs[_CurrentFrame].X, _Uvs[_CurrentFrame].Y), _Colour),
            };
        }

        /// <summary> Updates the sprite </summary>
        /// <param name="timeSinceUpdate"></param>
        public virtual void Update(TimeSpan timeSinceUpdate)
        {
            if (!Playing) return;
            _ElapsedTime += timeSinceUpdate;
            if (_ElapsedTime > _ChangeTime)
            {
                _ElapsedTime -= _ChangeTime;
                if (Fps > 0)
                {
                    CurrentFrame++;
                }
                else
                {
                    CurrentFrame--;
                }
                CheckBounds();
            }
        }

        /// <summary> Checks the bounds of the animation </summary>
        protected virtual void CheckBounds()
        {
            if ((Fps > 0 && _CurrentFrame >= _Uvs.Length) || (Fps < 0 && _CurrentFrame < 0))
            {
                OnEndReached();
            }
        }

        /// <summary> Called when the end of the animation is reached - it will perform a different action based on the set behaviour </summary>
        protected virtual void OnEndReached()
        {
            switch (_EndBehaviour)
            {
                case EndBehaviour.LOOP:
                    if (Fps > 0) _CurrentFrame = 0;
                    else _CurrentFrame = _Uvs.Length - 1;
                    break;
                case EndBehaviour.REVERSE:
                    if (Fps > 0) _CurrentFrame = _Uvs.Length - 2;
                    else _CurrentFrame = Math.Min(1, _Uvs.Length - 1);
                    Fps = -Fps;
                    break;
                case EndBehaviour.STOP:
                    Playing = false;
                    if (Fps > 0) _CurrentFrame = _Uvs.Length - 1;
                    else _CurrentFrame = 0;
                    break;
            }
            OnComplete?.Invoke(this);
        }

        /// <summary> Disposes of the sprite </summary>
        public override void Dispose()
        {
            base.Dispose();
            UpdateManager.Instance.RemoveUpdatable(this);
        }

        /// <summary> Whether or not the AnimatedSprite can update </summary>
        /// <returns></returns>
        public virtual Boolean CanUpdate()
        {
            return Playing;
        }
    }
}