using System;
using System.Collections.Generic;
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

        HP = m_MaxHP;
        m_MoveSpeed = drMonster.MoveSpeed;
        m_RotateSpeed = drMonster.RotateSpeed;
    }
}