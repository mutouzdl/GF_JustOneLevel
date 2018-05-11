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
    /// 死亡实体阵营类型
    /// </summary>
    public CampType CampType {
        get;
        private set;
    }

    /// <summary>
    /// 死亡奖励
    /// </summary>
    /// <returns></returns>
    public int Prize {
        get;
        private set;
    }

    /// <summary>
    /// 清理事件。
    /// </summary>
    public override void Clear () {
        Prize = 0;
    }

    /// <summary>
    /// 填充事件
    /// </summary>
    /// <param name="UserData"></param>
    public DeadEventArgs Fill (CampType type, int prize) {
        this.CampType = type;
        this.Prize = prize;
        return this;
    }
}