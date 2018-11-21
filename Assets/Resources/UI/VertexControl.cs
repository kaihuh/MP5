using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexControl : MonoBehaviour {

    public SliderWithEcho row, col, rotation;
    public Cmesh mymesh;
    // Use this for initialization
    void Start () {
       row.InitSliderRange(4f, 20f, 10f);
       col.InitSliderRange(4f, 20f, 10f);
       rotation.InitSliderRange(10f, 360f, 275f);

       row.SetSliderListener(XValueChanged);
       col.SetSliderListener(YValueChanged);
       rotation.SetSliderListener(ZValueChanged);
    }

    //---------------------------------------------------------
    // resopond to sldier bar value changes
    void XValueChanged(float v)
    {
        mymesh.setRow((int)v);
    }

    void YValueChanged(float v)
    {
        mymesh.setCol((int)v);
    }

    void ZValueChanged(float v)
    {
        mymesh.setRotation((int)v);
    }
}
