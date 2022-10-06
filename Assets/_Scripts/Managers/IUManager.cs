using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class IUManager : StaticInstance<IUManager> {
    // Reference IU elements here
    [SerializeField] private Button battleButton;
    [SerializeField] private Button ShopButton;
    [SerializeField] private TMP_Text shopItemName1;
    [SerializeField] private TMP_Text shopItemName2;
    [SerializeField] private TMP_Text shopItemName3;
    [SerializeField] private GameObject shopDisplayer;
    private readonly Vector2[] shopPos = { new Vector2(-1, -2), new Vector2(0, -2), new Vector2(1, -2) }; 
    private WeaponScriptableObject[] shopWeapons;

    private void Start() {
        SceneManager.sceneLoaded += SceneDoneLoading;
        shopWeapons = new WeaponScriptableObject[3];
        battleButton.onClick.AddListener(BattleClick);
        ShopButton.onClick.AddListener(ShopClick);
    }
    private void OnDestroy() {
        SceneManager.sceneLoaded -= SceneDoneLoading;
    }

    private void SceneDoneLoading(Scene arg0, LoadSceneMode arg1) {
        if (GameManager.Instance.State.Equals(GameState.Shop)) {
            PopulateShop();
            shopDisplayer.SetActive(true);
        }
    }

    private void BattleClick() {
        if (GameManager.Instance.State.Equals(GameState.Battle)) return;
        // Maybe should be doing an event here??? IDK this seems to work just fine, we will always have a gamemanager anyway
        GameManager.Instance.ChangeState(GameState.Battle);
    }  
    
    private void ShopClick() {
        if (GameManager.Instance.State.Equals(GameState.Shop)) return;
        GameManager.Instance.ChangeState(GameState.Shop);
    }

    private void PopulateShop() {
        // TODO make a shop item lockable, maybe even more shop slots? Reroll etc. 
        for (int i = 0; i < shopWeapons.Length; i++) { 
            shopWeapons[i] = ResourceSystem.Instance.GetRandomWeapon();
            Instantiate(shopWeapons[i].Prefab.gameObject, shopPos[i], Quaternion.identity).GetComponent<Weapon>().SetposPrefered(i - 3);
            // Let the weapon instantiated know that its not cool and is only in the shop
        }
            shopItemName1.text = shopWeapons[0].WeaponType.ToString();
            shopItemName2.text = shopWeapons[1].WeaponType.ToString();
            shopItemName3.text = shopWeapons[2].WeaponType.ToString();
        // TODO Make more information available
    }

    public WeaponScriptableObject GetShopWeapon(int index) {
        return shopWeapons[index+3];
    }

}
