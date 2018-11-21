using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cmesh : MonoBehaviour {

    public GameObject LocalTester;
    public GameObject vertex;

    private int r = 5;
    private int h = 20;

    private int row = 10;
    private int col = 10;
    private int angle = 275;

    public GameObject[] mControllers;
    public Vector3[] dirs; 
    private LineSegment[] mNormals;
    private void Awake()
    {
        InitalMesh();
    }
    // Use this for initialization
    void Start () {
        ShowVertices(false);
        ShowXfrom(false);  
    }
	
	// Update is called once per frame
	void Update () {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        Vector3[] v = theMesh.vertices;
        Vector3[] n = theMesh.normals;
        for (int i = 0; i < mControllers.Length; i++)
        {
            v[i] = mControllers[i].transform.localPosition;
        }

        theMesh.vertices = v;
        theMesh.RecalculateNormals();
        Nedge();
        UpdateNormals(v, theMesh.normals);
    }

    public void InitalMesh()
    {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;   // get the mesh component
        theMesh.Clear();    // delete whatever is there!!

        Vector3[] v = new Vector3[row * col];  // vertices
        int[] t = new int[(row - 1) * 2 * (col - 1) * 3];    // triangles
        dirs = new Vector3[row * col];

        Vector3 center;
        float deltaY = (float)h / (col - 1);
        int index = 0;
        float deltaAngle = (float)angle / (row - 1);

        for (int i = 0; i < row; i++)
        {
            float theAngle = deltaAngle * i;
            float z = Mathf.Sin(theAngle * Mathf.Deg2Rad) * r;
            float x = Mathf.Cos(theAngle * Mathf.Deg2Rad) * r;

            for (int j = 0; j < col; j++)
            {
                v[index] = new Vector3(x, deltaY * j, z);

                center = new Vector3(0, deltaY * j, 0);
                dirs[index] = Vector3.Normalize(v[index] - center);

                index++;
            }
        }

        index = 0;
        for (int i = 0; i <= (row - 2) * col; i = i + col)
        {
            for (int j = i; j < i + col - 1; j++)
            {
                t[index] = j;
                index++;
                t[index] = j + col + 1;
                index++;
                t[index] = j + col;
                index++;

                t[index] = j;
                index++;
                t[index] = j + 1;
                index++;
                t[index] = j + col + 1;
                index++;
            }
        }

        theMesh.vertices = v; //  new Vector3[3];
        theMesh.triangles = t; //  new int[3];
        theMesh.RecalculateNormals();
        Nedge();
        InitControllers(v);
        InitNormals(v, theMesh.normals);
    }

    void InitControllers(Vector3[] v)
    {
        mControllers = new GameObject[v.Length];

        for (int i = 0; i < col; i++)
        {
            mControllers[i] = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            mControllers[i].transform.localScale = new Vector3(0.6f, 0.4f, 0.6f);
            mControllers[i].transform.localPosition = v[i];
            mControllers[i].transform.parent = vertex.transform;
            mControllers[i].transform.name = i.ToString();

        }
        for (int i = col; i < v.Length; i++)
        {
            mControllers[i] = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            mControllers[i].GetComponent<Renderer>().material.color = Color.black;
            mControllers[i].transform.localScale = new Vector3(0.6f,0.4f,0.6f);
            mControllers[i].transform.localPosition = v[i];
            mControllers[i].transform.parent = vertex.transform;
            mControllers[i].layer = 2;
        }
    }

    public void ShowVertices(bool show)
    {
        for (int i = vertex.transform.childCount - 1; i >= 0; i--)
        {
            if (show == true) {
                vertex.transform.GetChild(i).GetComponent<Renderer>().enabled = true;
            } else {
                vertex.transform.GetChild(i).GetComponent<Renderer>().enabled = false;
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
                if (child.name == "X" || child.name == "Y" || child.name == "Z")
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

    void Nedge()
    {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        Vector3[] n = theMesh.normals;

        int left = n.Length - col * 2;
        int right = col;
        for (int i = 0; i < col; i++)
        {
            if (i == 0)
            {
                n[i] = n[left] + n[right];
            } else
            {
                n[i] = n[left] + n[right] + n[i - 1];
            }
            n[i].Normalize();
            left++;
            right++;
        }

        int index = n.Length - col;
        for (int i = 0; i < col; i++)
        {
            n[index] = n[i];
            index++;
        }
        theMesh.normals = n;
    }

    void InitNormals(Vector3[] v, Vector3[] n)
    {
        mNormals = new LineSegment[v.Length];
        for (int i = 0; i < v.Length; i++)
        {
            GameObject o = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            o.layer = 2;
            mNormals[i] = o.AddComponent<LineSegment>();
            mNormals[i].SetWidth(0.1f);
            mNormals[i].transform.parent = vertex.transform;
        }
        UpdateNormals(v, n);
    }

    void UpdateNormals(Vector3[] v, Vector3[] n)
    {
        for (int i = 0; i < v.Length; i++)
        {
            mNormals[i].SetEndPoints(v[i], v[i] + 4f * n[i]);
        }
    }

    public int getcol()
    {
        return col;
    }

    public int getrow()
    {
        return row;
    }

    public int geth()
    {
        return h;
    }

    public void setRow(int input)
    {
        row = input;
        ClearMesh();
        InitalMesh();
        ShowVertices(false);
        ShowXfrom(false);
    }

    public void setCol(int input)
    {
        col = input;
        ClearMesh();
        InitalMesh();
        ShowVertices(false);
        ShowXfrom(false);
    }

    public void setRotation(int input)
    {
        angle = input;
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        float deltaAngle = (float)angle / (row - 1);
        float deltaY = (float)h / (col - 1);

        dirs = new Vector3[row * col];
        Vector3 center;
        int index = 0;
        for (int i = 0; i < row; i++)
        {
            float theAngle = deltaAngle * i;
            for (int j = 0; j < col; j++)
            {               
                center = new Vector3(0, mControllers[index].transform.localPosition.y, 0);
                Vector3 dis = mControllers[index].transform.localPosition - center;
                float z = Mathf.Sin(theAngle * Mathf.Deg2Rad) * dis.magnitude;
                float x = Mathf.Cos(theAngle * Mathf.Deg2Rad) * dis.magnitude;

                Vector3 p = new Vector3(x, mControllers[index].transform.localPosition.y, z);
                mControllers[index].transform.localPosition = p;
                dirs[index] = Vector3.Normalize(dis);
                index++;
            }
        }
    }

    void ClearMesh()
    {
        //int count = 0;
        for (int i = vertex.transform.childCount - 1; i >= 0; i--)
        { 
            Destroy(vertex.transform.GetChild(i).gameObject);
        }
    }
}
