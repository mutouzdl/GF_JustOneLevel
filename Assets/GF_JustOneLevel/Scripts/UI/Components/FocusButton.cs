using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 获取焦点后，按钮变色
/// </summary>
public class FocusButton : MonoBehaviour, ISelectHandler, IDeselectHandler {
    [SerializeField]
    private Color OnFocusColor = new Color (100 / 255f, 181 / 255f, 246 / 255f);
    [SerializeField]
    private Color OffFocusColor = new Color (227 / 255f, 242 / 255f, 253 / 255f);

    [SerializeField]
    private Color OnFocusFontColor = Color.black;
    [SerializeField]
    private Color OffFocusFontColor = Color.black;

    [SerializeField]
    private int OnFocusFontSize = 20;
    [SerializeField]
    private int OffFocusFontSize = 16;

    [SerializeField]
    private float OnFocusScale = 1.1f;
    [SerializeField]
    private float OffFocusScale = 1.0f;

    [SerializeField]
    private bool IsAutoOffFocus = false; /* 是否自动执行失去焦点动作（恢复颜色） */

    public EventHandler<FocusButton> OnSelectListener;
    public EventHandler<FocusButton> OnDeselectListener;

    protected Button btn = null;

    private bool _isSelected = false;
    private FocusButtonGroup focusButtonGroup = null;

    void Start () {
        btn = GetComponent<Button> ();
    }

    private void OnDestroy () {
        NotifyParentForReCheck ();
    }

    /// <summary>
    /// 新的焦点按钮被创建或销毁时，主动通知焦点按钮组
    /// </summary>
    public void NotifyParentForReCheck () {
        if (focusButtonGroup == null) {
            focusButtonGroup = GetComponentInParent<FocusButtonGroup> ();
        }

        if (focusButtonGroup != null) {
            focusButtonGroup.ReCheckChilds ();
        }
    }

    public void OnSelect (BaseEventData eventData) {
        Select ();
    }

    public void OnDeselect (BaseEventData eventData) {
        if (IsAutoOffFocus) {
            Deselect ();
        } else {
            NotifyParentForReCheck ();
        }
    }

    public void Select () {
        _isSelected = true;

        if (btn == null) {
            btn = GetComponent<Button> ();
        }

        btn.transform.localScale = new Vector3 (OnFocusScale, OnFocusScale, OnFocusScale);

        btn.GetComponent<Image> ().color = OnFocusColor;

        Text[] texts = btn.GetComponentsInChildren<Text> ();

        if (texts.Length > 0) {
            foreach (Text text in texts) {
                text.fontSize = OnFocusFontSize;
                text.color = OnFocusFontColor;
            }
        }

        if (OnSelectListener != null) {
            OnSelectListener.Invoke (this, this);
        }

        OnSelectEnd (btn);
    }

    public void Deselect () {
        _isSelected = false;

        btn.transform.localScale = new Vector3 (OffFocusScale, OffFocusScale, OffFocusScale);

        btn.GetComponent<Image> ().color = OffFocusColor;

        Text[] texts = btn.GetComponentsInChildren<Text> ();

        if (texts.Length > 0) {
            foreach (Text text in texts) {
                text.fontSize = OffFocusFontSize;
                text.color = OffFocusFontColor;
            }
        }

        if (OnDeselectListener != null) {
            OnDeselectListener.Invoke (this, this);
        }

        OnDeselectEnd (btn);
    }

    public bool IsSelected { get { return _isSelected; } }

    protected virtual void OnSelectEnd (Button btn) { }
    protected virtual void OnDeselectEnd (Button btn) { }
}