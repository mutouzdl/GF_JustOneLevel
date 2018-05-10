using GameFramework.Event;
using UnityGameFramework.Runtime;

/// <summary>
/// 死亡事件。
/// </summary>
public sealed class DeadEventArgs : GameEventArgs {
    /// <summary>
    /// 事件编号。
    /// </summary>
    public static readonly int EventId = typeof (DeadEventArgs).GetHashCode ();

    /// <summary>
    /// 获取事件编号。
    /// </summary>
    public override int Id {
        get {
            return EventId;
        }
    }

    /// <summary>
    /// 获取用户自定义数据。
    /// </summary>
    public object UserData {
        get;
        private set;
    }

    /// <summary>
    /// 清理事件。
    /// </summary>
    public override void Clear () {
        UserData = default (object);
    }
}