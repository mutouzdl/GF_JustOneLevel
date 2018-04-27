//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;
using GameFramework.Network;

/// <summary>
/// 英雄移动事件。
/// </summary>
public sealed class HeroWalkEventArgs : GameEventArgs {
    /// <summary>
    /// 事件编号。
    /// </summary>
    public static readonly int EventId = typeof (HeroWalkEventArgs).GetHashCode ();

    /// <summary>
    /// 事件编号。
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

    public override void Clear () {
        UserData = default (object);
    }
}