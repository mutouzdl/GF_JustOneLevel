using System.Collections.Generic;
using GameFramework.DataTable;

/// <summary>
/// 英雄表
/// </summary>
public class DRHero : DREntity {
    /// <summary>
    /// 魔法值
    /// </summary>
    /// <returns></returns>
    public int MP {
        get;
        private set;
    }

    /// <summary>
    /// 血量吸收百分比
    /// </summary>
    /// <value></value>
    public float HPAbsorbPercent {
        get;
        private set;
    }

    /// <summary>
    /// 防御吸收百分比
    /// </summary>
    /// <value></value>
    public float DefAbsorbPercent {
        get;
        private set;
    }

    /// <summary>
    /// 攻击吸收百分比
    /// </summary>
    /// <value></value>
    public float AtkAbsorbPercent {
        get;
        private set;
    }

    /// <summary>
    /// 攻速吸收百分比
    /// </summary>
    /// <value></value>
    public float AtkSpeedAbsorbPercent {
        get;
        private set;
    }

    /// <summary>
    /// 血量最低吸收值
    /// </summary>
    /// <value></value>
    public int HPMinAbsorb {
        get;
        private set;
    }
    
    /// <summary>
    /// 防御最低吸收值
    /// </summary>
    /// <value></value>
    public int DefMinAbsorb {
        get;
        private set;
    }
    
    /// <summary>
    /// 攻击最低吸收值
    /// </summary>
    /// <value></value>
    public int AtkMinAbsorb {
        get;
        private set;
    }
    
    /// <summary>
    /// 攻速最低吸收值
    /// </summary>
    /// <value></value>
    public float AtkSpeedMinAbsorb {
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
        Def = int.Parse (text[index++]);
        AtkSpeed = float.Parse (text[index++]);
        HP = int.Parse (text[index++]);
        MP = int.Parse(text[index++]);
        ParseWeapon(text[index++]);
        HPAbsorbPercent = float.Parse(text[index++]);
        DefAbsorbPercent = float.Parse(text[index++]);
        AtkAbsorbPercent = float.Parse(text[index++]);
        AtkSpeedAbsorbPercent = float.Parse(text[index++]);
        HPMinAbsorb = int.Parse(text[index++]);
        DefMinAbsorb = int.Parse(text[index++]);
        AtkMinAbsorb = int.Parse(text[index++]);
        AtkSpeedMinAbsorb = float.Parse(text[index++]);
    }

    private void AvoidJIT () {
        new Dictionary<int, DRHero> ();
    }
}