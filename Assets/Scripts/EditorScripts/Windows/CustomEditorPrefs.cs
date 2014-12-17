#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

public class CustomEditorPrefs: EditorWindow
{
    private static string _defaulGUIPath = "Assets/Res/GUISkins/MyGizmoGuiSkin.guiskin";
    private static GUISkin _gizmoGuiSkin;

    private static string GizmoGuiSkinKey
    {
        get { return PlayerSettings.productName + "GizmoGuiSkin"; }
    }

    public static GUISkin GizmoGuiSkin
    {
        get
        {
            if (EditorPrefs.HasKey(GizmoGuiSkinKey))
            {
                if (_gizmoGuiSkin == null)
                {
                    var path = EditorPrefs.GetString(GizmoGuiSkinKey);
                    _gizmoGuiSkin = AssetDatabase.LoadAssetAtPath(path, typeof (GUISkin)) as GUISkin;
                }
                return _gizmoGuiSkin;
            }

            _gizmoGuiSkin = AssetDatabase.LoadAssetAtPath(_defaulGUIPath, typeof(GUISkin)) as GUISkin;
            return _gizmoGuiSkin ?? GUI.skin;
        }
    }

    [MenuItem("Window/CustomSettings")]
    private static void Init()
    {
        CustomEditorPrefs win = GetWindow<CustomEditorPrefs>();
        _gizmoGuiSkin = GizmoGuiSkin;
        win.Show();
    }

    private void OnGUI()
    {
        var skin = EditorGUILayout.ObjectField(_gizmoGuiSkin, typeof (GUISkin), false);

        if (skin != null)
        {
            _gizmoGuiSkin = (GUISkin) skin;

            if (GizmoGuiSkin != _gizmoGuiSkin)
            {
                EditorPrefs.SetString(GizmoGuiSkinKey, AssetDatabase.GetAssetPath(_gizmoGuiSkin));
            }
        }

        if (GUILayout.Button("Save"))
        {
            EditorPrefs.SetString(GizmoGuiSkinKey, AssetDatabase.GetAssetPath(_gizmoGuiSkin));
        }
    }
}

#endif