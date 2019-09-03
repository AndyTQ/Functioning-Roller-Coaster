using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private float sizeX = 1f;
    [SerializeField]
    private float sizeY = 1f;


    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        position -= transform.position;

        int xCount = Mathf.RoundToInt(position.x / sizeX);
        int yCount = Mathf.RoundToInt(position.y / sizeY);
        int zCount = Mathf.RoundToInt(position.z / 1);

        Vector3 result = new Vector3(
            (float)xCount * sizeX,
            (float)yCount * sizeY,
            (float)zCount * 1f);

        result += transform.position;

        return result;
    }

    private void OnValidate() {
        sizeX = Mathf.Max(sizeX, 0.5f);
        sizeY = Mathf.Max(sizeY, 0.5f);
    }

}