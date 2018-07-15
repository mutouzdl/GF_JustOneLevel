using DG.Tweening;
using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;

public class MonsterHurtState : MonsterBaseActionState {
    private float hurtTimeCounter = 0;
    private float preHurtTime = 0; // 上一次受伤的时间
    private int hurtTimes = 0; // 连续受伤次数

    /// <summary>
    /// 有限状态机状态初始化时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    protected override void OnInit (IFsm<Monster> fsm) {
        base.OnInit (fsm);
    }

    /// <summary>
    /// 有限状态机状态进入时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    protected override void OnEnter (IFsm<Monster> fsm) {
        base.OnEnter (fsm);

        hurtTimeCounter = 0;

        // 积累受伤次数
        if (preHurtTime != 0 && Time.time - preHurtTime < 2.5f) {
            hurtTimes++;
        } else {
            preHurtTime = 0;
            hurtTimes = 0;
        }

        // 累积受伤3次，则向后弹一段距离
        if (hurtTimes == 3) {
            Vector3 pos = PositionUtility.GetAjustPositionWithMap (fsm.Owner.CachedTransform.position - fsm.Owner.CachedTransform.forward * 2);
            fsm.Owner.CachedTransform.DOMove (pos, 0.5f);
            hurtTimes = 0;
        }

        // 播放动画
        fsm.Owner.ChangeAnimation (FightEntityAnimationState.hurt);

        // 执行受伤逻辑
        int damageHP = fsm.GetData<VarInt> (Constant.EntityData.DamageHP).Value;
        fsm.Owner.OnDamage (damageHP);

        preHurtTime = Time.time;
    }

    /// <summary>
    /// 有限状态机状态轮询时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
    /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
    protected override void OnUpdate (IFsm<Monster> fsm, float elapseSeconds, float realElapseSeconds) {
        base.OnUpdate (fsm, elapseSeconds, realElapseSeconds);

        hurtTimeCounter += elapseSeconds;

        if (hurtTimeCounter > 0.3) {
            ChangeState<MonsterIdleState> (fsm);
        }
    }

    /// <summary>
    /// 有限状态机状态离开时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    /// <param name="isShutdown">是否是关闭有限状态机时触发。</param>
    protected override void OnLeave (IFsm<Monster> fsm, bool isShutdown) {
        base.OnLeave (fsm, isShutdown);
    }

    /// <summary>
    /// 有限状态机状态销毁时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    protected override void OnDestroy (IFsm<Monster> fsm) {
        base.OnDestroy (fsm);
    }
}