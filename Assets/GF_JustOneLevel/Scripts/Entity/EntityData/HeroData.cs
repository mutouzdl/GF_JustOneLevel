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
        AtkAnimTime = drHero.AtkAnimTime;
        AtkRange = drHero.AtkRange;
        Def = drHero.Def;
        HPAbsorbPercent = drHero.HPAbsorbPercent;
        DefAbsorbPercent = drHero.DefAbsorbPercent;
        AtkAbsorbPercent = drHero.AtkAbsorbPercent;
        AtkSpeedAbsorbPercent = drHero.AtkSpeedAbsorbPercent;
        HPMinAbsorb = drHero.HPMinAbsorb;
        DefMinAbsorb = drHero.DefMinAbsorb;
        AtkMinAbsorb = drHero.AtkMinAbsorb;
        AtkSpeedMinAbsorb = drHero.AtkSpeedMinAbsorb;

        for (int i = 0; i < drHero.GetWeaponCount (); i++) {
            weaponDatas.Add (new WeaponData (EntityExtension.GenerateSerialId (), drHero.GetWeaponID (i), Id, Camp));
        }
    }

    /// <summary>
    /// 根据给定的属性及英雄的吸收能力，加强英雄
    /// </summary>
    public void PowerUpByAbsorbPower (int hp, int def, int atk, float atkSpeed) {
        int hpPowerUp = (int) (hp * HPAbsorbPercent);
        int defPowerUp = (int) (def * DefAbsorbPercent);
        int atkPowerUp = (int) (atk * AtkAbsorbPercent);
        float atkSpeedPowerUp = atkSpeed * AtkSpeedAbsorbPercent;

        hpPowerUp = hpPowerUp > HPMinAbsorb ? hpPowerUp : HPMinAbsorb;
        defPowerUp = defPowerUp > DefMinAbsorb ? defPowerUp : DefMinAbsorb;
        atkPowerUp = atkPowerUp > AtkMinAbsorb ? atkPowerUp : AtkMinAbsorb;
        atkSpeedPowerUp = atkSpeedPowerUp > AtkSpeedMinAbsorb ? atkSpeedPowerUp : AtkSpeedMinAbsorb;

        PowerUpByAbsValue(hpPowerUp, defPowerUp, atkPowerUp, atkSpeedPowerUp);
    }

    /// <summary>
    /// 根据给定的属性值，加强英雄
    /// </summary>
    /// <param name="hpPowerUp"></param>
    /// <param name="defPowerUp"></param>
    /// <param name="atkPowerUp"></param>
    /// <param name="atkSpeedPowerUp"></param>
    public void PowerUpByAbsValue (int hpPowerUp, int defPowerUp, int atkPowerUp, float atkSpeedPowerUp) {
        if (hpPowerUp != 0) {
            this.MaxHP += hpPowerUp;
            this.HP = this.MaxHP;
        }

        this.Def += defPowerUp;
        this.Atk += atkPowerUp;
        this.AtkSpeed -= atkSpeedPowerUp;

        if (this.AtkSpeed < 0.1f) {
            this.AtkSpeed = 0.1f;
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

    /// <summary>
    /// 血量吸收百分比
    /// </summary>
    /// <value></value>
    public float HPAbsorbPercent {
        get;
        private set;
    }

    /// <summary>
    /// 防御吸收百分比
    /// </summary>
    /// <value></value>
    public float DefAbsorbPercent {
        get;
        private set;
    }

    /// <summary>
    /// 攻击吸收百分比
    /// </summary>
    /// <value></value>
    public float AtkAbsorbPercent {
        get;
        private set;
    }

    /// <summary>
    /// 攻速吸收百分比
    /// </summary>
    /// <value></value>
    public float AtkSpeedAbsorbPercent {
        get;
        private set;
    }

    /// <summary>
    /// 血量最低吸收值
    /// </summary>
    /// <value></value>
    public int HPMinAbsorb {
        get;
        private set;
    }

    /// <summary>
    /// 防御最低吸收值
    /// </summary>
    /// <value></value>
    public int DefMinAbsorb {
        get;
        private set;
    }

    /// <summary>
    /// 攻击最低吸收值
    /// </summary>
    /// <value></value>
    public int AtkMinAbsorb {
        get;
        private set;
    }

    /// <summary>
    /// 攻速最低吸收值
    /// </summary>
    /// <value></value>
    public float AtkSpeedMinAbsorb {
        get;
        private set;
    }

}