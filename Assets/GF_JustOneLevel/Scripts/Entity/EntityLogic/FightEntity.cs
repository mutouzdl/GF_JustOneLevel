using System.Collections.Generic;
using System.Linq;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 可作为目标的实体类。
/// 参考来源：https://github.com/EllanJiang/StarForce
/// </summary>
public abstract class FightEntity : Entity {
    [SerializeField]
    private FightEntityData fightEntityData = null;

    /// <summary>
    /// 血量条
    /// </summary>
    private PowerBar hpBar;

    /// <summary>
    /// 移动控制器
    /// </summary>
    protected IMoveController moveController = null;

    /// <summary>
    /// 手动触发类型武器
    /// </summary>
    /// <typeparam name="Weapon"></typeparam>
    /// <returns></returns>
    protected List<Weapon> manualWeapons = new List<Weapon> ();
    /// <summary>
    /// 自动触发类型武器
    /// </summary>
    /// <typeparam name="Weapon"></typeparam>
    /// <returns></returns>
    protected List<Weapon> autoWeapons = new List<Weapon> ();
    /// <summary>
    /// 技能触发类型武器
    /// </summary>
    /// <typeparam name="Weapon"></typeparam>
    /// <returns></returns>
    protected List<Weapon> skillWeapons = new List<Weapon> ();

    protected override void OnInit (object userData) {
        base.OnInit (userData);
        // CachedTransform.SetLayerRecursively (Constant.Layer.TargetableObjectLayerId);
    }

    protected override void OnShow (object userData) {
        base.OnShow (userData);

        fightEntityData = userData as FightEntityData;
        if (fightEntityData == null) {
            Log.Error ("fightEntityData is invalid.");
            return;
        }

        // CachedTransform.localScale = Vector3.one;

        manualWeapons.Clear ();
        autoWeapons.Clear ();
        skillWeapons.Clear ();

        /* 附加血量条 */
        PowerBarData hpBarData = new PowerBarData (EntityExtension.GenerateSerialId (), 1, this.Id, CampType.Player);
        EntityExtension.ShowPowerBar (typeof (PowerBar), "PowerBarGroup", hpBarData);
    }

    protected override void OnUpdate (float elapseSeconds, float realElapseSeconds) {
        base.OnUpdate (elapseSeconds, realElapseSeconds);

        if (IsDead) {
            return;
        }

        // 自动类型的武器需要主动调用TryAutoAttack执行攻击逻辑
        foreach (Weapon weapon in autoWeapons) {
            weapon.TryAutoAttack (elapseSeconds, fightEntityData.Atk);
        }
    }

    protected override void OnAttached (EntityLogic childEntity, Transform parentTransform, object userData) {
        base.OnAttached (childEntity, parentTransform, userData);

        if (childEntity is PowerBar) {
            hpBar = (PowerBar) childEntity;
            hpBar.UpdatePower (fightEntityData.HP, fightEntityData.MaxHP);
            return;
        } else if (childEntity is Weapon) {
            WeaponData weaponData = (WeaponData) userData;
            Weapon weapon = (Weapon) childEntity;

            switch (weaponData.AttackType) {
                case WeaponAttackType.手动触发:
                    manualWeapons.Add (weapon);
                    break;
                case WeaponAttackType.自动触发:
                    autoWeapons.Add (weapon);
                    break;
                case WeaponAttackType.技能触发:
                    skillWeapons.Add (weapon);
                    break;
            }
            return;
        }
    }

    /// <summary>
    /// 真正执行伤害/加血逻辑
    /// </summary>
    /// <param name="damageHP">伤害值，可以正负，如果小于1，则按百分比执行</param>
    /// <param name="effectFollowMaxHP">是否以最大生命值为准</param>
    public void OnDamage (float damageHP, bool effectFollowMaxHP = false) {
        int changeHP = 0;

        // 按百分比改变当前血量
        if ((damageHP > 0 && damageHP < 1) || (damageHP < 0 && damageHP > -1)) {
            if (effectFollowMaxHP) {
                changeHP = (int) (fightEntityData.MaxHP * damageHP);
            } else {
                changeHP = (int) (fightEntityData.HP * damageHP);
            }
        } else {
            changeHP = (int) damageHP;
        }

        bool forceAdd = false;
        // 伤害
        if (changeHP > 0) {
            changeHP -= fightEntityData.Def;

            if (changeHP < 0) {
                changeHP = 1;
            }

            if (changeHP > 0) {
                fightEntityData.HP -= changeHP;

                OnHurt ();
            }
        }
        // 加血
        else if (changeHP < 0) {
            forceAdd = true;

            if (fightEntityData.HP - changeHP > fightEntityData.MaxHP) {
                changeHP = fightEntityData.HP - fightEntityData.MaxHP;
            }

            fightEntityData.HP += -changeHP;

            OnCure ();
        }

        FlowTextData flowTextData = new FlowTextData (EntityExtension.GenerateSerialId (), this.Id, -changeHP, forceAdd);
        flowTextData.Position = this.CachedTransform.position + new Vector3 (0.4f, 1, 0);
        flowTextData.Rotation = Camera.main.transform.rotation;

        EntityExtension.ShowFlowText (
            typeof (FlowText),
            "FlowTextGroup", flowTextData
        );

        // 更新血量条
        RefreshHPBar ();

        if (fightEntityData.HP <= 0) {
            OnDead ();
        }
    }

    /// <summary>
    /// 切换动画
    /// </summary>
    /// <param name="state"></param>
    public void ChangeAnimation (FightEntityAnimationState state) {
        // Log.Info("Hero ChangeAnimation:" + state);
        ResetAnimation ();

        if (state == FightEntityAnimationState.walk) {
            cachedAnimator.SetBool ("IsWalking", true);
        } else if (state == FightEntityAnimationState.idle) { } else if (state == FightEntityAnimationState.atk) {
            cachedAnimator.SetBool ("IsAttacking", true);
        } else if (state == FightEntityAnimationState.hurt) {
            cachedAnimator.SetBool ("IsHurting", true);
        } else if (state == FightEntityAnimationState.dead) {
            cachedAnimator.SetBool ("IsDead", true);
        }
    }

    /// <summary>
    /// 转身
    /// </summary>
    /// <param name="destVec">目标位置</param>
    public void Rotate (Vector3 destVec) {
        CachedTransform.Rotate (destVec);
    }

    /// <summary>
    /// 向前移动
    /// </summary>
    /// <param name="distance"></param>
    public void Forward (float distance) {
        Vector3 nextPos = CachedTransform.position + CachedTransform.forward * distance * fightEntityData.MoveSpeed;

        CachedTransform.position = PositionUtility.GetAjustPositionWithMap (nextPos);
    }

    /// <summary>
    /// 是否在攻击范围内
    /// </summary>
    /// <param name="distance"></param>
    /// <returns></returns>
    public bool CheckInAtkRange (float distance) {
        return distance <= fightEntityData.AtkRange;
    }

    /// <summary>
    /// 选择指定的武器开火
    /// </summary>
    /// <param name="attackType"></param>
    /// <param name="weaponID"></param>
    public void FireWeapon (WeaponAttackType attackType, int weaponID) {
        Weapon weapon = null;
        switch (attackType) {
            case WeaponAttackType.手动触发:
                weapon = manualWeapons.Where (w => w.GetTypeId () == weaponID).SingleOrDefault ();
                break;
            case WeaponAttackType.自动触发:
                weapon = autoWeapons.Where (w => w.GetTypeId () == weaponID).SingleOrDefault ();
                break;
            case WeaponAttackType.技能触发:
                weapon = skillWeapons.Where (w => w.GetTypeId () == weaponID).SingleOrDefault ();
                break;
        }

        if (weapon != null) {
            if (weapon.CostMP > 0 && fightEntityData.MP < weapon.CostMP) {
                return;
            }

            fightEntityData.CostMP (weapon.CostMP);
            weapon.Attack (fightEntityData.Atk);

            OnFireWeapon ();
        }
    }

    protected void InitWeapon () {
        List<WeaponData> weaponDatas = fightEntityData.GetWeaponDatas ();
        for (int i = 0; i < weaponDatas.Count; i++) {
            EntityExtension.ShowWeapon (typeof (Weapon), "WeaponGroup", weaponDatas[i]);
        }
    }

    /// <summary>
    /// 更新血量条
    /// </summary>
    protected void RefreshHPBar () {
        hpBar.UpdatePower (fightEntityData.HP, fightEntityData.MaxHP);
    }

    private void ResetAnimation () {
        cachedAnimator.SetBool ("IsWalking", false);
        cachedAnimator.SetBool ("IsAttacking", false);
        cachedAnimator.SetBool ("IsHurting", false);
        cachedAnimator.SetBool ("IsDead", false);
    }
    private void OnTriggerEnter (Collider other) {
        Entity entity = other.gameObject.GetComponent<Entity> ();
        if (entity == null) {
            return;
        }

        if (entity is FightEntity && entity.Id >= Id) {
            // 碰撞事件由 Id 小的一方处理，避免重复处理
            return;
        }

        AIUtility.PerformCollision (this, entity);
    }

    #region 虚函数

    /// <summary>
    /// 接受伤害
    /// </summary>
    /// <param name="damageHP"></param>
    public virtual void ApplyDamage (int damageHP) { }

    /// <summary>
    /// 当子类需要在特殊情况下才能创建移动控制器时，可以重写该函数
    /// </summary>
    /// <returns></returns>
    protected virtual IMoveController CreateMoveController () {
        return null;
    }

    protected virtual void OnHurt () { }
    protected virtual void OnCure () { }

    protected virtual void OnDead () {
        // GameEntry.Entity.HideEntity (this.Entity);
    }

    protected virtual void OnFireWeapon () { }

    #endregion

    #region 公开的属性

    public bool IsDead {
        get {
            return fightEntityData.HP <= 0;
        }
    }

    /// <summary>
    /// 移动控制器
    /// </summary>
    /// <returns></returns>
    public IMoveController MoveController {
        get {
            if (moveController == null) {
                moveController = CreateMoveController ();
            }
            return moveController;
        }
    }

    /// <summary>
    /// 攻击是否正在冷却
    /// </summary>
    /// <returns></returns>
    public bool IsAtkCDing {
        get;
        protected set;
    } = false;

    /// <summary>
    /// 重置攻击冷却
    /// </summary>
    public void ResetAtkCD () {
        IsAtkCDing = false;
    }

    public abstract ImpactData GetImpactData ();

    #endregion
}