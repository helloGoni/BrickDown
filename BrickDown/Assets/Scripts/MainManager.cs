using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainManager : MonoBehaviour
{
    #region 데이터

    public TMP_Text scoreText, moneyText;

    #endregion


    void Start() {
        RenewText();
    }

    public void RenewText() {
        scoreText.text = SaveManager.instance.ReturnScore().ToString();
        moneyText.text = SaveManager.instance.ReturnMoney().ToString();
    }

    public void PlayGame() {
        SceneManager.LoadScene("GameScene");
    }






}
