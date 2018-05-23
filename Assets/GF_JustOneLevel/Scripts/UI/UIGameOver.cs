using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class UIGameOver : UGuiForm {
    private ProcedureGame procedureGame = null;

    /// <summary>
    /// 界面打开。
    /// </summary>
    /// <param name="userData">用户自定义数据。</param>
    protected override void OnOpen (object userData) {
        base.OnOpen(userData);

        procedureGame = userData as ProcedureGame;

        /* 订阅事件 */
        GameEntry.Event.Subscribe(DeadEventArgs.EventId, OnDeadEvent);
    }

    /// <summary>
    /// 界面关闭。
    /// </summary>
    /// <param name="userData">用户自定义数据。</param>
    protected override void OnClose (object userData) {
        base.OnClose(userData);

        /* 取消订阅事件 */
        GameEntry.Event.Unsubscribe(DeadEventArgs.EventId, OnDeadEvent);
    }

    private void OnDeadEvent (object sender, GameEventArgs e) {
        DeadEventArgs deadEventArgs = e as DeadEventArgs;
        
        if (deadEventArgs.CampType == CampType.Player) {
            Log.Info("游戏结束");
        }
    }

    public void OnContinueClick() {
        if (procedureGame != null) {
            procedureGame.Continue();
        }
    }

    public void OnBackClick() {
        if (procedureGame != null) {
            procedureGame.Back();
        }
    }
}