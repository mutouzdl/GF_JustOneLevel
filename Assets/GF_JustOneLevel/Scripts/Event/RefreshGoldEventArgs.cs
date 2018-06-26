using GameFramework.Event;
using UnityGameFramework.Runtime;

/// <summary>
/// 刷新金币事件。
/// </summary>
public sealed class RefreshGoldEventArgs : GameEventArgs {
    /// <summary>
    /// 事件编号。
    /// </summary>
    public static readonly int EventId = typeof (RefreshGoldEventArgs).GetHashCode ();

    /// <summary>
    /// 获取事件编号。
    /// </summary>
    public override int Id {
        get {
            return EventId;
        }
    }

    /// <summary>
    /// 清理事件。
    /// </summary>
    public override void Clear () {
    }
}