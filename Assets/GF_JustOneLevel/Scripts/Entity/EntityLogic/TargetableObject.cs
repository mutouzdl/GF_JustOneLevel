using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 可作为目标的实体类。
/// 参考来源：https://github.com/EllanJiang/StarForce
/// </summary>
public abstract class TargetableObject : Entity {
    [SerializeField]
    private TargetableObjectData m_TargetableObjectData = null;

    /// <summary>
    /// 血量条
    /// </summary>
    private PowerBar m_HPBar;
    public bool IsDead {
        get {
            return m_TargetableObjectData.HP <= 0;
        }
    }

    public abstract ImpactData GetImpactData ();

    /// <summary>
    /// 接受伤害
    /// </summary>
    /// <param name="damageHP"></param>
    public virtual void ApplyDamage (int damageHP) { }

    /// <summary>
    /// 真正执行伤害逻辑
    /// </summary>
    /// <param name="damageHP"></param>
    public void OnDamage (int damageHP) {
        damageHP -= m_TargetableObjectData.Def;

        if (damageHP < 0) {
            damageHP = 0;
        }

        float fromHPRatio = m_TargetableObjectData.HPRatio;
        m_TargetableObjectData.HP -= damageHP;
        float toHPRatio = m_TargetableObjectData.HPRatio;
        if (fromHPRatio > toHPRatio) {
            // GameEntry.HPBar.ShowHPBar (this, fromHPRatio, toHPRatio);
        }

        m_HPBar.UpdatePower (m_TargetableObjectData.HP, m_TargetableObjectData.MaxHP);

        if (m_TargetableObjectData.HP <= 0) {
            OnDead ();
        }
    }

    protected override void OnInit (object userData) {
        base.OnInit (userData);
        CachedTransform.SetLayerRecursively (Constant.Layer.TargetableObjectLayerId);
    }

    protected override void OnShow (object userData) {
        base.OnShow (userData);

        m_TargetableObjectData = userData as TargetableObjectData;
        if (m_TargetableObjectData == null) {
            Log.Error ("Targetable object data is invalid.");
            return;
        }

        /* 附加血量条 */
        PowerBarData hpBarData = new PowerBarData (EntityExtension.GenerateSerialId (), 1, this.Id, CampType.Player);
        EntityExtension.ShowPowerBar (typeof (PowerBar), "PowerBarGroup", hpBarData);

    }

    protected override void OnAttached (EntityLogic childEntity, Transform parentTransform, object userData) {
        base.OnAttached (childEntity, parentTransform, userData);

        if (childEntity is PowerBar) {
            m_HPBar = (PowerBar) childEntity;
            m_HPBar.UpdatePower (m_TargetableObjectData.HP, m_TargetableObjectData.MaxHP);
            return;
        }
    }

    protected virtual void OnDead () {
        GameEntry.Entity.HideEntity (this.Entity);
    }

    private void OnTriggerEnter (Collider other) {
        Entity entity = other.gameObject.GetComponent<Entity> ();
        if (entity == null) {
            return;
        }

        if (entity is TargetableObject && entity.Id >= Id) {
            // 碰撞事件由 Id 小的一方处理，避免重复处理
            return;
        }

        AIUtility.PerformCollision (this, entity);
    }
}