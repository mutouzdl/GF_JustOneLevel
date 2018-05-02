using GameFramework;
using GameFramework.Fsm;
using UnityEngine;

public class MonsterWalkState : FsmState<MonsterLogic> {
    private float rotateTimeCounter = 0;
    private float idleTimeCounter = 0;

    /// <summary>
    /// 有限状态机状态初始化时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    protected override void OnInit (IFsm<MonsterLogic> fsm) { }

    /// <summary>
    /// 有限状态机状态进入时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    protected override void OnEnter (IFsm<MonsterLogic> fsm) {
        fsm.Owner.ChangeAnimation (MonsterAnimationState.walk);
    }

    /// <summary>
    /// 有限状态机状态轮询时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
    /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
    protected override void OnUpdate (IFsm<MonsterLogic> fsm, float elapseSeconds, float realElapseSeconds) {
        idleTimeCounter += elapseSeconds;
        rotateTimeCounter += elapseSeconds;

        // 移动
        fsm.Owner.Forward (elapseSeconds);

        // 随机转身
        if(rotateTimeCounter > 5) {
            rotateTimeCounter = 0;
            if (Utility.Random.GetRandom(100) <= 80) {
                fsm.Owner.Rotate(new Vector3(0, Utility.Random.GetRandom(-180, 180), 0));
            }
        }

        // 随机站立
        if (idleTimeCounter > 5) {
            idleTimeCounter = 0;

            if (Utility.Random.GetRandom (100) <= 10) {
                ChangeState<MonsterIdleState>(fsm);
            }
        }

        // 碰到障碍物停止

        // 进入攻击状态
    }

    /// <summary>
    /// 有限状态机状态离开时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    /// <param name="isShutdown">是否是关闭有限状态机时触发。</param>
    protected override void OnLeave (IFsm<MonsterLogic> fsm, bool isShutdown) {

    }

    /// <summary>
    /// 有限状态机状态销毁时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    protected override void OnDestroy (IFsm<MonsterLogic> fsm) {
        base.OnDestroy (fsm);
    }

}