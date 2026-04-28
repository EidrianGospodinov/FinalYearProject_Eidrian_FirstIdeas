using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

public class TorqueMaterialImporter : EditorWindow
{
    [MenuItem("Tools/Import Torque Materials")]
    static void ImportMaterials()
    {
        string[] files = Directory.GetFiles(Application.dataPath, "materials.txt", SearchOption.AllDirectories);

        foreach (string file in files)
        {
            string text = File.ReadAllText(file);

            foreach (Match block in Regex.Matches(text, @"singleton Material\((.*?)\)\s*\{(.*?)\};", RegexOptions.Singleline))
            {
                string body = block.Groups[2].Value;

                string mapTo = MatchValue(body, @"mapTo\s*=\s*""(.*?)"";");
                string diffuse = MatchValue(body, @"diffuseMap\[0\]\s*=\s*""(.*?)"";");
                string normal = MatchValue(body, @"normalMap\[0\]\s*=\s*""(.*?)"";");
                string color = MatchValue(body, @"diffuseColor\[0\]\s*=\s*""(.*?)"";");

                if (string.IsNullOrEmpty(mapTo))
                    continue;

                Material mat = new Material(Shader.Find("Standard"));
                mat.name = mapTo;

                Texture2D diffuseTex = FindTexture(diffuse);
                if (diffuseTex)
                    mat.SetTexture("_MainTex", diffuseTex);

                Texture2D normalTex = FindTexture(normal);
                if (normalTex)
                {
                    mat.SetTexture("_BumpMap", normalTex);
                    mat.EnableKeyword("_NORMALMAP");
                }

                if (!string.IsNullOrEmpty(color))
                {
                    string[] parts = color.Split(' ');
                    if (parts.Length >= 3)
                    {
                        mat.color = new Color(
                            float.Parse(parts[0]),
                            float.Parse(parts[1]),
                            float.Parse(parts[2]),
                            parts.Length >= 4 ? float.Parse(parts[3]) : 1f
                        );
                    }
                }

                string dir = "Assets/GeneratedMaterials";
                if (!AssetDatabase.IsValidFolder(dir))
                    AssetDatabase.CreateFolder("Assets", "GeneratedMaterials");

                string path = $"{dir}/{mapTo}.mat";
                AssetDatabase.CreateAsset(mat, AssetDatabase.GenerateUniqueAssetPath(path));
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Torque materials imported.");
    }

    static string MatchValue(string text, string pattern)
    {
        Match m = Regex.Match(text, pattern);
        return m.Success ? m.Groups[1].Value : "";
    }

    static Texture2D FindTexture(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        name = Path.GetFileNameWithoutExtension(name);

        string[] guids = AssetDatabase.FindAssets(name + " t:Texture2D");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (Path.GetFileNameWithoutExtension(path) == name)
                return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        }

        return null;
    }
}