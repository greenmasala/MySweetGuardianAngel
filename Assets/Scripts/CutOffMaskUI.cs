using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;

public class CutOffMaskUI : Image
{
    public override Material materialForRendering
    {
        get
        {
            Material mat = new Material(base.materialForRendering);
            mat.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            return mat;
        }
    }
}
