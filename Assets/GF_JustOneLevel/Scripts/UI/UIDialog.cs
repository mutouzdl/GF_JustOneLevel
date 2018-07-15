using GameFramework;
using UnityEngine;
using UnityEngine.UI;

public class UIDialog : UGuiForm {
    [SerializeField]
    private Text titleText = null;

    [SerializeField]
    private Text messageText = null;

    [SerializeField]
    private GameObject[] modeObjects = null;

    [SerializeField]
    private Text[] confirmTexts = null;

    [SerializeField]
    private Text[] cancelTexts = null;

    [SerializeField]
    private Text[] otherTexts = null;

    private DialogParams.DialogMode dialogMode = DialogParams.DialogMode.单按钮;
    private bool pauseGame = false;
    private object userData = null;
    private GameFrameworkFunc<object, bool> onClickConfirm = null;
    private GameFrameworkAction<object> onClickCancel = null;
    private GameFrameworkFunc<object, bool> onClickOther = null;

    public DialogParams.DialogMode DialogMode {
        get {
            return dialogMode;
        }
    }

    public bool PauseGame {
        get {
            return pauseGame;
        }
    }

    public object UserData {
        get {
            return userData;
        }
    }

    public void OnConfirmButtonClick () {
        if (onClickConfirm != null) {
            if (onClickConfirm (userData)) {
                Close(true);
            }
        }
        else {
            Close();
        }
    }

    public void OnCancelButtonClick () {
        if (onClickCancel != null) {
            onClickCancel (userData);
        }

        Close(true);
    }

    public void OnOtherButtonClick () {
        if (onClickOther != null) {
            if (onClickOther (userData)) {
                Close(true);
            }
        }
        else {
            Close();
        }
    }

    protected override void OnOpen (object userData)
    {
        base.OnOpen (userData);

        DialogParams dialogParams = (DialogParams) userData;
        if (dialogParams == null) {
            Log.Warning ("DialogParams is invalid.");
            return;
        }

        dialogMode = dialogParams.Mode;
        RefreshDialogMode ();

        titleText.text = dialogParams.Title;
        messageText.text = dialogParams.Message;

        pauseGame = dialogParams.PauseGame;
        RefreshPauseGame ();

        userData = dialogParams.UserData;

        RefreshConfirmText (dialogParams.ConfirmText);
        onClickConfirm = dialogParams.OnClickConfirm;

        RefreshCancelText (dialogParams.CancelText);
        onClickCancel = dialogParams.OnClickCancel;

        RefreshOtherText (dialogParams.OtherText);
        onClickOther = dialogParams.OnClickOther;
    }

    protected override void OnClose (object userData)
    {
        if (pauseGame) {
            GameEntry.Base.ResumeGame ();
        }

        dialogMode = DialogParams.DialogMode.单按钮;
        titleText.text = string.Empty;
        messageText.text = string.Empty;
        pauseGame = false;
        userData = null;

        RefreshConfirmText (string.Empty);
        onClickConfirm = null;

        RefreshCancelText (string.Empty);
        onClickCancel = null;

        RefreshOtherText (string.Empty);
        onClickOther = null;

        base.OnClose (userData);
    }

    private void RefreshDialogMode () {
        for (int i = 1; i <= modeObjects.Length; i++) {
            modeObjects[i - 1].SetActive (i == (int)dialogMode);
        }
    }

    private void RefreshPauseGame () {
        if (pauseGame) {
            GameEntry.Base.PauseGame ();
        }
    }

    private void RefreshConfirmText (string confirmText) {
        if (string.IsNullOrEmpty (confirmText)) {
            confirmText = GameEntry.Localization.GetString ("Dialog.ConfirmButton");
        }

        for (int i = 0; i < confirmTexts.Length; i++) {
            confirmTexts[i].text = confirmText;
        }
    }

    private void RefreshCancelText (string cancelText) {
        if (string.IsNullOrEmpty (cancelText)) {
            cancelText = GameEntry.Localization.GetString ("Dialog.CancelButton");
        }

        for (int i = 0; i < cancelTexts.Length; i++) {
            cancelTexts[i].text = cancelText;
        }
    }

    private void RefreshOtherText (string otherText) {
        if (string.IsNullOrEmpty (otherText)) {
            otherText = GameEntry.Localization.GetString ("Dialog.OtherButton");
        }

        for (int i = 0; i < otherTexts.Length; i++) {
            otherTexts[i].text = otherText;
        }
    }
}