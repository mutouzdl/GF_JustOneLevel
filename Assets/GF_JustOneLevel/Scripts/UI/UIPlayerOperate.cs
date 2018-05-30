using GameFramework;
using UnityGameFramework.Runtime;

public class UIPlayerOperate : UGuiForm {
    private ProcedureGame procedureGame = null;
    
    /// <summary>
    /// 界面打开。
    /// </summary>
    /// <param name="userData">用户自定义数据。</param>
    protected override void OnOpen (object userData) {
        base.OnOpen(userData);

        procedureGame = userData as ProcedureGame;
    }

    /// <summary>
    /// 点击攻击按钮
    /// </summary>
    public void OnAtkClick() {
        GameEntry.Event.Fire(this, ReferencePool.Acquire<ClickAttackButtonEventArgs>());
    }

    /// <summary>
    /// 点击愤怒技能按钮
    /// </summary>
    public void OnAngerSkillClick() {
        ClickAttackButtonEventArgs args = ReferencePool.Acquire<ClickAttackButtonEventArgs>()
            .Fill(WeaponAttackType.技能触发, 200);
        GameEntry.Event.Fire(this, args);
    }

    /// <summary>
    /// 点击返回按钮
    /// </summary>
    public void OnBackClick() {
        if (procedureGame != null) {
            procedureGame.Back();
        }
    }
}