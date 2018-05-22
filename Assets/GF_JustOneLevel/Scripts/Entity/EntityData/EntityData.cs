using System;
using UnityEngine;

[Serializable]
public abstract class EntityData {
    [SerializeField]
    private int id = 0;

    [SerializeField]
    private int typeId = 0;

    [SerializeField]
    private Vector3 position = Vector3.zero;

    [SerializeField]
    private Quaternion rotation = Quaternion.identity;

    public EntityData (int entityId, int typeId) {
        id = entityId;
        this.typeId = typeId;
    }

    /// <summary>
    /// 实体编号。
    /// </summary>
    public int Id {
        get {
            return id;
        }
    }

    /// <summary>
    /// 实体类型编号。
    /// </summary>
    public int TypeId {
        get {
            return typeId;
        }
    }

    /// <summary>
    /// 实体位置。
    /// </summary>
    public Vector3 Position {
        get {
            return position;
        }
        set {
            position = value;
        }
    }

    /// <summary>
    /// 实体朝向。
    /// </summary>
    public Quaternion Rotation {
        get {
            return rotation;
        }
        set {
            rotation = value;
        }
    }
}