using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class UIPlayerMessage : UGuiForm {
    [SerializeField]
    private Text prizeText = null;
    [SerializeField]
    private Text atkText = null;
    [SerializeField]
    private Text defText = null;
    [SerializeField]
    private Text hpText = null;

    /// <summary>
    /// 累计获得奖励
    /// </summary>
    private int totalPrize = 0;

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
        GameEntry.Event.Subscribe(RefreshHeroPropsEventArgs.EventId, OnRefreshHeroProps);
    }

    /// <summary>
    /// 界面关闭。
    /// </summary>
    /// <param name="userData">用户自定义数据。</param>
    protected override void OnClose (object userData) {
        base.OnClose(userData);

        /* 取消订阅事件 */
        GameEntry.Event.Unsubscribe(DeadEventArgs.EventId, OnDeadEvent);
        GameEntry.Event.Unsubscribe(RefreshHeroPropsEventArgs.EventId, OnRefreshHeroProps);
    }

    private void OnDeadEvent (object sender, GameEventArgs e) {
        DeadEventArgs deadEventArgs = e as DeadEventArgs;

        if (deadEventArgs.CampType == CampType.Enemy) {
            MonsterData data = (MonsterData)deadEventArgs.EntityData;
            totalPrize += data.Prize;
            
            prizeText.text = totalPrize.ToString();

        } else if (deadEventArgs.CampType == CampType.Player) {
            Log.Info("游戏结束");
        }
    }

    private void OnRefreshHeroProps(object sender, GameEventArgs e) {
        RefreshHeroPropsEventArgs eventArgs = e as RefreshHeroPropsEventArgs;

        atkText.text = eventArgs.HeroData.Atk.ToString();
        defText.text = eventArgs.HeroData.Def.ToString();
        hpText.text = eventArgs.HeroData.HP.ToString();
    }

    /// <summary>
    /// 总共获得的金币奖励数量
    /// </summary>
    /// <returns></returns>
    public int TotalPrize() {
        return totalPrize;
    }
}