using System;
using UnityEngine;

[Serializable]
public abstract class TargetableObjectData : EntityData {
    [SerializeField]
    private CampType m_Camp = CampType.Unknown;

    public TargetableObjectData (int entityId, int typeId, CampType camp) : base (entityId, typeId) {
        m_Camp = camp;
    }

    /// <summary>
    /// 角色阵营。
    /// </summary>
    public CampType Camp {
        get {
            return m_Camp;
        }
    }

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
    public abstract int MaxHP {
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