using System.Collections.Generic;
using GameFramework.DataTable;

/// <summary>
/// 武器表。
/// </summary>
public class DRWeapon : IDRAssetsRow {
    /// <summary>
    /// 武器编号。
    /// </summary>
    public int Id {
        get;
        private set;
    }

    /// <summary>
    /// 资源名称。
    /// </summary>
    public string AssetName {
        get;
        protected set;
    }

    /// <summary>
    /// 攻击力。
    /// </summary>
    public int Attack {
        get;
        private set;
    }

    /// <summary>
    /// 子弹编号。
    /// </summary>
    public int BulletId {
        get;
        private set;
    }

    /// <summary>
    /// 子弹速度。
    /// </summary>
    public float BulletSpeed {
        get;
        private set;
    }
    
    /// <summary>
    /// 子弹攻击速度。
    /// </summary>
    public float AtkSpeed {
        get;
        private set;
    }

    /// <summary>
    /// 子弹声音编号。
    /// </summary>
    public int BulletSoundId {
        get;
        private set;
    }

    /// <summary>
    /// 攻击类型
    /// </summary>
    public int AttackType {
        get;
        private set;
    }

    /// <summary>
    /// 消耗MP
    /// </summary>
    /// <returns></returns>
    public int CostMP {
        get;
        private set;
    }

    /// <summary>
    /// 名称
    /// </summary>
    /// <returns></returns>
    public string Name {
        get;
        private set;
    }

    public void ParseDataRow (string dataRowText) {
        string[] text = DataTableExtension.SplitDataRow (dataRowText);
        int index = 0;
        index++;
        Id = int.Parse (text[index++]);
        index++; // 跳过备注列
        Name = text[index++];
        AssetName = text[index++];
        Attack = int.Parse (text[index++]);
        BulletId = int.Parse (text[index++]);
        BulletSpeed = float.Parse (text[index++]);
        AtkSpeed = float.Parse (text[index++]);
        BulletSoundId = int.Parse (text[index++]);
        AttackType = int.Parse(text[index++]);
        CostMP = int.Parse(text[index++]);
    }

    private void AvoidJIT () {
        new Dictionary<int, DRWeapon> ();
    }
}