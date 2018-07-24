using GameFramework;
using GameFramework.Fsm;
using UnityEngine;

public class MonsterWalkState : MonsterSeekAimState {

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

        fsm.Owner.ChangeAnimation (FightEntityAnimationState.walk);
    }

    /// <summary>
    /// 有限状态机状态轮询时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
    /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
    protected override void OnUpdate (IFsm<Monster> fsm, float elapseSeconds, float realElapseSeconds) {
        base.OnUpdate (fsm, elapseSeconds, realElapseSeconds);

        if (fsm.Owner.IsLockingAim) {
            FightEntity aim = fsm.Owner.LockingAim;

            if (aim != null && aim.IsDead == false) {
                fsm.Owner.transform.LookAt (aim.transform);

                // 除非敌人离开了自己的攻击范围，否则，在锁定目标的过程中，不进行移动（避免不断和敌人靠近，直至重合）
                float distance = AIUtility.GetDistance (fsm.Owner, aim);
                if (fsm.Owner.CheckInAtkRange (distance) == false) {
                    fsm.Owner.Forward (elapseSeconds);
                }

                return;
            }
        }

        if (fsm.Owner.MoveController == null) {
            return;
        }

        Vector3 inputVec = fsm.Owner.MoveController.GetInput ();

        if (inputVec.y != 0) {
            // 移动
            fsm.Owner.Forward (inputVec.y * elapseSeconds);
        } else {
            // 站立
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