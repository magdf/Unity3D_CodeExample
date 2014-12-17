using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class CustomEditorPrefsProxy
{
    public static GUISkin GizmoGuiSkin
    {
        get
        {          
#if UNITY_EDITOR
            return CustomEditorPrefs.GizmoGuiSkin;
#endif
            return GUI.skin;
        }
    }

}
