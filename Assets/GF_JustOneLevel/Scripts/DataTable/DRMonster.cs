using System.Collections.Generic;
using GameFramework.DataTable;

/// <summary>
/// 怪物表
/// </summary>
public class DRMonster : IDataRow {
    /// <summary>
    /// 编号
    /// </summary>
    public int Id {
        get;
        private set;
    }

    /// <summary>
    /// 移动速度。
    /// </summary>
    public float MoveSpeed {
        get;
        private set;
    }

    /// <summary>
    /// 旋转速度。
    /// </summary>
    public float RotateSpeed {
        get;
        private set;
    }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name {
        get;
        private set;
    }

    public void ParseDataRow (string dataRowText) {
        string[] text = DataTableExtension.SplitDataRow (dataRowText);
        int index = 0;
        index++;
        Id = int.Parse (text[index++]);
        Name = text[index++];
        MoveSpeed = float.Parse (text[index++]);
        RotateSpeed = float.Parse (text[index++]);
    }

    private void AvoidJIT () {
        new Dictionary<int, DRMonster> ();
    }
}