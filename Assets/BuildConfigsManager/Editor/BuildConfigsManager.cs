using Unity.Build;
using UnityEditor;
using UnityEngine;

public class BuildConfigsManager : MonoBehaviour
{
    const string k_CurrentActionIndexKey = "BuildConfigurationInspector.CurrentActionIndex";

    static int CurrentActionIndex
    {
        get => EditorPrefs.GetInt(k_CurrentActionIndexKey, 1);
        set => EditorPrefs.SetInt(k_CurrentActionIndexKey, value);
    }

    [MenuItem(itemName: "BuildManager/Build WebGL Platform")]
    public static void BuildWebGLPlatform()
    {
        var currentTarget = BuildTarget.WebGL;
        var result = BuildByTarget(currentTarget);
        if (!result)
            Application.Quit(1);
    }

    [MenuItem(itemName: "BuildManager/Build Windows Platform")]
    public static void BuildWindowsPlatform()
    {
        var currentTarget = BuildTarget.StandaloneWindows64;
        var result = BuildByTarget(currentTarget);
        if (!result)
            Application.Quit(1);
    }

    [MenuItem(itemName: "BuildManager/Build MacOS Platform")]
    public static void BuildMacOSPlatform()
    {
        var currentTarget = BuildTarget.StandaloneOSX;
        var result = BuildByTarget(currentTarget);
        if (!result)
            Application.Quit(1);
    }

    [MenuItem(itemName: "BuildManager/Build Current Platform")]
    public static void BuildCurrentPlatform()
    {
        var currentTarget = EditorUserBuildSettings.activeBuildTarget;
        var result = BuildByTarget(currentTarget);
        if (!result)
            Application.Quit(1);
    }

    private static bool BuildByTarget(BuildTarget target)
    {
        Debug.Log("///////////////////////////////////////////////////////////////////");
        Debug.Log("building:" + target);
        Debug.Log("before GetSettings");
        var config = BuildSettings.GetOrCreateSettings();
        Debug.Log("after GetSettings");
        if (!config.ContainsKey(target))
            return false;
        Debug.Log("CONTAINS!");
        var buildConfigs = config[target];
        Debug.Log("building:" + buildConfigs);
        if (buildConfigs.Length == 0)
            return false;

        var oldActionIndex = CurrentActionIndex;

        Debug.Log("Setting BUILD");
        CurrentActionIndex = 0;

        foreach (var buildConfig in buildConfigs)
        {
            if (buildConfig == null)
            {
                Debug.Log("BUILD CONFIG NULL!");
            }
            
            var result = buildConfig.Build();

            Debug.Log("FOR RESULT" + result);
        }

        CurrentActionIndex = oldActionIndex;

        Debug.Log("result:" + true);
        return true;
    }
}