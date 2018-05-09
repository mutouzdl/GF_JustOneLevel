using System;
using System.Collections.Generic;
using System.Reflection;
using GameFramework;
using GameFramework.Event;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;

public class Hero : TargetableObject {
    [SerializeField]
    private HeroData m_heroData = null;

    [SerializeField]
    private Rigidbody m_Rigidbody = null;

    /// <summary>
    /// 攻击是否正在冷却
    /// </summary>
    private bool m_IsAtkCDing = false;
    /// <summary>
    /// 状态类状态机：CD空闲、CD
    /// </summary>
    private GameFramework.Fsm.IFsm<Hero> m_HeroStateFsm;
    /// <summary>
    /// 行动类状态机：空闲、行走、攻击、受伤
    /// </summary>
    private GameFramework.Fsm.IFsm<Hero> m_HeroActionFsm;

    protected override void OnInit (object userData) {
        base.OnInit (userData);
        m_Rigidbody = gameObject.GetComponent<Rigidbody> ();
    }

    protected override void OnShow (object userData) {
        base.OnShow (userData);

        m_heroData = userData as HeroData;
        if (m_heroData == null) {
            Log.Error ("Hero data is invalid.");
            return;
        }

        /* 创建状态机 */
        m_HeroStateFsm = GameEntry.Fsm.CreateFsm<Hero> ("heroStateFsm", this, new FsmState<Hero>[] {
            new HeroCDIdleState (),
                new HeroAtkCDState (),
        });

        m_HeroActionFsm = GameEntry.Fsm.CreateFsm<Hero> ("heroActionFsm", this, new FsmState<Hero>[] {
            new HeroIdleState (),
                new HeroWalkState (),
                new HeroAtkState (),
                new HeroHurtState (),
        });

        /* 启动状态机 */
        m_HeroStateFsm.Start<HeroCDIdleState> ();
        m_HeroActionFsm.Start<HeroIdleState> ();

        /* 订阅事件 */
        GameEntry.Event.Subscribe (ClickAttackButtonEventArgs.EventId, OnClickAttackButton);
    }

    protected override void OnUpdate (float elapseSeconds, float realElapseSeconds) {
        base.OnUpdate (elapseSeconds, realElapseSeconds);

        /* 旋转镜头 */
        float inputHorizontal = Input.GetAxis ("Horizontal");
        if (inputHorizontal != 0) {
            Quaternion deltaRotation = Quaternion.Euler (0, inputHorizontal * Time.deltaTime * m_heroData.RotateSpeed, 0);
            m_Rigidbody.MoveRotation (m_Rigidbody.rotation * deltaRotation);
        }
    }

    protected override void OnHide (object userData) {
        base.OnHide (userData);

        GameEntry.Fsm.DestroyFsm<Hero> ();
        GameEntry.Event.Unsubscribe (ClickAttackButtonEventArgs.EventId, OnClickAttackButton);
    }

    public override ImpactData GetImpactData () {
        return new ImpactData (m_heroData.Camp, m_heroData.HP, 0, m_heroData.Def);
    }

    /// <summary>
    /// 向前移动
    /// </summary>
    /// <param name="distance"></param>
    public void Forward (float distance) {
        m_Rigidbody.MovePosition (CachedTransform.position + CachedTransform.forward * distance * m_heroData.MoveSpeed);
    }

    /// <summary>
    /// 是否在攻击范围内
    /// </summary>
    /// <param name="distance"></param>
    /// <returns></returns>
    public bool CheckInAtkRange (float distance) {
        return distance <= m_heroData.AtkRange;
    }

    /// <summary>
    /// 执行攻击
    /// </summary>
    /// <param name="aimEntity">攻击目标</param>
    public void PerformAttack (TargetableObject aimEntity) {
        m_IsAtkCDing = true;
        m_HeroStateFsm.FireEvent (this, HeroAttackEventArgs.EventId);
        aimEntity.ApplyDamage (m_heroData.Atk);
    }

    /// <summary>
    /// 接受伤害
    /// </summary>
    /// <param name="damageHP"></param>
    public override void ApplyDamage (int damageHP) {
        m_HeroActionFsm.FireEvent (this, ApplyDamageEventArgs.EventId, damageHP);
    }

    /// <summary>
    /// 重置攻击冷却
    /// </summary>
    public void ResetAtkCD () {
        m_IsAtkCDing = false;
    }

    private void OnClickAttackButton (object sender, GameEventArgs e) {
        if (m_IsAtkCDing == false) {
            m_HeroActionFsm.FireEvent (this, ClickAttackButtonEventArgs.EventId);
        }
    }

    public HeroData HeroData {
        get {
            return m_heroData;
        }
    }
}