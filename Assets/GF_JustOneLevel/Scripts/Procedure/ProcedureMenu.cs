using GameFramework;
using GameFramework.Event;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using System;
using System.Threading;

public class ProcedureMenu : ProcedureBase {
    private UIMenu m_UIMenu = null;

    protected override void OnInit (ProcedureOwner procedureOwner) {
        base.OnInit (procedureOwner);

        this.m_ProcedureOwner = procedureOwner;
    }

    protected override void OnEnter (ProcedureOwner procedureOwner) {
        base.OnEnter (procedureOwner);

        // 订阅事件
        GameEntry.Event.Subscribe (OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

        // 加载UI
        GameEntry.UI.OpenUIForm (AssetUtility.GetUIFormAsset ("UIMenu"), "DefaultGroup", this);
        
    }

    protected override void OnLeave (ProcedureOwner procedureOwner, bool isShutdown) {
        base.OnLeave (procedureOwner, isShutdown);
        
        // 取消订阅
        GameEntry.Event.Unsubscribe (OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

        // 关闭UI
        if (m_UIMenu != null) {
            GameEntry.UI.CloseUIForm (m_UIMenu.UIForm);
            m_UIMenu = null;
        }
    }

    protected override void OnUpdate (ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds) {
    }
    
    private void OnOpenUIFormSuccess (object sender, GameEventArgs e) {
        OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs) e;

        if (ne.UIForm.Logic.GetType () == typeof (UIMenu)) {
            m_UIMenu = (UIMenu) ne.UIForm.Logic;
        }
    }

    public void StartGame() {
        m_ProcedureOwner.SetData<VarInt> (Constant.ProcedureData.NextSceneId, GameEntry.Config.GetInt ("Scene.Game"));
        ChangeState<ProcedureChangeScene> (m_ProcedureOwner);
    }
}