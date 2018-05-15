using GameFramework;
using UnityGameFramework.Runtime;

public class UIPlayerOperate : UGuiForm {
    private ProcedureGame m_ProcedureGame = null;
    
    /// <summary>
    /// 界面打开。
    /// </summary>
    /// <param name="userData">用户自定义数据。</param>
    protected override void OnOpen (object userData) {
        base.OnOpen(userData);

        m_ProcedureGame = userData as ProcedureGame;
    }

    /// <summary>
    /// 点击攻击按钮
    /// </summary>
    public void OnAtkClick() {
        GameEntry.Event.Fire(this, new ClickAttackButtonEventArgs(){ });
    }

    /// <summary>
    /// 点击返回按钮
    /// </summary>
    public void OnBackClick() {
        if (m_ProcedureGame != null) {
            m_ProcedureGame.Back();
        }
    }
}