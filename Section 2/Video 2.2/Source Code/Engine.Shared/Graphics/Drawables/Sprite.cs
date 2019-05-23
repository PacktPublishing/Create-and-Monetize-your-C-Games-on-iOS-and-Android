using OpenTK;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Engine.Shared.Graphics.Drawables
{
    /// <summary> A simple sprite </summary>
    public class Sprite : Drawable
    {
        /// <summary> The position of the sprite </summary>
        protected Vector2 _Position;
        /// <summary> The positional offset for the sprite </summary>
        protected Vector2 _Offset;
        /// <summary> The scale of the sprite </summary>
        protected Vector2 _Scale;
        /// <summary> The scale origin of the sprite relative to its position </summary>
        protected Vector2 _ScaleOrigin;
        /// <summary> The rotation of the sprite </summary>
        protected Double _Rotation;
        /// <summary> The rotation origin of the sprite relative to its position </summary>
        protected Vector2 _RotationOrigin;
        /// <summary> Whether or not the scale matrix is invalid </summary>
        protected Boolean _ScaleMatrixInvalid;
        /// <summary> Whether or not the rotation matrix is invalid </summary>
        protected Boolean _RotationMatrixInvalid;
        /// <summary> Whether or not the translate matrix is invalid </summary>
        protected Boolean _TranslateMatrixInvalid;
        /// <summary> The scale matrix used for the world matrix </summary>
        protected Matrix4 _ScaleMatrix;
        /// <summary> The translation matrix  </summary>
        protected Matrix4 _TranslateMatrix;
        /// <summary> The rotation matrix for the sprite </summary>
        protected Matrix4 _RotationMatrix;

        /// <summary> The position of the sprite </summary>
        public virtual Vector2 Position
        {
            get { return _Position; }
            set
            {
                _Position = value;
                WVPMatrixInvalid = true;
                _TranslateMatrixInvalid = true;
            }
        }
        /// <summary> The offset for the sprite </summary>
        public Vector2 Offset
        {
            get { return _Offset; }
            set
            {
                _Offset = value;
                WVPMatrixInvalid = true;
                _TranslateMatrixInvalid = true;
            }
        }
        /// <summary> Sets the scale of the sprite </summary>
        public Vector2 Scale
        {
            get { return _Scale; }
            set
            {
                _Scale = value;
                WVPMatrixInvalid = true;
                _ScaleMatrixInvalid = true;
            }
        }
        /// <summary> The scale origin of the sprite </summary>
        public Vector2 ScaleOrigin
        {
            get { return _ScaleOrigin; }
            set
            {
                _ScaleOrigin = value;
                WVPMatrixInvalid = true;
                _ScaleMatrixInvalid = true;
            }
        }
        /// <summary> The rotation of the sprite </summary>
        public Double Rotation
        {
            get { return _Rotation; }
            set
            {
                _Rotation = value;
                WVPMatrixInvalid = true;
                _RotationMatrixInvalid = true;
            }
        }
        /// <summary> The rotation origin of the sprite </summary>
        public Vector2 RotationOrigin
        {
            get { return _RotationOrigin; }
            set
            {
                _RotationOrigin = value;
                WVPMatrixInvalid = true;
                _RotationMatrixInvalid = true;
            }
        }

        /// <summary> Creates a sprite </summary>
        /// <param name="canvas"></param>
        /// <param name="zOrder"></param>
        /// <param name="texture"></param>
        public Sprite(Canvas canvas, Int32 zOrder, Texture texture) : base(canvas, zOrder, texture)
        {
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Rotation = 0;
            WVPMatrixInvalid = true;
        }

        /// <summary> Creates the sprite from CSV data </summary>
        /// <param name="canvas"></param>
        /// <param name="data"></param>
        public Sprite(Canvas canvas, String[] data)
            : base(canvas)
        {
            Vector2 position = Vector2.Zero;
            Vector2 scale = Vector2.One;
            Single rotation = 0;

            foreach (String spriteData in data)
            {
                String[] splitData = spriteData.Split('|');
                switch (splitData[0])
                {
                    case "ZOrder": ZOrder = Int32.Parse(splitData[1]); break;
                    case "Texture":
                        _Texture = Texture.GetTexture(splitData[1]);
                        Width = _Texture.Width;
                        Height = _Texture.Height;
                        break;
                    case "Position":
                        position = new Vector2(Single.Parse(splitData[1], CultureInfo.InvariantCulture), Single.Parse(splitData[2], CultureInfo.InvariantCulture));
                        break;
                    case "Offset":
                        Offset = new Vector2(Single.Parse(splitData[1], CultureInfo.InvariantCulture), Single.Parse(splitData[2], CultureInfo.InvariantCulture));
                        break;
                    case "Scale":
                        scale = new Vector2(Single.Parse(splitData[1], CultureInfo.InvariantCulture), Single.Parse(splitData[2], CultureInfo.InvariantCulture));
                        break;
                    case "ScaleOrigin":
                        ScaleOrigin = new Vector2(Single.Parse(splitData[1], CultureInfo.InvariantCulture), Single.Parse(splitData[2], CultureInfo.InvariantCulture));
                        break;
                    case "Rotation":
                        rotation = Single.Parse(splitData[1], CultureInfo.InvariantCulture);
                        break;
                    case "RotationOrigin":
                        RotationOrigin = new Vector2(Single.Parse(splitData[1], CultureInfo.InvariantCulture), Single.Parse(splitData[2], CultureInfo.InvariantCulture));
                        break;
                    case "Colour":
                        Colour = new Vector4(Single.Parse(splitData[1], CultureInfo.InvariantCulture), Single.Parse(splitData[2], CultureInfo.InvariantCulture), Single.Parse(splitData[3], CultureInfo.InvariantCulture), Single.Parse(splitData[4], CultureInfo.InvariantCulture));
                        break;
                    case "Width":
                        Width = Single.Parse(splitData[1], CultureInfo.InvariantCulture);
                        break;
                    case "Height":
                        Height = Single.Parse(splitData[1], CultureInfo.InvariantCulture);
                        break;
                    case "Visible":
                        Visible = Boolean.Parse(splitData[1]);
                        break;
                }
            }
            if (_Texture == null) throw new ArgumentNullException(nameof(_Texture), "The given sprite does not have a texture defined");

            Position = position;
            Rotation = rotation;
            Scale = scale;
            WVPMatrixInvalid = true;
        }

        /// <summary> Generates the indices </summary>
        /// <returns></returns>
        public override List<UInt32> GenerateIndices(UInt32 offset)
        {
            List<UInt32> indices = new List<UInt32>
            {
                offset, offset + 2, offset + 1, offset, offset + 3, offset + 2
            };

            return indices;
        }

        /// <summary> Generates the vertices - just creates 4 vertices </summary>
        /// <returns></returns>
        public override List<Vertex> GenerateVertices()
        {
            return new List<Vertex>
            {
                new Vertex(new Vector3(0, 0, 0f), new Vector2(0, 1), _Colour),
                new Vertex(new Vector3(_Width, 0, 0f), new Vector2(1, 1), _Colour),
                new Vertex(new Vector3(_Width, _Height, 0f), new Vector2(1, 0), _Colour),
                new Vertex(new Vector3(0, _Height, 0f), new Vector2(0, 0), _Colour),
            };
        }

        /// <summary> Updates the World View Projection matrix </summary>
        /// <param name="vpMatrix"></param>
        /// <returns></returns>
        public override Matrix4 UpdateWVP(Matrix4 vpMatrix)
        {
            if (_ScaleMatrixInvalid)
            {
                _ScaleMatrix = Matrix4.CreateTranslation(-_ScaleOrigin.X, -_ScaleOrigin.Y, 0) * Matrix4.Scale(new Vector3(_Scale.X, _Scale.Y, 1)) * Matrix4.CreateTranslation(_ScaleOrigin.X, _ScaleOrigin.Y, 0);
                _ScaleMatrixInvalid = false;
            }

            if (_RotationMatrixInvalid)
            {
                _RotationMatrix = Matrix4.CreateTranslation(-_RotationOrigin.X, -_RotationOrigin.Y, 0) * Matrix4.CreateRotationZ((Single)_Rotation) * Matrix4.CreateTranslation(_RotationOrigin.X, _RotationOrigin.Y, 0);
                _RotationMatrixInvalid = false;
            }

            if (_TranslateMatrixInvalid)
            {
                _TranslateMatrix = Matrix4.CreateTranslation(new Vector3(_Position.X - _Offset.X, _Position.Y - _Offset.Y, 0));
                _TranslateMatrixInvalid = false;
            }
            WorldMatrix = _RotationMatrix * _ScaleMatrix * _TranslateMatrix;
            return Matrix4.Mult(WorldMatrix, vpMatrix);
        }
    }
}
