using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance = null;

    public GameData data = new GameData();
    public string fileName = "BrickData.json";

    void Awake() {
        if(instance == null) {
            instance = this;
            LoadData();
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }
    }



    #region Json 세이브 / 로드

    public void LoadData() {
        string filePath = Application.persistentDataPath + "/" + fileName;

        if(File.Exists(filePath)) {
            string tempData = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<GameData>(tempData);
            print("성공");
        }
    
    }

    public void SaveData() {
        string jsonData = JsonUtility.ToJson(data,true);
        string filePath = Application.persistentDataPath + "/" + fileName;

        File.WriteAllText(filePath, jsonData);
        print("저장완");
    }

    #endregion

    #region 데이터 관리 함수

    public bool CostMoney(int val) {
        if(val <= data.money) {
            data.money -= val;
            SaveData();
            return true;
        } else {
            return false;
        }
    }

    public void Upgrade(int num) {
        data.upgradeData[num]++;
        SaveData();
    }

    public void Gameover(int endScore,int money) {
        if(endScore > data.highScore) {
            data.highScore = endScore;
        }
        /*
        if(data.upgradeData[1] == 0 && endScore > 50) {
            data.upgradeData[1] = 1;
        }
        if(data.upgradeData[2] == 0 && endScore > 100) {
            data.upgradeData[2] = 1;
        }
        if(data.upgradeData[3] == 0 && endScore > 200) {
            data.upgradeData[3] = 1;
        }
        */
        data.money += money;
        SaveData();
    }

    public void GetMoney(int val) {
        data.money += val;
        SaveData();
    }
    #endregion

    #region 데이터 리턴 함수

    public int ReturnMoney() {
        return data.money;
    }

    public int ReturnScore() {
        return data.highScore;
    }

    public int[] ReturnUpgrade() {
        return data.upgradeData;
    }


    #endregion

    #region 데이터 에디트 함수(삭제요망)

    public void Addmoney() {
        data.money += 1000;
        SaveData();
    }

    public void UpgradeNormalBall() {
        data.upgradeData[0]++;
        SaveData();
    }

    #endregion
}
