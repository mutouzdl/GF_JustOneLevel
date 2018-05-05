using System.Collections.Generic;
using GameFramework.DataTable;

/// <summary>
/// 英雄表
/// </summary>
public class DRHero : DREntity {
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
        AtkRange = int.Parse (text[index++]);
        Def = int.Parse (text[index++]);
        AtkSpeed = float.Parse (text[index++]);
        HP = int.Parse (text[index++]);
    }

    private void AvoidJIT () {
        new Dictionary<int, DRHero> ();
    }
}