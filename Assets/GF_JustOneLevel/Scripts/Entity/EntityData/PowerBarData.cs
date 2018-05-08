using System;
using GameFramework.DataTable;
using UnityEngine;

[Serializable]
public class PowerBarData : AccessoryObjectData {
    public PowerBarData (int entityId, int typeId, int ownerId, CampType ownerCamp) : base (entityId, typeId, ownerId, ownerCamp) {
        IDataTable<DRPowerBar> dtPowerBar = GameEntry.DataTable.GetDataTable<DRPowerBar> ();
        DRPowerBar drPowerBar = dtPowerBar.GetDataRow (TypeId);
        if (drPowerBar == null) {
            return;
        }

        Color = drPowerBar.Color;
    }

    /// <summary>
    /// 颜色
    /// </summary>
    public string Color {
        get;
        private set;
    }
}