using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

// Manages UI elements on the screen
// Also manages the weapons in the shop
public class IUManagerScreen : StaticInstance<IUManagerScreen> {
    // Reference IU elements here

    [SerializeField] private TMP_Text shopItemName1;
    [SerializeField] private TMP_Text shopItemName2;
    [SerializeField] private TMP_Text shopItemName3;
    [SerializeField] private TMP_Text weapon1Cooldown;
    [SerializeField] private TMP_Text weapon2Cooldown;
    [SerializeField] private TMP_Text weapon3Cooldown;
    [SerializeField] private TMP_Text weapon1DMG;
    [SerializeField] private TMP_Text weapon2DMG;
    [SerializeField] private TMP_Text weapon3DMG; 
    [SerializeField] private TMP_Text weapon1Crit;
    [SerializeField] private TMP_Text weapon2Crit;
    [SerializeField] private TMP_Text weapon3Crit;
    [SerializeField] private TMP_Text weapon1Cost;
    [SerializeField] private TMP_Text weapon2Cost;
    [SerializeField] private TMP_Text weapon3Cost;

    [SerializeField] private GameObject shopDisplayer;
    public readonly Vector3[] shopPos = { new Vector2(-1, -2), new Vector2(0, -2), new Vector2(1, -2) }; 
    private WeaponScriptableObject[] shopWeapons;
    [SerializeField] private Button battleButton;
    [SerializeField] private Button ShopButton;

    private void BattleClick()
    {
        if (GameManager.Instance.State.Equals(GameState.Battle)) return;
        // Maybe should be doing an event here??? IDK this seems to work just fine, we will always have a gamemanager anyway
        //GameManager.Instance.ChangeState(GameState.Battle);
        GameManager.Instance.ChangeState(GameState.Loading);
        SceneOperator.Instance.ChangeScene(GameState.Battle);
    }

    private void ShopClick()
    {
        if (GameManager.Instance.State.Equals(GameState.Shop)) return;
        //GameManager.Instance.ChangeState(GameState.Shop);
        GameManager.Instance.ChangeState(GameState.Loading); 
        SceneOperator.Instance.ChangeScene(GameState.Shop);
    }
    protected override void Awake() {
        base.Awake();
        GameManager.OnGameStateChanged += ChangeState;
        shopWeapons = new WeaponScriptableObject[3];
        battleButton.onClick.AddListener(BattleClick);
        ShopButton.onClick.AddListener(ShopClick);
    }
    private void OnDestroy() {
        GameManager.OnGameStateChanged -= ChangeState;
    }
   
    private void ChangeState(GameState t) {
        // TODO Maybe some nice transition where the shop fades in?
        if (t.Equals(GameState.Shop)) {
            gameObject.GetComponent<Canvas>().worldCamera = FindObjectOfType<Camera>();
            PopulateShop();
            shopDisplayer.SetActive(true);
            UpdateShopText();
        } else {
            shopDisplayer.SetActive(false);
        }
    }

    private void UpdateShopText() {
        // Update all show text
        for (int i = 0; i < shopWeapons.Length; i++) {
            WeaponScriptableObject weapon = shopWeapons[i];
            if (weapon == null) continue;
            switch (i) {
                case 0:
                    weapon1Cooldown.text = weapon.BaseStats.fireRate.ToString();
                    weapon1DMG.text = weapon.BaseStats.bulletDamage.ToString();
                    weapon1Crit.text = weapon.BaseStats.critRate.ToString();
                    weapon1Cost.text = weapon.BaseStats.cost.ToString();
                    break;
                case 1:
                    weapon2Cooldown.text = weapon.BaseStats.fireRate.ToString();
                    weapon2DMG.text = weapon.BaseStats.bulletDamage.ToString();
                    weapon2Crit.text = weapon.BaseStats.critRate.ToString();
                    weapon2Cost.text = weapon.BaseStats.cost.ToString();
                    break;
                case 2:
                    weapon3Cooldown.text = weapon.BaseStats.fireRate.ToString();
                    weapon3DMG.text = weapon.BaseStats.bulletDamage.ToString();
                    weapon3Crit.text = weapon.BaseStats.critRate.ToString();
                    weapon3Cost.text = weapon.BaseStats.cost.ToString();
                    break;
            }
        }
    }

    private void PopulateShop() {
        // TODO make a shop item lockable, maybe even more shop slots? Reroll etc. 
        for (int i = 0; i < shopWeapons.Length; i++) {
            shopWeapons[i] = ResourceSystem.Instance.GetRandomWeapon();
            Instantiate(shopWeapons[i].prefab.gameObject, shopPos[i], Quaternion.identity).GetComponent<Weapon>().SetposPrefered(i - 3);
            // Let the weapon instantiated know that its not cool and is only in the shop -3, -2, -1 are shop slots
        }
            shopItemName1.text = shopWeapons[0].weaponName.ToString();
            shopItemName2.text = shopWeapons[1].weaponName.ToString();
            shopItemName3.text = shopWeapons[2].weaponName.ToString();
        // TODO Make more information available
    }


    public WeaponScriptableObject GetShopWeapon(int index) {
        return shopWeapons[index+3];
    }

}
