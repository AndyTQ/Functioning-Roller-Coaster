using UnityEngine;

namespace WSMGameStudio.Splines
{
    [System.Serializable]
    public class OrientedPoint
    {
        private Vector3 _position;
        private Quaternion _rotation;

        [SerializeField]
        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        [SerializeField]
        public Quaternion Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public OrientedPoint()
        {
            _position = Vector3.zero;
            _rotation = Quaternion.identity;
        }

        public OrientedPoint(Vector3 position, Quaternion rotation)
        {
            _position = position;
            _rotation = rotation;
        }

        public Vector3 LocalToWorld(Vector3 point)
        {
            return _position + _rotation * point;
        }

        public Vector3 WorldToLocal(Vector3 point)
        {
            return Quaternion.Inverse(_rotation) * (point - _position);
        }

        public Vector3 LocalToWorldDirection(Vector3 direction)
        {
            return _rotation * direction;
        }
    } 
}
