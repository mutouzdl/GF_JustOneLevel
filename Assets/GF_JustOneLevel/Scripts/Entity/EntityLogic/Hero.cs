using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameFramework;
using GameFramework.Event;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;

public class Hero : FightEntity {
    private HeroData heroData = null;

    /// <summary>
    /// 状态类状态机：CD空闲、CD
    /// </summary>
    private GameFramework.Fsm.IFsm<Hero> heroStateFsm;
    /// <summary>
    /// 行动类状态机：空闲、行走、攻击、受伤
    /// </summary>
    private GameFramework.Fsm.IFsm<Hero> heroActionFsm;
    public WeaponTrialController weaponTrailController;
    private TextMesh msgText = null;

    protected override void OnInit (object userData) {
        base.OnInit (userData);

        msgText = this.gameObject.GetComponentInChildren<TextMesh> ();;
        GameObject weaponTrailObj = GameObject.FindGameObjectWithTag ("WeaponTrail");
        WeaponTrail weaponTrail = weaponTrailObj.GetOrAddComponent<WeaponTrail> ();
        weaponTrail.height = 0.6f;
        weaponTrail.time = 0.3f;

        weaponTrailController = new WeaponTrialController (weaponTrail);
    }

    protected override void OnShow (object userData) {
        base.OnShow (userData);

        heroData = userData as HeroData;
        if (heroData == null) {
            Log.Error ("Hero data is invalid.");
            return;
        }

        weaponTrailController.Reset ();

        /* 如果不想一直显示武器效果，可以注释掉下面这行，并且把PlayTrailEffect和ClearTrailEffect里的函数注释放开 */
        weaponTrailController.PlayTrailEffect ();

        ResetAtkCD ();

        /* 初始化状态机 */
        InitFSM ();

        /* 加载武器 */
        InitWeapon ();

        /* 刷新描述文本 */
        RefreshMsgText();

        /* 订阅事件 */
        SubscribeEvent ();

        /* 发送刷新属性消息 */
        SendRefreshPropEvent ();
    }

    /// <summary>
    /// 初始化状态机
    /// </summary>
    private void InitFSM () {
        /* 创建状态机 */
        heroStateFsm = GameEntry.Fsm.CreateFsm<Hero> ("heroStateFsm", this, new FsmState<Hero>[] {
            new HeroCDIdleState (),
                new HeroAtkCDState (),
        });

        heroActionFsm = GameEntry.Fsm.CreateFsm<Hero> ("heroActionFsm", this, new FsmState<Hero>[] {
            new HeroIdleState (),
                new HeroWalkState (),
                new HeroAtkState (),
                new HeroHurtState (),
                new HeroDeadState (),
        });

        /* 启动状态机 */
        heroStateFsm.Start<HeroCDIdleState> ();
        heroActionFsm.Start<HeroIdleState> ();
    }

    /// <summary>
    /// 订阅事件
    /// </summary>
    private void SubscribeEvent () {
        GameEntry.Event.Subscribe (ClickAttackButtonEventArgs.EventId, OnClickAttackButton);
        GameEntry.Event.Subscribe (DeadEventArgs.EventId, OnDeadEvent);
        GameEntry.Event.Subscribe (ResurgenceEventArgs.EventId, OnResurgenceEvent);
    }

    /// <summary>
    /// 刷新描述文本
    /// </summary>
    private void RefreshMsgText () {
        if (msgText == null) {
            return;
        }

        int power = this.heroData.GetPower();
        int powerLevel = this.heroData.GetPowerLevel(power);

        msgText.text = $"{this.heroData.Name}(战力：{power})";

        /* 显示战斗力图标 */
        GameObject powerRawImagePanelObj = GameObject.FindGameObjectWithTag("PowerRawImagePanel");
        RectTransform powerRawImagePanel = powerRawImagePanelObj.GetComponent<RectTransform>();

        // 1. 如果图标总数量小于战斗力等级，则新增图标
        if (powerRawImagePanel.childCount < powerLevel) {
            GameObject powerRawImage = powerRawImagePanel.GetChild(0).gameObject;
            for (int i = 0; i < powerLevel - powerRawImagePanel.childCount; i++) {
                GameObject child = GameObject.Instantiate(powerRawImage);
                child.transform.SetParent(powerRawImagePanel, false);
            }
        }

        // 2. 计算已经显示的图标数量
        int powerLevelIconCount = 0;
        for (int i = 0; i < powerRawImagePanel.childCount; i++) {
            if (powerRawImagePanel.GetChild(i).gameObject.activeSelf) {
                powerLevelIconCount++;
            }
        }

        // 3. 显示图标数量，以达到战斗力等级
        for (int i = powerLevelIconCount; i < powerLevel; i++) {
            powerRawImagePanel.GetChild(i).gameObject.SetActive(true);
        }

        // 4. 隐藏多余的图标
        for (int i = powerLevel; i < powerRawImagePanel.childCount; i++) {
            powerRawImagePanel.GetChild(i).gameObject.SetActive(false);
        }
    }

    protected override void OnUpdate (float elapseSeconds, float realElapseSeconds) {
        base.OnUpdate (elapseSeconds, realElapseSeconds);
    }

    void LateUpdate () {
        weaponTrailController.Update ();
    }

    protected override void OnHide (object userData) {
        base.OnHide (userData);

        GameEntry.Fsm.DestroyFsm (heroActionFsm);
        GameEntry.Fsm.DestroyFsm (heroStateFsm);

        GameEntry.Event.Unsubscribe (ClickAttackButtonEventArgs.EventId, OnClickAttackButton);
        GameEntry.Event.Unsubscribe (DeadEventArgs.EventId, OnDeadEvent);
        GameEntry.Event.Unsubscribe (ResurgenceEventArgs.EventId, OnResurgenceEvent);
    }

    protected override void OnHurt () {
        GameEntry.Sound.PlaySound (Constant.Sound.HURT_SOUND_ID);

        /* 累积魔法值 */
        this.heroData.AddMP (1);

        /* 发送刷新属性消息 */
        SendRefreshPropEvent ();
    }

    protected override void OnCure () {
        /* 发送刷新属性消息 */
        SendRefreshPropEvent ();
    }

    protected override void OnDead () {
        base.OnDead ();
        heroActionFsm.FireEvent (this, DeadEventArgs.EventId, this.Id);
    }

    protected override IMoveController CreateMoveController () {
        GameObject leftJoystickObj = GameObject.FindGameObjectWithTag ("LeftJoystick");

        if (leftJoystickObj == null) {
            return null;
        }

        LeftJoystick leftJoystick = leftJoystickObj.GetOrAddComponent<LeftJoystick> ();
        leftJoystick.joystickStaysInFixedPosition = true;

        return new LeftJoystickMoveController (leftJoystick);
    }

    protected override void OnFireWeapon () {
        SendRefreshPropEvent ();
    }

    public override ImpactData GetImpactData () {
        return new ImpactData (heroData.Camp, heroData.HP, 0, heroData.Def);
    }

    /// <summary>
    /// 执行攻击
    /// </summary>
    public void PerformAttack () {
        if (IsDead) {
            return;
        }

        IsAtkCDing = true;
        heroStateFsm.FireEvent (this, HeroAttackEventArgs.EventId);

        // 发射手动触发类型的武器
        foreach (Weapon weapon in manualWeapons) {
            if (weapon.CostMP > 0 && heroData.MP < weapon.CostMP) {
                continue;
            }

            heroData.CostMP (weapon.CostMP);
            weapon.Attack (heroData.Atk);
        }

        SendRefreshPropEvent ();
    }

    /// <summary>
    /// 接受伤害
    /// </summary>
    /// <param name="damageHP"></param>
    public override void ApplyDamage (int damageHP) {
        heroActionFsm.FireEvent (this, ApplyDamageEventArgs.EventId, damageHP);
    }

    /// <summary>
    /// 复活
    /// </summary>
    public void Resurgence () {
        heroData.HP = heroData.MaxHP;

        /* 发送刷新属性消息 */
        SendRefreshPropEvent ();

        Time.timeScale = 1;
    }

    /// <summary>
    /// 播放拖尾效果
    /// </summary>
    public void PlayTrailEffect () {
        /* 英雄在攻击怪物时会调用这个函数，我注释掉是因为我希望武器效果一直存在，这样比较酷 */
        // weaponTrailController.PlayTrailEffect ();
    }

    /// <summary>
    /// 清除拖尾效果
    /// </summary>
    public void ClearTrialEffect () {
        /* 英雄在停止攻击时会调用这个函数，我注释掉是因为我希望武器效果一直存在，这样比较酷 */
        // weaponTrailController.ClearTrialEffect ();
    }

    public HeroData HeroData {
        get {
            return heroData;
        }
    }

    /// <summary>
    /// 发送刷新属性消息
    /// </summary>
    public void SendRefreshPropEvent () {
        GameEntry.Event.Fire (this,
            ReferencePool.Acquire<RefreshHeroPropsEventArgs> ().Fill (this.heroData));
    }

    /// <summary>
    /// 根据给定的属性，加强英雄
    /// </summary>
    public void PowerUpByAbsValue (int hp, int def, int atk, float atkSpeed) {
        this.heroData.PowerUpByAbsValue (hp, def, atk, atkSpeed);
        this.RefreshMsgText();
    }

    /// <summary>
    /// 吸收怪物属性，加强英雄属性
    /// </summary>
    /// <param name="data"></param>
    private void PowerUpByMonster (MonsterData data) {
        this.heroData.PowerUpByAbsorbPower (data.HP, data.Def, data.Atk, data.AtkSpeed);
        this.RefreshMsgText();
    }

    #region 事件消息
    private void OnClickAttackButton (object sender, GameEventArgs e) {
        if (IsDead) {
            return;
        }

        if (IsAtkCDing == false) {
            ClickAttackButtonEventArgs args = (ClickAttackButtonEventArgs) e;
            heroActionFsm.FireEvent (this, ClickAttackButtonEventArgs.EventId, args);
        }
    }

    private void OnResurgenceEvent (object sender, GameEventArgs e) {
        heroActionFsm.FireEvent (this, ResurgenceEventArgs.EventId);
    }

    private void OnDeadEvent (object sender, GameEventArgs e) {
        if (IsDead) {
            return;
        }

        DeadEventArgs deadEventArgs = e as DeadEventArgs;

        if (deadEventArgs.CampType == CampType.Enemy) {
            MonsterData data = (MonsterData) deadEventArgs.EntityData;

            /* 加强英雄属性 */
            PowerUpByMonster (data);

            /* 刷新血量条 */
            RefreshHPBar ();

            /* 发送刷新属性消息 */
            SendRefreshPropEvent ();
        }
    }
    #endregion
}