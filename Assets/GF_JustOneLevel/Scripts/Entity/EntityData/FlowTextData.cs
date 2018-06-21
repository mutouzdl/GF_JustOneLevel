using System;
using GameFramework.DataTable;
using UnityEngine;

[Serializable]
public class FlowTextData : EntityData {
    /// <summary>
    /// 普通的显示漂浮文字构造器
    /// </summary>
    /// <param name="entityId"></param>
    /// <param name="ownerId"></param>
    /// <param name="msg"></param>
    /// <param name="rgbColor"></param>
    public FlowTextData (int entityId, int ownerId, string msg, string rgbColor = "#000000") : base(entityId, 0) {
        Text = msg;
        Color = rgbColor;
        OwnerId = ownerId;
    }

    /// <summary>
    /// 显示血量漂浮文字构造器
    /// </summary>
    /// <param name="entityId"></param>
    /// <param name="ownerId"></param>
    /// <param name="addHP"></param>
    public FlowTextData (int entityId, int ownerId, int addHP, bool forceAdd = false) : base(entityId, 0) {
        if (addHP > 0 || forceAdd) {
            Text = $"+{addHP}";
            Color = "#98FB98";
        }
        else {
            Text = $"{addHP}";
            Color = "#FF0000";
        }
        OwnerId = ownerId;
    }

    /// <summary>
    /// 拥有者ID
    /// </summary>
    /// <returns></returns>
    public int OwnerId {
        get;
        private set;
    } = 0;

    /// <summary>
    /// 文字内容
    /// </summary>
    /// <returns></returns>
    public string Text {
        get;
        private set;
    }

    /// <summary>
    /// 颜色
    /// </summary>
    public string Color {
        get;
        private set;
    }
}