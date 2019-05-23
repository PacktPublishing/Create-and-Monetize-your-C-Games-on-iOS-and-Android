using OpenTK;
using System;

namespace Engine.Shared.Graphics
{
    public class Camera
    {
        /// <summary> The position of the camera </summary>
        protected Vector2 _Position;
        /// <summary> The dimensions of the camera viewport </summary>
        protected Vector2 _Dimensions;
        /// <summary> The view matrix for our camera </summary>
        protected Matrix4 _ViewMatrix;
        /// <summary> The projection matrix for our camera </summary>
        protected Matrix4 _ProjectionMatrix;
        /// <summary> The view projection matrix for the camera </summary>
        protected Matrix4 _ViewProjectionMatrix;

        /// <summary> The position of the camera </summary>
        public Vector2 Position
        {
            get { return _Position; }
            set
            {
                _Position = value;
                MatrixInvalid = true;
            }
        }
        /// <summary> The view projection matrix for the camera </summary>
        public Matrix4 ViewProjectionMatrix => _ViewProjectionMatrix;
        /// <summary> Whether or not the view projection matrix is invalid </summary>
        public Boolean MatrixInvalid { get; protected set; }

        public Camera(Vector2 position, Vector2 dimensions)
        {
            _Dimensions = dimensions;
            _Position = position;
            _ProjectionMatrix = Matrix4.CreateOrthographic(_Dimensions.X, _Dimensions.Y, 1, 1000);
            UpdateViewProjectionMatrix();
        }

        /// <summary> Updates the view projection matrix when something has changed on the camera </summary>
        public virtual void UpdateViewProjectionMatrix()
        {
            _ViewProjectionMatrix = Matrix4.Identity;
            _ViewMatrix = Matrix4.LookAt(new Vector3(_Position.X, _Position.Y, 1), new Vector3(_Position.X, _Position.Y, 0), new Vector3(0, 1, 0));
            _ViewProjectionMatrix = Matrix4.Mult(_ViewMatrix, _ProjectionMatrix);
            MatrixInvalid = false;
        }
    }
}
