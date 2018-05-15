using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class UIPlayerMessage : UGuiForm {
    [SerializeField]
    private Text m_PrizeText = null;

    /// <summary>
    /// 累计获得奖励
    /// </summary>
    private int m_TotalPrize = 0;

    private ProcedureGame m_ProcedureGame = null;

    /// <summary>
    /// 界面打开。
    /// </summary>
    /// <param name="userData">用户自定义数据。</param>
    protected override void OnOpen (object userData) {
        base.OnOpen(userData);

        m_ProcedureGame = userData as ProcedureGame;

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

    protected void OnDeadEvent (object sender, GameEventArgs e) {
        DeadEventArgs deadEventArgs = e as DeadEventArgs;

        if (deadEventArgs.CampType == CampType.Enemy) {
            m_TotalPrize += deadEventArgs.Prize;
            
            m_PrizeText.text = m_TotalPrize.ToString();
        }
    }

    /// <summary>
    /// 总共获得的金币奖励数量
    /// </summary>
    /// <returns></returns>
    public int TotalPrize() {
        return m_TotalPrize;
    }
}