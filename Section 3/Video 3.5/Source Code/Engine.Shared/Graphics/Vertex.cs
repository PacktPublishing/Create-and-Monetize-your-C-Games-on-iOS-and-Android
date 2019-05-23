using OpenTK;

namespace Engine.Shared.Graphics
{
    public class Vertex
    {
        /// <summary> The position of the vertex </summary>
        public Vector3 Position { get; set; }
        /// <summary> The texture coordinate of the vertex </summary>
        public Vector2 Uv { get; set; }
        /// <summary> The colour of the vertex </summary>
        public Vector4 Colour { get; set; }

        /// <summary> Creates the vertex with the given position & uv </summary>
        /// <param name="position"></param>
        /// <param name="uv"></param>
        public Vertex(Vector3 position, Vector2 uv, Vector4 colour)
        {
            Position = position;
            Uv = uv;
            Colour = colour;
        }
    }
}
