
using System.Collections.Generic;
using GameFramework;

public sealed class PlayerData
{
    private PlayerData() {

    }

    /// <summary>
    /// 玩家金币数量
    /// </summary>
    /// <returns></returns>
    public static int Gold {
        get {
            return GameEntry.Setting.GetInt (Constant.Player.Gold, 0);
        }
        set {
            GameEntry.Setting.SetInt (Constant.Player.Gold, value);
        }
    }

    /// <summary>
    /// 当前出战的英雄ID
    /// </summary>
    /// <returns></returns>
    public static int CurrentFightHeroID {
        get {
            return GameEntry.Setting.GetInt (Constant.Player.CurrentFightHero, 1);
        }
        set {
            GameEntry.Setting.SetInt (Constant.Player.CurrentFightHero, value);
        }
    }

    /// <summary>
    /// 设置当前出战的英雄ID
    /// </summary>
    /// <param name="heroID"></param>
    public static void SetFightHero(int heroID) {
        CurrentFightHeroID = heroID;
    }

    /// <summary>
    /// 判断是否拥有某个ID的英雄
    /// </summary>
    /// <param name="heroID"></param>
    /// <returns></returns>
    public static bool HasHero(int heroID) {
        string[] heroIDs = OwnHeros.Split('_');
        foreach(string ID in heroIDs) {
            if (ID == heroID.ToString()) {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 添加英雄ID
    /// </summary>
    /// <param name="heroID"></param>
    public static void AddHero(int heroID) {
        if (HasHero(heroID) == false) {
            OwnHeros = OwnHeros + "_" + heroID;
        }
    }
    
    /// <summary>
    /// 拥有的英雄ID
    /// </summary>
    /// <returns></returns>
    private static string OwnHeros {
        get {
            return GameEntry.Setting.GetString(Constant.Player.OwnHero, "1");
        }
        set {
            GameEntry.Setting.SetString(Constant.Player.OwnHero, value);
        }
    }

}