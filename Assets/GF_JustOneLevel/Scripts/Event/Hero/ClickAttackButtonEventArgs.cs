using GameFramework.Event;
using UnityGameFramework.Runtime;

/// <summary>
/// 点击攻击按钮事件。
/// </summary>
public sealed class ClickAttackButtonEventArgs : GameEventArgs {
    /// <summary>
    /// 事件编号。
    /// </summary>
    public static readonly int EventId = typeof (ClickAttackButtonEventArgs).GetHashCode ();

    /// <summary>
    /// 获取事件编号。
    /// </summary>
    public override int Id {
        get {
            return EventId;
        }
    }


    /// <summary>
    /// 武器攻击类型
    /// </summary>
    /// <returns></returns>
    public WeaponAttackType AttackType {
        get;
        private set;
    }

    /// <summary>
    /// 武器ID
    /// </summary>
    /// <returns></returns>
    public int? WeaponID {
        get;
        private set;
    } = null;

    /// <summary>
    /// 清理事件。
    /// </summary>
    public override void Clear () {
        AttackType = WeaponAttackType.手动触发;
        WeaponID = null;
    }

    public ClickAttackButtonEventArgs Fill(WeaponAttackType attackType, int? weaponID = null) {
        AttackType = attackType;
        WeaponID = weaponID;

        return this;
    }
}