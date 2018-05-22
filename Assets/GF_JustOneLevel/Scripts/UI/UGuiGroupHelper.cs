using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

/// <summary>
/// uGUI 界面组辅助器
/// 参考来源：https://github.com/EllanJiang/StarForce
/// </summary>
public class UGuiGroupHelper : UIGroupHelperBase {
    public const int DepthFactor = 10000;

    private int depth = 0;
    private Canvas cachedCanvas = null;

    /// <summary>
    /// 设置界面组深度。
    /// </summary>
    /// <param name="depth">界面组深度。</param>
    public override void SetDepth (int depth) {
        this.depth = depth;
        cachedCanvas.overrideSorting = true;
        cachedCanvas.sortingOrder = DepthFactor * depth;
    }

    private void Awake () {
        cachedCanvas = gameObject.GetOrAddComponent<Canvas> ();
        gameObject.GetOrAddComponent<GraphicRaycaster> ();
    }

    private void Start () {
        cachedCanvas.overrideSorting = true;
        cachedCanvas.sortingOrder = DepthFactor * depth;

        RectTransform transform = GetComponent<RectTransform> ();
        transform.anchorMin = Vector2.zero;
        transform.anchorMax = Vector2.one;
        transform.anchoredPosition = Vector2.zero;
        transform.sizeDelta = Vector2.zero;
    }
}