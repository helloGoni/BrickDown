using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShopManager : MonoBehaviour
{
    #region 자료

    public GameManager GM;
    public GameObject[] ballUpgradeSlot;
    public TMP_Text[] ballLevelText, ballCostText;
    public TMP_Text moneyText;

    public int[] upgrade = new int[10];


    #endregion

    void Start() {
        SetUpgradeSlot();
    }    
// 1725 , 2000
    public void SetUpgradeSlot() {
        upgrade = SaveManager.instance.ReturnUpgrade();
        for(int i = 1 ; i < ballUpgradeSlot.Length ; i++) {
            if( upgrade[i] > 0) {
                //ballUpgradeSlot[i].SetActive(true);
                ballLevelText[i].text = "<size=64>" + upgrade[i].ToString() + "</size> Level";
                ballCostText[i].text = (upgrade[i]*50+i*200).ToString();
            }
        }
        moneyText.text = SaveManager.instance.ReturnMoney().ToString();
    }

    public void UpgradeWeapon(int num) {
        int cost = upgrade[num]*50+num*200;
        if(SaveManager.instance.CostMoney(cost)) {
            SaveManager.instance.Upgrade(num);
        } else {
            Debug.Log("돈부족");
        }
        SetUpgradeSlot();
    }
//신기록은 그 전기록을빼서 돈 2배로 줌
//노말볼은 50 100 150 (+50)
//파랑볼은 300 100 150 (+50)
//빨강볼은 600 125 150 (+50)
//하얀볼은 900 100 150 (+50)


}
