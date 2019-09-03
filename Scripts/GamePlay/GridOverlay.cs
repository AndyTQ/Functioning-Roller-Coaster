using UnityEngine;
using System.Collections;
using TMPro;
public class GridOverlay : MonoBehaviour
{

    //public GameObject plane;
    private Camera cam;
    public GameObject gridNumPrefab;

    public bool showMain = true;
    public bool showSub = false;
    private int numIndex;

    public int gridSizeX;
    public int gridSizeY;
    public int gridSizeZ;

    private int gridSizeXPre;

    public float smallStep;
    public float smallStepX = (Mathf.PI / 2);
    public float largeStep;
    public float largeStepX = Mathf.PI;

    public float startX;
    public float startY;
    public float startZ;

    public bool firstQuad;
    public bool secondQuad;
    public bool thirdQuad;
    public bool fourthQuad;

    // public float startXScreenSpace;
    // public float startYScreenSpace;
    // public float startZScreenSpace;


    private Material lineMaterial;
    private GameObject GridNums;

    public Color mainColor = new Color(0f, 1f, 0f, 1f);
    public Color subColor = new Color(0f, 0.5f, 0f, 1f);
    public Color axisColor = new Color(0f, 0f, 0f, 1f);
    public int mode = 0; // normal or trig;

    void Start()
    { 
        startZ = -10;
        numIndex = 0;
        cam = Camera.main;
        GridNums = GameObject.Find("GridNums");
        InvokeRepeating("drawNumbers", 2.5f, 0.02f);
    }


    void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            var shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    void OnPostRender()
    {   
        try{
        // Convert screenspace into worldspace.
        // Vector3 screenspace = new Vector3(startXScreenSpace,startYScreenSpace,startZScreenSpace);
        // Vector3 worldspace = cam.ScreenToWorldPoint(screenspace);
        // startX = worldspace.x;
        // startY = worldspace.y;
        // startZ = worldspace.z;

        


        CreateLineMaterial();
        // set the current material
        lineMaterial.SetPass(0);

        GL.Begin(GL.LINES);

        drawLines(startX, startY, startZ);
        for(float i = -0.01f; i < 0.01f; i += 0.001f)
        {
        drawLines(startX + i, startY + i, startZ);
        drawLines(startX - i, startY - i, startZ);
        }


        
        
        GL.End();
    }
    catch{}
    }

    public void toggleState(){
        GridOverlay gridOverlay = gameObject.GetComponent<GridOverlay>();
        gridOverlay.enabled = !gridOverlay.enabled; //this changes the state from on to off and vice-versa
    }
    
    private void drawLines(float startX, float startY, float startZ){

            if (showSub)
            {
                //Layers
                for (float j = 0; j <= gridSizeY; j += smallStep)
                {
                    if (j != 0)
                    {
                        GL.Color(subColor);
                    } else {GL.Color(axisColor);}
                        // horizontal line in q1
                        GL.Vertex3(startX, startY + j, startZ);
                        GL.Vertex3(startX + gridSizeX, startY + j, startZ);
                        // horizontal line in q2
                        GL.Vertex3(startX, startY + j, startZ);
                        GL.Vertex3(startX - gridSizeX, startY + j, startZ);
                }

                for (float j = 0; j >= -gridSizeY; j -= smallStep)
                {
                    if (j != 0)
                    {
                        GL.Color(subColor);
                    } else {GL.Color(axisColor);}
                        // horizontal line in q3
                        GL.Vertex3(startX, startY + j, startZ);
                        GL.Vertex3(startX + gridSizeX, startY + j, startZ);
                        // horizontal line in q4
                        GL.Vertex3(startX, startY + j, startZ);
                        GL.Vertex3(startX - gridSizeX, startY + j, startZ);
                }

                for (float k = 0; k <= gridSizeX; k += mode == 0 ? smallStep : smallStepX)
                {
                    if (k != 0)
                    {
                        GL.Color(subColor);
                    } else {GL.Color(axisColor);}
                    // vertical line in q1
                    GL.Vertex3(startX + k, startY, startZ);
                    GL.Vertex3(startX + k, startY + gridSizeY, startZ);
                    // vertical line in q2
                    GL.Vertex3(startX - k, startY, startZ);
                    GL.Vertex3(startX - k, startY + gridSizeY, startZ);
                }

                for (float k = 0; k >= -gridSizeX; k -= mode == 0 ? smallStep : smallStepX)
                {
                    if (k != 0)
                    {
                        GL.Color(subColor);
                    } else {GL.Color(axisColor);}
                    // vertical line in q3
                    GL.Vertex3(startX + k, startY, startZ);
                    GL.Vertex3(startX + k, startY - gridSizeY, startZ);
                    // vertical line in q4
                    GL.Vertex3(startX - k, startY, startZ);
                    GL.Vertex3(startX - k, startY - gridSizeY, startZ);
                }


            }

            if (showMain)
            {

                //Layers
                    //Layers
                for (float j = 0; j <= gridSizeY; j += largeStep)
                {
                    if (j != 0)
                    {
                        GL.Color(mainColor);
                    } else {GL.Color(axisColor);}
                        // horizontal line in q1
                        GL.Vertex3(startX, startY + j, startZ);
                        GL.Vertex3(startX + gridSizeX, startY + j, startZ);
                        // horizontal line in q2
                        GL.Vertex3(startX, startY + j, startZ);
                        GL.Vertex3(startX - gridSizeX, startY + j, startZ);
                }

                for (float j = 0; j >= -gridSizeY; j -= largeStep)
                {
                     if (j != 0)
                    {
                        GL.Color(mainColor);
                    } else {GL.Color(axisColor);}
                        // horizontal line in q3
                        GL.Vertex3(startX, startY + j, startZ);
                        GL.Vertex3(startX + gridSizeX, startY + j, startZ);
                        // horizontal line in q4
                        GL.Vertex3(startX, startY + j, startZ);
                        GL.Vertex3(startX - gridSizeX, startY + j, startZ);
                }

                for (float k = 0; k <= gridSizeX; k += mode == 0 ? largeStep : largeStepX)
                {
                     if (k != 0)
                    {
                        GL.Color(mainColor);
                    } else {GL.Color(axisColor);}
                    // vertical line in q1
                    GL.Vertex3(startX + k, startY, startZ);
                    GL.Vertex3(startX + k, startY + gridSizeY, startZ);
                    // vertical line in q2
                    GL.Vertex3(startX - k, startY, startZ);
                    GL.Vertex3(startX - k, startY + gridSizeY, startZ);
                }

                for (float k = 0; k >= -gridSizeX; k -= mode == 0 ? largeStep : largeStepX)
                {
                     if (k != 0)
                    {
                        GL.Color(mainColor);
                    } else {GL.Color(axisColor);}
                    // vertical line in q3
                    GL.Vertex3(startX + k, startY, startZ);
                    GL.Vertex3(startX + k, startY - gridSizeY, startZ);
                    // vertical line in q4
                    GL.Vertex3(startX - k, startY, startZ);
                    GL.Vertex3(startX - k, startY - gridSizeY, startZ);
                }
            }
        
        
}

    private void drawNumbers()
    {
        try{
        if (GameObject.Find("GridNum" + numIndex) == null && numIndex <= gridSizeX)
        {
            if (numIndex != 0)
            {
                GameObject numPositiveX;
                GameObject numNegativeX;
                GameObject numPositiveY;
                GameObject numNegativeY;
        

                numPositiveX = Instantiate(gridNumPrefab, new Vector3(-0.49f - 0.03f + (mode == 0 ? numIndex : numIndex * Mathf.PI / 2), -0.2f - 0.03f, startZ), Quaternion.identity);
                numPositiveX.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = numIndex.ToString();

                numNegativeX = Instantiate(gridNumPrefab, new Vector3(-0.49f - 0.03f - (mode == 0 ? numIndex : numIndex * Mathf.PI / 2), -0.2f - 0.03f, startZ), Quaternion.identity);
                numNegativeX.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = "-" + numIndex.ToString();

                if (mode == 1)
                {
                    GameObject numPositiveXDenominator = Instantiate(gridNumPrefab, new Vector3(-0.52f + (mode == 0 ? numIndex : numIndex * Mathf.PI / 2), -0.75f, startZ), Quaternion.identity);
                    GameObject numNegativeXDenominator = Instantiate(gridNumPrefab, new Vector3(-0.52f - (mode == 0 ? numIndex : numIndex * Mathf.PI / 2), -0.75f, startZ), Quaternion.identity);
                    numPositiveXDenominator.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = 2.ToString();
                    numNegativeXDenominator.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = 2.ToString();
                    GameObject negativeDivider = Instantiate(gridNumPrefab, new Vector3(-0.14f + -(mode == 0 ? numIndex : numIndex * Mathf.PI / 2), -0.45f, startZ), Quaternion.identity);
                    negativeDivider.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = "—π";
                    GameObject positiveDivider = Instantiate(gridNumPrefab, new Vector3(-0.14f + (mode == 0 ? numIndex : numIndex * Mathf.PI / 2), -0.45f, startZ), Quaternion.identity);
                    positiveDivider.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = "—π";
                    numPositiveXDenominator.transform.SetParent(GridNums.transform);
                    numNegativeXDenominator.transform.SetParent(GridNums.transform);
                    negativeDivider.transform.SetParent(GridNums.transform);
                    positiveDivider.transform.SetParent(GridNums.transform);
                }

                numPositiveX.name = "GridNum" + numIndex;
                numPositiveX.transform.SetParent(GridNums.transform);
                numNegativeX.name = "GridNum" + "-" + numIndex;
                numNegativeX.transform.SetParent(GridNums.transform);

                numPositiveY = Instantiate(gridNumPrefab, new Vector3(-0.5f - 0.03f, -0.25f  + numIndex - 0.03f, startZ), Quaternion.identity);
                numPositiveY.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = numIndex.ToString();
                numNegativeY = Instantiate(gridNumPrefab, new Vector3(-0.5f - 0.03f, -0.25f  - numIndex - 0.03f, startZ), Quaternion.identity);
                numNegativeY.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = "-" + numIndex.ToString();

                numPositiveY.name = "GridNum" + numIndex;
                numNegativeY.name = "GridNum" + "-" + numIndex;
                numPositiveY.transform.SetParent(GridNums.transform);
                numNegativeY.transform.SetParent(GridNums.transform);

            }
            else
            {
                GameObject numZero = Instantiate(gridNumPrefab, new Vector3(-0.54f, -0.22f, startZ), Quaternion.identity);
                numZero.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = 0.ToString();
                numZero.transform.SetParent(GridNums.transform);
            }
            numIndex ++;
        }
        }
        catch(System.NullReferenceException){
            Debug.Log("oo!");
        }

    }

}
