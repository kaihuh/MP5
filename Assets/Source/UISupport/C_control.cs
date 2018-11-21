using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // for GUI elements: Button, Toggle
using UnityEngine.EventSystems;

public class C_control : MonoBehaviour
{

    // reference to all UI elements in the Canvas
    public Camera MainCamera = null;
    public Cmesh myMesh = null;
    public GameObject Xfrom = null;

    GameObject selectedObj;
    GameObject selectedVertex;
    GameObject selectedAxis;
    GameObject currentVetex;
    Vector3 mousePosition;

    // Use this for initialization
    void Start()
    {
        Debug.Assert(MainCamera != null);
        Debug.Assert(myMesh != null);
        Debug.Assert(Xfrom != null);
    }

    // Update is called once per frame
    void Update()
    {
        ProcessMouseEvents();
    }

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

                if (MouseSelectObjectAt(out selectedObj, out hitPoint, LayerMask.GetMask("Default")))
                {
                    if (selectedObj.name == "X" || selectedObj.name == "Y" || selectedObj.name == "Z")
                    {
                        selectedAxis = selectedObj;
                    }
                    else
                    {
                        selectedVertex = selectedObj;
                        Xfrom.transform.localPosition = selectedObj.transform.localPosition;
                        myMesh.ShowXfrom(true);
                    }
                    mousePosition = Input.mousePosition;
                }
                else
                {
                    selectedObj = null;
                    selectedVertex = null;
                    selectedAxis = null;
                    myMesh.ShowXfrom(false);
                }
            }
            if (Input.GetMouseButton(0)) // Mouse Drag
            {
                if (selectedAxis != null && selectedVertex != null)
                {
                    Vector3 newMousePosition = Input.mousePosition;

                    float deltaX = (newMousePosition - mousePosition).x;  // newX - origX;
                    float deltaY = (newMousePosition - mousePosition).y; // newY - origY;

                    Vector3[] dirs = myMesh.dirs;
                    int index = System.Int32.Parse(selectedVertex.transform.name);

                    if (selectedAxis.name == "X")
                    {
                        selectedAxis.GetComponent<Renderer>().material.color = Color.yellow;                  
                        myMesh.mControllers[index].transform.localPosition += -selectedAxis.transform.up * deltaX * Time.deltaTime * 1f;
                        Xfrom.transform.localPosition = myMesh.mControllers[index].transform.localPosition;
                        index = index + myMesh.getcol();
                        for (int i = 1; i < myMesh.getrow(); i++)
                        {
                            if (deltaX >= 0)
                            {
                                myMesh.mControllers[index].transform.localPosition += dirs[index] * deltaX * Time.deltaTime * 1f;
                            } else {
                                myMesh.mControllers[index].transform.localPosition += dirs[index] * deltaX * Time.deltaTime * 1f;
                            }                  

                            if (i == 0)
                            {
                                Xfrom.transform.localPosition = myMesh.mControllers[index].transform.localPosition;
                            }
                            index = index + myMesh.getcol();
                        }
                    }
                    if (selectedAxis.name == "Y")
                    {
                        selectedAxis.GetComponent<Renderer>().material.color = Color.yellow;
                        for (int i = 0; i < myMesh.getrow(); i++)
                        {
                            myMesh.mControllers[index].transform.localPosition += selectedAxis.transform.up * deltaY * Time.deltaTime * 1f;
                            if (i == 0)
                            {
                                Xfrom.transform.localPosition = myMesh.mControllers[index].transform.localPosition;
                            }
                            index = index + myMesh.getcol();
                        }
                    }
                    if (selectedAxis.name == "Z")
                    {
                        selectedAxis.GetComponent<Renderer>().material.color = Color.yellow;
                        myMesh.mControllers[index].transform.localPosition += selectedAxis.transform.up * deltaX * Time.deltaTime * 1f;
                        Xfrom.transform.localPosition = myMesh.mControllers[index].transform.localPosition;
                        index = index + myMesh.getcol();
                        for (int i = 1; i < myMesh.getrow(); i++)
                        {
                            if (deltaX >= 0)
                            {
                                myMesh.mControllers[index].transform.localPosition += dirs[index] * deltaX * Time.deltaTime * 0.6f;
                            }
                            else
                            {
                                myMesh.mControllers[index].transform.localPosition += dirs[index] * deltaX * Time.deltaTime * 0.6f;
                            }
                            index = index + myMesh.getcol();
                        }
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
            if (selectedObj == null)
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
        if (selectedVertex == null)
        {
            return;
        }

        int index = System.Int32.Parse(selectedVertex.transform.name);
        Mesh theMesh = myMesh.GetComponent<MeshFilter>().mesh;
        Vector3[] n = theMesh.normals;
        for (int i = 0; i < myMesh.getrow(); i++)
        {
            myMesh.mControllers[index].transform.localPosition += n[index] * deltax * Time.deltaTime * 0.3f;
            index = index + myMesh.getcol();
        }

        Xfrom.transform.localPosition = selectedAxis.transform.up * deltax * Time.deltaTime * 0.3f;
    }
}
