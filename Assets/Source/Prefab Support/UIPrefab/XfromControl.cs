using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XfromControl : MonoBehaviour
{

    public SliderWithEcho ROW, COL;
    public MyMesh myMesh;

    // Use this for initialization
    void Start()
    {

        ROW.SetSliderListener(XValueChanged);
        COL.SetSliderListener(YValueChanged);
        //X.SetSliderValue(5);
        SetRange();


    }

    void Update()
    {

    }

    //---------------------------------------------------------------------------------
    // Initialize slider bars to specific function
    void SetRange()
    {
        float p = ReadObjectXfrom();
        //mPreviousSliderValues = p;
        ROW.InitSliderRange(2, 20, p);
        p = ReadObjectYfrom();
        COL.InitSliderRange(2, 20, p);
        //Z.InitSliderRange(-30, 30, p.z);

    }
    //---------------------------------------------------------------------------------

    //---------------------------------------------------------------------------------
    // resopond to sldier bar value changes
    void XValueChanged(float v)
    {
        float p = ReadObjectXfrom();
        ROW.TheEcho.text = (p).ToString();
        myMesh.SetRow((int)p);
    }

    void YValueChanged(float v)
    {
        float p = ReadObjectYfrom();
        COL.TheEcho.text = (p).ToString();
        myMesh.SetCol((int)p);
    }
    //---------------------------------------------------------------------------------

    private float ReadObjectXfrom()
    {
        float p;
        p = ROW.GetSliderValue();
        //Debug.Log("p = " + p);
        //p.y = Y.GetSliderValue();
        //p.z = Z.GetSliderValue();
        return p;
    }

    private float ReadObjectYfrom()
    {
        float p;
        p = COL.GetSliderValue();
        //Debug.Log("p = " + p);
        //p.y = Y.GetSliderValue();
        //p.z = Z.GetSliderValue();
        return p;
    }
}
