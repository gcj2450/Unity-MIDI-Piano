using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetBundleMenuItems
{
    [MenuItem("AssetBundles/Build Resource", false, 12)]
    public static void BuildAndroidResource()
    {
        BuildAssetBundles(BuildTarget.Android);
    }

    [MenuItem("AssetBundles/Build Resource for windows", false, 12)]
    public static void BuildWindowsResource()
    {
        BuildAssetBundles(BuildTarget.StandaloneWindows);
    }

    public static void BuildAssetBundles(BuildTarget buildTarget)
    {
        string assetPath = Application.dataPath + "/StreamingAssets/" + buildTarget;
        if (Directory.Exists(assetPath))
        {
            Directory.Delete(assetPath, true);
        }
        Directory.CreateDirectory(assetPath);
        BuildPipeline.BuildAssetBundles(assetPath, BuildAssetBundleOptions.None, buildTarget);
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }
}
