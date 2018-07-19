using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class UIPlayerOperate : UGuiForm {
    private ProcedureGame procedureGame = null;
    [SerializeField]
    private Transform attackButtonParent = null;
    [SerializeField]
    private Transform skillButtonParent = null;
    [SerializeField]
    private GameObject buttonPrefab = null;

    /// <summary>
    /// 界面初始化
    /// </summary>
    /// <param name="userData"></param>
    protected override void OnInit (object userData) {
        base.OnInit (userData);
    }

    /// <summary>
    /// 界面打开。
    /// </summary>
    /// <param name="userData">用户自定义数据。</param>
    protected override void OnOpen (object userData) {
        base.OnOpen (userData);

        procedureGame = userData as ProcedureGame;

        InitWeaponButtons ();
    }

    protected override void OnClose (object userData) {
        base.OnClose(userData);

        for (int i = 0; i < attackButtonParent.childCount; i++) {
            Destroy (attackButtonParent.GetChild (i).gameObject);
        }
        for (int i = 0; i < skillButtonParent.childCount; i++) {
            Destroy (skillButtonParent.GetChild (i).gameObject);
        }
    }

    /// <summary>
    /// 根据玩家角色拥有的武器，初始化操作按钮
    /// </summary>
    private void InitWeaponButtons () {
        HeroData heroData = new HeroData (EntityExtension.GenerateSerialId (), PlayerData.CurrentFightHeroID, CampType.Player);
        List<WeaponData> weaponDatas = heroData.GetWeaponDatas ();

        for (int i = 0; i < weaponDatas.Count; i++) {
            WeaponData weaponData = weaponDatas[i];
            string buttonText = $"{weaponData.Name}({weaponData.CostMP}MP)";

            switch (weaponDatas[i].AttackType) {
                case WeaponAttackType.手动触发:
                    CreateButton (attackButtonParent, buttonText, OnAtkClick);
                    break;
                case WeaponAttackType.自动触发:
                    break;
                case WeaponAttackType.技能触发:
                    CreateButton (skillButtonParent, buttonText, () => {
                        OnSkillClick (weaponData);
                    });
                    break;
            }
        }
    }

    /// <summary>
    /// 创建一个操作按钮
    /// </summary>
    /// <param name="parent">父控件</param>
    /// <param name="text">按钮文字</param>
    /// <param name="onClick">点击事件</param>
    private void CreateButton (Transform parent, string text, UnityAction onClick) {
        GameObject buttonObj = Instantiate (buttonPrefab);
        buttonObj.transform.SetParent (parent, false);

        buttonObj.GetComponentInChildren<Text> ().text = text;
        buttonObj.GetComponent<Button> ().onClick.AddListener (onClick);
    }

    /// <summary>
    /// 点击攻击按钮
    /// </summary>
    public void OnAtkClick () {
        GameEntry.Event.FireNow (this, ReferencePool.Acquire<ClickAttackButtonEventArgs> ());
    }

    /// <summary>
    /// 点击技能按钮
    /// </summary>
    public void OnSkillClick (WeaponData data) {
        ClickAttackButtonEventArgs args = ReferencePool.Acquire<ClickAttackButtonEventArgs> ()
            .Fill (data.AttackType, data.TypeId);
        GameEntry.Event.FireNow (this, args);
    }

    /// <summary>
    /// 点击返回按钮
    /// </summary>
    public void OnBackClick () {
        if (procedureGame != null) {
            procedureGame.Back ();
        }
    }
}