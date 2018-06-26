using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

/// <summary>
/// 参考来源：https://github.com/EllanJiang/StarForce
/// </summary>
public class ProcedureCheckVersion : ProcedureBase {
    private bool resourceInitComplete = false;

    protected override void OnEnter (ProcedureOwner procedureOwner) {
        base.OnEnter (procedureOwner);

        resourceInitComplete = false;

        GameEntry.Event.Subscribe (WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
        GameEntry.Event.Subscribe (WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
        GameEntry.Event.Subscribe (ResourceInitCompleteEventArgs.EventId, OnResourceInitComplete);

        /* 暂时没有资源更新的功能，直接初始化资源 */
        // RequestVersion (); 
        GameEntry.Resource.InitResources ();
    }

    protected override void OnLeave (ProcedureOwner procedureOwner, bool isShutdown) {
        GameEntry.Event.Unsubscribe (WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
        GameEntry.Event.Unsubscribe (WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
        GameEntry.Event.Unsubscribe (ResourceInitCompleteEventArgs.EventId, OnResourceInitComplete);

        base.OnLeave (procedureOwner, isShutdown);
    }

    protected override void OnUpdate (ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds) {
        base.OnUpdate (procedureOwner, elapseSeconds, realElapseSeconds);

        if (!resourceInitComplete) {
            return;
        }

        ChangeState<ProcedurePreload> (procedureOwner);
    }

    private void RequestVersion () {
        string deviceId = SystemInfo.deviceUniqueIdentifier;
        string deviceName = SystemInfo.deviceName;
        string deviceModel = SystemInfo.deviceModel;
        string processorType = SystemInfo.processorType;
        string processorCount = SystemInfo.processorCount.ToString ();
        string memorySize = SystemInfo.systemMemorySize.ToString ();
        string operatingSystem = SystemInfo.operatingSystem;
        string iOSGeneration = string.Empty;
        string iOSSystemVersion = string.Empty;
        string iOSVendorIdentifier = string.Empty;
#if UNITY_IOS && !UNITY_EDITOR
        iOSGeneration = UnityEngine.iOS.Device.generation.ToString ();
        iOSSystemVersion = UnityEngine.iOS.Device.systemVersion;
        iOSVendorIdentifier = UnityEngine.iOS.Device.vendorIdentifier ?? string.Empty;
#endif
        string gameVersion = GameEntry.Base.GameVersion;
        string platform = Application.platform.ToString ();
        string language = GameEntry.Localization.Language.ToString ();
        string unityVersion = Application.unityVersion;
        string installMode = Application.installMode.ToString ();
        string sandboxType = Application.sandboxType.ToString ();
        string screenWidth = Screen.width.ToString ();
        string screenHeight = Screen.height.ToString ();
        string screenDpi = Screen.dpi.ToString ();
        string screenOrientation = Screen.orientation.ToString ();
        string screenResolution = string.Format ("{0} x {1} @ {2}Hz", Screen.currentResolution.width.ToString (), Screen.currentResolution.height.ToString (), Screen.currentResolution.refreshRate.ToString ());
        string useWifi = (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork).ToString ();

        WWWForm wwwForm = new WWWForm ();
        wwwForm.AddField ("DeviceId", WebUtility.EscapeString (deviceId));
        wwwForm.AddField ("DeviceName", WebUtility.EscapeString (deviceName));
        wwwForm.AddField ("DeviceModel", WebUtility.EscapeString (deviceModel));
        wwwForm.AddField ("ProcessorType", WebUtility.EscapeString (processorType));
        wwwForm.AddField ("ProcessorCount", WebUtility.EscapeString (processorCount));
        wwwForm.AddField ("MemorySize", WebUtility.EscapeString (memorySize));
        wwwForm.AddField ("OperatingSystem", WebUtility.EscapeString (operatingSystem));
        wwwForm.AddField ("IOSGeneration", WebUtility.EscapeString (iOSGeneration));
        wwwForm.AddField ("IOSSystemVersion", WebUtility.EscapeString (iOSSystemVersion));
        wwwForm.AddField ("IOSVendorIdentifier", WebUtility.EscapeString (iOSVendorIdentifier));
        wwwForm.AddField ("GameVersion", WebUtility.EscapeString (gameVersion));
        wwwForm.AddField ("Platform", WebUtility.EscapeString (platform));
        wwwForm.AddField ("Language", WebUtility.EscapeString (language));
        wwwForm.AddField ("UnityVersion", WebUtility.EscapeString (unityVersion));
        wwwForm.AddField ("InstallMode", WebUtility.EscapeString (installMode));
        wwwForm.AddField ("SandboxType", WebUtility.EscapeString (sandboxType));
        wwwForm.AddField ("ScreenWidth", WebUtility.EscapeString (screenWidth));
        wwwForm.AddField ("ScreenHeight", WebUtility.EscapeString (screenHeight));
        wwwForm.AddField ("ScreenDPI", WebUtility.EscapeString (screenDpi));
        wwwForm.AddField ("ScreenOrientation", WebUtility.EscapeString (screenOrientation));
        wwwForm.AddField ("ScreenResolution", WebUtility.EscapeString (screenResolution));
        wwwForm.AddField ("UseWifi", WebUtility.EscapeString (useWifi));

        GameEntry.WebRequest.AddWebRequest (GameEntry.BuiltinData.BuildInfo.CheckVersionUrl, wwwForm, this);
    }

    private void OnWebRequestSuccess (object sender, GameEventArgs e) {
        WebRequestSuccessEventArgs ne = (WebRequestSuccessEventArgs) e;
        if (ne.UserData != this) {
            return;
        }

        string responseJson = Utility.Converter.GetString (ne.GetWebResponseBytes ());
        VersionInfo versionInfo = Utility.Json.ToObject<VersionInfo> (responseJson);
        if (versionInfo == null) {
            Log.Error ("Parse VersionInfo failure.");
            return;
        }

        Log.Info ("Latest game version is '{0}', local game version is '{1}'.", versionInfo.LatestGameVersion, GameEntry.Base.GameVersion);

        if (versionInfo.ForceGameUpdate) {
            GameEntry.UI.OpenDialog (new DialogParams {
                Mode = DialogParams.DialogMode.双按钮,
                Title = GameEntry.Localization.GetString ("ForceUpdate.Title"),
                Message = GameEntry.Localization.GetString ("ForceUpdate.Message"),
                ConfirmText = GameEntry.Localization.GetString ("ForceUpdate.UpdateButton"),
                OnClickConfirm = delegate (object userData) { Application.OpenURL (versionInfo.GameUpdateUrl); return true; },
                CancelText = GameEntry.Localization.GetString ("ForceUpdate.QuitButton"),
                OnClickCancel = delegate (object userData) { UnityGameFramework.Runtime.GameEntry.Shutdown (ShutdownType.Quit);  },
            });

            return;
        }

        GameEntry.Resource.InitResources ();
    }

    private void OnWebRequestFailure (object sender, GameEventArgs e) {
        WebRequestFailureEventArgs ne = (WebRequestFailureEventArgs) e;
        if (ne.UserData != this) {
            return;
        }

        Log.Warning ("Check version failure.");

        GameEntry.Resource.InitResources ();
    }

    private void OnResourceInitComplete (object sender, GameEventArgs e) {
        resourceInitComplete = true;

        Log.Info ("Init resource complete.");
    }
}