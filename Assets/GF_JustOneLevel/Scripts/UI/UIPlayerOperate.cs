using GameFramework;
using UnityGameFramework.Runtime;

public class UIPlayerOperate : UIFormLogic {
    /// <summary>
    /// 玩家点击攻击按钮
    /// </summary>
    public void OnAtkClick() {
        Log.Info("OnAtkClick");
        GameEntry.Event.Fire(this, new ClickAttackButtonEventArgs(){ });
    }
}