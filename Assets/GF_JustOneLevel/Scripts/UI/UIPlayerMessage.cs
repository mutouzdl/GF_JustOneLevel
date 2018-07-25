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
    [SerializeField]
    private Text timeText = null;


    /// <summary>
    /// 界面打开。
    /// </summary>
    /// <param name="userData">用户自定义数据。</param>
    protected override void OnOpen (object userData) {
        base.OnOpen (userData);

        GlobalGame.totalPrize = 0;
        GlobalGame.killCount = 0;
        
        RefreshGold ();
        RefreshKillCount();

        HeroData heroData = new HeroData (EntityExtension.GenerateSerialId (), PlayerData.CurrentFightHeroID, CampType.Player);

        RefreshHeroMsg(heroData);

        /* 订阅事件 */
        GameEntry.Event.Subscribe (DeadEventArgs.EventId, OnDeadEvent);
        GameEntry.Event.Subscribe (RefreshHeroPropsEventArgs.EventId, OnRefreshHeroProps);
        GameEntry.Event.Subscribe (RefreshGoldEventArgs.EventId, OnRefreshGoldProps);
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds) {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        timeText.text = $"{GlobalGame.GameTimes.ToString("F0")}s";
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
        killCountText.text = GlobalGame.killCount.ToString();
    }

    /// <summary>
    /// 刷新英雄信息
    /// </summary>
    /// <param name="heroData"></param>
    private void RefreshHeroMsg(HeroData heroData) {
        string atkSpeedStr = heroData.AtkSpeed.ToString("F4");

        atkText.text = heroData.Atk.ToString ();
        defText.text = heroData.Def.ToString ();
        atkSpeedText.text = $"{atkSpeedStr}s";
        hpText.text = $"{heroData.HP}/{heroData.MaxHP}";
        mpText.text = $"{heroData.MP}/{heroData.MaxMP}";
    }

    private void OnDeadEvent (object sender, GameEventArgs e) {
        DeadEventArgs deadEventArgs = e as DeadEventArgs;

        if (deadEventArgs.CampType == CampType.Enemy) {
            MonsterData data = (MonsterData) deadEventArgs.EntityData;
            GlobalGame.totalPrize += data.Prize;

            // 保存获得的金币
            int gold = PlayerData.Gold;
            PlayerData.Gold = gold + data.Prize;

            RefreshGold ();

            // 累积击杀数量
            GlobalGame.killCount++;
            RefreshKillCount();
        }
    }

    private void OnRefreshHeroProps (object sender, GameEventArgs e) {
        RefreshHeroPropsEventArgs eventArgs = e as RefreshHeroPropsEventArgs;

        RefreshHeroMsg(eventArgs.HeroData);
    }

    private void OnRefreshGoldProps (object sender, GameEventArgs e) {
        RefreshGold();
    }
}