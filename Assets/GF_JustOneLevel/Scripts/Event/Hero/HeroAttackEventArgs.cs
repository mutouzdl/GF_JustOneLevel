using GameFramework.Event;
using UnityGameFramework.Runtime;

/// <summary>
/// 英雄攻击事件。
/// </summary>
public sealed class HeroAttackEventArgs : GameEventArgs {
    /// <summary>
    /// 事件编号。
    /// </summary>
    public static readonly int EventId = typeof (HeroAttackEventArgs).GetHashCode ();

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