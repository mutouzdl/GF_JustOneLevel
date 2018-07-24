using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

public class SurvivalGame {
    private Hero m_Hero = null;

    public void Initialize () {
        // 订阅事件
        GameEntry.Event.Subscribe (ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
        GameEntry.Event.Subscribe (ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
        GameEntry.Event.Subscribe (ResurgenceEventArgs.EventId, OnResurgenceEvent);

        GlobalGame.GameTimes = 0;
        GlobalGame.IsPause = false;
        m_Hero = null;

        CreateCreatures();
    }

    /// <summary>
    /// 创建生物
    /// </summary>
    public void CreateCreatures () {
        // 创建主角
        HeroData heroData = new HeroData (EntityExtension.GenerateSerialId (), PlayerData.CurrentFightHeroID, CampType.Player);
        heroData.Position = new Vector3 (15, 0, 15);
        EntityExtension.ShowHero (typeof (Hero), "PlayerGroup", heroData);

        // 创建魔力泉
        IDataTable<DRMagicWater> dtMagicWater = GameEntry.DataTable.GetDataTable<DRMagicWater> ();
        DRMagicWater[] magicWaters = dtMagicWater.GetAllDataRows ();

        foreach (DRMagicWater magicWater in magicWaters) {
            MagicWaterData magicWaterData = new MagicWaterData (EntityExtension.GenerateSerialId (), magicWater.Id);
            EntityExtension.ShowMagicWater (typeof (MagicWater), "MagicWaterGroup", magicWaterData);
        }

        // 创建怪物生成器
        IDataTable<DRMonsterCreater> dtMonsterCreater = GameEntry.DataTable.GetDataTable<DRMonsterCreater> ();
        DRMonsterCreater[] creaters = dtMonsterCreater.GetAllDataRows ();

        foreach (DRMonsterCreater creater in creaters) {
            MonsterCreaterData monsterCreaterData = new MonsterCreaterData (EntityExtension.GenerateSerialId (), creater.Id);
            monsterCreaterData.Position = new Vector3 (3, 0, 3);
            EntityExtension.ShowMonsterCreater (typeof (MonsterCreater), "MonsterCreaterGroup", monsterCreaterData);
        }

    }

    public void Shutdown () {
        GameEntry.Event.Unsubscribe (ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
        GameEntry.Event.Unsubscribe (ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
        GameEntry.Event.Unsubscribe (ResurgenceEventArgs.EventId, OnResurgenceEvent);
    }

    public void Update (float elapseSeconds, float realElapseSeconds) {
        if (m_Hero != null && m_Hero.IsDead) {
            GlobalGame.IsPause = true;
            return;
        }
    }

    protected void OnShowEntitySuccess (object sender, GameEventArgs e) {
        ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs) e;
        if (ne.EntityLogicType == typeof (Hero)) {
            m_Hero = (Hero) ne.Entity.Logic;
        }
    }

    protected void OnShowEntityFailure (object sender, GameEventArgs e) {
        ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs) e;
        Log.Warning ("Show entity failure with error message '{0}'.", ne.ErrorMessage);
    }

    private void OnResurgenceEvent (object sender, GameEventArgs e) {
        GlobalGame.IsPause = false;
    }
}