using GameFramework;
using GameFramework.Event;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using System;

public class ProcedureGame : ProcedureBase {
    private SurvivalGame survivalGame = null;
    /// <summary>
    /// 玩家操作UI
    /// </summary>
    private UIPlayerOperate uiPlayerOperate = null;
    /// <summary>
    /// 玩家信息UI
    /// </summary>
    private UIPlayerMessage uiPlayerMessage = null;

    protected override void OnInit (ProcedureOwner procedureOwner) {
        base.OnInit (procedureOwner);

        survivalGame = new SurvivalGame ();
    }

    protected override void OnEnter (ProcedureOwner procedureOwner) {
        base.OnEnter (procedureOwner);

        survivalGame.Initialize ();

        // 播放音乐
        GameEntry.Sound.PlayMusic(Constant.Sound.GAME_MUSIC_ID);

        // 订阅事件
        GameEntry.Event.Subscribe (OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

        // 加载UI
        GameEntry.UI.OpenUIForm (AssetUtility.GetUIFormAsset ("UIPlayerOperate"), "DefaultGroup", this);
        GameEntry.UI.OpenUIForm (AssetUtility.GetUIFormAsset ("UIPlayerMessage"), "DefaultGroup", this);
    }

    protected override void OnLeave (ProcedureOwner procedureOwner, bool isShutdown) {
        base.OnLeave (procedureOwner, isShutdown);

        // 取消订阅
        GameEntry.Event.Unsubscribe (OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

        // 停止音乐
        GameEntry.Sound.StopMusic();
        
        // 关闭UI
        if (uiPlayerOperate != null) {
            GameEntry.UI.CloseUIForm (uiPlayerOperate.UIForm);
            uiPlayerOperate = null;
        }

        if (uiPlayerMessage != null) {
            GameEntry.UI.CloseUIForm (uiPlayerMessage.UIForm);
            uiPlayerMessage = null;
        }
    }

    protected override void OnUpdate (ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds) {
        if (survivalGame != null) {
            if (!survivalGame.GameOver) {
                survivalGame.Update (elapseSeconds, realElapseSeconds);
            } else {
                GameOver (procedureOwner);
            }
        }
    }

    /// <summary>
    /// 返回菜单
    /// </summary>
    public void Back () {
        survivalGame.Shutdown();
        m_ProcedureOwner.SetData<VarInt> (Constant.ProcedureData.NextSceneId, GameEntry.Config.GetInt ("Scene.Menu"));
        ChangeState<ProcedureChangeScene> (m_ProcedureOwner);
    }

    private void GameOver (ProcedureOwner procedureOwner) {
        // 保存获得的金币
        int gold = GameEntry.Setting.GetInt (Constant.Player.Gold, 0);
        GameEntry.Setting.SetInt (Constant.Player.Gold, gold + uiPlayerMessage.TotalPrize ());

        // 返回菜单场景
        BackToMenu (procedureOwner);
    }

    /// <summary>
    /// 返回菜单
    /// </summary>
    private void BackToMenu (ProcedureOwner procedureOwner) {
    }

    private void OnOpenUIFormSuccess (object sender, GameEventArgs e) {
        OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs) e;

        if (ne.UIForm.Logic.GetType () == typeof (UIPlayerOperate)) {
            uiPlayerOperate = (UIPlayerOperate) ne.UIForm.Logic;
        } else if (ne.UIForm.Logic.GetType () == typeof (UIPlayerMessage)) {
            uiPlayerMessage = (UIPlayerMessage) ne.UIForm.Logic;
        }
    }
}