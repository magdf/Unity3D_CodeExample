using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class UET : EditorWindow
{
    private float _btnWidth = 150;

	[MenuItem("Window/UET/Unity Editor Toolbar")]
	public static void CreateWindow(){
		UET window = GetWindow<UET>();
		window.title = "UET";
		Object.DontDestroyOnLoad(window);
		window.Load();
	}
	
	public static int _count_btn = 0;
	public static List<string> names = new List<string>();
	public static List<string> paths = new List<string>();
	void OnGUI(){
		
		if(GUI.Button(new Rect(5, 5, 20, 20), (Texture2D)Resources.Load("add"))){
			EditorApplication.ExecuteMenuItem("Window/UET/UET Add Batton");
		}
		if(GUI.Button(new Rect(30, 5, 60, 20), "Load")){
			Load();
		}
		for(int i = 0; i < _count_btn; i++){
            if (GUI.Button(new Rect(10, 30 + 25 * i, _btnWidth, 20), names[i]))
            {
				EditorApplication.ExecuteMenuItem(paths[i]);
			}
		}
	}

    private RecompileClass recompile;
    private class RecompileClass
    {}
    public void AfterRecompileScripts()
    {
        // check if recompile variable is null 
        if (this.recompile == null)
        {
            // if yes assume recompile then create a reference to a recompile class 
            this.recompile = new RecompileClass();
            
            // do what you need to do here after detecting a recompile 
            Load();
        }

        // do regular stuff here
    }
	
	private void Load(){
		names.Clear();
		paths.Clear();
        if (!File.Exists(Application.dataPath + "/3rdParty/Poq Xert/Unity Editor Toolbar/Resources/Data/data.uet"))
            return;
        StreamReader sr = new StreamReader(Application.dataPath + "/3rdParty/Poq Xert/Unity Editor Toolbar/Resources/Data/data.uet");
		if(sr.EndOfStream) return;
		_count_btn = int.Parse(sr.ReadLine());
		for(int i = 0; i < _count_btn; i++){
			names.Add(sr.ReadLine());
			paths.Add(sr.ReadLine());
		}
		sr.Close();
	}
	
	public static void Save(){
        StreamWriter sw = new StreamWriter(Application.dataPath + "/3rdParty/Poq Xert/Unity Editor Toolbar/Resources/Data/data.uet");
		sw.Write("");
		sw.WriteLine(_count_btn);
		for(int i = 0; i < _count_btn; i++){
			sw.WriteLine(names[i]);
			sw.WriteLine(paths[i]);
		}
		sw.Close();
	}
}
