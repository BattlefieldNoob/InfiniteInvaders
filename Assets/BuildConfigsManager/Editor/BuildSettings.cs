using System;
using System.Linq;
using Unity.Build;
using UnityEditor;
using UnityEngine;

[Serializable]
public class BuildSettings : ScriptableObject
{
    public const string k_BuildSettingsPath = "Assets/Editor/BuildSettings.asset";

    public string[] WebGLConfigs = new string[0];
    public string[] WindowsConfigs = new string[0];
    public string[] MacOSConfigs = new string[0];

    public static BuildSettings GetOrCreateSettings()
    {
        var settings = AssetDatabase.LoadAssetAtPath<BuildSettings>(k_BuildSettingsPath);
        if (settings == null)
        {
            settings = CreateInstance<BuildSettings>();
            AssetDatabase.CreateAsset(settings, k_BuildSettingsPath);
            AssetDatabase.SaveAssets();
        }

        return settings;
    }

    public static SerializedObject GetSerializedSettings()
    {
        return new SerializedObject(GetOrCreateSettings());
    }

    public BuildConfiguration[] GetConfigsByTarget(BuildTarget target)
    {
        var data = target switch
        {
            BuildTarget.StandaloneOSX => MacOSConfigs,
            BuildTarget.StandaloneWindows64 => WindowsConfigs,
            BuildTarget.WebGL => WebGLConfigs,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };

        //configs GUID are saved into Dictionary
        return data.Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<BuildConfiguration>).ToArray();
    }

    public void SetConfigsForTarget(BuildTarget target, BuildConfiguration[] configurations)
    {
        var data = configurations.Select(AssetDatabase.GetAssetPath)
            .Select(AssetDatabase.AssetPathToGUID).ToArray();

        switch (target)
        {
            case BuildTarget.StandaloneOSX:
                MacOSConfigs = data;
                break;
            case BuildTarget.StandaloneWindows64:
                WindowsConfigs = data;
                break;
            case BuildTarget.WebGL:
                WebGLConfigs = data;
                break;
            default: throw new ArgumentOutOfRangeException(nameof(target), target, null);
        }
    }

    public bool IsTargetSupported(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.StandaloneOSX:
            case BuildTarget.StandaloneWindows64:
            case BuildTarget.WebGL:
                return true;
            default:
                return false;
        }
    }

    public void Save()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
}