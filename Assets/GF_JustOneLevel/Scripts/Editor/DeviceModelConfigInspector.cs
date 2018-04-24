using UnityEditor;
using UnityEngine;

/// <summary>
/// 参考来源：https://github.com/EllanJiang/StarForce
/// </summary>
namespace Editor {
    [CustomEditor (typeof (DeviceModelConfig))]
    public class DeviceModelConfigInspector : UnityEditor.Editor {
        public override void OnInspectorGUI () {
            if (GUILayout.Button ("Open Device Model Config Editor")) {
                DeviceModelConfigEditorWindow.OpenWindow ((DeviceModelConfig) target);
            }
        }
    }
}