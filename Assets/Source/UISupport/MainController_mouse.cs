using System; // for assert
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // for GUI elements: Button, Toggle
using UnityEngine.EventSystems;

public partial class MainController : MonoBehaviour
{
    GameObject selectedObj;
    GameObject selectedVertex;
    GameObject selectedAxis;

   Vector3 mousePosition;
    void ProcessMouseEvents()
    {

        Vector3 hitPoint;
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            if (selectedVertex != null)
            {
                selectedVertex.GetComponent<Renderer>().material.color = Color.white;
                //selectedVertex = null;
            }
            //show vertices
            myMesh.ShowVertices(true);
            if (Input.GetMouseButtonDown(0)) // Click event
            {
                //Debug.Log("left button down");
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    if (MouseSelectObjectAt(out selectedObj, out hitPoint, LayerMask.GetMask("Default")))
                    {
                        if (selectedObj.name == "Sphere")
                        {
                            selectedVertex = selectedObj;
                            Xfrom.transform.localPosition = selectedObj.transform.localPosition;
                            myMesh.ShowXfrom(true);
                        }
                        if (selectedObj.name == "X" || selectedObj.name == "Y" || selectedObj.name == "Z")
                        {
                            selectedAxis = selectedObj;
                        }
                        mousePosition = Input.mousePosition;
                    }
                }
            }
            if (Input.GetMouseButton(0)) // Mouse Drag
            {
                //Debug.Log("left button held");
                //Debug.Log(selectedObj.name);
                //if (!EventSystem.current.IsPointerOverGameObject())

                //if (MouseSelectObjectAt(out selectedObj, out hitPoint, 1 << 0)) // Notice the two ways of getting the mask
                if (selectedAxis != null)
                {
                    Vector3 newMousePosition = Input.mousePosition;
                        
                    float deltaX = (newMousePosition - mousePosition).x;  // newX - origX;
                    float deltaY = (newMousePosition - mousePosition).y; // newY - origY;
                    //Debug.Log("holding = " + selectedObj.name);
                    if (selectedAxis.name == "X")
                    {
                        selectedAxis.GetComponent<Renderer>().material.color = Color.yellow;
                        ComputeMove(-deltaX);
                    }
                    if (selectedAxis.name == "Y")
                    {
                        selectedAxis.GetComponent<Renderer>().material.color = Color.yellow;
                        ComputeMove(deltaY);
                    }
                    if (selectedAxis.name == "Z")
                    {
                        //selectedObj = selectedObj;
                        selectedAxis.GetComponent<Renderer>().material.color = Color.yellow;
                        ComputeMove(deltaX);
                    }
                    mousePosition = newMousePosition;
                }  
                    
            }
            if (Input.GetMouseButtonUp(0)) // Mouse up
            {
                //Debug.Log("left button release");
                //Debug.Log("release = " + selectedAxis.name);
                if (selectedAxis != null)
                {
                    if (selectedAxis.name == "X")
                    {
                        selectedAxis.GetComponent<Renderer>().material.color = Color.red;
                    }
                    else if (selectedAxis.name == "Y")
                    {
                        selectedAxis.GetComponent<Renderer>().material.color = Color.green;                        
                    }
                    else if (selectedAxis.name == "Z")
                    {
                        selectedAxis.GetComponent<Renderer>().material.color = Color.blue;
                    }
                    //selectedVertex = null;
                    selectedAxis = null;
                }
            }

        }
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            if (selectedObj == null || (!(selectedObj.name == "Sphere" || selectedObj.name == "X" || selectedObj.name == "Y" || selectedObj.name == "Z")))
            {
                //Debug.Log("select obj = " + selectedObj.name);
                myMesh.ShowVertices(false);
                myMesh.ShowXfrom(false);

            }

        }
    }

    bool MouseSelectObjectAt(out GameObject g, out Vector3 p, int layerMask)
    {
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(MainCamera.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, layerMask);
        // Debug.Log("MouseSelect:" + layerMask + " Hit=" + hit);
        if (hit)
        {
            g = hitInfo.transform.gameObject;
            p = hitInfo.point;
        }
        else
        {
            g = null;
            p = Vector3.zero;
        }
        return hit;
    }
    void ComputeMove(float deltax)
    {
        //Debug.Log("vertex = " + selectedVertex);
        selectedVertex.transform.localPosition += selectedAxis.transform.up * deltax * Time.deltaTime *0.3f;
        Xfrom.transform.localPosition = selectedVertex.transform.localPosition;
    }
}
