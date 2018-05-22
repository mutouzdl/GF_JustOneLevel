using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 实体逻辑
/// 参考来源：https://github.com/EllanJiang/StarForce
/// </summary>
public abstract class Entity : EntityLogic {
        [SerializeField]
        private EntityData entityData = null;

        public int Id {
                get {
                        return Entity.Id;
                }
        }

        public Animator cachedAnimator {
                get;
                private set;
        }

        protected override void OnInit (object userData)
        {
                base.OnInit (userData);
                cachedAnimator = GetComponent<Animator> ();
        }

        protected override void OnShow (object userData)
        {
                base.OnShow (userData);

                entityData = userData as EntityData;
                if (entityData == null) {
                        Log.Error ("Entity data is invalid.");
                        return;
                }

                Name = string.Format ("[Entity {0}]", Id.ToString ());
                CachedTransform.localPosition = entityData.Position;
                CachedTransform.localRotation = entityData.Rotation;
        }

        protected override void OnHide (object userData)
        {
                base.OnHide (userData);
        }

        protected override void OnAttached (EntityLogic childEntity, Transform parentTransform, object userData)
        {
                base.OnAttached (childEntity, parentTransform, userData);
        }

        protected override void OnDetached (EntityLogic childEntity, object userData)
        {
                base.OnDetached (childEntity, userData);
        }

        protected override void OnAttachTo (EntityLogic parentEntity, Transform parentTransform, object userData)
        {
                base.OnAttachTo (parentEntity, parentTransform, userData);
        }

        protected override void OnDetachFrom (EntityLogic parentEntity, object userData)
        {
                base.OnDetachFrom (parentEntity, userData);
        }

        protected override void OnUpdate (float elapseSeconds, float realElapseSeconds)
        {
                base.OnUpdate (elapseSeconds, realElapseSeconds);
        }
}