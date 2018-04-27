using System.Collections.Generic;
using GameFramework;
using GameFramework.Fsm;
using UnityEngine;

public class HeroLogic : TargetableObject {
    [SerializeField]
    private HeroData m_heroData = null;

    private Vector3 m_TargetPosition = Vector3.zero;

    private Animation anim;
    private List<string> animationNames;
    private HeroAnimationState curState = HeroAnimationState.idle;
    private IFsmManager m_FsmManager;
    private GameFramework.Fsm.IFsm<HeroLogic> m_HeroFsm;

    protected override void OnInit (object userData) {
        base.OnInit (userData);

        FsmState<HeroLogic>[] heroStates = new FsmState<HeroLogic>[] {
            new HeroIdleState (),
            new HeroWalkState (),
        };

        m_FsmManager = GameFrameworkEntry.GetModule<IFsmManager> ();
        m_HeroFsm = GameEntry.Fsm.CreateFsm<HeroLogic> (this, heroStates);
        anim = GetComponent<Animation> ();
        animationNames = GetAnimationNames ();

        m_HeroFsm.Start<HeroIdleState> ();
    }

    protected override void OnShow (object userData) {
        base.OnShow (userData);

        m_heroData = userData as HeroData;
        if (m_heroData == null) {
            Log.Error ("My aircraft data is invalid.");
            return;
        }

    }

    protected override void OnUpdate (float elapseSeconds, float realElapseSeconds) {
        base.OnUpdate (elapseSeconds, realElapseSeconds);

        float inputHorizontal = Input.GetAxis ("Horizontal");
        if (inputHorizontal != 0) {
            /* 旋转镜头 */
            transform.Rotate (new Vector3 (0, inputHorizontal * 0.8f * m_heroData.RotateSpeed, 0));
        }
    }

    protected override void OnHide (object userData) {
        base.OnHide (userData);

        GameEntry.Fsm.DestroyFsm<HeroWalkState> ();
    }

    public override ImpactData GetImpactData () {
        return new ImpactData (m_heroData.Camp, m_heroData.HP, 0, m_heroData.Defense);
    }

    /// <summary>
    /// 向前移动
    /// </summary>
    /// <param name="distance"></param>
    public void Forward (float distance) {
        CachedTransform.position += CachedTransform.forward * distance * m_heroData.MoveSpeed;
    }

    /// <summary>
    /// 切换动画
    /// </summary>
    /// <param name="state"></param>
    public void ChangeAnimation(HeroAnimationState state) {
        anim.CrossFade (animationNames[(int) state], 0.01f);
    }

    /// <summary>
    /// 获取所有动作动画名字
    /// </summary>
    /// <returns></returns>
    private List<string> GetAnimationNames () {
        List<string> tmpArray = new List<string> ();
        foreach (AnimationState state in gameObject.GetComponent<Animation> ()) {
            tmpArray.Add (state.name);
        }
        return tmpArray;
    }
}