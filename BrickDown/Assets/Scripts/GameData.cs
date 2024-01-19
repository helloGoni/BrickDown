using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameData {
    public int[] upgradeData = new int[10];
    public int highScore, money;

    public GameData() {
        upgradeData[0] = 1;
        for(int i = 1 ; i < 2 ; i++) {
            upgradeData[i] = -1;
        }
        highScore = 0;
        money = 0;
    }
}
