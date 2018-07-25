using System;
using GameFramework;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;

public static class EntityExtension {
    // 关于 EntityId 的约定：
    // 0 为无效
    // 正值用于和服务器通信的实体（如玩家角色、NPC、怪等，服务器只产生正值）
    // 负值用于本地生成的临时实体（如特效、FakeObject等）
    private static int serialId = 0;

    public static void ShowMonster (Type logicType, string entityGroup, EntityData data) {
        ShowEntity<DRMonster> (logicType, entityGroup, data, (assetName) => AssetUtility.GetEntityAsset (assetName));
    }

    public static void ShowHero (Type logicType, string entityGroup, EntityData data) {
        ShowEntity<DRHero> (logicType, entityGroup, data, (assetName) => AssetUtility.GetEntityAsset (assetName));
    }

    public static void ShowPowerBar (Type logicType, string entityGroup, EntityData data) {
        ShowEntity<DRPowerBar> (logicType, entityGroup, data, (assetName) => AssetUtility.GetEntityAsset (assetName));
    }

    public static void ShowMagicWater (Type logicType, string entityGroup, EntityData data) {
        ShowEntity<DRMagicWater> (logicType, entityGroup, data, (assetName) => AssetUtility.GetEntityAsset (assetName));
    }

    public static void ShowFlowText (Type logicType, string entityGroup, EntityData data) {
        if (data == null) {
            Log.Warning ("Data is invalid.");
            return;
        }

        GameEntry.Entity.ShowEntity (data.Id, logicType, AssetUtility.GetEntityAsset ("FlowText"), entityGroup, data);
    }

    public static void ShowBullet (Type logicType, string entityGroup, EntityData data) {
        ShowEntity<DRBullet> (logicType, entityGroup, data, (assetName) => AssetUtility.GetEntityAsset (assetName));
    }

    public static void ShowBulletEffect (Type logicType, string entityGroup, EntityData data) {
        ShowEntity<DRBulletEffect> (logicType, entityGroup, data, (assetName) => AssetUtility.GetBulletEffectAsset (assetName));
    }

    public static void ShowWeapon (Type logicType, string entityGroup, EntityData data) {
        ShowEntity<DRWeapon> (logicType, entityGroup, data, (assetName) => AssetUtility.GetEntityAsset (assetName));
    }

    public static void ShowParticle (Type logicType, string entityGroup, EntityData data) {
        ShowEntity<DRParticle> (logicType, entityGroup, data, (assetName) => AssetUtility.GetEntityAsset (assetName));
    }

    public static void ShowMonsterCreater (Type logicType, string entityGroup, EntityData data) {
        ShowEntity<DRMonsterCreater> (logicType, entityGroup, data, (assetName) => AssetUtility.GetEntityAsset (assetName));
    }

    private static void ShowEntity<T_DRType> (Type logicType, string entityGroup, EntityData data, Func<string, string> getAssetPath)
    where T_DRType : IDRAssetsRow {

        if (data == null) {
            Log.Warning ("Data is invalid.");
            return;
        }

        IDataTable<T_DRType> dtEntity = GameEntry.DataTable.GetDataTable<T_DRType> ();
        T_DRType drEntity = dtEntity.GetDataRow (data.TypeId);
        if (drEntity == null) {
            Log.Warning ("Can not load entity id '{0}' from data table.", data.TypeId.ToString ());
            return;
        }

        GameEntry.Entity.ShowEntity (data.Id, logicType, getAssetPath (drEntity.AssetName), entityGroup, data);
    }

    public static int GenerateSerialId () {
        return --serialId;
    }
}