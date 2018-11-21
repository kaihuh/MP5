using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyMesh : MonoBehaviour {
    LineSegment[] mNormals;

    void InitNormals(Vector3[] v, Vector3[] n)
    {
        mNormals = new LineSegment[v.Length];
        for (int i = 0; i < v.Length; i++)
        {
            GameObject o = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            mNormals[i] = o.AddComponent<LineSegment>();
            mNormals[i].SetWidth(0.01f);
            mNormals[i].transform.SetParent(this.transform);
        }
        UpdateNormals(v, n);
    }

    void UpdateNormals(Vector3[] v, Vector3[] n)
    {
        for (int i = 0; i < v.Length; i++)
        {
            mNormals[i].SetEndPoints(v[i], v[i] + 0.2f * n[i]);
        }
    }

    Vector3 FaceNormal(Vector3[] v, int i0, int i1, int i2)
    {
        Vector3 a = v[i1] - v[i0];
        Vector3 b = v[i2] - v[i0];
        return Vector3.Cross(a, b).normalized;
    }

    void ComputeNormals(Vector3[] v, Vector3[] n)
    {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        int[] t = theMesh.triangles;
        //Debug.Log("t.length = " + t.Length);
        int numberOfTri = t.Length / 3;
        Vector3[] triNormal = new Vector3[numberOfTri];
        for(int i = 0; i < numberOfTri; i++)
        {
            triNormal[i] = FaceNormal(v, t[i * 3], t[i * 3 + 1], t[i * 3 + 2]);
            //Debug.Log("tri[" + i + "]=" + t[i * 3] + "," + t[i * 3 + 1] + "," + t[i * 3 + 2]);
        }
        for (int i = 0; i < n.Length; i++)
        {
            //find the i's row and col
            int row = i / M;
            int col = i % M;
            if (row == 0 && col == 0)
            {
                n[i] = (triNormal[0] + triNormal[1]).normalized;
            }

            else if (row == N - 1 && col == M - 1)
            {
                n[i] = (triNormal[numberOfTri - 1] + triNormal[numberOfTri - 2]).normalized;
            }
            else if(row == 0 && col == M - 1)
            {
                n[i] = triNormal[(i - 1) * 2 + 1].normalized;
            }
            else if (row == N - 1 && col == 0)
            {
                n[i] = triNormal[((i - M + 1) - (row - 1) - 1) * 2].normalized;
            }
            else if(row == 0)
            {
                //Debug.Log("i = " + i);
                //Debug.Log("index = " + ((i - 1) * 2 + 1) + "," + ((i + 1) - 1) * 2 + "," + (((i + 1) - 1) * 2 + 1));
                n[i] = (triNormal[(i - 1) * 2 + 1] + triNormal[((i + 1) - 1) * 2] + triNormal[((i + 1) - 1) * 2 + 1]).normalized;
                

            }
            else if(row == N - 1)
            {
                n[i] = (triNormal[((i - M) - (row - 1) - 1) * 2] + triNormal[((i - M) - (row - 1) - 1) * 2 + 1] + triNormal[((i - M + 1) - (row - 1) - 1) * 2]).normalized;
            }
            else if(col == 0)
            {
                n[i] = (triNormal[((i - M + 1) - (row - 1) - 1) * 2] + triNormal[((i + 1) - row - 1) * 2] + triNormal[((i + 1) - row - 1) * 2 + 1]).normalized;
            }
            else if (col == M - 1)
            {
                n[i] = (triNormal[(i - row - 1) * 2 + 1] + triNormal[((i - M) - (row - 1) - 1) * 2] + triNormal[((i - M) - (row - 1) - 1) * 2 + 1]).normalized;
            }
            else
            {
                n[i] = (triNormal[(i - row - 1) * 2 + 1] + triNormal[((i + 1) - row - 1) * 2] + triNormal[((i + 1) - row - 1) * 2 + 1] + 
                    triNormal[((i - M) - (row - 1) - 1) * 2] + triNormal[((i - M) - (row - 1) - 1) * 2 + 1] +
                    triNormal[((i - M + 1) - (row - 1) - 1) * 2]).normalized;
            }
            //n[0] = (triNormal[0] + triNormal[1]).normalized;
            //n[1] = (triNormal[1] + triNormal[2] + triNormal[3]).normalized;
            //n[2] = triNormal[3].normalized;
            //n[3] = (triNormal[0] + triNormal[4] + triNormal[5]).normalized;
            //n[4] = (triNormal[0] + triNormal[1] + triNormal[2] + triNormal[5] + triNormal[6] + triNormal[7]).normalized;
            //n[5] = (triNormal[2] + triNormal[3]).normalized;
            //n[6] = triNormal[4].normalized;
            //n[7] = (triNormal[4] + triNormal[5] + triNormal[6]).normalized;
            //n[8] = (triNormal[6] + triNormal[7]).normalized;
            UpdateNormals(v, n);
        }
    }
}
