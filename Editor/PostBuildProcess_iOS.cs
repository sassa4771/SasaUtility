using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
using System.IO;

public class PostBuildProcess_iOS
{
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            WriteInfoPlist(path);
        }
    }

    private static void WriteInfoPlist(string folderpath)
    {
        string path = Path.Combine(folderpath, "Info.plist");
        PlistDocument plist = new PlistDocument();
        plist.ReadFromFile(path);
        PlistElementDict rootDict = plist.root;

        // Add custom properties
        rootDict.SetBoolean("UIFileSharingEnabled", true);
        rootDict.SetBoolean("LSSupportsOpeningDocumentsInPlace", true);

        plist.WriteToFile(path);
    }
}

#endif