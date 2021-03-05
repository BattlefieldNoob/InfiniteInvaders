using System;
using Unity.Build;
using UnityEditor;
using UnityEngine;

[Serializable]
public class BuildSettings : ScriptableObject
{
    public const string k_BuildSettingsPath = "Assets/Editor/BuildSettings.asset";

    public BuildConfiguration[] WebGLConfigs = new BuildConfiguration[0];
    public BuildConfiguration[] WindowsConfigs = new BuildConfiguration[0];
    public BuildConfiguration[] MacOSConfigs = new BuildConfiguration[0];


    public bool ContainsKey(BuildTarget key)
    {
        switch (key)
        {
            case BuildTarget.StandaloneOSX:
            case BuildTarget.StandaloneWindows64:
            case BuildTarget.WebGL:
                return true;
            default:
                return false;
        }
    }

    public BuildConfiguration[] this[BuildTarget key]
    {
        get
        {
            return key switch
            {
                BuildTarget.StandaloneOSX => MacOSConfigs,
                BuildTarget.StandaloneWindows64 => WindowsConfigs,
                BuildTarget.WebGL => WebGLConfigs,
                _ => throw new ArgumentOutOfRangeException(nameof(key), key, null)
            };
        }
        set
        {
            switch (key)
            {
                case BuildTarget.StandaloneOSX:
                    MacOSConfigs = value;
                    break;
                case BuildTarget.StandaloneWindows64:
                    WindowsConfigs = value;
                    break;
                case BuildTarget.WebGL:
                    WebGLConfigs = value;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(key), key, null);
            }
        }
    }


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


    public void Save()
    {
        Debug.Log("SAVING!");
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
}