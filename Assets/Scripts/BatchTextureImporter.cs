using UnityEngine;
using UnityEditor;

public class BatchTextureImporter : EditorWindow
{
    [MenuItem("Tools/Convert Textures to 2D UI")]
    static void ConvertTextures()
    {
        string path = "Assets/UI"; 
        string[] files = System.IO.Directory.GetFiles(path, "*.png", System.IO.SearchOption.AllDirectories);

        foreach (string file in files)
        {
            string assetPath = file.Replace(Application.dataPath, "Assets");
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.SaveAndReimport();
            }
        }

        Debug.Log("All textures converted to 2D UI!");
    }
}
