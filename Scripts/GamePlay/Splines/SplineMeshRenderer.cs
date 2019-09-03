using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WSMGameStudio.Splines
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class SplineMeshRenderer : UniqueMesh
    {
        //Mesh generation parameters
        [Tooltip("Guide spline")]
        public Spline _spline;
        [Tooltip("Mesh generated in real time (EDITOR ONLY)")]
        public bool realtimeMeshGeneration = false;
        [Tooltip("Mesh that will be rendered along the spline")]
        public Mesh baseMesh;
        [Tooltip("Enable collision for generated mesh")]
        public bool enableCollision = false;
        [Tooltip("(Optional) Mesh offset from spline")]
        public Vector3 meshOffset;
        [Tooltip("(Optional) Custom mesh colliders generated using the same spline")]
        public List<SplineMeshRenderer> customMeshColliders;

        private Transform _auxTransform1;
        private Transform _auxTransform2;
        private MeshCollider _meshCollider;
        //Base mesh scanned values
        private float _baseMeshLength;
        private float _baseMeshMinZ;
        private float _baseMeshMaxZ;
        //Mesh generations values
        private List<Vector3> _vertices = new List<Vector3>();
        private List<Vector3> _normals = new List<Vector3>();
        private List<Vector2> _uvs = new List<Vector2>();
        private List<Vector4> _tangents = new List<Vector4>();
        private List<List<int>> _subMeshTriangles = new List<List<int>>();

        public Mesh GeneratedMesh
        {
            get { return mesh; }
        }

        public Spline Spline
        {
            get { return _spline; }
            set { _spline = value; }
        }

        void OnEnable()
        {
            if (_spline == null)
                _spline = GetComponent<Spline>();

            _meshCollider = GetComponent<MeshCollider>();
            GetAuxTranforms();

            ExtrudeMesh();
        }

        /// <summary>
        /// Generate Custom Colliders meshes
        /// </summary>
        private void GenerateCustomColliders()
        {
            if (customMeshColliders != null)
            {
                foreach (var customCollider in customMeshColliders)
                {
                    customCollider.Spline = _spline;
                    customCollider.realtimeMeshGeneration = false;
                    customCollider.enableCollision = true;
                    customCollider.GetComponent<MeshRenderer>().enabled = false;
                    customCollider.ExtrudeMesh();
                }
            }
        }

        /// <summary>
        /// Get aux tranforms used for mesh generation
        /// </summary>
        private void GetAuxTranforms()
        {
            foreach (Transform child in transform)
            {
                switch (child.name)
                {
                    case "Aux1":
                        _auxTransform1 = child;
                        break;
                    case "Aux2":
                        _auxTransform2 = child;
                        break;
                }
            }
        }

        private void Update()
        {
            //EDITOR ONLY MESH GENERATION
            if (realtimeMeshGeneration && !Application.isPlaying)
                ExtrudeMesh();
        }

        /// <summary>
        /// Extrude base mesh along spline
        /// </summary>
        public void ExtrudeMesh()
        {
            if (baseMesh == null)
            {
                Debug.LogWarning("Base Mesh Cannot be null");
                return;
            }

            ResetAuxTranforms();

            if (_auxTransform1 != null)
            {
                //Reset mesh for new mesh creation
                ResetMeshValues();
                //Scan mesh
                BaseMeshScan();
                //Create extruded mesh
                CreateMesh();
                //Update mesh renderer
                UpdateMeshRenderer();
                //Enable/disable mesh collider
                UpdateCollider();
                //Generate Custom colliders
                GenerateCustomColliders();
            }
        }

        /// <summary>
        /// Reset auxiliar tranforms values
        /// </summary>
        private void ResetAuxTranforms()
        {
            if (_auxTransform1 != null)
                GetAuxTranforms();

            if (_auxTransform1 == null) //Avoid error if not found yet
                return;

            _auxTransform1.position = Vector3.zero;
            _auxTransform1.rotation = new Quaternion();
            _auxTransform2.position = Vector3.zero;
            _auxTransform2.rotation = new Quaternion();
        }

        /// <summary>
        /// Update mesh collider
        /// </summary>
        private void UpdateCollider()
        {
            _meshCollider.enabled = enableCollision;
            if (_meshCollider.enabled)
                _meshCollider.sharedMesh = mesh;
        }

        /// <summary>
        /// Revove old mesh values
        /// </summary>
        private void ResetMeshValues()
        {
            _vertices = new List<Vector3>();
            _normals = new List<Vector3>();
            _uvs = new List<Vector2>();
            _tangents = new List<Vector4>();
            _subMeshTriangles = new List<List<int>>();
        }

        /// <summary>
        /// Scan base mesh for lenght
        /// </summary>
        private void BaseMeshScan()
        {
            float min_z = 0.0f;
            float max_z = 0.0f;
            float min_x = 0.0f;
            float max_x = 0.0f;

            // find length
            for (int i = 0; i < baseMesh.vertexCount; i++)
            {
                Vector3 vertice = baseMesh.vertices[i];
                min_z = (vertice.z < min_z) ? vertice.z : min_z;
                max_z = (vertice.z > max_z) ? vertice.z : max_z;
                min_x = (vertice.x < min_x) ? vertice.x : min_x;
                max_x = (vertice.x > max_x) ? vertice.x : max_x;
            }

            _baseMeshMinZ = min_z;
            _baseMeshMaxZ = max_z;
            _baseMeshLength = max_z - min_z;
            _baseMeshLength /= 2.5f;
        }

        /// <summary>
        /// Calculate mesh generation parameters
        /// </summary>
        private void CreateMesh()
        {
            if (_auxTransform1 != null)
                GetAuxTranforms();

            if (_auxTransform1 == null)
                return;

            float totalDistance = _spline.GetTotalDistance();

            for (float dist = 0.0f; dist < totalDistance; dist += _baseMeshLength)
            {
                float initialT = Mathf.Clamp01(dist / totalDistance);
                float finalT = Mathf.Clamp01((dist + _baseMeshLength) / totalDistance);

                OrientedPoint startPoint = _spline.GetOrientedPoint(initialT);
                OrientedPoint endPoint = _spline.GetOrientedPoint(finalT);

                _auxTransform1.rotation = startPoint.Rotation;
                _auxTransform1.position = startPoint.Position + meshOffset;

                _auxTransform2.rotation = endPoint.Rotation;
                _auxTransform2.position = endPoint.Position + meshOffset;

                bool exceededVertsLimit = false;
                bool notUVMapped = false;
                AddSegment(_auxTransform1, _auxTransform2, out exceededVertsLimit, out notUVMapped);

                if (notUVMapped)
                    break;

                if (exceededVertsLimit)
                {
                    realtimeMeshGeneration = false;
                    break;
                }
            }
        }

        /// <summary>
        /// Add new mesh segment along spline
        /// </summary>
        private void AddSegment(Transform start, Transform end, out bool exceededVertsLimit, out bool notUVMapped)
        {
            exceededVertsLimit = false;
            notUVMapped = false;
            int[] indices;

            for (int sub = 0; sub < baseMesh.subMeshCount; sub++)
            {
                indices = baseMesh.GetIndices(sub);
                for (int i = 0; i < indices.Length; i += 3)
                {
                    CreateTriangle(start, end, new int[] { indices[i], indices[i + 1], indices[i + 2] }, sub, out exceededVertsLimit, out notUVMapped);

                    if (exceededVertsLimit || notUVMapped)
                        return;
                }
            }
        }

        /// <summary>
        /// Recover vetorial values (Vector2, Vector3, Vector4)  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="vectorArray"></param>
        /// <param name="indices"></param>
        /// <returns></returns>
        private T[] GetVector<T>(T[] vectorArray, int[] indices)
        {
            T[] ret = new T[3];

            for (int i = 0; i < ret.Length; i++)
                ret[i] = vectorArray[indices[i]];

            return ret;
        }

        /// <summary>
        /// Create new triangle
        /// </summary>
        /// <param name="indices"></param>
        /// <param name="submesh"></param>
        private void CreateTriangle(Transform start, Transform end, int[] indices, int submesh, out bool exceededVertsLimit, out bool notUVMapped)
        {
            exceededVertsLimit = false;
            notUVMapped = false;

            if (baseMesh.uv == null || baseMesh.uv.Length == 0)
            {
                Debug.LogWarning("Base Mesh segment is not UV mapped. Mesh segment must be UV mapped for mesh generation to work.");
                notUVMapped = true;
                return;
            }

            Vector3[] vertices = GetVector<Vector3>(baseMesh.vertices, indices);
            Vector3[] normals = GetVector<Vector3>(baseMesh.normals, indices);
            Vector2[] uvs = GetVector<Vector2>(baseMesh.uv, indices);
            Vector4[] tangents = GetVector<Vector4>(baseMesh.tangents, indices);

            // apply offset
            float lerpValue = 0.0f;
            Vector3 verticesStart, verticesEnd;
            Vector3 normalsStart, normalsEnd;
            Vector4 tangentsStart, tangentsEnd;
            Matrix4x4 localToWorld_A = start.localToWorldMatrix;
            Matrix4x4 localToWorld_B = end.localToWorldMatrix;
            Matrix4x4 worldToLocal = transform.worldToLocalMatrix;

            for (int i = 0; i < 3; i++)
            {
                lerpValue = GetLerpValue(vertices[i].z, _baseMeshMinZ, _baseMeshMaxZ, 0.0f, 1.0f);
                vertices[i].z = 0.0f;

                //Calculate vertices worlds positions and length
                verticesStart = localToWorld_A.MultiplyPoint(vertices[i]);
                verticesEnd = localToWorld_B.MultiplyPoint(vertices[i]);
                vertices[i] = worldToLocal.MultiplyPoint(Vector3.Lerp(verticesStart, verticesEnd, lerpValue));

                //Calculate normals worlds positions and length
                normalsStart = localToWorld_A.MultiplyVector(normals[i]);
                normalsEnd = localToWorld_B.MultiplyVector(normals[i]);
                normals[i] = worldToLocal.MultiplyVector(Vector3.Lerp(normalsStart, normalsEnd, lerpValue));

                //Calculate tangents worlds positions and length
                tangentsStart = localToWorld_A.MultiplyVector(tangents[i]);
                tangentsEnd = localToWorld_B.MultiplyVector(tangents[i]);
                tangents[i] = worldToLocal.MultiplyVector(Vector3.Lerp(tangentsStart, tangentsEnd, lerpValue));
            }

            exceededVertsLimit = (_vertices.Count + vertices.Length > 65000);

            if (exceededVertsLimit)
            {
                string warning = string.Format("Mesh cannot have more than 65000 vertices. {0}If you need to go even further, please reduce the size of your spline using the Remove Curve button, until your mesh has less than 65000 vertices. Then, add a new Mesh Renderer using the Connect New Renderer Button.", System.Environment.NewLine);
                warning += realtimeMeshGeneration ? string.Format("{0}Realtime mesh generation disabled.", System.Environment.NewLine) : string.Empty;
                Debug.LogWarning(warning);
                return;
            }
            else
                AddTriangle(vertices, normals, uvs, tangents, submesh);
        }

        /// <summary>
        /// Add created triangle
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="normals"></param>
        /// <param name="uvs"></param>
        /// <param name="tangents"></param>
        /// <param name="submesh"></param>
        public void AddTriangle(Vector3[] vertices, Vector3[] normals, Vector2[] uvs, Vector4[] tangents, int submesh)
        {
            int vertCount = _vertices.Count;

            int duplicateIndex = 0;
            int[] poligonIndexes = new int[3];
            int duplicatesFound = 0;
            for (int i = 0; i < 3; i++)
            {
                //if (!IsDuplicate(_vertices, vertices[i]))
                if (!IsDuplicate(vertices[i], normals[i], uvs[i], out duplicateIndex))
                {
                    _vertices.Add(vertices[i]);
                    _normals.Add(normals[i]);
                    _uvs.Add(uvs[i]);
                    _tangents.Add(tangents[i]);

                    poligonIndexes[i] = (vertCount + i - duplicatesFound);//New vertex
                }
                else
                {
                    duplicatesFound++;
                    poligonIndexes[i] = duplicateIndex;//Duplicate, use original poligon index for the triangle
                }
            }

            //Add new submesh
            if (_subMeshTriangles.Count < submesh + 1)
            {
                for (int i = _subMeshTriangles.Count; i < submesh + 1; i++)
                {
                    _subMeshTriangles.Add(new List<int>());
                }
            }

            //Add vertices indexes to submesh triangles
            for (int i = 0; i < 3; i++)
            {
                _subMeshTriangles[submesh].Add(poligonIndexes[i]);
            }
        }

        /// <summary>
        /// Check for duplicated vertices
        /// </summary>
        /// <param name="vertice"></param>
        /// <param name="normal"></param>
        /// <param name="uv"></param>
        /// <param name="originalIndex"></param>
        /// <returns></returns>
        private bool IsDuplicate(Vector3 vertice, Vector3 normal, Vector2 uv, out int originalIndex)
        {
            bool duplicated = false;
            originalIndex = 0;

            int lastSegmentStartIndex = (_vertices.Count - baseMesh.vertexCount);

            //First segment validation
            if (lastSegmentStartIndex < 0)
                lastSegmentStartIndex = 0;

            for (int i = lastSegmentStartIndex; i < _vertices.Count; i++)
            {
                duplicated = (_vertices[i] == vertice && _normals[i] == normal && _uvs[i] == uv);
                if (duplicated)
                {
                    originalIndex = i;
                    break;
                }
            }

            return duplicated;
        }

        /// <summary>
        /// Update Mesh values
        /// </summary>
        public void UpdateMeshRenderer()
        {
            mesh.Clear();
            mesh.SetVertices(_vertices);

            mesh.SetNormals(_normals);
            mesh.SetUVs(0, _uvs);
            mesh.SetUVs(1, _uvs);
            if (_tangents.Count > 1) mesh.SetTangents(_tangents);
            mesh.subMeshCount = _subMeshTriangles.Count;

            for (int i = 0; i < _subMeshTriangles.Count; i++)
                mesh.SetTriangles(_subMeshTriangles[i], i);

            //If not editing realtime, show mesh generation log
            if (!realtimeMeshGeneration)
                PrintMeshDetails();
        }

        /// <summary>
        /// Print mesh details on console
        /// </summary>
        public void PrintMeshDetails()
        {
            if (_vertices.Count > 0)

                Debug.Log(string.Format("Mesh Generated{0}Vertices: {1} Normals: {2} Uvs: {3} Tangents: {4} subMeshCount: {5} (Base Mesh Vertices: {6} Segments Count: {7})"
                                        , System.Environment.NewLine, _vertices.Count, _normals.Count, _uvs.Count, _tangents.Count, mesh.subMeshCount, baseMesh.vertexCount, (_vertices.Count / baseMesh.vertexCount)
                                        ));
            else
                Debug.Log(string.Format("Could not generated mesh. Check warning messages for more details."));
        }

        /// <summary>
        /// Create new renderer at the end of the current one
        /// </summary>
        public GameObject ConnectNewRenderer()
        {
            Vector3 lastPointPosition = _spline.GetControlPointPosition(_spline.ControlPointCount - 1);
            Quaternion lastPointRotation = _spline.GetRotation(1);

            Vector3 position = transform.TransformPoint(lastPointPosition);
            GameObject clone = Instantiate(this.gameObject, position, _spline.GetRotation(1) * Quaternion.LookRotation(_spline.GetDirection(1)));

            Spline newRendererSpline = clone.GetComponent<Spline>();
            newRendererSpline.Reset();
            newRendererSpline.ResetRotations(lastPointRotation);

            SplineMeshRenderer newRendererSplineMeshRenderer = clone.GetComponent<SplineMeshRenderer>();
            newRendererSplineMeshRenderer.realtimeMeshGeneration = realtimeMeshGeneration;
            newRendererSplineMeshRenderer.ExtrudeMesh();

            return clone;
        }

        private float GetLerpValue(float value, float oldMin, float oldMax, float newMin, float newMax)
        {
            return ((value - oldMin) / (oldMax - oldMin)) * (newMax - newMin) + newMin;
        }
    }
}
