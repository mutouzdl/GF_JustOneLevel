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

    /// <summary>
    /// 武器ID列表
    /// </summary>
    /// <returns></returns>
    private List<int> m_WeaponIDs = new List<int>();

    /// <summary>
    /// 获取武器ID
    /// </summary>
    /// <param name="index">武器索引</param>
    /// <returns></returns>
    public int GetWeaponID(int index) {
        if (m_WeaponIDs.Count > index) {
            return m_WeaponIDs[index];
        }

        return 0;
    }

    /// <summary>
    /// 获取武器数量
    /// </summary>
    /// <returns></returns>
    public int GetWeaponCount() {
        return m_WeaponIDs.Count;
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
        AtkRange = float.Parse (text[index++]);
        SeekRange = float.Parse (text[index++]);
        Def = int.Parse (text[index++]);
        AtkSpeed = float.Parse (text[index++]);
        HP = int.Parse (text[index++]);

        /* 加载武器ID列表，武器以_分割 */
        string strWeaponIDs = text[index++];
        if (!string.IsNullOrEmpty(strWeaponIDs)) {
            string[] arrWeaponIDs = strWeaponIDs.Split('_');
            foreach(string weaponID in arrWeaponIDs) {
                m_WeaponIDs.Add(int.Parse(weaponID));
            }
        }
    }

    private void AvoidJIT () {
        new Dictionary<int, DRMonster> ();
    }
}