using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public Text upgradeDescription;
    public Text moneyCount;
    public GameObject floatText;
    public List<UpgradePurchaseButtonController> UpgradeButtons;
    private UpgradePurchaseButtonController selectedUpgradeButton;

    public Text fixPriceText;
    private int fixFullPrice=100;
    

    private void Start()
    {
        GameData.onMoneyChange += updateMoney;
        refresh();
    }
    public void refresh()
    {
        upgradeDescription.text = "";
        selectedUpgradeButton = null;
        updateMoney();
        updateFixPriceText();
        for (int i = 0; i < UpgradeButtons.Count; i++)
        {
            UpgradeButtons[i].setSlot(GameData.upgrades.getRandomUpgrade());
            UpgradeButtons[i].unSelect();
        }
    }

    public void pressUpgradeButton(UpgradePurchaseButtonController sender)
    {
        upgradeDescription.text = GameData.upgrades.getItemNextLevelDescription(sender.item);

        if(selectedUpgradeButton==sender)
        {   
            tryBuyUpgrade();
        }
        else
        {
            if(selectedUpgradeButton!=null)
            {
                selectedUpgradeButton.unSelect();
            }
            selectedUpgradeButton = sender;
            sender.select();
        }
    }
    public void pressHealButton()
    {
        tryBuyFix();
    }
    public void pressConfirmBuyButton()
    {
        if(selectedUpgradeButton==null)
        {
            return;
        }
        tryBuyUpgrade();
    }
    public void tryBuyUpgrade()
    {
        if(selectedUpgradeButton.isSold()==true)
        {
            alarmText("Is sold");
            return;
        }
        if(GameData.money >= selectedUpgradeButton.price)
        {
            GameData.money -= selectedUpgradeButton.price;
            GameData.upgrades.upgradeItem(selectedUpgradeButton.item);
            selectedUpgradeButton.setSold();
        }
        else
        {
            alarmText("No money");
        }
    }
    public void tryBuyFix()
    {
        int price = getFixPrice();
        if (GameData.money >= price)
        {
            GameData.money -= price;
            GameData.curHp = GameData.maxHp;
        }
        else
        {
            alarmText("No money");
        }
        updateFixPriceText();
    }
    public int getFixPrice()
    {
        return (int)(fixFullPrice*(1 - GameData.curHp / GameData.maxHp));
    }
    public void updateFixPriceText()
    {
        int price = getFixPrice();
        if(price==0)
        {
            fixPriceText.text = "FULL";
        }
        else
        {
            fixPriceText.text = price.ToString();
        }
    }

    private void updateMoney()
    {
        moneyCount.text = GameData.money.ToString();
    }
    private void alarmText(string text)
    {
        GameObject infText = Instantiate(floatText);
        infText.GetComponent<Text>().text = text;
        infText.transform.SetParent(transform);
        infText.transform.localPosition = Vector3.zero;
    }

}
