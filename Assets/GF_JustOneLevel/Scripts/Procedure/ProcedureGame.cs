using GameFramework;
using GameFramework.Event;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using System;

public class ProcedureGame : ProcedureBase {
    private SurvivalGame m_SurvivalGame = null;
    /// <summary>
    /// 玩家操作UI
    /// </summary>
    private UIPlayerOperate m_UIPlayerOperate = null;
    /// <summary>
    /// 玩家信息UI
    /// </summary>
    private UIPlayerMessage m_UIPlayerMessage = null;

    protected override void OnInit (ProcedureOwner procedureOwner) {
        base.OnInit (procedureOwner);

        m_SurvivalGame = new SurvivalGame ();
    }

    protected override void OnEnter (ProcedureOwner procedureOwner) {
        base.OnEnter (procedureOwner);

        m_SurvivalGame.Initialize ();

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
        if (m_UIPlayerOperate != null) {
            GameEntry.UI.CloseUIForm (m_UIPlayerOperate.UIForm);
            m_UIPlayerOperate = null;
        }

        if (m_UIPlayerMessage != null) {
            GameEntry.UI.CloseUIForm (m_UIPlayerMessage.UIForm);
            m_UIPlayerMessage = null;
        }
    }

    protected override void OnUpdate (ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds) {
        if (m_SurvivalGame != null) {
            if (!m_SurvivalGame.GameOver) {
                m_SurvivalGame.Update (elapseSeconds, realElapseSeconds);
            } else {
                GameOver (procedureOwner);
            }
        }
    }

    /// <summary>
    /// 返回菜单
    /// </summary>
    public void Back () {
        m_SurvivalGame.Shutdown();
        m_ProcedureOwner.SetData<VarInt> (Constant.ProcedureData.NextSceneId, GameEntry.Config.GetInt ("Scene.Menu"));
        ChangeState<ProcedureChangeScene> (m_ProcedureOwner);
    }

    private void GameOver (ProcedureOwner procedureOwner) {
        // 保存获得的金币
        int gold = GameEntry.Setting.GetInt (Constant.Player.Gold, 0);
        GameEntry.Setting.SetInt (Constant.Player.Gold, gold + m_UIPlayerMessage.TotalPrize ());

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
            m_UIPlayerOperate = (UIPlayerOperate) ne.UIForm.Logic;
        } else if (ne.UIForm.Logic.GetType () == typeof (UIPlayerMessage)) {
            m_UIPlayerMessage = (UIPlayerMessage) ne.UIForm.Logic;
        }
    }
}