using System.Collections.Generic;
using GameFramework.DataTable;

/// <summary>
/// 实体表。
/// </summary>
public class DREntity : IDataRow {
    /// <summary>
    /// 实体编号。
    /// </summary>
    public int Id {
        get;
        protected set;
    }

    /// <summary>
    /// 资源名称。
    /// </summary>
    public string AssetName {
        get;
        protected set;
    }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name {
        get;
        protected set;
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
    /// <returns></returns>
    public int Atk {
        get;
        protected set;
    }

    /// <summary>
    /// 攻击范围
    /// </summary>
    /// <returns></returns>
    public int AtkRange {
        get;
        protected set;
    }

    /// <summary>
    /// 防御力
    /// </summary>
    /// <returns></returns>
    public int Def {
        get;
        protected set;
    }

    /// <summary>
    /// 攻速(秒)
    /// </summary>
    /// <returns></returns>
    public float AtkSpeed {
        get;
        protected set;
    }

    /// <summary>
    /// 血量
    /// </summary>
    /// <returns></returns>
    public int HP {
        get;
        protected set;
    }

    public virtual void ParseDataRow (string dataRowText) {
        throw new System.NotImplementedException ();
    }

    private void AvoidJIT () {
        new Dictionary<int, DREntity> ();
    }
}