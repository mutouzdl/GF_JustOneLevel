using GameFramework;
using GameFramework.DataTable;
using GameFramework.Resource;
using GameFramework.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class UIGameOver : UGuiForm {
    [SerializeField]
    private Text textPrize = null;
    [SerializeField]
    private Text textGold = null;
    [SerializeField]
    private Text textKill = null;
    [SerializeField]
    private Text textHoldTime = null;

    private ProcedureGame procedureGame = null;


    void Start () {
    }

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

        textGold.text = PlayerData.Gold.ToString();
        textHoldTime.text = $"{GlobalGame.GameTimes.ToString("F1")}s";
        textKill.text = GlobalGame.killCount.ToString();
        textPrize.text = GlobalGame.totalPrize.ToString();
    }


    /// <summary>
    /// 点击返回按钮
    /// </summary>
    public void OnBackClick () {
        if (procedureGame != null) {
            procedureGame.Back ();
        }
    }

    /// <summary>
    /// 点击继续按钮
    /// </summary>
    public void OnContinueClick () {
        if (procedureGame != null) {
            procedureGame.Continue ();
        }
    }
}