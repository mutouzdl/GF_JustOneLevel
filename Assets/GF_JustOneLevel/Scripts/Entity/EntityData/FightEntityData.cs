using System;
using System.Collections.Generic;
using GameFramework.DataTable;
using UnityEngine;

[Serializable]
public class FightEntityData : TargetableObjectData {
    protected List<WeaponData> weaponDatas = new List<WeaponData> ();
    
    public FightEntityData (int entityId, int typeId, CampType camp) : base (entityId, typeId, camp) {
    }

    /// <summary>
    /// 获取武器数据列表
    /// </summary>
    /// <returns></returns>
    public List<WeaponData> GetWeaponDatas() {
        return weaponDatas;
    }

    /// <summary>
    /// 最大生命。
    /// </summary>
    public override int MaxHP {
        get;
        protected set;
    }

    /// <summary>
    /// 移动速度。
    /// </summary>
    public float MoveSpeed {
        get;
        protected set;
    }

    /// <summary>
    /// 旋转速度。
    /// </summary>
    public float RotateSpeed {
        get;
        protected set;
    }

    /// <summary>
    /// 攻击力
    /// </summary>
    public int Atk {
        get;
        protected set;
    }

    /// <summary>
    /// 攻击范围
    /// </summary>
    public float AtkRange {
        get;
        protected set;
    }
    
    /// <summary>
    /// 攻速
    /// </summary>
    public float AtkSpeed {
        get;
        protected set;
    }

    public int DeadEffectId {
        get;
        protected set;
    }

    public int DeadSoundId {
        get;
        protected set;
    }

    private void RefreshData () {
        // m_MaxHP = 0;
        // m_Defense = 0;
        // for (int i = 0; i < m_ArmorDatas.Count; i++)
        // {
        //     m_MaxHP += m_ArmorDatas[i].MaxHP;
        //     m_Defense += m_ArmorDatas[i].Defense;
        // }

        if (HP > MaxHP) {
            HP = MaxHP;
        }
    }
}