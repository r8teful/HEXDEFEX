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
    [SerializeField] private TMP_Text weapon1Desc;
    [SerializeField] private TMP_Text weapon2Desc;
    [SerializeField] private TMP_Text weapon3Desc;
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
    private GameObject[] shopWeaponsGameObjects;
    [SerializeField] private Button battleButton;
    [SerializeField] private Button ShopButton;
    [SerializeField] private Button RerollButton;

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

    private void RerollClick() {
        // Check if we have enough currency toodo 5 for now but it has to scale I think?
        if (ShipManager.Instance.GetCurrency() < 5) return;
        // Spend the currency
        ShipManager.Instance.RemoveCurrency(5);
        // Reroll is just populating??
        PopulateShop();
    }


    protected override void Awake() {
        base.Awake();
        GameManager.OnGameStateChanged += ChangeState;
        InputManager.OnWeaponBuy += WeaponBuy;
        shopWeapons = new WeaponScriptableObject[3];
        shopWeaponsGameObjects = new GameObject[3];
        battleButton.onClick.AddListener(BattleClick);
        ShopButton.onClick.AddListener(ShopClick);
        RerollButton.onClick.AddListener(RerollClick);

    }

   

    private void OnDestroy() {
        GameManager.OnGameStateChanged -= ChangeState;
        InputManager.OnWeaponBuy -= WeaponBuy;
    }
   
    private void ChangeState(GameState t) {
        // TODO Maybe some nice transition where the shop fades in?
        if (t.Equals(GameState.Shop)) {
           gameObject.GetComponent<Canvas>().worldCamera = GameObject.Find("UICamera").GetComponent<Camera>();//FindObjectOfType<Camera>();
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
        // Destroy all shop weapons gameobjects
        for (int i = 0; i < shopWeaponsGameObjects.Length; i++) {
            if (shopWeaponsGameObjects[i] != null) Destroy(shopWeaponsGameObjects[i]);
        }
        shopWeaponsGameObjects = new GameObject[3];
        shopWeapons = new WeaponScriptableObject[3];
        // clear shop weapon array
        //Array.Clear(shopWeapons, 0, shopWeapons.Length);
        //Array.Clear(shopWeaponsGameObjects, 0, shopWeaponsGameObjects.Length);



        for (int i = 0; i < shopWeapons.Length; i++) {
            shopWeapons[i] = ResourceSystem.Instance.GetRandomWeapon(); // TODO change this so that it increases chances of better level for scaling
            //shopWeapons[i].level = 2; we can do something like that
            shopWeaponsGameObjects[i] = Instantiate(shopWeapons[i].prefab.gameObject, shopPos[i], Quaternion.identity);
            // Let the weapon instantiated know that its not cool and is only in the shop -3, -2, -1 are shop slots
            shopWeaponsGameObjects[i].GetComponent<Weapon>().SetPosPrefered(i - 3);

        }
        shopItemName1.text = shopWeapons[0].weaponName.ToString();
        shopItemName2.text = shopWeapons[1].weaponName.ToString();
        shopItemName3.text = shopWeapons[2].weaponName.ToString();
        weapon1Desc.text = shopWeapons[0].description;
        weapon2Desc.text = shopWeapons[1].description;
        weapon3Desc.text = shopWeapons[2].description;
        // TODO Make more information available
    }

    private void WeaponBuy(GameObject g) {
        // Loop through the shopWeaponsGameObjects array
        for (int i = 0; i < shopWeaponsGameObjects.Length; i++) {
            // Check if the element is not null and has a positive preferred position
            if (shopWeaponsGameObjects[i]?.GetComponent<Weapon>().GetPosPrefered() >= 0) {
                shopWeaponsGameObjects[i] = null; // Remove the element
            }
        }
    }


    public WeaponScriptableObject GetShopWeapon(int index) {
        return shopWeapons[index+3];
    }

}
