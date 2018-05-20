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
    public WeaponTrail m_WeaponTrail;

    protected override void OnInit (object userData) {
        base.OnInit (userData);
        m_Rigidbody = gameObject.GetComponent<Rigidbody> ();
        m_WeaponTrail = FindObjectOfType<WeaponTrail> ();
    }

    protected override void OnShow (object userData) {
        base.OnShow (userData);

        m_heroData = userData as HeroData;
        if (m_heroData == null) {
            Log.Error ("Hero data is invalid.");
            return;
        }

        m_WeaponTrail.SetTime (0.0f, 0, 1);

        /* 加载武器 */
        List<WeaponData> weaponDatas = m_heroData.GetWeaponDatas ();
        for (int i = 0; i < weaponDatas.Count; i++) {
            EntityExtension.ShowWeapon (typeof (Weapon), "WeaponGroup", weaponDatas[i]);
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
                new HeroDeadState (),
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
    private float t = 0.033f;
    private float animationIncrement = 0.003f;
    private float tempT = 0;
    void LateUpdate () {
        t = Mathf.Clamp (Time.deltaTime, 0, 0.066f);

        if (t > 0) {
            while (tempT < t) {
                tempT += animationIncrement;

                if (m_WeaponTrail.time > 0) {
                    m_WeaponTrail.Itterate (Time.time - t + tempT);
                } else {
                    m_WeaponTrail.ClearTrail ();
                }
            }

            tempT -= t;

            if (m_WeaponTrail.time > 0) {
                m_WeaponTrail.UpdateTrail (Time.time, t);
            }
        }
    }
    protected override void OnAttached (EntityLogic childEntity, Transform parentTransform, object userData) {
        base.OnAttached (childEntity, parentTransform, userData);

        if (childEntity is Weapon) {
            m_Weapons.Add ((Weapon) childEntity);
            return;
        }
    }

    protected override void OnHide (object userData) {
        base.OnHide (userData);

        GameEntry.Fsm.DestroyFsm (m_HeroActionFsm);
        GameEntry.Fsm.DestroyFsm (m_HeroStateFsm);
        GameEntry.Event.Unsubscribe (ClickAttackButtonEventArgs.EventId, OnClickAttackButton);
    }

    protected override void OnHurt() {
        GameEntry.Sound.PlaySound(Constant.Player.HURT_SOUND_ID);
    }

    protected override void OnDead () {
        base.OnDead ();
        m_HeroActionFsm.FireEvent (this, DeadEventArgs.EventId, this.Id);
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

        foreach (Weapon weapon in m_Weapons) {
            weapon.Attack (aimEntity.Id, m_heroData.Atk);
        }

    }

    /// <summary>
    /// 播放拖尾效果
    /// </summary>
    public void PlayTrailEffect() {
        if (m_WeaponTrail == null) {
            return;
        }

        //设置拖尾时长  
        m_WeaponTrail.SetTime (2.0f, 0.0f, 1.0f);
        
        //开始进行拖尾  
        m_WeaponTrail.StartTrail (0.5f, 0.4f);
    }

    /// <summary>
    /// 清除拖尾效果
    /// </summary>
    public void ClearTrialEffect() {
        if (m_WeaponTrail == null) {
            return;
        }

        //清除拖尾  
		m_WeaponTrail.ClearTrail ();
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