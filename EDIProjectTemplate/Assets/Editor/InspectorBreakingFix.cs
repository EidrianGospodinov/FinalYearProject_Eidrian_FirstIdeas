using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class InspectorBreakingFix : EditorWindow
{
    private static string path = "Assets/QuickMaterial.mat";

    [MenuItem("Tools/Force Inspector Refresh &#l")]
    public static void ForceRefresh()
    {
        // clear spam logs
        var assembly = System.Reflection.Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);

        // create temp material
        string tempPath = "Assets/ForceRefreshTemp.mat";
        Material tempMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        
        AssetDatabase.CreateAsset(tempMat, tempPath);
        
        //delete the material
        AssetDatabase.DeleteAsset(tempPath);
        AssetDatabase.Refresh();

        Debug.Log("<color=orange><b>Unity UI Thread Reset Successfully!</b></color>");
    }
}