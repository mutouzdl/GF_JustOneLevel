using System;
using System.Collections.Generic;
using GameFramework.DataTable;
using UnityEngine;

[Serializable]
public class HeroData : FightEntityData {
    public HeroData (int entityId, int typeId, CampType camp) : base (entityId, typeId, camp) {
        IDataTable<DRHero> dtHero = GameEntry.DataTable.GetDataTable<DRHero> ();
        DRHero drHero = dtHero.GetDataRow (typeId);
        if (drHero == null) {
            return;
        }

        HP = drHero.HP;
        MaxHP = HP;
        MoveSpeed = drHero.MoveSpeed;
        RotateSpeed = drHero.RotateSpeed;
        Atk = drHero.Atk;
        AtkRange = drHero.AtkRange;
        Def = drHero.Def;
        AtkSpeed = drHero.AtkSpeed;
        
        for (int i = 0; i < drHero.GetWeaponCount(); i++) {
            m_WeaponDatas.Add (new WeaponData (EntityExtension.GenerateSerialId (), drHero.GetWeaponID(i), Id, Camp));
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

        if (HP > MaxHP) {
            HP = MaxHP;
        }
    }
}