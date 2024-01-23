using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    #region 자료

    public GameManager GM;
    public GameObject[] ballUpgradeSlot;

    public int[] upgrade = new int[10];


    #endregion
    
    public void SetUpgradeSlot() {
        GetUpgrade();
        for(int i = 0 ; i < 10 ; i++) {
            if(upgrade[i] < 1) {
                ballUpgradeSlot[i].SetActive(false);
            }
        }
    }

    public void GetUpgrade() {
        upgrade = SaveManager.instance.ReturnUpgrade();
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
