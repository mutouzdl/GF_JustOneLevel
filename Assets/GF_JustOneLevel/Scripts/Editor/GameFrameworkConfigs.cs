using GameFramework;
using UnityEngine;
using UnityGameFramework.Editor;
using UnityGameFramework.Editor.AssetBundleTools;

namespace Editor {
    public static class GameFrameworkConfigs {
        [BuildSettingsConfigPath]
        public static string BuildSettingsConfig = Utility.Path.GetCombinePath (Application.dataPath, "GF_JustOneLevel/Configs/BuildSettings.xml");

        [AssetBundleBuilderConfigPath]
        public static string AssetBundleBuilderConfig = Utility.Path.GetCombinePath (Application.dataPath, "GF_JustOneLevel/Configs/AssetBundleBuilder.xml");

        [AssetBundleEditorConfigPath]
        public static string AssetBundleEditorConfig = Utility.Path.GetCombinePath (Application.dataPath, "GF_JustOneLevel/Configs/AssetBundleEditor.xml");

        [AssetBundleCollectionConfigPath]
        public static string AssetBundleCollectionConfig = Utility.Path.GetCombinePath (Application.dataPath, "GF_JustOneLevel/Configs/AssetBundleCollection.xml");
    }
}