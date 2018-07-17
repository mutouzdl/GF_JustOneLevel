using System;
using System.Collections.Generic;
using System.Reflection;
using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;

public class Monster : FightEntity {
    [SerializeField]
    private MonsterData monsterData = null;

    /// <summary>
    /// 状态类状态机：CD空闲、CD
    /// </summary>
    private GameFramework.Fsm.IFsm<Monster> monsterStateFsm;
    /// <summary>
    /// 行动类状态机：空闲、行走、攻击、受伤
    /// </summary>
    private GameFramework.Fsm.IFsm<Monster> monsterActionFsm;
    private TextMesh msgText = null;

    /// <summary>
    /// 是否正在追踪目标
    /// </summary>
    /// <returns></returns>
    public bool IsLockingAim { get; set; }
    /// <summary>
    /// 锁定的目标
    /// </summary>
    /// <returns></returns>
    public FightEntity LockingAim { get; set; }

    protected override void OnInit (object userData) {
        base.OnInit (userData);

        moveController = new FoolishAIMoveController ();
        msgText = this.gameObject.GetComponentInChildren<TextMesh>();;
    }

    protected override void OnShow (object userData) {
        base.OnShow (userData);

        monsterData = userData as MonsterData;
        if (monsterData == null) {
            Log.Error ("Monster data is invalid.");
            return;
        }

        IsLockingAim = false;

        ResetAtkCD ();

        /* 加载武器 */
        InitWeapon ();

        /* 创建状态机 */
        InitFSM ();

        /* 初始化描述文本 */
        InitMsgText ();
    }

    /// <summary>
    /// 初始化状态机
    /// </summary>
    private void InitFSM () {
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

    /// <summary>
    /// 初始化描述文本
    /// </summary>
    private void InitMsgText () {
        if (msgText == null) {
            return;
        }

        int percent = (int)((monsterData.PowerPercent - 1) * 100);
        string power = "";
        if (monsterData.PowerPercent > 1) {
            power = $"<color=red>Up+{percent}%</color>";
        } 
        else if (monsterData.PowerPercent < 1) {
            power = $"<color=gray>Down{percent}%</color>";
        }

        msgText.text = $"{monsterData.Name} {power}\n攻{monsterData.Atk}+ 防{monsterData.Def}";
    }

    protected override void OnUpdate (float elapseSeconds, float realElapseSeconds) {
        base.OnUpdate (elapseSeconds, realElapseSeconds);
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
    public void PerformAttack () {
        IsAtkCDing = true;
        monsterStateFsm.FireEvent (this, MonsterAttackEventArgs.EventId);

        foreach (Weapon weapon in manualWeapons) {
            weapon.Attack (monsterData.Atk);
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
    /// 锁定目标
    /// </summary>
    /// <param name="aim"></param>
    public void LockAim (FightEntity aim) {
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