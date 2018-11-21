using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyMesh : MonoBehaviour
{
    public GameObject LocalTester;
    public GameObject Origin;
    private int N = 5;//number of vertex per row
    private int M = 5;// number of vertex per column 
    private float height = 5.0f;
    private float length = 5.0f;
    private string last = "X";

    private float rotation = 0;
    private Vector2 position = Vector2.zero;
    private Vector2 scale = Vector2.one; 
    Vector2[] mInitUV = null;

    // Use this for initialization
    private void Awake()
    {
        InitalMesh();
    }
    void Start()
    {
        Debug.Assert(LocalTester != null);
        Debug.Assert(Origin != null);
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        Vector3[] v = theMesh.vertices;

        //Set origin
        Vector3 dir = (v[mControllers.Length - 1] - v[0]).normalized;
        float distance = (v[mControllers.Length - 1] - v[0]).magnitude;
        Origin.transform.localPosition = v[0] + distance * 0.5f * dir;

        ShowVertices(false);
        ShowXfrom(false);
    }
    void InitalMesh()
    {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        //Debug.Log("Inital called");
        theMesh.Clear();    // delete whatever is there!!
        //calculate deltx and delty
        float delty = height / (N - 1);
        float deltx = length / (M - 1);
        //Debug.Log("deltx = " + deltx);
        //Debug.Log("delty = " + delty);
        //# of triangles for n *m vertex
        int Tri = (N - 1) * (M - 1) * 2;
        Vector3[] v = new Vector3[N * M];   // 2x2 mesh needs 3x3 vertices
        int[] t = new int[Tri * 3];         // Number of triangles: 2x2 mesh and 2x triangles on each mesh-unit
        Vector3[] n = new Vector3[N * M];   // MUST be the same as number of vertices, normal for each vertex
        Vector2[] uv = new Vector2[N * M];

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < M; j++)
            {
                v[i * M + j] = new Vector3(j * deltx, 0, i * delty);
                n[i * M + j] = new Vector3(0, 1, 0);
                uv[i * M + j] = new Vector3(j * deltx / length, i * delty / height);
            }
        }

        for (int i = 0; i < Tri; i++)
        {
            //for each triangle , find its row and col
            int row = i / ((M - 1) * 2);
            int col = (i % ((M - 1) * 2)) / 2;
            if (i % 2 == 0)
            {
                t[i * 3 + 0] = row * M + col;
                t[i * 3 + 1] = (row + 1) * M + col;
                t[i * 3 + 2] = (row + 1) * M + col + 1;
            }
            else
            {
                t[i * 3 + 0] = row * M + col;
                t[i * 3 + 1] = (row + 1) * M + col + 1;
                t[i * 3 + 2] = row * M + col + 1;
            }
        }

        theMesh.vertices = v; //  new Vector3[3];
        theMesh.triangles = t; //  new int[3];
        theMesh.normals = n;
        theMesh.uv = uv;

        mInitUV = new Vector2[uv.Length];
        for (int i = 0; i < uv.Length; i++)
        {
            mInitUV[i] = uv[i];
        }

        InitControllers(v);
        InitNormals(v, n);
    }
    // Update is called once per frame
    void Update()
    {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        Vector3[] v = theMesh.vertices;
        Vector3[] n = theMesh.normals;
        for (int i = 0; i < mControllers.Length; i++)
        {
            v[i] = mControllers[i].transform.localPosition;
        }

        ComputeNormals(v, n);

        theMesh.vertices = v;
        theMesh.normals = n;
    }

    public void SetRow(int n)
    {
        N = n;
        ClearMesh();
        InitalMesh();
        ShowVertices(false);
        ShowXfrom(false);
    }

    public void SetCol(int m)
    {
        M = m;
        ClearMesh();
        InitalMesh();
        ShowVertices(false);
        ShowXfrom(false);
    }
    void ClearMesh()
    {
        //int count = 0;
        for (int i = transform.childCount - 1; i >= 0 ; i--)
        { //m_Collider.GetType() == typeof(CapsuleCollider)
            if (transform.GetChild(i).gameObject.name == "Sphere" || transform.GetChild(i).name == "Cylinder")
            {
                //Debug.Log("Find");
                Destroy(transform.GetChild(i).gameObject);
                //count++;
            }
        }
        //Debug.Log("count = " + count);
    }

    public void updateUV()
    {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        Vector2[] uv = theMesh.uv;
        for (int i = 0; i < uv.Length; i++)
        {
            Matrix3x3 temp = Matrix3x3Helpers.CreateTRS(position, rotation, scale);
            uv[i] = Matrix3x3.MultiplyVector2(temp, mInitUV[i]);
        }
        theMesh.uv = uv;
    }

    public void setX(float input)
    {
        position.x = input;
        updateUV();
    }

    public void setY(float input)
    {
        position.y = input;
        updateUV();
    }

    public void setSX(float input)
    {
        scale.x = input;
        updateUV();
    }

    public void setSY(float input)
    {
        scale.y = input;
        updateUV();
    }

    public void setR(float input)
    {
        rotation = input;
        updateUV();
    }
}
