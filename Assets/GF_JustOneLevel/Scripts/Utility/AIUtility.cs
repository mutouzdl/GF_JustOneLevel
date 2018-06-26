using System;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;

/// <summary>
/// AI 工具类。
/// </summary>
public static class AIUtility {
    private static Dictionary<CampPair, RelationType> campPairToRelation = new Dictionary<CampPair, RelationType> ();
    private static Dictionary<KeyValuePair<CampType, RelationType>, CampType[]> campAndRelationToCamps = new Dictionary<KeyValuePair<CampType, RelationType>, CampType[]> ();

    static AIUtility () {
        campPairToRelation.Add (new CampPair (CampType.Player, CampType.Player), RelationType.Friendly);
        campPairToRelation.Add (new CampPair (CampType.Player, CampType.CloneHero), RelationType.Friendly);
        campPairToRelation.Add (new CampPair (CampType.CloneHero, CampType.CloneHero), RelationType.Friendly);
        campPairToRelation.Add (new CampPair (CampType.Player, CampType.Enemy), RelationType.Hostile);
        campPairToRelation.Add (new CampPair (CampType.CloneHero, CampType.Enemy), RelationType.Hostile);
        campPairToRelation.Add (new CampPair (CampType.Enemy, CampType.Enemy), RelationType.Friendly);
    }

    /// <summary>
    /// 获取两个阵营之间的关系。
    /// </summary>
    /// <param name="first">阵营一。</param>
    /// <param name="second">阵营二。</param>
    /// <returns>阵营间关系。</returns>
    public static RelationType GetRelation (CampType first, CampType second) {
        if (first > second) {
            CampType temp = first;
            first = second;
            second = temp;
        }

        RelationType relationType;
        if (campPairToRelation.TryGetValue (new CampPair (first, second), out relationType)) {
            return relationType;
        }

        Log.Warning ("Unknown relation between '{0}' and '{1}'.", first.ToString (), second.ToString ());
        return RelationType.Unknown;
    }

    /// <summary>
    /// 获取和指定具有特定关系的所有阵营。
    /// </summary>
    /// <param name="camp">指定阵营。</param>
    /// <param name="relation">关系。</param>
    /// <returns>满足条件的阵营数组。</returns>
    public static CampType[] GetCamps (CampType camp, RelationType relation) {
        KeyValuePair<CampType, RelationType> key = new KeyValuePair<CampType, RelationType> (camp, relation);
        CampType[] result = null;
        if (campAndRelationToCamps.TryGetValue (key, out result)) {
            return result;
        }

        List<CampType> camps = new List<CampType> ();
        Array campTypes = Enum.GetValues (typeof (CampType));
        for (int i = 0; i < campTypes.Length; i++) {
            CampType campType = (CampType) campTypes.GetValue (i);
            if (GetRelation (camp, campType) == relation) {
                camps.Add (campType);
            }
        }

        result = camps.ToArray ();
        campAndRelationToCamps[key] = result;

        return result;
    }

    /// <summary>
    /// 获取实体间的距离。
    /// </summary>
    /// <returns>实体间的距离。</returns>
    public static float GetDistance (Entity fromEntity, Entity toEntity) {
        Transform fromTransform = fromEntity.CachedTransform;
        Transform toTransform = toEntity.CachedTransform;
        return (toTransform.position - fromTransform.position).magnitude;
    }

    public static void PerformCollision (FightEntity entity, Entity other) {
        if (entity == null || other == null) {
            return;
        }

        Bullet bullet = other as Bullet;
        if (bullet != null) {
            ImpactData entityImpactData = entity.GetImpactData ();
            ImpactData bulletImpactData = bullet.GetImpactData ();
            if (GetRelation (entityImpactData.Camp, bulletImpactData.Camp) == RelationType.Friendly) {
                return;
            }

            entity.ApplyDamage (bulletImpactData.Attack);

            if (bullet.SubEffectTimes ()) {
                GameEntry.Entity.HideEntity (bullet.Id);
            }
            return;
        }
    }

    private struct CampPair {
        private readonly CampType m_First;
        private readonly CampType m_Second;

        public CampPair (CampType first, CampType second) {
            m_First = first;
            m_Second = second;
        }

        public CampType First {
            get {
                return m_First;
            }
        }

        public CampType Second {
            get {
                return m_Second;
            }
        }
    }
}