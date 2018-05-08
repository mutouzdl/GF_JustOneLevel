using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;

public class MonsterSeekAimState : MonsterListenDamageState {
    /// <summary>
    /// 有限状态机状态初始化时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    protected override void OnInit (IFsm<MonsterLogic> fsm) {
        base.OnInit (fsm);
    }

    /// <summary>
    /// 有限状态机状态进入时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    protected override void OnEnter (IFsm<MonsterLogic> fsm) {
        base.OnEnter (fsm);
    }

    /// <summary>
    /// 有限状态机状态轮询时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
    /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
    protected override void OnUpdate (IFsm<MonsterLogic> fsm, float elapseSeconds, float realElapseSeconds) {
        base.OnUpdate (fsm, elapseSeconds, realElapseSeconds);

        if (fsm.Owner.IsAtkCDing ()) {
            return;
        }

        if (fsm.Owner.IsLockingAim) {
            Entity aim = fsm.Owner.LockingAim;
            float distance = AIUtility.GetDistance (fsm.Owner, aim);
            /* 进入攻击范围，直接攻击 */
            if (fsm.Owner.CheckInAtkRange (distance)) {
                fsm.SetData<VarInt> (Constant.EntityData.LockAimID, aim.Entity.Id);
                ChangeState<MonsterAtkState> (fsm);
            }
            else if (fsm.Owner.CheckInSeekRange(distance) == false) {
                fsm.Owner.UnlockAim();
            }
            return;
        }

        /* 判断是否有英雄进入追踪或攻击范围 */
        GameObject[] heros = GameObject.FindGameObjectsWithTag ("Hero");
        foreach (GameObject obj in heros) {
            TargetableObject hero = obj.GetComponent<TargetableObject> ();

            if (hero.IsDead == false) {
                float distance = AIUtility.GetDistance (fsm.Owner, hero);

                /* 进入攻击范围，直接攻击 */
                if (fsm.Owner.CheckInAtkRange (distance)) {
                    fsm.SetData<VarInt> (Constant.EntityData.LockAimID, hero.Entity.Id);
                    ChangeState<MonsterAtkState> (fsm);
                    break;
                }
                /* 进入追踪范围，向目标移动 */
                else if (fsm.Owner.CheckInSeekRange (distance)) {
                    fsm.Owner.LockAim(hero);
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
    protected override void OnLeave (IFsm<MonsterLogic> fsm, bool isShutdown) {
        base.OnLeave (fsm, isShutdown);
    }

    /// <summary>
    /// 有限状态机状态销毁时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    protected override void OnDestroy (IFsm<MonsterLogic> fsm) {
        base.OnDestroy (fsm);
    }
}