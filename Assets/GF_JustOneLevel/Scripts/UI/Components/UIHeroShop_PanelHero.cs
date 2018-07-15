using System;
using GameFramework;
using GameFramework.Resource;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 英雄商店-英雄面板
/// </summary>
public class UIHeroShop_PanelHero : MonoBehaviour {

    [SerializeField]
    private Text textDes = null;
    [SerializeField]
    private Text textName = null;
    [SerializeField]
    private Image imgHero = null;

    private DRHeroShop drHeroShop = null;

    public void Init (DRHeroShop drHeroShop, Action<UIHeroShop_PanelHero> onSelectCallback) {
        this.drHeroShop = drHeroShop;

        this.gameObject.GetOrAddComponent<Button>();

        FocusButton focusButton = this.gameObject.GetComponent<FocusButton>();
        focusButton.OnSelectListener += (sender, button) => {
            onSelectCallback(this);
        };

        textName.text = drHeroShop.Name;
        textDes.text = drHeroShop.Des;

        string assetName = AssetUtility.GetUISpriteAsset (drHeroShop.AssetName);

        GameEntry.Resource.LoadAsset (assetName, new LoadAssetCallbacks (
            (_assetName, _asset, _duration, _userData) => {

                imgHero.sprite = ((GameObject)_asset).GetComponent<SpriteRenderer>().sprite;
            },
            (string _assetName, LoadResourceStatus status, string errorMessage, object userData) => {
                Log.Warning("error:" + errorMessage);
            }
        ));
    }

    public DRHeroShop GetHeroShop() {
        return drHeroShop;
    }
}