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
    [SerializeField] private GameObject shopDisplayer;
    public readonly Vector3[] shopPos = { new Vector2(-1, -2), new Vector2(0, -2), new Vector2(1, -2) }; 
    private WeaponScriptableObject[] shopWeapons;
    [SerializeField] private Button battleButton;
    [SerializeField] private Button ShopButton;

    private void BattleClick()
    {
        if (GameManager.Instance.State.Equals(GameState.Battle)) return;
        // Maybe should be doing an event here??? IDK this seems to work just fine, we will always have a gamemanager anyway
        GameManager.Instance.ChangeState(GameState.Battle);
    }

    private void ShopClick()
    {
        if (GameManager.Instance.State.Equals(GameState.Shop)) return;
        GameManager.Instance.ChangeState(GameState.Shop);
    }
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
        // TODO Maybe some nice transition where the shop fades in?
        if (GameManager.Instance.State.Equals(GameState.Shop)) {
            gameObject.GetComponent<Canvas>().worldCamera = FindObjectOfType<Camera>();
            PopulateShop();
            shopDisplayer.SetActive(true);
        } else
        {
            shopDisplayer.SetActive(false);
        }
    }


    private void PopulateShop() {
        // TODO make a shop item lockable, maybe even more shop slots? Reroll etc. 
        for (int i = 0; i < shopWeapons.Length; i++) {
            shopWeapons[i] = ResourceSystem.Instance.GetRandomWeapon();
            Instantiate(shopWeapons[i].Prefab.gameObject, shopPos[i], Quaternion.identity).GetComponent<Weapon>().SetposPrefered(i - 3);
            // Let the weapon instantiated know that its not cool and is only in the shop -3, -2, -1 are shop slots
        }
            shopItemName1.text = shopWeapons[0].WeaponName.ToString();
            shopItemName2.text = shopWeapons[1].WeaponName.ToString();
            shopItemName3.text = shopWeapons[2].WeaponName.ToString();
        // TODO Make more information available
    }


    public WeaponScriptableObject GetShopWeapon(int index) {
        return shopWeapons[index+3];
    }

}
