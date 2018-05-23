using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

public class SurvivalGame {
    public bool GameOver {
        get;
        protected set;
    }

    private float m_ElapseSeconds = 0f;

    private Hero m_Hero = null;

    public void Initialize () {
        // 订阅事件
        GameEntry.Event.Subscribe (ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
        GameEntry.Event.Subscribe (ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
        GameEntry.Event.Subscribe (ResurgenceEventArgs.EventId, OnResurgenceEvent);

        // 创建主角
        HeroData heroData = new HeroData (EntityExtension.GenerateSerialId (), 1, CampType.Player);
        heroData.Position = new Vector3 (3, 0, 3);
        EntityExtension.ShowHero (typeof (Hero), "PlayerGroup", heroData);

        // 创建怪物生成器
        IDataTable<DRMonsterCreater> dtMonsterCreater = GameEntry.DataTable.GetDataTable<DRMonsterCreater> ();
        DRMonsterCreater[] creaters = dtMonsterCreater.GetAllDataRows ();

        foreach (DRMonsterCreater creater in creaters) {
            MonsterCreaterData monsterCreaterData = new MonsterCreaterData (EntityExtension.GenerateSerialId (), creater.Id);
            monsterCreaterData.Position = new Vector3 (3, 0, 3);
            EntityExtension.ShowMonsterCreater (typeof (MonsterCreater), "MonsterCreaterGroup", monsterCreaterData);
        }

        GameOver = false;
        m_Hero = null;
    }

    public void Shutdown () {
        GameEntry.Event.Unsubscribe (ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
        GameEntry.Event.Unsubscribe (ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
        GameEntry.Event.Unsubscribe (ResurgenceEventArgs.EventId, OnResurgenceEvent);
    }

    public void Update (float elapseSeconds, float realElapseSeconds) {
        if (m_Hero != null && m_Hero.IsDead) {
            GameOver = true;
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

    private void OnResurgenceEvent(object sender, GameEventArgs e) {
        GameOver = false;
    }
}