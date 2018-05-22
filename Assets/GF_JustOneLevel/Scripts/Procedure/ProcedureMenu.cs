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

    protected override void OnInit (ProcedureOwner procedureOwner) {
        base.OnInit (procedureOwner);

        this.m_ProcedureOwner = procedureOwner;
    }

    protected override void OnEnter (ProcedureOwner procedureOwner) {
        base.OnEnter (procedureOwner);

        // 播放音乐
        GameEntry.Sound.PlayMusic(Constant.Sound.MENU_MUSIC_ID);

        // 订阅事件
        GameEntry.Event.Subscribe (OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

        // 加载UI
        GameEntry.UI.OpenUIForm (AssetUtility.GetUIFormAsset ("UIMenu"), "DefaultGroup", this);
        
    }

    protected override void OnLeave (ProcedureOwner procedureOwner, bool isShutdown) {
        base.OnLeave (procedureOwner, isShutdown);
        
        // 取消订阅
        GameEntry.Event.Unsubscribe (OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

        // 停止音乐
        GameEntry.Sound.StopMusic();
        
        // 关闭UI
        if (uiMenu != null) {
            GameEntry.UI.CloseUIForm (uiMenu.UIForm);
            uiMenu = null;
        }
    }

    protected override void OnUpdate (ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds) {
    }
    
    private void OnOpenUIFormSuccess (object sender, GameEventArgs e) {
        OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs) e;

        if (ne.UIForm.Logic.GetType () == typeof (UIMenu)) {
            uiMenu = (UIMenu) ne.UIForm.Logic;
        }
    }

    public void StartGame() {
        m_ProcedureOwner.SetData<VarInt> (Constant.ProcedureData.NextSceneId, GameEntry.Config.GetInt ("Scene.Game"));
        ChangeState<ProcedureChangeScene> (m_ProcedureOwner);
    }
}