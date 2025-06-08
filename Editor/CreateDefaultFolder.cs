using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateDefaultFolders : EditorWindow
{
    [MenuItem("Tools/Create Default Folders")]
    static void CreateFolders()
    {
        string[] folders = new string[]
        {
            "Scenes",
            "Scripts",
            "Prefabs",
            "Materials",
            "Textures",
            "Models",
            "Animations",
            "Audio/Music",
            "Audio/SFX",
            "UI",
            "Shaders",
            "Plugins",
            "Resources",
            "Editor",
            "ThirdParty"
        };

        foreach (var folder in folders)
        {
            string path = Path.Combine(Application.dataPath, folder);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Debug.Log($"Created folder: Assets/{folder}");
            }
            else
            {
                Debug.Log($"Folder already exists: Assets/{folder}");
            }
        }

        AssetDatabase.Refresh();
    }
}
