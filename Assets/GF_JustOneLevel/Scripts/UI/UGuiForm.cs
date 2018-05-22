using System.Collections;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

/// <summary>
/// 参考来源：https://github.com/EllanJiang/StarForce
/// </summary>
public abstract class UGuiForm : UIFormLogic {
    public const int DepthFactor = 100;
    private const float FadeTime = 0.3f;

    private static Font mainFont = null;
    private Canvas cachedCanvas = null;
    private CanvasGroup canvasGroup = null;

    public int OriginalDepth {
        get;
        private set;
    }

    public int Depth {
        get {
            return cachedCanvas.sortingOrder;
        }
    }

    public void Close () {
        Close (false);
    }

    public void Close (bool ignoreFade) {
        StopAllCoroutines ();

        if (ignoreFade) {
            GameEntry.UI.CloseUIForm (this);
        } else {
            StartCoroutine (CloseCo (FadeTime));
        }
    }

    public void PlayUISound (int uiSoundId) {
        GameEntry.Sound.PlayUISound (uiSoundId);
    }

    public static void SetMainFont (Font mainFont) {
        if (mainFont == null) {
            Log.Error ("Main font is invalid.");
            return;
        }

        UGuiForm.mainFont = mainFont;

        GameObject go = new GameObject ();
        go.AddComponent<Text> ().font = mainFont;
        Destroy (go);
    }

    protected override void OnInit (object userData)
    {
        base.OnInit (userData);

        cachedCanvas = gameObject.GetOrAddComponent<Canvas> ();
        cachedCanvas.overrideSorting = true;
        OriginalDepth = cachedCanvas.sortingOrder;

        canvasGroup = gameObject.GetOrAddComponent<CanvasGroup> ();

        RectTransform transform = GetComponent<RectTransform> ();
        transform.anchorMin = Vector2.zero;
        transform.anchorMax = Vector2.one;
        transform.anchoredPosition = Vector2.zero;
        transform.sizeDelta = Vector2.zero;

        gameObject.GetOrAddComponent<GraphicRaycaster> ();

        Text[] texts = GetComponentsInChildren<Text> (true);
        for (int i = 0; i < texts.Length; i++) {
            texts[i].font = mainFont;

            if (!string.IsNullOrEmpty (texts[i].text)) {
                string[] keys = texts[i].text.Split (',');
                StringBuilder value = new StringBuilder ();
                foreach (string key in keys) {
                    value.Append (GameEntry.Localization.GetString (key));
                }

                texts[i].text = value.ToString();
            }
        }
    }

    protected override void OnOpen (object userData)
    {
        base.OnOpen (userData);

        canvasGroup.alpha = 0f;
        StopAllCoroutines ();
        StartCoroutine (canvasGroup.FadeToAlpha (1f, FadeTime));
    }

    protected override void OnClose (object userData)
    {
        base.OnClose (userData);
    }

    protected override void OnPause ()
    {
        base.OnPause ();
    }

    protected override void OnResume ()
    {
        base.OnResume ();

        canvasGroup.alpha = 0f;
        StopAllCoroutines ();
        StartCoroutine (canvasGroup.FadeToAlpha (1f, FadeTime));
    }

    protected override void OnCover ()
    {
        base.OnCover ();
    }

    protected override void OnReveal ()
    {
        base.OnReveal ();
    }

    protected override void OnRefocus (object userData)
    {
        base.OnRefocus (userData);
    }

    protected override void OnUpdate (float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate (elapseSeconds, realElapseSeconds);
    }

    protected override void OnDepthChanged (int uiGroupDepth, int depthInUIGroup)
    {
        int oldDepth = Depth;
        base.OnDepthChanged (uiGroupDepth, depthInUIGroup);
        int deltaDepth = UGuiGroupHelper.DepthFactor * uiGroupDepth + DepthFactor * depthInUIGroup - oldDepth + OriginalDepth;
        Canvas[] canvases = GetComponentsInChildren<Canvas> (true);
        for (int i = 0; i < canvases.Length; i++) {
            canvases[i].sortingOrder += deltaDepth;
        }
    }

    private IEnumerator CloseCo (float duration) {
        yield return canvasGroup.FadeToAlpha (0f, duration);
        GameEntry.UI.CloseUIForm (this);
    }
}