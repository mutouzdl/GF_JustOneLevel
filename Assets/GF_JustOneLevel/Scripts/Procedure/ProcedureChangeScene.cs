using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

/// <summary>
/// 参考来源：https://github.com/EllanJiang/StarForce
/// </summary>
public partial class ProcedureChangeScene : ProcedureBase {
    private bool isChangeSceneComplete = false;
    private int backgroundMusicId = 0;
    private int? uiLoadingID = null;
    private float changeSceneDelayTime = 0; // 延迟切换场景时间记录

    protected override void OnEnter (ProcedureOwner procedureOwner) {
        base.OnEnter (procedureOwner);

        GameEntry.Event.Subscribe (LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
        GameEntry.Event.Subscribe (LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
        GameEntry.Event.Subscribe (LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
        GameEntry.Event.Subscribe (LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);

        changeSceneDelayTime = 0;
        isChangeSceneComplete = false;

        // 停止所有声音
        GameEntry.Sound.StopAllLoadingSounds ();
        GameEntry.Sound.StopAllLoadedSounds ();

        // 隐藏所有实体
        GameEntry.Entity.HideAllLoadingEntities ();
        GameEntry.Entity.HideAllLoadedEntities ();

        // 卸载所有场景
        string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames ();
        for (int i = 0; i < loadedSceneAssetNames.Length; i++) {
            GameEntry.Scene.UnloadScene (loadedSceneAssetNames[i]);
        }

        // 还原游戏速度
        GameEntry.Base.ResetNormalGameSpeed ();
       
        // 打开LoadingUI
        uiLoadingID = GameEntry.UI.OpenUIForm(UIFormId.Loading, this);

        int sceneId = procedureOwner.GetData<VarInt> (Constant.ProcedureData.NextSceneId).Value;
        IDataTable<DRScene> dtScene = GameEntry.DataTable.GetDataTable<DRScene> ();
        DRScene drScene = dtScene.GetDataRow (sceneId);
        if (drScene == null) {
            Log.Warning ("Can not load scene '{0}' from data table.", sceneId.ToString ());
            return;
        }

        GameEntry.Scene.LoadScene (AssetUtility.GetSceneAsset (drScene.AssetName), this);
        backgroundMusicId = drScene.BackgroundMusicId;
    }

    protected override void OnLeave (ProcedureOwner procedureOwner, bool isShutdown) {
        GameEntry.Event.Unsubscribe (LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
        GameEntry.Event.Unsubscribe (LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
        GameEntry.Event.Unsubscribe (LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
        GameEntry.Event.Unsubscribe (LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);

        base.OnLeave (procedureOwner, isShutdown);
    }

    protected override void OnUpdate (ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds) {
        base.OnUpdate (procedureOwner, elapseSeconds, realElapseSeconds);

        changeSceneDelayTime += Time.deltaTime;
        if (!isChangeSceneComplete) {
            return;
        }

        if (changeSceneDelayTime < 0.6f) {
            return;
        }

        int sceneId = procedureOwner.GetData<VarInt> (Constant.ProcedureData.NextSceneId).Value;

        if (sceneId == GameEntry.Config.GetInt("Scene.Game")) {
            ChangeState<ProcedureGame>(procedureOwner);
        }
        else if (sceneId == GameEntry.Config.GetInt("Scene.Menu")) {
            ChangeState<ProcedureMenu>(procedureOwner);
        }

        if (uiLoadingID != null) {
            GameEntry.UI.CloseUIForm((int)uiLoadingID);
        }
    }

    private void OnLoadSceneSuccess (object sender, GameEventArgs e) {
        LoadSceneSuccessEventArgs ne = (LoadSceneSuccessEventArgs) e;
        if (ne.UserData != this) {
            return;
        }

        Log.Info ("Load scene '{0}' OK.", ne.SceneAssetName);

        if (backgroundMusicId > 0) {
            GameEntry.Sound.PlayMusic (backgroundMusicId);
        }

        isChangeSceneComplete = true;
    }

    private void OnLoadSceneFailure (object sender, GameEventArgs e) {
        LoadSceneFailureEventArgs ne = (LoadSceneFailureEventArgs) e;
        if (ne.UserData != this) {
            return;
        }

        Log.Error ("Load scene '{0}' failure, error message '{1}'.", ne.SceneAssetName, ne.ErrorMessage);
    }

    private void OnLoadSceneUpdate (object sender, GameEventArgs e) {
        LoadSceneUpdateEventArgs ne = (LoadSceneUpdateEventArgs) e;
        if (ne.UserData != this) {
            return;
        }

        Log.Info ("Load scene '{0}' update, progress '{1}'.", ne.SceneAssetName, ne.Progress.ToString ("P2"));
    }

    private void OnLoadSceneDependencyAsset (object sender, GameEventArgs e) {
        LoadSceneDependencyAssetEventArgs ne = (LoadSceneDependencyAssetEventArgs) e;
        if (ne.UserData != this) {
            return;
        }

        Log.Info ("Load scene '{0}' dependency asset '{1}', count '{2}/{3}'.", ne.SceneAssetName, ne.DependencyAssetName, ne.LoadedCount.ToString (), ne.TotalCount.ToString ());
    }
}