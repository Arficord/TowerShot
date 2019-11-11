using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePurchaseButtonController : MonoBehaviour
{
    public Upgrades.UpgradesEnum item;
    public int price;
    bool soldFlag = false;

    public Text priceText;

    public void setSlot(Upgrades.UpgradesEnum upgrade)
    {
        item = upgrade;
        price = GameData.upgrades.getItemBasePrice(upgrade);
        soldFlag = false;
        UpdateView();
    }
    private void UpdateView()
    {
        //Set Img
        priceText.text = price.ToString();
    }
    public void select()
    {
        priceText.color = Color.red;
    }
    public void unSelect()
    {
        priceText.color = Color.black;
    }
    public void setSold()
    {
        soldFlag = true;
        priceText.text = "SOLD";
    }
    public bool isSold()
    {
        return soldFlag;
    }
}
