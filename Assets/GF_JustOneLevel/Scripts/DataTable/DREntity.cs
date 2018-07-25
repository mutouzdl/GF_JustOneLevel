using System.Collections.Generic;
using GameFramework.DataTable;

/// <summary>
/// 实体表。
/// </summary>
public class DREntity : IDRAssetsRow {
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
    /// 攻击动画时间（秒）
    /// </summary>
    public float AtkAnimTime {
        get;
        protected set;
    }

    /// <summary>
    /// 攻击范围
    /// </summary>
    /// <returns></returns>
    public float AtkRange {
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
    
    public virtual void ParseDataRow (string dataRowText) {
        throw new System.NotImplementedException ();
    }

    protected void ParseWeapon(string strWeaponIDs) {
        /* 加载武器ID列表，武器以_分割 */
        if (!string.IsNullOrEmpty(strWeaponIDs)) {
            string[] arrWeaponIDs = strWeaponIDs.Split('_');
            foreach(string weaponID in arrWeaponIDs) {
                m_WeaponIDs.Add(int.Parse(weaponID));
            }
        }
    }

    private void AvoidJIT () {
        new Dictionary<int, DREntity> ();
    }
}