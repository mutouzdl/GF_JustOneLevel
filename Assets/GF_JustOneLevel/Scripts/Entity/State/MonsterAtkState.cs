using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;

public class MonsterAtkState : MonsterListenDamageState {
    private float atkTimeCounter = 0;

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

        atkTimeCounter = 0;

        fsm.Owner.ChangeAnimation (FightEntityAnimationState.atk);
    }

    /// <summary>
    /// 有限状态机状态轮询时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
    /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
    protected override void OnUpdate (IFsm<Monster> fsm, float elapseSeconds, float realElapseSeconds) {
        base.OnUpdate (fsm, elapseSeconds, realElapseSeconds);

        if (GlobalGame.IsPause) {
            ChangeState<MonsterIdleState> (fsm);
        }

        atkTimeCounter += elapseSeconds;

        if (atkTimeCounter > fsm.Owner.MonsterData.AtkAnimTime) {
            int lockAimID = fsm.GetData<VarInt> (Constant.EntityData.LockAimID).Value;
            FightEntity aim = (FightEntity) GameEntry.Entity.GetEntity (lockAimID).Logic;

            if (aim.IsDead == false) {
                fsm.Owner.transform.LookAt (aim.transform);
                fsm.Owner.PerformAttack ();
            }
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