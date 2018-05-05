using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.DataTable;
using UnityEngine;

[Serializable]
public class MonsterData : FightEntityData {
    public MonsterData (int entityId, int typeId, CampType camp) : base (entityId, typeId, camp) {
        IDataTable<DRMonster> dtMonster = GameEntry.DataTable.GetDataTable<DRMonster> ();
        DRMonster drMonster = dtMonster.GetDataRow (typeId);
        if (drMonster == null) {
            return;
        }
Log.Info("drMonster.HP:" + drMonster.HP);
        HP = drMonster.HP;
        MaxHP = HP;
        MoveSpeed = drMonster.MoveSpeed;
        RotateSpeed = drMonster.RotateSpeed;
        Atk = drMonster.Atk;
        AtkRange = drMonster.AtkRange;
        Def = drMonster.Def;
        AtkSpeed = drMonster.AtkSpeed;
    }
}