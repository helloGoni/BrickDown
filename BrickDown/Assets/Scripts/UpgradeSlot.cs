using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSlot : MonoBehaviour
{

    public GameObject lockObj;

    public int ballNumber;


    public void Setup(int num) {
        ballNumber = num;
        switch(num) {
            case 0:
                break;
            default:
                break;
        }
    }

    public void Upgrade() {
        if(SaveManager.instance.CostMoney(500)) {
            SaveManager.instance.Upgrade(ballNumber);
            Setup(0);
        }
    }


    public void ShowNormalBall() {
        
    }


}
