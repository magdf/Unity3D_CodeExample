using UnityEngine;
using System.Collections;

public class ResizeLabelWidth : MonoBehaviour
{
    private void Start()
    {
        Resize();
    }


    public void Resize()
    {
        var lbl = GetComponent<UILabel>();
        lbl.UpdateNGUIText();
        float width = NGUIText.CalculatePrintedSize(lbl.text).x;// *lbl.fontSize;
        lbl.width = (int)width;
    }

}
