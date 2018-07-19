using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class UIPlayerMessage : UGuiForm {
    [SerializeField]
    private Text killCountText = null;
    [SerializeField]
    private Text goldText = null;
    [SerializeField]
    private Text atkText = null;
    [SerializeField]
    private Text defText = null;
    [SerializeField]
    private Text atkSpeedText = null;
    [SerializeField]
    private Text hpText = null;
    [SerializeField]
    private Text mpText = null;

    /// <summary>
    /// 累计获得奖励
    /// </summary>
    private int totalPrize = 0;
    /// <summary>
    /// 击杀数量
    /// </summary>
    private int killCount = 0;

    /// <summary>
    /// 界面打开。
    /// </summary>
    /// <param name="userData">用户自定义数据。</param>
    protected override void OnOpen (object userData) {
        base.OnOpen (userData);

        totalPrize = 0;
        killCount = 0;
        
        RefreshGold ();

        /* 订阅事件 */
        GameEntry.Event.Subscribe (DeadEventArgs.EventId, OnDeadEvent);
        GameEntry.Event.Subscribe (RefreshHeroPropsEventArgs.EventId, OnRefreshHeroProps);
        GameEntry.Event.Subscribe (RefreshGoldEventArgs.EventId, OnRefreshGoldProps);
    }

    /// <summary>
    /// 界面关闭。
    /// </summary>
    /// <param name="userData">用户自定义数据。</param>
    protected override void OnClose (object userData) {
        base.OnClose (userData);

        /* 取消订阅事件 */
        GameEntry.Event.Unsubscribe (DeadEventArgs.EventId, OnDeadEvent);
        GameEntry.Event.Unsubscribe (RefreshHeroPropsEventArgs.EventId, OnRefreshHeroProps);
        GameEntry.Event.Unsubscribe (RefreshGoldEventArgs.EventId, OnRefreshGoldProps);
    }

    /// <summary>
    /// 刷新金币信息
    /// </summary>
    public void RefreshGold () {
        goldText.text = PlayerData.Gold.ToString ();
    }

    /// <summary>
    /// 刷新怪物击杀数量
    /// </summary>
    private void RefreshKillCount () {
        killCountText.text = killCount.ToString();
    }

    private void OnDeadEvent (object sender, GameEventArgs e) {
        DeadEventArgs deadEventArgs = e as DeadEventArgs;

        if (deadEventArgs.CampType == CampType.Enemy) {
            MonsterData data = (MonsterData) deadEventArgs.EntityData;
            totalPrize += data.Prize;

            // 保存获得的金币
            int gold = PlayerData.Gold;
            PlayerData.Gold = gold + data.Prize;

            RefreshGold ();

            // 累积击杀数量
            killCount++;
            RefreshKillCount();
        }
    }

    private void OnRefreshHeroProps (object sender, GameEventArgs e) {
        RefreshHeroPropsEventArgs eventArgs = e as RefreshHeroPropsEventArgs;

        string atkSpeedStr = eventArgs.HeroData.AtkSpeed.ToString("F4");

        atkText.text = eventArgs.HeroData.Atk.ToString ();
        defText.text = eventArgs.HeroData.Def.ToString ();
        atkSpeedText.text = $"{atkSpeedStr}s";
        hpText.text = $"{eventArgs.HeroData.HP}/{eventArgs.HeroData.MaxHP}";
        mpText.text = $"{eventArgs.HeroData.MP}/{eventArgs.HeroData.MaxMP}";
    }

    private void OnRefreshGoldProps (object sender, GameEventArgs e) {
        RefreshGold();
    }
}