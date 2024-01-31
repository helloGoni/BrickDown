using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShopManager : MonoBehaviour
{
    #region 자료

    public GameManager GM;
    public GameObject[] ballUpgradeSlot;
    public TMP_Text[] ballLevelText;

    public int[] upgrade = new int[10];


    #endregion

    void Start() {
        SetUpgradeSlot();
    }    

    public void SetUpgradeSlot() {
        upgrade = SaveManager.instance.ReturnUpgrade();
        for(int i = 0 ; i < ballUpgradeSlot.Length ; i++) {
            if( upgrade[i] > 0) {
                ballUpgradeSlot[i].SetActive(true);
                ballLevelText[i].text ="Level : " + upgrade[i].ToString();
            }
        }
    }


    public void UpgradeWeapon(int num) {
        if(SaveManager.instance.CostMoney(100)) {
            SaveManager.instance.Upgrade(num);
        } else {
            Debug.Log("돈부족");
        }
        SetUpgradeSlot();
    }

}
