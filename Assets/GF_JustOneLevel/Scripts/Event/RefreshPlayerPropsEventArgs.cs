using GameFramework.Event;
using UnityGameFramework.Runtime;

/// <summary>
/// 刷新英雄属性事件。
/// </summary>
public sealed class RefreshHeroPropsEventArgs : GameEventArgs {
    /// <summary>
    /// 事件编号。
    /// </summary>
    public static readonly int EventId = typeof (RefreshHeroPropsEventArgs).GetHashCode ();

    /// <summary>
    /// 获取事件编号。
    /// </summary>
    public override int Id {
        get {
            return EventId;
        }
    }

    /// <summary>
    /// 英雄数据
    /// </summary>
    /// <returns></returns>
    public HeroData HeroData {
        get;
        private set;
    }

    /// <summary>
    /// 清理事件。
    /// </summary>
    public override void Clear () {
        HeroData = null;
    }

    /// <summary>
    /// 填充事件
    /// </summary>
    /// <param name="UserData"></param>
    public RefreshHeroPropsEventArgs Fill (HeroData heroData) {
        this.HeroData = heroData;
        return this;
    }
}