using GameFramework;
using GameFramework.Event;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using System;

public class ProcedureGame : ProcedureBase {
    private SurvivalGame survivalGame = null;
    /// <summary>
    /// 玩家操作UI
    /// </summary>
    private UIPlayerOperate uiPlayerOperate = null;
    /// <summary>
    /// 玩家信息UI
    /// </summary>
    private UIPlayerMessage uiPlayerMessage = null;
    /// <summary>
    /// 游戏结束UI
    /// </summary>
    private UIGameOver uiGameOver = null;

    private bool isPause = false;

    protected override void OnInit (ProcedureOwner procedureOwner) {
        base.OnInit (procedureOwner);

        survivalGame = new SurvivalGame ();
    }

    protected override void OnEnter (ProcedureOwner procedureOwner) {
        base.OnEnter (procedureOwner);

        survivalGame.Initialize ();

        // 播放音乐
        GameEntry.Sound.PlayMusic (Constant.Sound.GAME_MUSIC_ID);

        // 订阅事件
        GameEntry.Event.Subscribe (OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

        // 加载UI
        GameEntry.UI.OpenUIForm (UIFormId.PlayerOperate, this);
        GameEntry.UI.OpenUIForm (UIFormId.PlayerMessage, this);

        isPause = false;
    }

    protected override void OnLeave (ProcedureOwner procedureOwner, bool isShutdown) {
        base.OnLeave (procedureOwner, isShutdown);

        // 取消订阅
        GameEntry.Event.Unsubscribe (OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

        // 停止音乐
        GameEntry.Sound.StopMusic ();

        // 关闭UI
        if (uiPlayerOperate != null) {
            GameEntry.UI.CloseUIForm (uiPlayerOperate.UIForm);
            uiPlayerOperate = null;
        }

        if (uiPlayerMessage != null) {
            GameEntry.UI.CloseUIForm (uiPlayerMessage.UIForm);
            uiPlayerMessage = null;
        }

        if (uiGameOver != null) {
            GameEntry.UI.CloseUIForm (uiGameOver.UIForm);
        }
    }

    protected override void OnUpdate (ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds) {
        if (isPause) {
            return;
        }

        GlobalGame.GameTimes += elapseSeconds;

        if (survivalGame != null) {
            if (!GlobalGame.IsPause) {
                survivalGame.Update (elapseSeconds, realElapseSeconds);
            } else {
                GameOver (procedureOwner);
            }
        }
    }

    /// <summary>
    /// 返回菜单
    /// </summary>
    public void Back () {
        survivalGame.Shutdown ();
        m_ProcedureOwner.SetData<VarInt> (Constant.ProcedureData.NextSceneId, GameEntry.Config.GetInt ("Scene.Menu"));
        ChangeState<ProcedureChangeScene> (m_ProcedureOwner);
    }

    /// <summary>
    /// 继续游戏，续命
    /// </summary>
    public bool Continue () {
        int gold = PlayerData.Gold;

        if (gold < GlobalGame.ContinueCostGold) {
            GameEntry.UI.OpenDialog (new DialogParams () {
                Title = GameEntry.Localization.GetString ("Alert.OperateFail"),
                    Message = GameEntry.Localization.GetString ("Message.GoldNotEnough"),
                    OnClickConfirm = (object userData) => { return true; },
            });
            return false;
        }

        // 扣除金币进行复活
        PlayerData.Gold = gold - GlobalGame.ContinueCostGold;

        // 发送复活消息
        GameEntry.Event.Fire (this, new ResurgenceEventArgs ());

        // 刷新金币信息
        uiPlayerMessage.RefreshGold ();

        isPause = false;

        if (uiGameOver != null) {
            GameEntry.UI.CloseUIForm (uiGameOver.UIForm);
        }

        return true;
    }

    private void GameOver (ProcedureOwner procedureOwner) {
        if (isPause) {
            return;
        }

        // 打开失败UI
        GameEntry.UI.OpenUIForm (UIFormId.GameOver, this);
        // string des = GameEntry.Localization.GetString ("GameOver.Des");
        // string ResurgenceDes = GameEntry.Localization.GetString ("GameOver.ResurgenceDes");
        // string message = $"{des}\n<color=red>{ResurgenceDes}</color>";

        // GameEntry.UI.OpenDialog (new DialogParams () {
        //     Mode = DialogParams.DialogMode.双按钮,
        //     Title = GameEntry.Localization.GetString ("GameOver.Title"),
        //     Message = message,
        //     ConfirmText = GameEntry.Localization.GetString("Operate.Continue"),
        //     CancelText = GameEntry.Localization.GetString("Operate.Back"),
        //     OnClickConfirm = (object userData) => { return Continue(); },
        //     OnClickCancel = (object userData) => { Back(); },
        // });

        isPause = true;
    }

    private void OnOpenUIFormSuccess (object sender, GameEventArgs e) {
        OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs) e;

        if (ne.UIForm.Logic is UIPlayerOperate) {
            uiPlayerOperate = (UIPlayerOperate) ne.UIForm.Logic;
        } else if (ne.UIForm.Logic is UIPlayerMessage) {
            uiPlayerMessage = (UIPlayerMessage) ne.UIForm.Logic;
        } else if (ne.UIForm.Logic is UIGameOver) {
            uiGameOver = (UIGameOver) ne.UIForm.Logic;
        }
    }
}