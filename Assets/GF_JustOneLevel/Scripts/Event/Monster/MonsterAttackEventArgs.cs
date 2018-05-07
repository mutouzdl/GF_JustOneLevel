using GameFramework.Event;
using UnityGameFramework.Runtime;

/// <summary>
/// 怪物攻击事件。
/// </summary>
public sealed class MonsterAttackEventArgs : GameEventArgs {
    /// <summary>
    /// 事件编号。
    /// </summary>
    public static readonly int EventId = typeof (MonsterAttackEventArgs).GetHashCode ();

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