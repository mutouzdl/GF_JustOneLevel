using GameFramework;

/// <summary>
/// 对话框显示数据
/// 参考来源：https://github.com/EllanJiang/StarForce
/// </summary>
public class DialogParams {
    public enum DialogMode {
        单按钮 = 1,
        双按钮,
        三按钮,
    }

    /// <summary>
    /// 模式，即按钮数量。取值 1、2、3。
    /// </summary>
    public DialogMode Mode {
        get;
        set;
    } = DialogMode.单按钮;

    /// <summary>
    /// 标题。
    /// </summary>
    public string Title {
        get;
        set;
    } = GameEntry.Localization.GetString("Alert.OperateFail");

    /// <summary>
    /// 消息内容。
    /// </summary>
    public string Message {
        get;
        set;
    }

    /// <summary>
    /// 弹出窗口时是否暂停游戏。
    /// </summary>
    public bool PauseGame {
        get;
        set;
    }

    /// <summary>
    /// 确认按钮文本。
    /// </summary>
    public string ConfirmText {
        get;
        set;
    } = GameEntry.Localization.GetString("Dialog.ConfirmButton");

    /// <summary>
    /// 确定按钮回调。
    /// </summary>
    public GameFrameworkFunc<object, bool> OnClickConfirm {
        get;
        set;
    }

    /// <summary>
    /// 取消按钮文本。
    /// </summary>
    public string CancelText {
        get;
        set;
    } = GameEntry.Localization.GetString("Dialog.CancelButton");

    /// <summary>
    /// 取消按钮回调。
    /// </summary>
    public GameFrameworkAction<object> OnClickCancel {
        get;
        set;
    }

    /// <summary>
    /// 中立按钮文本。
    /// </summary>
    public string OtherText {
        get;
        set;
    }

    /// <summary>
    /// 其它按钮回调。
    /// </summary>
    public GameFrameworkFunc<object, bool> OnClickOther {
        get;
        set;
    }

    /// <summary>
    /// 用户自定义数据。
    /// </summary>
    public string UserData {
        get;
        set;
    }
}