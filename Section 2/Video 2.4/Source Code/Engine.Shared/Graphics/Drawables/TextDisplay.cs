using System;
using System.Collections.Generic;
using OpenTK;
using System.Linq;
using System.Globalization;

namespace Engine.Shared.Graphics.Drawables
{
    /// <summary> This drawable will be used to display text </summary>
    public class TextDisplay : Sprite
    {
        /// <summary> The alignment of the text </summary>
        public enum Alignment
        {
            LEFT,
            CENTER,
            RIGHT
        }
        /// <summary> The dictionary of characters mapped to 4 UV's  </summary>
        protected readonly Dictionary<Char, Vector4> _CharacterMap = new Dictionary<Char, Vector4>();
        /// <summary> The text to display on the text displayer </summary>
        protected String _Text;
        /// <summary> The text split into separate strings </summary>
        protected String[] _StringArray;
        /// <summary> The width of each character </summary>
        protected Single _CharacterWidth;
        /// <summary> The height of each character </summary>
        protected Single _CharacterHeight;
        /// <summary> The spacing between individual characters </summary>
        protected Single _CharacterSpacing;
        /// <summary> The spacing between lines </summary>
        protected Single _LineSpacing;
        /// <summary> The alignment of the text </summary>
        protected Alignment _Alignment;
        /// <summary> The text currently being displayed </summary>
        public String Text
        {
            get { return _Text; }
            set
            {
                if (_Text.Equals(value)) return;
                Int32 previousCount = _Text?.Length ?? 0;
                Int32 newCount = value?.Length ?? 0;
                _Text = value;
                _StringArray = _Text.Split('\n');
                CalculateDimensions();
                _VerticesShouldUpdate = true;
                _IndicesShouldUpdate = previousCount != newCount;
            }
        }
        /// <summary> The spacing between each character in pixels </summary>
        public Single CharacterSpacing
        {
            get { return _CharacterSpacing; }
            set
            {
                if (Math.Abs(_CharacterSpacing - value) < 0.01f) return;
                _CharacterSpacing = value;
                CalculateDimensions();
                _VerticesShouldUpdate = true;
            }
        }
        /// <summary> The spacing between each line in pixels </summary>
        public Single LineSpacing
        {
            get { return _LineSpacing; }
            set
            {
                if (Math.Abs(_LineSpacing - value) < 0.01f) return;
                _LineSpacing = value;
                CalculateDimensions();
                _VerticesShouldUpdate = true;
            }
        }
        /// <summary> Whether or not the text display is visible </summary>
        public override Boolean Visible
        {
            get => base.Visible && !String.IsNullOrEmpty(_Text);
            set => base.Visible = value;
        }
        /// <summary> The alignemnt of the text </summary>
        public Alignment TextAlignment
        {
            get { return _Alignment; }
            set
            {
                if (_Alignment == value) return;
                _Alignment = value;
                _VerticesShouldUpdate = true;
            }
        }
        public TextDisplay(Canvas canvas, Int32 zOrder, Texture texture, String letters, Single characterWidth, Single characterHeight)
            : base(canvas, zOrder, texture)
        {

            _CharacterWidth = characterWidth;
            _CharacterHeight = characterHeight;
            GenerateCharacterMap(letters);
            _Text = "";
            _StringArray = new String[0];
            _VerticesShouldUpdate = true;
        }

        /// <summary> Creates the text display from CSV </summary>
        /// <param name="canvas"></param>
        /// <param name="data"></param>
        public TextDisplay(Canvas canvas, String[] data)
            : base(canvas, data)
        {
            _CharacterWidth = -1f;
            _CharacterHeight = -1f;
            String characters = "";
            _Text = "";
            _StringArray = new String[0];

            foreach (String stringData in data)
            {
                String[] splitData = stringData.Split('|');
                switch (splitData[0])
                {
                    case "CharacterWidth":
                        _CharacterWidth = Single.Parse(splitData[1], CultureInfo.InvariantCulture);
                        break;
                    case "CharacterHeight":
                        _CharacterHeight = Single.Parse(splitData[1], CultureInfo.InvariantCulture);
                        break;
                    case "Characters":
                        characters = splitData[1];
                        break;
                    case "Text":
                        Text = splitData[1];
                        break;
                    case "CharacterSpacing":
                        _CharacterSpacing = Single.Parse(splitData[1], CultureInfo.InvariantCulture);
                        break;
                    case "LineSpacing":
                        _LineSpacing = Single.Parse(splitData[1], CultureInfo.InvariantCulture);
                        break;
                    case "Alignment":
                        TextAlignment = (Alignment)Enum.Parse(typeof(Alignment), splitData[1]);
                        break;
                }
            }
            if (_CharacterWidth < 0) throw new ArgumentOutOfRangeException(nameof(_CharacterWidth), "A character width has to be specified for the text display");
            if (_CharacterHeight < 0) throw new ArgumentOutOfRangeException(nameof(_CharacterHeight), "A character height has to be specified for the text display");
            if (String.IsNullOrEmpty(characters)) throw new ArgumentOutOfRangeException(nameof(characters), "Characters need to be defined for the TextDisplay");

            GenerateCharacterMap(characters);
            CalculateDimensions();
            _VerticesShouldUpdate = true;
        }

        /// <summary> Generates the characters from the texture </summary>
        private void GenerateCharacterMap(String characters)
        {
            Single currentX = 0;
            Single currentY = 0;
            Single frameWidth = (Single)_CharacterWidth / _Texture.Width;
            Single frameHeight = (Single)_CharacterHeight / _Texture.Height;
            foreach (Char character in characters)
            {
                if (_CharacterMap.ContainsKey(character)) throw new ArgumentOutOfRangeException("Character in TextDisplay string is duplicated");
                Single xProportion = currentX / _Texture.Width;
                Single yProportion = currentY / _Texture.Height;
                _CharacterMap.Add(character, new Vector4(xProportion, yProportion, xProportion + frameWidth, yProportion + frameHeight));
                currentX += _CharacterWidth;
                if (currentX >= _Texture.Width)
                {
                    currentX = 0;
                    currentY += _CharacterHeight;
                }
            }
        }

        /// <summary> Calculates the dimensions of the text display </summary>
        private void CalculateDimensions()
        {
            if (String.IsNullOrEmpty(_Text)) return;
            Int32 maxChars = _StringArray.Max(s => s.Length);
            _Width = maxChars * (_CharacterWidth + _CharacterSpacing);
            _Height = _StringArray.Length * (_CharacterHeight + _LineSpacing);
        }

        /// <summary> Generates the indices for the display </summary>
        /// <returns></returns>
        public override List<UInt32> GenerateIndices(UInt32 offset)
        {
            List<UInt32> indices = new List<UInt32>();
            if (String.IsNullOrEmpty(_Text)) return indices;
            String[] reverseArray = _StringArray.Reverse().ToArray();
            foreach (String line in reverseArray)
            {
                foreach (Char character in line)
                {
                    indices.AddRange(new List<UInt32> { offset, offset + 2, offset + 1, offset, offset + 3, offset + 2 });
                    offset += 4;
                }
            }
            return indices;
        }

        /// <summary> Generates the vertices of the text displayer </summary>
        /// <returns></returns>
        public override List<Vertex> GenerateVertices()
        {
            List<Vertex> vertices = new List<Vertex>();
            Single currentHeight = 0;
            if (String.IsNullOrEmpty(_Text)) return vertices;
            String[] reverseText = _StringArray.Reverse().ToArray();
            foreach (String line in reverseText)
            {
                Single offset = 0;
                switch (_Alignment)
                {
                    case Alignment.CENTER: offset = (_Width - (line.Length * (_CharacterWidth + _CharacterSpacing))) / 2; break;
                    case Alignment.RIGHT: offset = (_Width - (line.Length * (_CharacterWidth + _CharacterSpacing))); break;
                }
                Single currentWidth = offset;
                foreach (Char character in line)
                {
                    if (!_CharacterMap.ContainsKey(character)) throw new ArgumentOutOfRangeException($"Given character {character} is not found in the TextDisplay");
                    Vector4 uv = _CharacterMap[character];
                    vertices.AddRange(new List<Vertex>
                    {
                        new Vertex(new Vector3(currentWidth, currentHeight, 0f), new Vector2(uv.X, uv.W), _Colour),
                        new Vertex(new Vector3(currentWidth + _CharacterWidth, currentHeight, 0f), new Vector2(uv.Z, uv.W), _Colour),
                        new Vertex(new Vector3(currentWidth + _CharacterWidth, currentHeight + _CharacterHeight, 0f), new Vector2(uv.Z, uv.Y), _Colour),
                        new Vertex(new Vector3(currentWidth, currentHeight + _CharacterHeight, 0f), new Vector2(uv.X, uv.Y), _Colour),
                    });
                    currentWidth += _CharacterWidth + _CharacterSpacing;
                }
                currentHeight += _CharacterHeight + _LineSpacing;
            }
            return vertices;
        }
    }
}