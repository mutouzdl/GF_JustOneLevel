using System.Collections.Generic;
using GameFramework;
using GameFramework.Fsm;
using UnityEngine;

public class MonsterLogic : TargetableObject {
    [SerializeField]
    private MonsterData m_MonsterData = null;

    /// 所有动画名称列表
    /// </summary>
    private List<string> m_AnimationNames = new List<string>();
    private GameFramework.Fsm.IFsm<MonsterLogic> m_MonsterFsm;

    protected override void OnInit (object userData) {
        base.OnInit (userData);

        /* 获取动画名称 */
        foreach (AnimationState state in gameObject.GetComponent<Animation> ()) {
            m_AnimationNames.Add (state.name);
        }

        /* 创建状态机 */
        FsmState<MonsterLogic>[] monsterStates = new FsmState<MonsterLogic>[] {
            new MonsterIdleState (),
            new MonsterWalkState (),
        };

        m_MonsterFsm = GameEntry.Fsm.CreateFsm<MonsterLogic> (this, monsterStates);

        /* 启动站立状态 */
        m_MonsterFsm.Start<MonsterIdleState> ();
    }

    protected override void OnShow (object userData) {
        base.OnShow (userData);

        m_MonsterData = userData as MonsterData;
        if (m_MonsterData == null) {
            Log.Error ("Monster data is invalid.");
            return;
        }
    }

    protected override void OnUpdate (float elapseSeconds, float realElapseSeconds) {
        base.OnUpdate (elapseSeconds, realElapseSeconds);
    }

    protected override void OnHide (object userData) {
        base.OnHide (userData);

        GameEntry.Fsm.DestroyFsm<MonsterLogic> ();
    }

    public override ImpactData GetImpactData () {
        return new ImpactData (m_MonsterData.Camp, m_MonsterData.HP, 0, m_MonsterData.Def);
    }

    /// <summary>
    /// 向前移动
    /// </summary>
    /// <param name="distance"></param>
    public void Forward (float distance) {
        CachedTransform.position += CachedTransform.forward * distance * m_MonsterData.MoveSpeed;
    }

    /// <summary>
    /// 转身
    /// </summary>
    /// <param name="destVec">目标位置</param>
    public void Rotate(Vector3 destVec) {
        CachedTransform.Rotate(destVec);
    }

    /// <summary>
    /// 切换动画
    /// </summary>
    /// <param name="state"></param>
    public void ChangeAnimation (MonsterAnimationState state) {
        CachedAnimation.CrossFade (m_AnimationNames[(int) state], 0.01f);
    }
}