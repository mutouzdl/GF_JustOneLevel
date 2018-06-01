using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameFramework;
using GameFramework.Event;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;

public class Hero : TargetableObject {
    private HeroData heroData = null;

    /// <summary>
    /// 攻击是否正在冷却
    /// </summary>
    private bool isAtkCDing = false;
    /// <summary>
    /// 状态类状态机：CD空闲、CD
    /// </summary>
    private GameFramework.Fsm.IFsm<Hero> heroStateFsm;
    /// <summary>
    /// 行动类状态机：空闲、行走、攻击、受伤
    /// </summary>
    private GameFramework.Fsm.IFsm<Hero> heroActionFsm;
    public WeaponTrail weaponTrail;

    /// <summary>
    /// 移动控制器
    /// </summary>
    private IMoveController moveController = null;

    protected override void OnInit (object userData) {
        base.OnInit (userData);
        weaponTrail = FindObjectOfType<WeaponTrail> ();
}

protected override void OnShow (object userData) {
    base.OnShow (userData);

    heroData = userData as HeroData;
    if (heroData == null) {
        Log.Error ("Hero data is invalid.");
        return;
    }

    weaponTrail.SetTime (0.0f, 0, 1);
    ResetAtkCD ();

    /* 初始化状态机 */
    InitFSM ();

    /* 加载武器 */
    InitWeapon ();

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
/// 初始化武器
/// </summary>
private void InitWeapon () {
    List<WeaponData> weaponDatas = heroData.GetWeaponDatas ();
    for (int i = 0; i < weaponDatas.Count; i++) {
        EntityExtension.ShowWeapon (typeof (Weapon), "WeaponGroup", weaponDatas[i]);
    }
}

/// <summary>
/// 订阅事件
/// </summary>
private void SubscribeEvent () {
    GameEntry.Event.Subscribe (ClickAttackButtonEventArgs.EventId, OnClickAttackButton);
    GameEntry.Event.Subscribe (DeadEventArgs.EventId, OnDeadEvent);
    GameEntry.Event.Subscribe (ResurgenceEventArgs.EventId, OnResurgenceEvent);
}

protected override void OnUpdate (float elapseSeconds, float realElapseSeconds) {
    base.OnUpdate (elapseSeconds, realElapseSeconds);

    /* 旋转镜头 */
    Vector3 inputVec = MoveController.GetInput ();
    if (inputVec.x != 0) {
        transform.Rotate (new Vector3 (0, inputVec.x * Time.deltaTime * heroData.RotateSpeed, 0));
    }
}

private float t = 0.033f;
private float animationIncrement = 0.003f;
private float tempT = 0;
void LateUpdate () {
    t = Mathf.Clamp (Time.deltaTime, 0, 0.066f);

    if (t > 0) {
        while (tempT < t) {
            tempT += animationIncrement;

            if (weaponTrail.time > 0) {
                weaponTrail.Itterate (Time.time - t + tempT);
            } else {
                weaponTrail.ClearTrail ();
            }
        }

        tempT -= t;

        if (weaponTrail.time > 0) {
            weaponTrail.UpdateTrail (Time.time, t);
        }
    }
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

    /* 累积愤怒值 */
    this.heroData.AddMP (1);

    /* 发送刷新属性消息 */
    SendRefreshPropEvent ();
}

protected override void OnDead () {
    base.OnDead ();
    heroActionFsm.FireEvent (this, DeadEventArgs.EventId, this.Id);
}

public override ImpactData GetImpactData () {
    return new ImpactData (heroData.Camp, heroData.HP, 0, heroData.Def);
}

/// <summary>
/// 向前移动
/// </summary>
/// <param name="distance"></param>
public void Forward (float distance) {
    Vector3 nextPos = CachedTransform.position + CachedTransform.forward * distance * heroData.MoveSpeed;

    CachedTransform.position = PositionUtility.GetAjustPositionWithMap (nextPos);
}

/// <summary>
/// 是否在攻击范围内
/// </summary>
/// <param name="distance"></param>
/// <returns></returns>
public bool CheckInAtkRange (float distance) {
    return distance <= heroData.AtkRange;
}

/// <summary>
/// 执行攻击
/// </summary>
/// <param name="aimEntity">攻击目标</param>
public void PerformAttack (TargetableObject aimEntity) {
    isAtkCDing = true;
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
/// 选择指定的武器开火
/// </summary>
/// <param name="attackType"></param>
/// <param name="weaponID"></param>
public void FireWeapon (WeaponAttackType attackType, int weaponID) {
    Weapon weapon = null;
    switch (attackType) {
        case WeaponAttackType.手动触发:
            weapon = manualWeapons.Where (w => w.GetTypeId () == weaponID).SingleOrDefault ();
            break;
        case WeaponAttackType.自动触发:
            weapon = autoWeapons.Where (w => w.GetTypeId () == weaponID).SingleOrDefault ();
            break;
        case WeaponAttackType.技能触发:
            weapon = skillWeapons.Where (w => w.GetTypeId () == weaponID).SingleOrDefault ();
            break;
    }

    if (weapon != null) {
        if (weapon.CostMP > 0 && heroData.MP < weapon.CostMP) {
            return;
        }

        heroData.CostMP (weapon.CostMP);
        weapon.Attack (heroData.Atk);

        SendRefreshPropEvent ();
    }
}

/// <summary>
/// 播放拖尾效果
/// </summary>
public void PlayTrailEffect () {
    if (weaponTrail == null) {
        return;
    }

    //设置拖尾时长  
    weaponTrail.SetTime (2.0f, 0.0f, 1.0f);

    //开始进行拖尾  
    weaponTrail.StartTrail (0.5f, 0.4f);
}

/// <summary>
/// 清除拖尾效果
/// </summary>
public void ClearTrialEffect () {
    if (weaponTrail == null) {
        return;
    }

    //清除拖尾  
    weaponTrail.ClearTrail ();
}

/// <summary>
/// 接受伤害
/// </summary>
/// <param name="damageHP"></param>
public override void ApplyDamage (int damageHP) {
    heroActionFsm.FireEvent (this, ApplyDamageEventArgs.EventId, damageHP);
}

/// <summary>
/// 重置攻击冷却
/// </summary>
public void ResetAtkCD () {
    isAtkCDing = false;
}

/// <summary>
/// 复活
/// </summary>
public void Resurgence () {
    heroData.HP = heroData.MaxHP;

    /* 发送刷新属性消息 */
    SendRefreshPropEvent ();
}

public HeroData HeroData {
    get {
        return heroData;
    }
}

/// <summary>
/// 移动控制器
/// </summary>
/// <returns></returns>
public IMoveController MoveController {
    get {
        if (moveController == null) {
            moveController = new LeftJoystickMoveController (FindObjectOfType<LeftJoystick> ());
        }
        return moveController;
    }
}

/// <summary>
/// 发送刷新属性消息
/// </summary>
public void SendRefreshPropEvent () {
    GameEntry.Event.Fire (this,
        ReferencePool.Acquire<RefreshHeroPropsEventArgs> ().Fill (this.heroData));
}

private void OnClickAttackButton (object sender, GameEventArgs e) {
    if (IsDead) {
        return;
    }

    if (isAtkCDing == false) {
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
        this.heroData.PowerUp (data.Atk, data.Def, data.MaxHP);

        /* 刷新血量条 */
        RefreshHPBar ();

        /* 发送刷新属性消息 */
        SendRefreshPropEvent ();
    }
}
}