using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSMGameStudio.Splines
{
    public class UniqueMesh : MonoBehaviour
    {
        [HideInInspector] int ownerID;
        private MeshFilter _meshFilter;

        MeshFilter meshFilter
        {
            get
            {
                _meshFilter = _meshFilter == null ? GetComponent<MeshFilter>() : _meshFilter;
                _meshFilter = _meshFilter == null ? gameObject.AddComponent<MeshFilter>() : _meshFilter;
                return _meshFilter;
            }
        }

        Mesh _mesh;
        protected Mesh mesh
        {
            get
            {
                bool isOwner = (ownerID == gameObject.GetInstanceID());

                ownerID = gameObject.GetInstanceID();
                string meshName = string.Format("Mesh [{0}]", ownerID);

                if (meshFilter.sharedMesh == null || !isOwner || (meshFilter.sharedMesh != null && meshFilter.sharedMesh.name != meshName))
                {
                    meshFilter.sharedMesh = _mesh = new Mesh();
                    _mesh.name = meshName;
                }
                return _mesh;
            }
        }
    }
}
