using GameFramework;
using UnityGameFramework.Runtime;

public class UIMenu : UGuiForm {
    private ProcedureMenu m_ProcedureMenu = null;
    
    /// <summary>
    /// 界面打开。
    /// </summary>
    /// <param name="userData">用户自定义数据。</param>
    protected override void OnOpen (object userData) {
        base.OnOpen(userData);

        m_ProcedureMenu = userData as ProcedureMenu;
    }

    /// <summary>
    /// 点击开始按钮
    /// </summary>
    public void OnStartClick() {
        if (m_ProcedureMenu != null) {
            m_ProcedureMenu.StartGame();
        }
    }

    /// <summary>
    /// 点击英雄商店按钮
    /// </summary>
    public void OnHeroShopClick() {
        if (m_ProcedureMenu != null) {
            m_ProcedureMenu.ShowHeroShop();
        }
    }
}