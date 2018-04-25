using System;
using System.Collections.Generic;
using GameFramework.DataTable;
using UnityEngine;

[Serializable]
public class HeroData : TargetableObjectData {
    // [SerializeField]
    // private ThrusterData m_ThrusterData = null;

    // [SerializeField]
    // private List<WeaponData> m_WeaponDatas = new List<WeaponData>();

    // [SerializeField]
    // private List<ArmorData> m_ArmorDatas = new List<ArmorData>();

    [SerializeField]
    private int m_MaxHP = 0;

    [SerializeField]
    private int m_Defense = 0;
    [SerializeField]
    private float m_MoveSpeed = 0;
    [SerializeField]
    private float m_RotateSpeed = 0;

    [SerializeField]
    private int m_DeadEffectId = 0;

    [SerializeField]
    private int m_DeadSoundId = 0;

    public HeroData (int entityId, int typeId, CampType camp) : base (entityId, typeId, camp) {
        IDataTable<DRHero> dtHero = GameEntry.DataTable.GetDataTable<DRHero> ();
        DRHero drHero = dtHero.GetDataRow (typeId);
        if (drHero == null) {
            return;
        }

        // m_ThrusterData = new ThrusterData(GameEntry.Entity.GenerateSerialId(), drAircraft.ThrusterId, Id, Camp);

        // for (int index = 0, weaponId = 0; (weaponId = drAircraft.GetWeaponIds(index)) > 0; index++)
        // {
        //     AttachWeaponData(new WeaponData(GameEntry.Entity.GenerateSerialId(), weaponId, Id, Camp));
        // }

        // for (int index = 0, armorId = 0; (armorId = drAircraft.GetArmorIds(index)) > 0; index++)
        // {
        //     AttachArmorData(new ArmorData(GameEntry.Entity.GenerateSerialId(), armorId, Id, Camp));
        // }

        HP = m_MaxHP;
        m_MoveSpeed = drHero.MoveSpeed;
        m_RotateSpeed = drHero.RotateSpeed;
    }

    /// <summary>
    /// 最大生命。
    /// </summary>
    public override int MaxHP {
        get {
            return m_MaxHP;
        }
    }

    /// <summary>
    /// 防御。
    /// </summary>
    public int Defense {
        get {
            return m_Defense;
        }
    }

    /// <summary>
    /// 移动速度。
    /// </summary>
    public float MoveSpeed {
        get {
            return m_MoveSpeed;
        }
    }

    /// <summary>
    /// 旋转速度。
    /// </summary>
    public float RotateSpeed {
        get {
            return m_RotateSpeed;
        }
    }

    public int DeadEffectId {
        get {
            return m_DeadEffectId;
        }
    }

    public int DeadSoundId {
        get {
            return m_DeadSoundId;
        }
    }

    // public ThrusterData GetThrusterData()
    // {
    //     return m_ThrusterData;
    // }

    // public List<WeaponData> GetAllWeaponDatas()
    // {
    //     return m_WeaponDatas;
    // }

    // public void AttachWeaponData(WeaponData weaponData)
    // {
    //     if (weaponData == null)
    //     {
    //         return;
    //     }

    //     if (m_WeaponDatas.Contains(weaponData))
    //     {
    //         return;
    //     }

    //     m_WeaponDatas.Add(weaponData);
    // }

    // public void DetachWeaponData(WeaponData weaponData)
    // {
    //     if (weaponData == null)
    //     {
    //         return;
    //     }

    //     m_WeaponDatas.Remove(weaponData);
    // }

    // public List<ArmorData> GetAllArmorDatas()
    // {
    //     return m_ArmorDatas;
    // }

    // public void AttachArmorData(ArmorData armorData)
    // {
    //     if (armorData == null)
    //     {
    //         return;
    //     }

    //     if (m_ArmorDatas.Contains(armorData))
    //     {
    //         return;
    //     }

    //     m_ArmorDatas.Add(armorData);
    //     RefreshData();
    // }

    // public void DetachArmorData(ArmorData armorData)
    // {
    //     if (armorData == null)
    //     {
    //         return;
    //     }

    //     m_ArmorDatas.Remove(armorData);
    //     RefreshData();
    // }

    private void RefreshData () {
        // m_MaxHP = 0;
        // m_Defense = 0;
        // for (int i = 0; i < m_ArmorDatas.Count; i++)
        // {
        //     m_MaxHP += m_ArmorDatas[i].MaxHP;
        //     m_Defense += m_ArmorDatas[i].Defense;
        // }

        if (HP > m_MaxHP) {
            HP = m_MaxHP;
        }
    }
}