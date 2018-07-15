using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

/// <summary>
/// 参考来源：https://github.com/EllanJiang/StarForce
/// </summary>
public class ProcedurePreload : ProcedureBase {
    private Dictionary<string, bool> loadedFlag = new Dictionary<string, bool> ();

    protected override void OnEnter (ProcedureOwner procedureOwner) {
        base.OnEnter (procedureOwner);

        GameEntry.Event.Subscribe (LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
        GameEntry.Event.Subscribe (LoadConfigFailureEventArgs.EventId, OnLoadConfigFailure);
        GameEntry.Event.Subscribe (LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
        GameEntry.Event.Subscribe (LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
        GameEntry.Event.Subscribe (LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
        GameEntry.Event.Subscribe (LoadDictionaryFailureEventArgs.EventId, OnLoadDictionaryFailure);

        loadedFlag.Clear ();

        PreloadResources ();
    }

    protected override void OnLeave (ProcedureOwner procedureOwner, bool isShutdown) {
        GameEntry.Event.Unsubscribe (LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
        GameEntry.Event.Unsubscribe (LoadConfigFailureEventArgs.EventId, OnLoadConfigFailure);
        GameEntry.Event.Unsubscribe (LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
        GameEntry.Event.Unsubscribe (LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
        GameEntry.Event.Unsubscribe (LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
        GameEntry.Event.Unsubscribe (LoadDictionaryFailureEventArgs.EventId, OnLoadDictionaryFailure);

        base.OnLeave (procedureOwner, isShutdown);
    }

    protected override void OnUpdate (ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds) {
        base.OnUpdate (procedureOwner, elapseSeconds, realElapseSeconds);

        IEnumerator<bool> iter = loadedFlag.Values.GetEnumerator ();
        while (iter.MoveNext ()) {
            if (!iter.Current) {
                return;
            }
        }

        procedureOwner.SetData<VarInt> (Constant.ProcedureData.NextSceneId, GameEntry.Config.GetInt ("Scene.Menu"));
        ChangeState<ProcedureChangeScene> (procedureOwner);
    }

    private void PreloadResources () {
        // Preload configs
        LoadConfig ("DefaultConfig");

        // Preload data tables
        LoadDataTable ("Hero");
        LoadDataTable ("Monster");
        LoadDataTable ("PowerBar");
        LoadDataTable ("MagicWater");
        LoadDataTable ("Particle");
        LoadDataTable ("Weapon");
        LoadDataTable ("Bullet");
        LoadDataTable ("BulletEffect");
        LoadDataTable ("MonsterCreater");
        LoadDataTable ("Scene");
        LoadDataTable ("Sound");
        LoadDataTable ("Music");
        LoadDataTable ("HeroShop");
        LoadDataTable ("UIForm");
        LoadDataTable ("UISound");

        // Preload dictionaries
        LoadDictionary ("Default");

        // Preload fonts
        LoadFont ("MainFont");
    }

    private void LoadConfig (string configName) {
        loadedFlag.Add (string.Format ("Config.{0}", configName), false);

        if (string.IsNullOrEmpty (configName)) {
            Log.Warning ("Config name is invalid.");
            return;
        }

        GameEntry.Config.LoadConfig (configName, AssetUtility.GetConfigAsset (configName), this);
    }

    private void LoadDataTable (string dataTableName) {
        loadedFlag.Add (string.Format ("DataTable.{0}", dataTableName), false);
        GameEntry.DataTable.LoadDataTable (dataTableName, this);
    }

    private void LoadDictionary (string dictionaryName) {
        loadedFlag.Add (string.Format ("Dictionary.{0}", dictionaryName), false);
        GameEntry.Localization.LoadDictionary (dictionaryName, this);
    }

    private void LoadFont (string fontName) {
        loadedFlag.Add (string.Format ("Font.{0}", fontName), false);
        GameEntry.Resource.LoadAsset (AssetUtility.GetFontAsset (fontName), new LoadAssetCallbacks (
            (assetName, asset, duration, userData) => {
                loadedFlag[string.Format ("Font.{0}", fontName)] = true;
                UGuiForm.SetMainFont ((Font) asset);
                Log.Info ("Load font '{0}' OK.", fontName);
            },

            (assetName, status, errorMessage, userData) => {
                Log.Error ("Can not load font '{0}' from '{1}' with error message '{2}'.", fontName, assetName, errorMessage);
            }));
    }

    private void OnLoadConfigSuccess (object sender, GameEventArgs e) {
        LoadConfigSuccessEventArgs ne = (LoadConfigSuccessEventArgs) e;
        if (ne.UserData != this) {
            return;
        }

        loadedFlag[string.Format ("Config.{0}", ne.ConfigName)] = true;
        Log.Info ("Load config '{0}' OK.", ne.ConfigName);
    }

    private void OnLoadConfigFailure (object sender, GameEventArgs e) {
        LoadConfigFailureEventArgs ne = (LoadConfigFailureEventArgs) e;
        if (ne.UserData != this) {
            return;
        }

        Log.Error ("Can not load config '{0}' from '{1}' with error message '{2}'.", ne.ConfigName, ne.ConfigAssetName, ne.ErrorMessage);
    }

    private void OnLoadDataTableSuccess (object sender, GameEventArgs e) {
        LoadDataTableSuccessEventArgs ne = (LoadDataTableSuccessEventArgs) e;
        if (ne.UserData != this) {
            return;
        }

        loadedFlag[string.Format ("DataTable.{0}", ne.DataTableName)] = true;
        Log.Info ("Load data table '{0}' OK.", ne.DataTableName);
    }

    private void OnLoadDataTableFailure (object sender, GameEventArgs e) {
        LoadDataTableFailureEventArgs ne = (LoadDataTableFailureEventArgs) e;
        if (ne.UserData != this) {
            return;
        }

        Log.Error ("Can not load data table '{0}' from '{1}' with error message '{2}'.", ne.DataTableName, ne.DataTableAssetName, ne.ErrorMessage);
    }

    private void OnLoadDictionarySuccess (object sender, GameEventArgs e) {
        LoadDictionarySuccessEventArgs ne = (LoadDictionarySuccessEventArgs) e;
        if (ne.UserData != this) {
            return;
        }

        loadedFlag[string.Format ("Dictionary.{0}", ne.DictionaryName)] = true;
        Log.Info ("Load dictionary '{0}' OK.", ne.DictionaryName);
    }

    private void OnLoadDictionaryFailure (object sender, GameEventArgs e) {
        LoadDictionaryFailureEventArgs ne = (LoadDictionaryFailureEventArgs) e;
        if (ne.UserData != this) {
            return;
        }

        Log.Error ("Can not load dictionary '{0}' from '{1}' with error message '{2}'.", ne.DictionaryName, ne.DictionaryAssetName, ne.ErrorMessage);
    }
}