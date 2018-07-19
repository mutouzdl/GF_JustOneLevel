using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.DataTable;
using UnityEngine;

[Serializable]
public class MonsterData : FightEntityData {

    public MonsterData (int entityId, int typeId, CampType camp, int prize) : base (entityId, typeId, camp) {
        IDataTable<DRMonster> dtMonster = GameEntry.DataTable.GetDataTable<DRMonster> ();
        DRMonster drMonster = dtMonster.GetDataRow (typeId);
        if (drMonster == null) {
            return;
        }

        Name = drMonster.Name;
        HP = drMonster.HP;
        MaxHP = HP;
        MoveSpeed = drMonster.MoveSpeed;
        RotateSpeed = drMonster.RotateSpeed;
        Atk = drMonster.Atk;
        AtkAnimTime = drMonster.AtkAnimTime;
        AtkRange = drMonster.AtkRange;
        SeekRange = drMonster.SeekRange;
        Def = drMonster.Def;
        AtkSpeed = drMonster.AtkSpeed;

        this.Prize = prize;

        for (int i = 0; i < drMonster.GetWeaponCount (); i++) {
            weaponDatas.Add (new WeaponData (EntityExtension.GenerateSerialId (), drMonster.GetWeaponID (i), Id, Camp));
        }
    }

    /// <summary>
    /// 调整属性
    /// </summary>
    /// <param name="powerPercent">调整百分比</param>
    public void AjustPower (float powerPercent) {
        SeekRange = SeekRange * powerPercent;
        AtkSpeed = AtkSpeed - AtkSpeed * (powerPercent - 1);
        Atk = (int) (Atk * powerPercent);
        Def = (int) (Def * powerPercent);
        HP = (int) (HP * powerPercent);

        MaxHP = HP;
        if (AtkSpeed < 0.5f) {
            AtkSpeed = 0.5f;
        }

        PowerPercent = powerPercent;
    }
    
    /// <summary>
    /// 强化百分比（正常为1）
    /// </summary>
    /// <returns></returns>
    public float PowerPercent {
        get;
        private set;
    }

    /// <summary>
    /// 攻击范围
    /// </summary>
    public float SeekRange {
        get;
        protected set;
    }
    

    /// <summary>
    /// 怪物奖励分值
    /// </summary>
    /// <returns></returns>
    public int Prize {
        get;
        protected set;
    }

}