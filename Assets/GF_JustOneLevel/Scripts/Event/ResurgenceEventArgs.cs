using GameFramework.Event;
using UnityGameFramework.Runtime;

/// <summary>
/// 死亡事件。
/// </summary>
public sealed class ResurgenceEventArgs : GameEventArgs {
    /// <summary>
    /// 事件编号。
    /// </summary>
    public static readonly int EventId = typeof (ResurgenceEventArgs).GetHashCode ();

    /// <summary>
    /// 获取事件编号。
    /// </summary>
    public override int Id {
        get {
            return EventId;
        }
    }

    public override void Clear() {
        
    }
}