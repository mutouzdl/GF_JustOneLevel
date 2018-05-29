using System.Collections.Generic;
using GameFramework.DataTable;

/// <summary>
/// 英雄表
/// </summary>
public class DRHero : DREntity {
    /// <summary>
    /// 愤怒值
    /// </summary>
    /// <returns></returns>
    public int MP {
        get;
        private set;
    }

    /// <summary>
    /// 技能ID列表
    /// </summary>
    /// <returns></returns>
    private List<int> m_SkillIDs = new List<int>();

    /// <summary>
    /// 获取技能ID
    /// </summary>
    /// <param name="index">技能索引</param>
    /// <returns></returns>
    public int GetSkillID(int index) {
        if (m_SkillIDs.Count > index) {
            return m_SkillIDs[index];
        }

        return 0;
    }

    /// <summary>
    /// 获取技能数量
    /// </summary>
    /// <returns></returns>
    public int GetSkillCount() {
        return m_SkillIDs.Count;
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
        Def = int.Parse (text[index++]);
        AtkSpeed = float.Parse (text[index++]);
        HP = int.Parse (text[index++]);
        MP = int.Parse(text[index++]);
        ParseWeapon(text[index++]);
        ParseSkill(text[index++]);
    }

    private void ParseSkill(string strSkillIDs) {
        /* 加载技能ID列表，技能以_分割 */
        if (!string.IsNullOrEmpty(strSkillIDs)) {
            string[] arrSkillIDs = strSkillIDs.Split('_');
            foreach(string skillID in arrSkillIDs) {
                m_SkillIDs.Add(int.Parse(skillID));
            }
        }
    }

    private void AvoidJIT () {
        new Dictionary<int, DRHero> ();
    }
}