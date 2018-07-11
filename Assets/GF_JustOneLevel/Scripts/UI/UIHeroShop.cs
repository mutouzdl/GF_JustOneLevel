using GameFramework;
using GameFramework.DataTable;
using GameFramework.Resource;
using GameFramework.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class UIHeroShop : UGuiForm {
    [SerializeField]
    private Transform panelHeroParent = null;

    [SerializeField]
    private GameObject buttonBuy = null;
    [SerializeField]
    private GameObject buttonFight = null;
    [SerializeField]
    private Text textPrice = null;
    [SerializeField]
    private Text textGold = null;

    private ProcedureMenu procedureMenu = null;
    private UIHeroShop_PanelHero currentSelectHeroPanel = null;

    void Start () {
        textGold.text = PlayerData.Gold.ToString ();
    }

    protected override void OnInit (object userData) {
        base.OnInit (userData);

        /* 创建英雄面板 */
        IDataTable<DRHeroShop> dtHeroShop = GameEntry.DataTable.GetDataTable<DRHeroShop> ();
        DRHeroShop[] drHeroShops = dtHeroShop.GetAllDataRows ();

        string assetName = AssetUtility.GetUIFormAsset ("UIHeroShop_PanelHero");

        GameEntry.Resource.LoadAsset (assetName, new LoadAssetCallbacks (
            (_assetName, _asset, _duration, _userData) => {
                foreach (DRHeroShop drHeroShop in drHeroShops) {
                    GameObject panelHeroObj = (GameObject) Instantiate ((Object) _asset);
                    panelHeroObj.transform.SetParent (panelHeroParent, false);

                    UIHeroShop_PanelHero panelHero = panelHeroObj.GetComponent<UIHeroShop_PanelHero> ();
                    panelHero.Init (drHeroShop, OnHeroPanelClick);
                }
            }
        ));
    }

    /// <summary>
    /// 界面打开。
    /// </summary>
    /// <param name="userData">用户自定义数据。</param>
    protected override void OnOpen (object userData) {
        base.OnOpen (userData);

        procedureMenu = userData as ProcedureMenu;
    }

    /// <summary>
    /// 点击购买按钮
    /// </summary>
    public void OnBuyClick () {
        if (procedureMenu != null && currentSelectHeroPanel != null) {
            if (procedureMenu.BuyHero (currentSelectHeroPanel.GetHeroShop ())) {
                textGold.text = PlayerData.Gold.ToString ();
                OnHeroPanelClick (currentSelectHeroPanel);
            }
        }
    }

    /// <summary>
    /// 点击出战按钮
    /// </summary>
    public void OnFightClick () {
        if (procedureMenu != null && currentSelectHeroPanel != null) {
            procedureMenu.SetFightHero (currentSelectHeroPanel.GetHeroShop ());
            OnHeroPanelClick (currentSelectHeroPanel);
        }
    }

    /// <summary>
    /// 点击返回按钮
    /// </summary>
    public void OnBackClick () {
        if (procedureMenu != null) {
            procedureMenu.CloseHeroShop ();
        }
    }

    /// <summary>
    /// 点击英雄面板
    /// </summary>
    public void OnHeroPanelClick (UIHeroShop_PanelHero heroPanel) {
        currentSelectHeroPanel = heroPanel;
        DRHeroShop drHeroShop = heroPanel.GetHeroShop ();

        // 显示/隐藏购买按钮
        if (PlayerData.HasHero (drHeroShop.Id)) {
            buttonBuy.SetActive (false);
            string boughtText = GameEntry.Localization.GetString ("HeroShop.Bought");

            // 显示/隐藏出战按钮
            if (PlayerData.CurrentFightHeroID != drHeroShop.Id) {
                buttonFight.SetActive (true);
                textPrice.text = $"{boughtText}";
            } else {
                buttonFight.SetActive (false);

                string fightText = GameEntry.Localization.GetString ("HeroShop.Fight");
                textPrice.text = $"{boughtText}，{fightText}";
            }
        } else {
            buttonBuy.SetActive (true);
            buttonFight.SetActive (false);
            string goldText = GameEntry.Localization.GetString ("Message.Gold");
            textPrice.text = $"{drHeroShop.Price}{goldText}";
        }
    }
}