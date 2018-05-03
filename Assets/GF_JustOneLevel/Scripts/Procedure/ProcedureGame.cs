using GameFramework;
using GameFramework.Event;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using System;

public class ProcedureGame : ProcedureBase {
    private SurvivalGame m_SurvivalGame = null;
    private UIPlayerOperate m_UIPlayerOperate = null;
    private float time = 0;

    protected override void OnInit (ProcedureOwner procedureOwner) {
        base.OnInit (procedureOwner);

        m_SurvivalGame = new SurvivalGame ();
    }

    protected override void OnEnter (ProcedureOwner procedureOwner) {
        base.OnEnter (procedureOwner);

        m_SurvivalGame.Initialize ();

        // 加载UI
        GameEntry.Event.Subscribe (OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

        GameEntry.UI.OpenUIForm ("Assets/GF_JustOneLevel/Prefabs/UI/UIPlayerOperate.prefab", "DefaultGroup");
    }

    protected override void OnLeave (ProcedureOwner procedureOwner, bool isShutdown) {
        base.OnLeave (procedureOwner, isShutdown);

        GameEntry.Event.Unsubscribe (OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

        if (m_UIPlayerOperate != null) {
            GameEntry.UI.CloseUIForm (m_UIPlayerOperate.UIForm);
            m_UIPlayerOperate = null;
        }
    }

    protected override void OnUpdate (ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds) {
        if (m_SurvivalGame != null && !m_SurvivalGame.GameOver) {
            m_SurvivalGame.Update (elapseSeconds, realElapseSeconds);
            return;
        }
    }

    private void OnOpenUIFormSuccess (object sender, GameEventArgs e) {
        OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs) e;

        m_UIPlayerOperate = (UIPlayerOperate) ne.UIForm.Logic;
    }
}