using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSMGameStudio.Splines
{
    [ExecuteInEditMode]
    public class SplinePrefabSpawner : MonoBehaviour
    {
        public Spline spline;
        public int frequency = 1;
        public GameObject[] prefabs;

        private List<GameObject> clones;

        void OnEnable()
        {
            if (spline == null)
                spline = GetComponent<Spline>();
        }

        /// <summary>
        /// Spawn prefabs along spline
        /// </summary>
        public void SpawnPrefabs()
        {
            ResetObjects();

            if (frequency <= 0 || prefabs == null || prefabs.Length == 0)
            {
                return;
            }

            float stepSize = frequency * prefabs.Length;
            if (spline.Loop || stepSize == 1)
                stepSize = 1f / stepSize;
            else
                stepSize = 1f / (stepSize - 1);

            for (int p = 0, f = 0; f < frequency; f++)
            {
                for (int i = 0; i < prefabs.Length; i++, p++)
                {
                    GameObject newClone = Instantiate(prefabs[i]); ;
                    Vector3 pointPosition = spline.GetPoint(p * stepSize);
                    Quaternion pointRotation = spline.GetRotation(p * stepSize);

                    newClone.transform.localPosition = pointPosition;

                    newClone.transform.LookAt(pointPosition + spline.GetDirection(p * stepSize));
                    newClone.transform.rotation *= pointRotation;

                    newClone.transform.parent = transform;

                    clones.Add(newClone);
                }
            }
        }

        /// <summary>
        /// Reset all objects
        /// </summary>
        private void ResetObjects()
        {
            if (clones != null && clones.Count > 0)
            {
                foreach (var clone in clones)
                {
                    DestroyImmediate(clone);
                }
            }

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
                DestroyImmediate(child.gameObject);
            }

            clones = new List<GameObject>();
        }
    }
}
