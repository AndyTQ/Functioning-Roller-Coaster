using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class DragHandler : MonoBehaviour, IDropHandler{
	private GameObject editorCursor;
	GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
	private GameObject canvas;
	private bool isDragging = false;
	private GameObject selectedPoint;
	private GameObject hoveredPoint;
	void Update()
	{
		//Set up the new Pointer Event
		m_PointerEventData = new PointerEventData(m_EventSystem);
		//Set the Pointer Event Position to that of the mouse position
		m_PointerEventData.position = Input.mousePosition;
		//Create a list of Raycast Results
		List<RaycastResult> results = new List<RaycastResult>();
		//Raycast using the Graphics Raycaster and mouse click position
		m_Raycaster.Raycast(m_PointerEventData, results);

		bool hoveredOverAnyPoint = false;
		
		if (results.Count > 0)
		{
			foreach (RaycastResult result in results)
			{
				if (result.gameObject.transform.parent.gameObject.name == "ControlPoints")
				{
					hoveredOverAnyPoint = true;
					hoveredPoint = result.gameObject;
				}
			}
		}

		if (hoveredOverAnyPoint)
		{
			// Find the corresponding spline, set toggling allowance to false.
			GameObject.Find("EditorCursor").GetComponent<SplineEditorCursor>().allowToggling = false;
			
			if (Input.GetMouseButtonDown(0)) // start draging!
			{
				CameraDrag.allowDrag = false;
				isDragging = true;
				selectedPoint = hoveredPoint;
			}
		}
		else {
			// Find the corresponding spline, set toggling allowance to true.
			if (!Input.GetMouseButton(0))
			{
				GameObject.Find("EditorCursor").GetComponent<SplineEditorCursor>().allowToggling = true;
			}
		}


		if (isDragging)
		{
			selectedPoint.transform.position = Input.mousePosition;
			if(!Input.GetMouseButton(0))
			{
				isDragging = false;
				CameraDrag.allowDrag = true;
			}
		}
	}
	void Start()
	{
		canvas = GameObject.Find("Canvas");
		editorCursor = GameObject.Find("Cursors").transform.GetChild(1).gameObject;
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = canvas.GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
	}

	// public void OnBeginDrag(PointerEventData eventData)
	// {
	// 	Vector3 newPos = new Vector3(Input.mousePosition[0], Input.mousePosition[1], 0);
	// 	transform.position += new Vector3(eventData.delta.x, eventData.delta.y, 0);
	// }

	// public void OnDrag (PointerEventData eventData)
	// {	
	// 	Vector3 newPos = new Vector3(Input.mousePosition[0], Input.mousePosition[1], 0);
	// 	transform.position += new Vector3(eventData.delta.x, eventData.delta.y, 0);
	// }


	// public void OnEndDrag(PointerEventData eventData)
	// {
	// 	CameraDrag.allowDrag = true;
	// }

	public void OnDrop(PointerEventData eventData)
	{

	}
}