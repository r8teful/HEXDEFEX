using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : StaticInstance<InputManager> {
    public static event Action<TouchPhase> OnTouchChanged; 
    public static event Action<GameObject> OnWeaponRelease; 
    public static event Action<GameObject> OnWeaponBuy; 
    private Vector3 positionInitial;
    private Vector2 touchPosition;
    private bool canBuy;
    private bool canSell;
    private GameObject sellectedOnShip = null;
    private GameObject sellectInShop = null;
    //private Vector3 shopPositionInitial;
    private void Update() {
        if (Input.touchCount == 0) return;
                
        Touch touch = Input.GetTouch(0);

        if (GameManager.Instance.State.Equals(GameState.Battle)) {
            // Logic for battle mode
            if ((touch.phase == TouchPhase.Began) && (positionInitial == Vector3.zero)) {
                    // Only do this thing once when you place down finger 
                touchPosition.Set(touch.position.x - Screen.width * .5f, touch.position.y - Screen.height * .5f);
                positionInitial = touchPosition;
                OnTouchChanged?.Invoke(TouchPhase.Began);
            }
            else if ((touch.phase == TouchPhase.Moved) || (touch.phase == TouchPhase.Stationary)) {
                touchPosition.Set(touch.position.x - Screen.width * .5f, touch.position.y - Screen.height * .5f);
                OnTouchChanged?.Invoke(touch.phase);
            }
            else if ((touch.phase == TouchPhase.Ended) || (touch.phase == TouchPhase.Canceled)) {
                 OnTouchChanged?.Invoke(TouchPhase.Ended);
                 positionInitial = Vector3.zero;
            }
        } else if (GameManager.Instance.State.Equals(GameState.Shop)) {
            // Shop input logic, mostly UI and object moving
            if (touch.phase == TouchPhase.Began) {
                touchPosition.Set(touch.position.x, touch.position.y);
                var worldTouchPos = Camera.main.ScreenToWorldPoint(touchPosition);
                var hitRay = Physics2D.Raycast(worldTouchPos, -Vector3.forward);
                if ((hitRay.collider != null) && hitRay.collider.gameObject.CompareTag("Weapon")) {
                    // Hit a weapon with the mouse, check if its in the shop or on the ship. 
                    if (hitRay.collider.gameObject.GetComponent<Weapon>().GetPosPrefered() >= 0) {
                        // Weapon is on ship, sellect the weapon and tell it that it has been sellected, so that it doesn't pull itself towards the ship
                        sellectedOnShip = hitRay.collider.gameObject;
                        sellectedOnShip.GetComponent<Weapon>().SetSellected(true);
                    } else {
                        // Weapon is in the shop and is sellected
                        //sellectInShop = hitRay.collider.gameObject;
                        sellectInShop = hitRay.collider.gameObject;
                        sellectInShop.GetComponent<Weapon>().SetSellected(true);
                        //shopPositionInitial = sellectInShop.transform.position;
                    }
                }
                ////////////////////////////////////////////////////
                /// I know it's a lot of else and if statements. The next two is only for when a weapon that is on the ship that is sellected
                /////////////////////////////////////////////////// 
            } else if ((sellectedOnShip != null) && ((touch.phase == TouchPhase.Moved) || (touch.phase == TouchPhase.Stationary))) {
                // A weapon on the ship is selected and we are moving our finger or holding it still
                touchPosition.Set(touch.position.x, touch.position.y);
                var toVec = new Vector3(Camera.main.ScreenToWorldPoint(touchPosition).x, Camera.main.ScreenToWorldPoint(touchPosition).y, 0);
                sellectedOnShip.transform.position = Vector3.Slerp(sellectedOnShip.transform.position, toVec, 40f * Time.deltaTime);
                // Rotates face to the middle. Based on a comment from a very generous person here:  http://answers.unity.com/comments/1482425/view.html 
                sellectedOnShip.transform.up = Vector3.Slerp(sellectedOnShip.transform.up, sellectedOnShip.transform.position, 40f * Time.deltaTime);
                //TODO tell the iuManager to draw the sell ui object! to notify to the user that they can sell this weapon if they release it in that area

                // Check if we want to sell the item while holding a weapon that is on the ship
                if (sellectedOnShip.transform.position.y < -1.4) {
                    // IF WE RELEASE NOW, WE WILL SELL THIS WEAPON // TODO y position is completely abritrary could be inconsistent with different screen sizes!
                    canSell = true;
                } else {
                    canSell = false;
                }
                // Check what will happen when we release the weapon that is on the ship. We either sell it, or move it to other position, or same position 
            } else if (canSell && (sellectedOnShip != null) && (touch.phase == TouchPhase.Ended)) {
                //SELL THE WEAPON!!!! 
                // Get weapon cost
                var cost = sellectedOnShip.GetComponent<Weapon>().GetWeaponData().BaseStats.cost;
                // Award the cost
                ShipManager.Instance.AddCurrency(cost);
                // Tell the weapon manager to remove the weapon from the ship
                WeaponManager.Instance.WeaponSell(sellectedOnShip);
                // Reset the sellected weapon
                sellectedOnShip = null;
                canSell = false;
            } else if (!canSell && (sellectedOnShip != null) && (touch.phase == TouchPhase.Ended)) {
                // We released the selected weapon but did not intend to sell it. Leave the rest of movement to the weaponmanager
                OnWeaponRelease?.Invoke(sellectedOnShip);
                sellectedOnShip.GetComponent<Weapon>().SetSellected(false);
                sellectedOnShip = null;
                ////////////////////////////////////////////////////////
                //SHOP// Done checking for the weapon sellected if its on the ship, now check for the weapon sellected when it is in the shop
                ////////////////////////////////////////////////////////
            } else if ((sellectInShop != null) && ((touch.phase == TouchPhase.Moved) || (touch.phase == TouchPhase.Stationary))) {
                // Only actually "purchase" the weapon if we cross the shop border to the ship
                touchPosition.Set(touch.position.x, touch.position.y);
                var toVec = new Vector3(Camera.main.ScreenToWorldPoint(touchPosition).x, Camera.main.ScreenToWorldPoint(touchPosition).y, 0);
                if (sellectInShop.transform.position.y > -0.75) {
                    // IF WE RELEASE NOW, WE WILL BUY THIS WEAPON // TODO MUST CHECK IF WE RELEASE SOMEHOW
                    canBuy = true;
                } else {
                    canBuy = false;
                }
                sellectInShop.transform.position = Vector3.Slerp(sellectInShop.transform.position, toVec, 40f * Time.deltaTime);
                // Keep the rotation on its slot in the shop, makes it looks nice. Only do this if we are stil inside the shop

                //sellectInShop.transform.up = Vector3.Slerp(sellectInShop.transform.up, shopPositionInitial - sellectInShop.transform.position, 40f * Time.deltaTime);
            } else if (!canBuy && sellectInShop != null && touch.phase == TouchPhase.Ended) {
                // Check if we dropped weapon while not buying it
                sellectInShop.GetComponent<Weapon>().SetSellected(false);
                sellectInShop = null;
                // sellectInShop.transform.position = ;
            } else if ( canBuy && (sellectInShop != null) && (touch.phase == TouchPhase.Ended)) {
                // BUY  IT. We dropped the sellected weapon out of the shop zone todo add logic for currency
                // Get weapon cost
                var cost = sellectInShop.GetComponent<Weapon>().GetWeaponData().BaseStats.cost;
                // Check if we have enough money! Or if ship is full
                if ((ShipManager.Instance.GetCurrency() < cost) || (!WeaponManager.Instance.SpaceForWeapon())) {
                    // We don't have enough money, don't buy it TODO notify UI that we cant buy it due to x
                    sellectInShop.GetComponent<Weapon>().SetSellected(false);
                    sellectInShop = null;
                    canBuy = false;
                    return;
                }
                ShipManager.Instance.RemoveCurrency(cost); // Could change this to an event instead
                WeaponManager.Instance.WeaponBuy(sellectInShop); // NEed to do this first to set posprefered, shop relies on it to remove it from shoparray
                OnWeaponBuy?.Invoke(sellectInShop); // Notifies shop
                sellectInShop.GetComponent<Weapon>().SetSellected(false);
                canBuy = false;
                sellectInShop = null;
                // Feels weard to do all this here but HEH
            }
        }
    }

    public Vector2 GetTouchPosition() {
        return touchPosition;
    }
    public Vector3 GetPositionInitial() {
        return positionInitial;
    }
}