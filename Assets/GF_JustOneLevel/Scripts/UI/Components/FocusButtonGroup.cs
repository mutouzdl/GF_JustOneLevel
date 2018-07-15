using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 焦点按钮组
/// </summary>
public class FocusButtonGroup : MonoBehaviour {
    private FocusButton[] buttonList = null;

    private FocusButton preSelectButton = null;

    void Start () {
        SetChildSelectCallback ();
    }

    /// <summary>
    /// 重新检查是否有新按钮被添加进来
    /// </summary>
    public void ReCheckChilds () {
        if (buttonList != null) {
            if (buttonList.Length < this.transform.childCount) {
                SetChildSelectCallback ();
            } else if (buttonList.Length > this.transform.childCount) {
                buttonList = this.GetComponentsInChildren<FocusButton> ();
            }
        }
    }

    private void SetChildSelectCallback () {
        buttonList = this.GetComponentsInChildren<FocusButton> ();

        foreach (FocusButton button in buttonList) {
            /* 其中一个按钮被选中后，其他按钮恢复原状 */
            button.OnSelectListener -= OnSelectCallback;
            button.OnSelectListener += OnSelectCallback;
        }
    }

    private void OnSelectCallback (object sender, FocusButton btn) {
        if (preSelectButton != null) {
            preSelectButton.Deselect ();
        }

        preSelectButton = btn;

        /* 可能存在一些按钮，是被动在代码中调用了Select函数的，即可能同时存在多个被select的按钮，需要再检查一遍 */
        foreach (FocusButton focusBtn in buttonList) {
            if (focusBtn.GetInstanceID () != preSelectButton.GetInstanceID () && focusBtn.IsSelected) {
                focusBtn.Deselect ();
            }
        }
    }
}