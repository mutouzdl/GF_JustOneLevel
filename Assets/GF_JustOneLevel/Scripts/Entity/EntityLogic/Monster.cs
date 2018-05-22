using System;
using System.Collections.Generic;
using System.Reflection;
using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;

public class Monster : TargetableObject {
    [SerializeField]
    private MonsterData monsterData = null;

    /// <summary>
    /// 攻击是否正在冷却
    /// </summary>
    private bool isAtkCDing = false;
    /// <summary>
    /// 状态类状态机：CD空闲、CD
    /// </summary>
    private GameFramework.Fsm.IFsm<Monster> monsterStateFsm;
    /// <summary>
    /// 行动类状态机：空闲、行走、攻击、受伤
    /// </summary>
    private GameFramework.Fsm.IFsm<Monster> monsterActionFsm;


    /// <summary>
    /// 是否正在追踪目标
    /// </summary>
    /// <returns></returns>
    public bool IsLockingAim { get; set; }
    /// <summary>
    /// 锁定的目标
    /// </summary>
    /// <returns></returns>
    public Entity LockingAim { get; set; }

    protected override void OnInit (object userData) {
        base.OnInit (userData);

    }

    protected override void OnShow (object userData) {
        base.OnShow (userData);

        monsterData = userData as MonsterData;
        if (monsterData == null) {
            Log.Error ("Monster data is invalid.");
            return;
        }

        IsLockingAim = false;

        /* 加载武器 */
        List<WeaponData> weaponDatas = monsterData.GetWeaponDatas ();
        for (int i = 0; i < weaponDatas.Count; i++) {
            EntityExtension.ShowWeapon (typeof (Weapon), "WeaponGroup", weaponDatas[i]);
        }

        /* 创建状态机 */
        monsterStateFsm = GameEntry.Fsm.CreateFsm<Monster> ("monsterStateFsm" + this.Id, this, new FsmState<Monster>[] {
            new MonsterCDIdleState (),
                new MonsterAtkCDState (),
        });

        monsterActionFsm = GameEntry.Fsm.CreateFsm<Monster> ("monsterActionFsm" + this.Id, this, new FsmState<Monster>[] {
            new MonsterIdleState (),
                new MonsterWalkState (),
                new MonsterAtkState (),
                new MonsterHurtState (),
                new MonsterDeadState (),
        });

        /* 启动状态机 */
        monsterStateFsm.Start<MonsterCDIdleState> ();
        monsterActionFsm.Start<MonsterIdleState> ();
    }

    protected override void OnUpdate (float elapseSeconds, float realElapseSeconds) {
        base.OnUpdate (elapseSeconds, realElapseSeconds);
    }

    protected override void OnAttached (EntityLogic childEntity, Transform parentTransform, object userData)
    {
        base.OnAttached (childEntity, parentTransform, userData);

        if (childEntity is Weapon) {
            weapons.Add ((Weapon) childEntity);
            return;
        }
    }

    protected override void OnHide (object userData) {
        base.OnHide (userData);

        GameEntry.Fsm.DestroyFsm (monsterStateFsm);
        GameEntry.Fsm.DestroyFsm (monsterActionFsm);
    }

    protected override void OnDead () {
        base.OnDead ();
        monsterActionFsm.FireEvent (this, DeadEventArgs.EventId, this.Id);

        GameEntry.Event.Fire (this,
            ReferencePool.Acquire<DeadEventArgs> ().Fill (this.monsterData.Camp, this.monsterData));
    }

    public override ImpactData GetImpactData () {
        return new ImpactData (monsterData.Camp, monsterData.HP, 0, monsterData.Def);
    }

    /// <summary>
    /// 向前移动
    /// </summary>
    /// <param name="distance"></param>
    public void Forward (float distance) {
        Vector3 nextPos = CachedTransform.position + CachedTransform.forward * distance * monsterData.MoveSpeed;

        CachedTransform.position = PositionUtility.GetAjustPositionWithMap(nextPos);
    }

    /// <summary>
    /// 转身
    /// </summary>
    /// <param name="destVec">目标位置</param>
    public void Rotate (Vector3 destVec) {
        CachedTransform.Rotate (destVec);
    }

    /// <summary>
    /// 是否在攻击范围内
    /// </summary>
    /// <param name="distance"></param>
    /// <returns></returns>
    public bool CheckInAtkRange (float distance) {
        return distance <= monsterData.AtkRange;
    }

    /// <summary>
    /// 是否在追踪范围内
    /// </summary>
    /// <param name="distance"></param>
    /// <returns></returns>
    public bool CheckInSeekRange (float distance) {
        return distance <= monsterData.SeekRange;
    }

    /// <summary>
    /// 执行攻击
    /// </summary>
    /// <param name="aimEntity">攻击目标</param>
    public void PerformAttack (TargetableObject aimEntity) {
        isAtkCDing = true;
        monsterStateFsm.FireEvent (this, MonsterAttackEventArgs.EventId);
        // aimEntity.ApplyDamage (m_MonsterData.Atk);

        foreach(Weapon weapon in weapons) {
            weapon.Attack(aimEntity.Id, monsterData.Atk);
        }
    }

    /// <summary>
    /// 接受伤害
    /// </summary>
    /// <param name="damageHP"></param>
    public override void ApplyDamage (int damageHP) {
        monsterActionFsm.FireEvent (this, ApplyDamageEventArgs.EventId, damageHP);
    }

    /// <summary>
    /// 攻击是否正在冷却
    /// </summary>
    /// <returns></returns>
    public bool IsAtkCDing () {
        return isAtkCDing;
    }

    /// <summary>
    /// 重置攻击冷却
    /// </summary>
    public void ResetAtkCD () {
        isAtkCDing = false;
    }

    /// <summary>
    /// 锁定目标
    /// </summary>
    /// <param name="aim"></param>
    public void LockAim (Entity aim) {
        this.LockingAim = aim;
        this.IsLockingAim = true;
    }

    /// <summary>
    /// 解除目标的锁定
    /// </summary>
    public void UnlockAim () {
        this.LockingAim = null;
        this.IsLockingAim = false;
    }

    public MonsterData MonsterData {
        get {
            return monsterData;
        }
    }
}