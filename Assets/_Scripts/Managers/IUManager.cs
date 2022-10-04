using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IUManager : StaticInstance<IUManager> {
    // Reference IU elements here
    [SerializeField] private Button battleButton;
    [SerializeField] private Button ShopButton;

    private void Start() {
        battleButton.onClick.AddListener(BattleClick);
        ShopButton.onClick.AddListener(ShopClick);
    }

    private void BattleClick() {
        GameManager.Instance.ChangeState(GameState.Battle);
    }  
    
    private void ShopClick() {
        GameManager.Instance.ChangeState(GameState.Shop);
    }
}
