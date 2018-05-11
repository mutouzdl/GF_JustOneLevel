using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.DataTable;
using UnityEngine;

[Serializable]
public class MonsterData : FightEntityData {
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

    public MonsterData (int entityId, int typeId, CampType camp, int prize) : base (entityId, typeId, camp) {
        IDataTable<DRMonster> dtMonster = GameEntry.DataTable.GetDataTable<DRMonster> ();
        DRMonster drMonster = dtMonster.GetDataRow (typeId);
        if (drMonster == null) {
            return;
        }

        HP = drMonster.HP;
        MaxHP = HP;
        MoveSpeed = drMonster.MoveSpeed;
        RotateSpeed = drMonster.RotateSpeed;
        Atk = drMonster.Atk;
        AtkRange = drMonster.AtkRange;
        SeekRange = drMonster.SeekRange;
        Def = drMonster.Def;
        AtkSpeed = drMonster.AtkSpeed;

        this.Prize = prize;
    }
}