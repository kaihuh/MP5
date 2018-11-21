using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureControl : MonoBehaviour
{

    public SliderWithEcho X, Y, SX, SY, R;
    public MyMesh mymesh;
    // Use this for initialization
    void Start()
    {
        X.InitSliderRange(-4f, 4f, 0f);
        Y.InitSliderRange(-4f, 4f, 0f);
        SX.InitSliderRange(0.1f, 10f, 1f);
        SY.InitSliderRange(0.1f, 10f, 1f);
        R.InitSliderRange(-180f, 180f, 0f);

        X.SetSliderListener(XValueChanged);
        Y.SetSliderListener(YValueChanged);
        SX.SetSliderListener(SXValueChanged);
        SY.SetSliderListener(SYValueChanged);
        R.SetSliderListener(RValueChanged);
    }

    //---------------------------------------------------------------------------------
    // resopond to sldier bar value changes
    void XValueChanged(float v)
    {
        mymesh.setX(v);
    }

    void YValueChanged(float v)
    {
        mymesh.setY(v);
    }

    void SXValueChanged(float v)
    {
        mymesh.setSX(v);
    }

    void SYValueChanged(float v)
    {
        mymesh.setSY(v);
    }

    void RValueChanged(float v)
    {
        mymesh.setR(v);
    }
}