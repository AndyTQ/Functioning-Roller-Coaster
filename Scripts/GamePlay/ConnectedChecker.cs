using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSMGameStudio.Splines;
using UnityEngine.UI;
public class ConnectedChecker : MonoBehaviour
{   
    public GameObject buttonGo;
    private float checkSphereRadius = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GraphTest();
    }

    void GraphTest()
    {
        Spline startSpline = null;
        Spline endSpline = null;
        Graph g = new Graph();
        foreach (Transform currSplineTransform in GameObject.Find("Splines").transform)
        {
            GameObject currSpline = currSplineTransform.gameObject;
            Spline spline = currSpline.GetComponent<Spline>();
            Vector2 splineStartPt = spline.GetControlPointPosition(0) + currSpline.transform.position;
            Vector2 splineEndPt = spline.GetControlPointPosition(3) + currSpline.transform.position;
            g.addEdge(g.roundedVector(splineStartPt), g.roundedVector(splineEndPt));
            if (string.Compare(currSpline.name,"StartingSpline") == 0)
            {
                startSpline = spline;
            }
            else if (string.Compare(currSpline.name,"EndingSpline") == 0)
            {
                endSpline = spline;
            }
        }

    

        // For chapter 4: Also add portals as edges.
        if (GameObject.Find("CheckWinController").GetComponent<CheckWin>().level.Contains("D"))
        {
            foreach (Transform portalTransform in GameObject.Find("Portals").transform)
            {
                GameObject currentPortal = portalTransform.gameObject;
                if (currentPortal.name.Contains("Left"))
                {
                    string discontName = currentPortal.name.Substring(0, currentPortal.name.IndexOf("Left"));
                    Debug.Log(currentPortal.name.Substring(0, currentPortal.name.IndexOf("Left")));
                    GameObject discontRight = GameObject.Find(discontName + "Right");
                    g.addEdge(g.roundedVector(currentPortal.transform.position), g.roundedVector(discontRight.transform.position));
                }
            }
        }

        Vector2 u = startSpline.GetControlPointPosition(3) + startSpline.gameObject.transform.position;
        Vector2 v = endSpline.GetControlPointPosition(0) + endSpline.gameObject.transform.position;


        if (g.isReachable(g.roundedVector(u), g.roundedVector(v)))
            buttonGo.GetComponent<Toggle>().interactable = true; // The track is ready to go.
        else
            buttonGo.GetComponent<Toggle>().interactable = false; // The track is not ready yet.
    }
}



class Graph 
{ 
    private int V;   // No. of vertices 
    private Dictionary<Vector2, LinkedList<Vector2>> adj; //Adjacency List 
  
    //Constructor 
    public Graph() 
    { 
        adj = new Dictionary<Vector2, LinkedList<Vector2>>(); 
    }


    public void GetVertices()
    {
        foreach (GameObject currSpline in GameObject.Find("Splines").transform)
        {
            Spline spline = currSpline.GetComponent<Spline>();  
            Vector2 splineStartPt = spline.GetControlPointPosition(0);
            Vector2 splineEndPt = spline.GetControlPointPosition(3);
        }
    }

    public Vector2 roundedVector(Vector2 v)
    {
        Vector2 roundedV = new Vector2(Mathf.Round(v.x * 10f) / 10f, Mathf.Round(v.y * 10f) / 10f);
        return roundedV;
    }

    //Function to add an edge into the graph 
    public void addEdge(Vector2 v, Vector2 w)  
    { 
        bool contains = adj.ContainsKey(v);
        if (contains)
        {
            //Mathf.Round(v * 100f) / 100f
            adj[v].AddLast(w);  
        }
        else 
        {
            adj.Add(v, new LinkedList<Vector2>());
            adj[v].AddLast(w);
        }
    }
    
    public bool isReachable(Vector2 s, Vector2 d)
    {
        Dictionary<Vector2, bool> visited = new Dictionary<Vector2, bool>();
        LinkedList<Vector2> queue = new LinkedList<Vector2>();


        visited[s] = true;
        queue.AddLast(s);

        LinkedList<Vector2> i;
        
        while (queue.Count != 0)
        {
            s = queue.First.Value;
            queue.RemoveFirst();

            Vector2 n;
            bool hasAdjacency = adj.ContainsKey(s);
            if (hasAdjacency) 
            {
                i = adj[s];
                LinkedListNode<Vector2> currNode = i.First;
                while (currNode != null)
                {
                    n = currNode.Value;

                    if (n == d)
                    {
                        return true;
                    }

                    if (!visited.ContainsKey(n))
                    {
                        visited[n] = true;
                        queue.AddLast(n);
                    }
                    currNode = currNode.Next;
                }
            }
        }
        return false;
    }
}




 

  