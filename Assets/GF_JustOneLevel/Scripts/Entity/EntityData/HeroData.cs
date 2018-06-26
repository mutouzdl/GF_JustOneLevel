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

        Name = drHero.Name;
        HP = drHero.HP;
        MaxMP = drHero.MP;
        MaxHP = HP;
        MP = 0;
        MoveSpeed = drHero.MoveSpeed;
        RotateSpeed = drHero.RotateSpeed;
        Atk = drHero.Atk;
        AtkRange = drHero.AtkRange;
        Def = drHero.Def;
        AtkSpeed = drHero.AtkSpeed;

        for (int i = 0; i < drHero.GetWeaponCount (); i++) {
            weaponDatas.Add (new WeaponData (EntityExtension.GenerateSerialId (), drHero.GetWeaponID (i), Id, Camp));
        }
    }

    /// <summary>
    /// 根据给定的属性，加强英雄
    /// </summary>
    /// <param name="atkPowerUp"></param>
    /// <param name="defPowerUp"></param>
    /// <param name="hpPowerUp"></param>
    public void PowerUp (int atkPowerUp, int defPowerUp, int hpPowerUp) {
        this.Atk += atkPowerUp;
        this.Def += defPowerUp;

        if (hpPowerUp != 0) {
            this.MaxHP += hpPowerUp;
            this.HP = this.MaxHP;
        }
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