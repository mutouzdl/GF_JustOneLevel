using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;

public class MonsterSeekAimState : MonsterListenDamageState {
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
    }

    /// <summary>
    /// 有限状态机状态轮询时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
    /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
    protected override void OnUpdate (IFsm<Monster> fsm, float elapseSeconds, float realElapseSeconds) {
        base.OnUpdate (fsm, elapseSeconds, realElapseSeconds);

        if (fsm.Owner.IsAtkCDing || GlobalGame.IsPause) {
            return;
        }

        if (fsm.Owner.IsLockingAim) {
            FightEntity aim = fsm.Owner.LockingAim;
            if (aim == null || aim.IsDead) {
                ChangeState<MonsterIdleState>(fsm);
                return;
            }

            float distance = AIUtility.GetDistance (fsm.Owner, aim);
            
            /* 进入攻击范围，直接攻击 */
            if (fsm.Owner.CheckInAtkRange (distance)) {
                fsm.SetData<VarInt> (Constant.EntityData.LockAimID, aim.Entity.Id);
                ChangeState<MonsterAtkState> (fsm);
            }
            /* 在视线范围内，则继续移动 */
            else if (fsm.Owner.CheckInSeekRange(distance)) {
                if (fsm.CurrentState.GetType() != typeof(MonsterWalkState)) {
                    ChangeState<MonsterWalkState>(fsm);
                }
            }
            else {
                fsm.Owner.UnlockAim();
            }
            return;
        }

        /* 判断是否有敌人进入追踪或攻击范围 */
        CampType camp = fsm.Owner.GetImpactData().Camp;
        GameObject[] aims = GameObject.FindGameObjectsWithTag ("Creature");
        foreach (GameObject obj in aims) {
            FightEntity aim = obj.GetComponent<FightEntity> ();

            if (aim.IsDead == false 
                && AIUtility.GetRelation(aim.GetImpactData().Camp, camp) == RelationType.Hostile) {
                float distance = AIUtility.GetDistance (fsm.Owner, aim);

                /* 进入攻击范围，直接攻击 */
                if (fsm.Owner.CheckInAtkRange (distance)) {
                    fsm.SetData<VarInt> (Constant.EntityData.LockAimID, aim.Entity.Id);
                    ChangeState<MonsterAtkState> (fsm);
                    break;
                }
                /* 进入追踪范围，向目标移动 */
                else if (fsm.Owner.CheckInSeekRange (distance)) {
                    fsm.Owner.LockAim(aim);
                    ChangeState<MonsterWalkState>(fsm);
                    break;
                }
            }
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