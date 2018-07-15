using GameFramework;
using GameFramework.Event;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using System;
using System.Threading;

public class ProcedureMenu : ProcedureBase {
    private UIMenu uiMenu = null;
    private UIHeroShop uiHeroShop = null;

    protected override void OnInit (ProcedureOwner procedureOwner) {
        base.OnInit (procedureOwner);

        this.m_ProcedureOwner = procedureOwner;
    }

    protected override void OnEnter (ProcedureOwner procedureOwner) {
        base.OnEnter (procedureOwner);

        // 播放音乐
        GameEntry.Sound.PlayMusic (Constant.Sound.MENU_MUSIC_ID);

        // 订阅事件
        GameEntry.Event.Subscribe (OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

        // 加载UI
        GameEntry.UI.OpenUIForm (UIFormId.Menu, this);

    }

    protected override void OnLeave (ProcedureOwner procedureOwner, bool isShutdown) {
        base.OnLeave (procedureOwner, isShutdown);

        // 取消订阅
        GameEntry.Event.Unsubscribe (OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

        // 停止音乐
        GameEntry.Sound.StopMusic ();

        // 关闭UI
        if (uiMenu != null) {
            GameEntry.UI.CloseUIForm (uiMenu.UIForm);
            uiMenu = null;
        }

        CloseHeroShop ();
    }

    protected override void OnUpdate (ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds) { }

    private void OnOpenUIFormSuccess (object sender, GameEventArgs e) {
        OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs) e;

        Type logicType = ne.UIForm.Logic.GetType ();
        if (logicType == typeof (UIMenu)) {
            uiMenu = (UIMenu) ne.UIForm.Logic;
        } else if (logicType == typeof (UIHeroShop)) {
            uiHeroShop = (UIHeroShop) ne.UIForm.Logic;
        }
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame () {
        m_ProcedureOwner.SetData<VarInt> (Constant.ProcedureData.NextSceneId, GameEntry.Config.GetInt ("Scene.Game"));
        ChangeState<ProcedureChangeScene> (m_ProcedureOwner);
    }

    /// <summary>
    /// 显示英雄商店界面
    /// </summary>
    public void ShowHeroShop () {
        // 加载英雄商店UI
        GameEntry.UI.OpenUIForm (UIFormId.HeroShop, this);
    }

    /// <summary>
    /// 关闭英雄商店界面
    /// </summary>
    public void CloseHeroShop () {
        if (uiHeroShop != null) {
            GameEntry.UI.CloseUIForm (uiHeroShop);
            uiHeroShop = null;
        }
    }

    /// <summary>
    /// 购买英雄
    /// </summary>
    /// <param name="drHeroShop"></param>
    public bool BuyHero (DRHeroShop drHeroShop) {
        int gold = PlayerData.Gold;

        if (gold <= drHeroShop.Price) {
            GameEntry.UI.OpenDialog (new DialogParams () {
                Title = GameEntry.Localization.GetString ("Alert.OperateFail"),
                    Message = GameEntry.Localization.GetString ("Message.GoldNotEnough"),
            });

            return false;
        } else {
            PlayerData.Gold = gold - drHeroShop.Price;
            PlayerData.AddHero (drHeroShop.Id);

            GameEntry.UI.OpenDialog (new DialogParams () {
                Title = GameEntry.Localization.GetString ("Alert.OperateSuccess"),
                    Message = GameEntry.Localization.GetString ("Message.BuySuccess"),
            });

            return true;
        }
    }

    /// <summary>
    /// 设置出战英雄
    /// </summary>
    /// <param name="drHeroShop"></param>
    public void SetFightHero (DRHeroShop drHeroShop) {
        PlayerData.SetFightHero(drHeroShop.Id);
    }
}