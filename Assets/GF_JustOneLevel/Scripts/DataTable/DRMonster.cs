using System.Collections.Generic;
using GameFramework;
using GameFramework.DataTable;

/// <summary>
/// 怪物表
/// </summary>
public class DRMonster : DREntity {
    /// <summary>
    /// 追踪范围
    /// </summary>
    /// <returns></returns>
    public float SeekRange {
        get;
        private set;
    }

    public override void ParseDataRow (string dataRowText) {
        string[] text = DataTableExtension.SplitDataRow (dataRowText);
        int index = 0;
        index++;
        Id = int.Parse (text[index++]);
        AssetName = text[index++];
        Name = text[index++];
        MoveSpeed = float.Parse (text[index++]);
        RotateSpeed = float.Parse (text[index++]);
        Atk = int.Parse (text[index++]);
        AtkAnimTime = float.Parse(text[index++]);
        AtkRange = float.Parse (text[index++]);
        SeekRange = float.Parse (text[index++]);
        Def = int.Parse (text[index++]);
        AtkSpeed = float.Parse (text[index++]);
        HP = int.Parse (text[index++]);

        ParseWeapon(text[index++]);
    }

    private void AvoidJIT () {
        new Dictionary<int, DRMonster> ();
    }
}