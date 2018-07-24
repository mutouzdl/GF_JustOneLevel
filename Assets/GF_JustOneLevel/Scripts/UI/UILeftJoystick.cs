using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class UILeftJoystick : UGuiForm {
    private ProcedureGame procedureGame = null;

    /// <summary>
    /// 界面初始化
    /// </summary>
    /// <param name="userData"></param>
    protected override void OnInit (object userData) {
        base.OnInit (userData);
    }

    /// <summary>
    /// 界面打开。
    /// </summary>
    /// <param name="userData">用户自定义数据。</param>
    protected override void OnOpen (object userData) {
        base.OnOpen (userData);

        procedureGame = userData as ProcedureGame;
    }

    protected override void OnClose (object userData) {
        base.OnClose(userData);
    }
}