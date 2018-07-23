using System;
using System.Collections.Generic;
using GameFramework.DataTable;
using UnityEngine;

[Serializable]
public class FightEntityData : EntityData {
    protected List<WeaponData> weaponDatas = new List<WeaponData> ();
    
    public FightEntityData (int entityId, int typeId, CampType camp) : base (entityId, typeId) {
        this.Camp = camp;
    }

    /// <summary>
    /// 改变名字
    /// </summary>
    /// <param name="name"></param>
    public void ChangeName (string name) {
        this.Name = name;
    }

    /// <summary>
    /// 增加MP
    /// </summary>
    /// <param name="value"></param>
    public void AddMP (int value) {
        this.MP += value;

        if (this.MP > this.MaxMP) {
            this.MP = this.MaxMP;
        }
    }

    /// <summary>
    /// 消耗MP
    /// </summary>
    /// <param name="value"></param>
    public void CostMP (int value) {
        this.MP -= value;

        if (this.MP < 0) {
            this.MP = 0;
        }
    }

    /// <summary>
    /// 获取战斗力数值
    /// </summary>
    /// <returns></returns>
    public int GetPower () {
        int hpPower = this.MaxHP / 5;
        int defPower = this.Def * 3;
        int atkPower = this.Atk;
        int atkSpeedPower = (int)(1 / this.AtkSpeed);
        int moveSpeedPower = (int)this.MoveSpeed;
        int atkRangePower = (int)(this.AtkRange / 2);
        
        return hpPower + defPower + atkPower + atkSpeedPower + moveSpeedPower + atkRangePower;
    }

    /// <summary>
    /// 获取战斗力对应的等级
    /// </summary>
    /// <param name="power"></param>
    /// <returns></returns>
    public int GetPowerLevel (int power = -1) {
        if (power < 0) {
            power = GetPower();
        }

        if (power < 15) return 0;
        if (power < 20) return 1;
        if (power < 35) return 2;
        if (power < 55) return 3;
        if (power < 85) return 4;
        if (power < 105) return 5;
        if (power < 135) return 6;
        if (power < 170) return 7;
        if (power < 230) return 8;

        return 9;
    }   

    /// <summary>
    /// 获取武器数据列表
    /// </summary>
    /// <returns></returns>
    public List<WeaponData> GetWeaponDatas() {
        return weaponDatas;
    }

    /// <summary>
    /// 名字
    /// </summary>
    /// <returns></returns>
    public string Name {
        get;
        protected set;
    }

    /// <summary>
    /// 魔法值
    /// </summary>
    /// <returns></returns>
    public int MP {
        get;
        protected set;
    }

    /// <summary>
    /// 最大魔法值
    /// </summary>
    /// <returns></returns>
    public int MaxMP {
        get;
        protected set;
    }

    /// <summary>
    /// 移动速度。
    /// </summary>
    public float MoveSpeed {
        get;
        protected set;
    }

    /// <summary>
    /// 旋转速度。
    /// </summary>
    public float RotateSpeed {
        get;
        protected set;
    }

    /// <summary>
    /// 攻击力
    /// </summary>
    public int Atk {
        get;
        protected set;
    }
    
    /// <summary>
    /// 攻击动画时间（秒）
    /// </summary>
    public float AtkAnimTime {
        get;
        protected set;
    }

    /// <summary>
    /// 攻击范围
    /// </summary>
    public float AtkRange {
        get;
        protected set;
    }
    
    /// <summary>
    /// 攻速
    /// </summary>
    public float AtkSpeed {
        get;
        protected set;
    }

    public int DeadEffectId {
        get;
        protected set;
    }

    public int DeadSoundId {
        get;
        protected set;
    }

    /// <summary>
    /// 角色阵营。
    /// </summary>
    public CampType Camp {
        get;
        protected set;
    } = CampType.Unknown;

    /// <summary>
    /// 当前生命。
    /// </summary>
    public int HP {
        get;
        set;
    }

    /// <summary>
    /// 最大生命。
    /// </summary>
    public int MaxHP {
        get;
        protected set;
    }

    /// <summary>
    /// 生命百分比。
    /// </summary>
    public float HPRatio {
        get {
            return MaxHP > 0 ? (float) HP / MaxHP : 0f;
        }
    }

    /// <summary>
    /// 防御。
    /// </summary>
    public int Def {
        get;
        protected set;
    }
}