using System.Collections;
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

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit (object userData)
#else
        protected internal override void OnInit (object userData)
#endif
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
                                texts[i].text = GameEntry.Localization.GetString (texts[i].text);
                        }
                }
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen (object userData)
#else
        protected internal override void OnOpen (object userData)
#endif
        {
                base.OnOpen (userData);

                canvasGroup.alpha = 0f;
                StopAllCoroutines ();
                StartCoroutine (canvasGroup.FadeToAlpha (1f, FadeTime));
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose (object userData)
#else
        protected internal override void OnClose (object userData)
#endif
        {
                base.OnClose (userData);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnPause ()
#else
        protected internal override void OnPause ()
#endif
        {
                base.OnPause ();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnResume ()
#else
        protected internal override void OnResume ()
#endif
        {
                base.OnResume ();

                canvasGroup.alpha = 0f;
                StopAllCoroutines ();
                StartCoroutine (canvasGroup.FadeToAlpha (1f, FadeTime));
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnCover ()
#else
        protected internal override void OnCover ()
#endif
        {
                base.OnCover ();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnReveal ()
#else
        protected internal override void OnReveal ()
#endif
        {
                base.OnReveal ();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnRefocus (object userData)
#else
        protected internal override void OnRefocus (object userData)
#endif
        {
                base.OnRefocus (userData);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnUpdate (float elapseSeconds, float realElapseSeconds)
#else
        protected internal override void OnUpdate (float elapseSeconds, float realElapseSeconds)
#endif
        {
                base.OnUpdate (elapseSeconds, realElapseSeconds);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnDepthChanged (int uiGroupDepth, int depthInUIGroup)
#else
        protected internal override void OnDepthChanged (int uiGroupDepth, int depthInUIGroup)
#endif
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