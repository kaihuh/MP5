using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyMesh : MonoBehaviour
{
    public void ShowVertices(bool show)
    {
        
        for (int i = transform.childCount - 1; i >= 0; i--)
        { //m_Collider.GetType() == typeof(CapsuleCollider)
            if (transform.GetChild(i).gameObject.name == "Sphere" || transform.GetChild(i).name == "Cylinder")
            {
                if(show == true)
                    transform.GetChild(i).GetComponent<Renderer>().enabled = true;
                if(show == false)
                    transform.GetChild(i).GetComponent<Renderer>().enabled = false;
            }
        }
    }
    public void ShowXfrom(bool show)
    {
        if (show == true)
        {
            LocalTester.GetComponent<Renderer>().enabled = true;
            for (int i = LocalTester.transform.childCount - 1; i >= 0; i--)
            { //m_Collider.GetType() == typeof(CapsuleCollider)
                Transform child = LocalTester.transform.GetChild(i);
                if ( child.name == "X" || child.name == "Y" || child.name == "Z")
                {
                    child.GetComponent<Renderer>().enabled = true;
                }
            }
        }

        else
        {
            LocalTester.GetComponent<Renderer>().enabled = false;
            for (int i = LocalTester.transform.childCount - 1; i >= 0; i--)
            { //m_Collider.GetType() == typeof(CapsuleCollider)
                Transform child = LocalTester.transform.GetChild(i);
                if (child.name == "X" || child.name == "Y" || child.name == "Z")
                {
                    child.GetComponent<Renderer>().enabled = false;
                }
            }
        }
            
    }
    //public void MoveVertex(Transform sphere, float deltax)
    //{

    //}
}