using GameFramework;
using GameFramework.DataTable;
using System;
using UnityGameFramework.Runtime;

    public static class EntityExtension
    {
        // 关于 EntityId 的约定：
        // 0 为无效
        // 正值用于和服务器通信的实体（如玩家角色、NPC、怪等，服务器只产生正值）
        // 负值用于本地生成的临时实体（如特效、FakeObject等）
        private static int serialId = 0;

        public static void ShowMonster(Type logicType, string entityGroup, EntityData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            IDataTable<DRMonster> dtEntity = GameEntry.DataTable.GetDataTable<DRMonster>();
            DRMonster drEntity = dtEntity.GetDataRow(data.TypeId);
            if (drEntity == null)
            {
                Log.Warning("Can not load entity id '{0}' from data table.", data.TypeId.ToString());
                return;
            }

            GameEntry.Entity.ShowEntity(data.Id, logicType, AssetUtility.GetEntityAsset(drEntity.AssetName), entityGroup, data);
        }

        public static void ShowHero(Type logicType, string entityGroup, EntityData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            IDataTable<DRHero> dtEntity = GameEntry.DataTable.GetDataTable<DRHero>();
            DRHero drEntity = dtEntity.GetDataRow(data.TypeId);
            if (drEntity == null)
            {
                Log.Warning("Can not load entity id '{0}' from data table.", data.TypeId.ToString());
                return;
            }

            GameEntry.Entity.ShowEntity(data.Id, logicType, AssetUtility.GetEntityAsset(drEntity.AssetName), entityGroup, data);
        }
        
        public static void ShowPowerBar(Type logicType, string entityGroup, EntityData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            IDataTable<DRPowerBar> dtEntity = GameEntry.DataTable.GetDataTable<DRPowerBar>();
            DRPowerBar drEntity = dtEntity.GetDataRow(data.TypeId);
            if (drEntity == null)
            {
                Log.Warning("Can not load entity id '{0}' from data table.", data.TypeId.ToString());
                return;
            }

            GameEntry.Entity.ShowEntity(data.Id, logicType, AssetUtility.GetEntityAsset(drEntity.AssetName), entityGroup, data);
        }

        public static void ShowMagicWater(Type logicType, string entityGroup, EntityData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            IDataTable<DRMagicWater> dtEntity = GameEntry.DataTable.GetDataTable<DRMagicWater>();
            DRMagicWater drEntity = dtEntity.GetDataRow(data.TypeId);
            if (drEntity == null)
            {
                Log.Warning("Can not load entity id '{0}' from data table.", data.TypeId.ToString());
                return;
            }

            GameEntry.Entity.ShowEntity(data.Id, logicType, AssetUtility.GetEntityAsset(drEntity.AssetName), entityGroup, data);
        }

        public static void ShowFlowText(Type logicType, string entityGroup, EntityData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            GameEntry.Entity.ShowEntity(data.Id, logicType, AssetUtility.GetEntityAsset("FlowText"), entityGroup, data);
        }

        public static void ShowBullet(Type logicType, string entityGroup, EntityData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            IDataTable<DRBullet> dtEntity = GameEntry.DataTable.GetDataTable<DRBullet>();
            DRBullet drEntity = dtEntity.GetDataRow(data.TypeId);
            if (drEntity == null)
            {
                Log.Warning("Can not load entity id '{0}' from data table.", data.TypeId.ToString());
                return;
            }

            GameEntry.Entity.ShowEntity(data.Id, logicType, AssetUtility.GetEntityAsset(drEntity.AssetName), entityGroup, data);
        }
        
        public static void ShowBulletEffect(Type logicType, string entityGroup, EntityData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            IDataTable<DRBulletEffect> dtEntity = GameEntry.DataTable.GetDataTable<DRBulletEffect>();
            DRBulletEffect drEntity = dtEntity.GetDataRow(data.TypeId);
            if (drEntity == null)
            {
                Log.Warning("Can not load entity id '{0}' from data table.", data.TypeId.ToString());
                return;
            }

            GameEntry.Entity.ShowEntity(data.Id, logicType, AssetUtility.GetBulletEffectAsset(drEntity.AssetName), entityGroup, data);
        }
        
        public static void ShowWeapon(Type logicType, string entityGroup, EntityData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            IDataTable<DRWeapon> dtEntity = GameEntry.DataTable.GetDataTable<DRWeapon>();
            DRWeapon drEntity = dtEntity.GetDataRow(data.TypeId);
            if (drEntity == null)
            {
                Log.Warning("Can not load entity id '{0}' from data table.", data.TypeId.ToString());
                return;
            }

            GameEntry.Entity.ShowEntity(data.Id, logicType, AssetUtility.GetEntityAsset(drEntity.AssetName), entityGroup, data);
        }

        public static void ShowParticle(Type logicType, string entityGroup, EntityData data)
        {
            Log.Warning("ShowParticle");
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            IDataTable<DRParticle> dtEntity = GameEntry.DataTable.GetDataTable<DRParticle>();
            DRParticle drEntity = dtEntity.GetDataRow(data.TypeId);
            if (drEntity == null)
            {
                Log.Warning("Can not load entity id '{0}' from data table.", data.TypeId.ToString());
                return;
            }

            GameEntry.Entity.ShowEntity(data.Id, logicType, AssetUtility.GetEntityAsset(drEntity.AssetName), entityGroup, data);
        }
        
        public static void ShowMonsterCreater(Type logicType, string entityGroup, EntityData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            IDataTable<DRMonsterCreater> dtEntity = GameEntry.DataTable.GetDataTable<DRMonsterCreater>();
            DRMonsterCreater drEntity = dtEntity.GetDataRow(data.TypeId);
            if (drEntity == null)
            {
                Log.Warning("Can not load entity id '{0}' from data table.", data.TypeId.ToString());
                return;
            }

            GameEntry.Entity.ShowEntity(data.Id, logicType, AssetUtility.GetEntityAsset(drEntity.AssetName), entityGroup, data);
        }

        public static int GenerateSerialId()
        {
            return --serialId;
        }
}
