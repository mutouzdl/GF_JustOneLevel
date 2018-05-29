using System;
using System.Collections.Generic;
using GameFramework.DataTable;
using UnityEngine;

[Serializable]
public class HeroData : FightEntityData {
    /// <summary>
    /// 愤怒值
    /// </summary>
    /// <returns></returns>
    public int MP {
        get;
        private set;
    }

    /// <summary>
    /// 最大愤怒值
    /// </summary>
    /// <returns></returns>
    public int MaxMP {
        get;
        private set;
    }

    public HeroData (int entityId, int typeId, CampType camp) : base (entityId, typeId, camp) {
        IDataTable<DRHero> dtHero = GameEntry.DataTable.GetDataTable<DRHero> ();
        DRHero drHero = dtHero.GetDataRow (typeId);
        if (drHero == null) {
            return;
        }

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
    /// 增加愤怒值
    /// </summary>
    /// <param name="value"></param>
    public void AddMP(int value) {
        this.MP += value;

        if (this.MP > this.MaxMP) {
            this.MP = this.MaxMP;
        }
    }

    /// <summary>
    /// 根据给定的属性，加强英雄
    /// </summary>
    /// <param name="atk"></param>
    /// <param name="def"></param>
    /// <param name="hp"></param>
    public void PowerUp (int atk, int def, int hp) {
        int atkPowerUp = atk / 2;
        int defPowerUp = def / 6;
        int hpPowerUp = hp / 4;

        this.Atk += atkPowerUp > 0 ? atkPowerUp : 1;
        this.Def += defPowerUp > 0 ? defPowerUp : 1;
        this.MaxHP += hpPowerUp > 0 ? hpPowerUp : 1;

        this.HP = this.MaxHP;
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