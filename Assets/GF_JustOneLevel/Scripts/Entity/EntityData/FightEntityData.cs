using System;
using System.Collections.Generic;
using GameFramework.DataTable;
using UnityEngine;

[Serializable]
public class FightEntityData : EntityData {
    protected List<WeaponData> weaponDatas = new List<WeaponData> ();
    
    public FightEntityData (int entityId, int typeId, CampType camp) : base (entityId, typeId) {
        this.Camp = camp;
    }

    /// <summary>
    /// 获取武器数据列表
    /// </summary>
    /// <returns></returns>
    public List<WeaponData> GetWeaponDatas() {
        return weaponDatas;
    }

    /// <summary>
    /// 移动速度。
    /// </summary>
    public float MoveSpeed {
        get;
        protected set;
    }

    /// <summary>
    /// 旋转速度。
    /// </summary>
    public float RotateSpeed {
        get;
        protected set;
    }

    /// <summary>
    /// 攻击力
    /// </summary>
    public int Atk {
        get;
        protected set;
    }

    /// <summary>
    /// 攻击范围
    /// </summary>
    public float AtkRange {
        get;
        protected set;
    }
    
    /// <summary>
    /// 攻速
    /// </summary>
    public float AtkSpeed {
        get;
        protected set;
    }

    public int DeadEffectId {
        get;
        protected set;
    }

    public int DeadSoundId {
        get;
        protected set;
    }

    /// <summary>
    /// 角色阵营。
    /// </summary>
    public CampType Camp {
        get;
        protected set;
    } = CampType.Unknown;

    /// <summary>
    /// 当前生命。
    /// </summary>
    public int HP {
        get;
        set;
    }

    /// <summary>
    /// 最大生命。
    /// </summary>
    public int MaxHP {
        get;
        protected set;
    }

    /// <summary>
    /// 生命百分比。
    /// </summary>
    public float HPRatio {
        get {
            return MaxHP > 0 ? (float) HP / MaxHP : 0f;
        }
    }
    
    /// <summary>
    /// 防御。
    /// </summary>
    public int Def {
        get;
        protected set;
    }
}