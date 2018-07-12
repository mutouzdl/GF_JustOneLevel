using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;

public class HeroAtkState : HeroBaseActionState {
    private float atkTimeCounter = 0;

    /// <summary>
    /// 有限状态机状态初始化时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    protected override void OnInit (IFsm<Hero> fsm) {
        base.OnInit (fsm);
    }

    /// <summary>
    /// 有限状态机状态进入时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    protected override void OnEnter (IFsm<Hero> fsm) {
        base.OnEnter (fsm);

        atkTimeCounter = 0;

        fsm.Owner.ChangeAnimation (FightEntityAnimationState.atk);
    }

    /// <summary>
    /// 有限状态机状态轮询时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
    /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
    protected override void OnUpdate (IFsm<Hero> fsm, float elapseSeconds, float realElapseSeconds) {
        base.OnUpdate (fsm, elapseSeconds, realElapseSeconds);

        atkTimeCounter += elapseSeconds;

        if (atkTimeCounter > fsm.Owner.HeroData.AtkAnimTime) {
            Attack (fsm);
            ChangeState<HeroIdleState> (fsm);
        }
    }

    /// <summary>
    /// 有限状态机状态离开时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    /// <param name="isShutdown">是否是关闭有限状态机时触发。</param>
    protected override void OnLeave (IFsm<Hero> fsm, bool isShutdown) {
        base.OnLeave (fsm, isShutdown);

        fsm.Owner.ClearTrialEffect ();
    }

    /// <summary>
    /// 有限状态机状态销毁时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    protected override void OnDestroy (IFsm<Hero> fsm) {
        base.OnDestroy (fsm);
    }

    /// <summary>
    /// 执行攻击逻辑
    /// </summary>
    /// <param name="fsm"></param>
    private void Attack (IFsm<Hero> fsm) {

        WeaponAttackType attackType = (WeaponAttackType) fsm.GetData<VarInt> ("AttackType").Value;

        switch (attackType) {
            case WeaponAttackType.手动触发:
                ManualAttack (fsm);
                break;
            case WeaponAttackType.自动触发:
                break;
            case WeaponAttackType.技能触发:
                int weaponID = fsm.GetData<VarInt> ("WeaponID").Value;
                SkillAttack (fsm, attackType, weaponID);
                break;
        }
    }

    /// <summary>
    ///  手动攻击
    /// </summary>
    /// <param name="fsm"></param>
    private void ManualAttack (IFsm<Hero> fsm) {
        fsm.Owner.PlayTrailEffect ();
        fsm.Owner.PerformAttack ();
    }

    /// <summary>
    /// 技能攻击
    /// </summary>
    /// <param name="fsm"></param>
    /// <param name="weaponID"></param>
    private void SkillAttack (IFsm<Hero> fsm, WeaponAttackType attackType, int weaponID) {
        fsm.Owner.FireWeapon (attackType, weaponID);
    }
}