using GameFramework;
using GameFramework.Fsm;
using UnityEngine;

public class MonsterIdleState : FsmState<MonsterLogic> {
    private float forwardTimeCounter = 0;

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
        fsm.Owner.ChangeAnimation (MonsterAnimationState.idle);
    }

    /// <summary>
    /// 有限状态机状态轮询时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
    /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
    protected override void OnUpdate (IFsm<MonsterLogic> fsm, float elapseSeconds, float realElapseSeconds) {
        forwardTimeCounter += elapseSeconds;

        // 随机移动
        if(forwardTimeCounter > 7) {
            forwardTimeCounter = 0;
            
            if (Utility.Random.GetRandom(100) < 40) {
                ChangeState<MonsterWalkState>(fsm);
            }
        }
        
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