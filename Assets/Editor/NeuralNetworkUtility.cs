using UnityEngine;
using System.Collections;
using UnityEditor;

public class NeuralNetworkUtility : EditorWindow
{
    //Editor Windows and layout
    private static Texture Background;
    private static Texture iconTexture;
    public static int sideWindowWidth = 400;
    public Rect sideWindowRect { get { return new Rect(0, 0, sideWindowWidth, position.height); } }
    public Rect canvasWindowRect { get { return new Rect(sideWindowWidth, 0, position.width - sideWindowWidth, position.height); } }

    public Brain brain;
    


    [MenuItem("Window/Neural Network Utility editor")]
    static void ShowEditor()
    {
        Background = (Texture)Resources.Load("Textures/background");
        NeuralNetworkUtility editor = EditorWindow.GetWindow<NeuralNetworkUtility>();
        
    }
    void OnGUI() {
        DrawLeftMenu();
        DrawTheCanvas();
    }
    void DrawLeftMenu() {
        GUILayout.BeginArea(sideWindowRect,GUI.skin.box);
        if (GUILayout.Button("Create Node 1"))
        {
            Debug.Log("Create Node 1");
            if (brain == null) {
                brain = ScriptableObject.CreateInstance<Brain>();
            }
            brain.AddNeuron();
        }
        GUILayout.EndArea();
    }

    void DrawTheCanvas() {
        //GUILayout.BeginArea(canvasWindowRect, GUI.skin.box);
        if (Background == null)
        {
            Background = (Texture)Resources.Load("Textures/background");    
        }
        
        GUI.DrawTexture(canvasWindowRect, Background);
        BeginWindows();

        EndWindows();

        //GUILayout.EndArea();
    }
}
