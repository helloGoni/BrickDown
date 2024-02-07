using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


//광고 수정
//박스때릴때shock안댐

public class MainManager : MonoBehaviour
{
    #region 데이터

    public TMP_Text scoreText, moneyText;
    public GameObject MainBall, ShopPanel, ParticleMain;
    public GameObject BallGroup, WallGroup, BrickGroup;
    public AudioSource MainBGM;
    #endregion


    void Start() {
        StartMain();
    }

    public void RenewText() {
        scoreText.text = SaveManager.instance.ReturnScore().ToString() + " Round";
        moneyText.text = SaveManager.instance.ReturnMoney().ToString();
    }

    public void PlayGame() {
        SceneManager.LoadScene("GameScene");
    }

    public void PushShopBtn() {
        MainBall.GetComponent<MainBall>().isShot = false;
        ShopPanel.SetActive(true);
        BallGroup.SetActive(false);
        WallGroup.SetActive(false);
        BrickGroup.SetActive(false);
    }

    public void ReturnMainBtn() {
        ShopPanel.SetActive(false);
        StartMain();
    }

    public void StartMain() {
        RenewText();
        if(PlayerPrefs.GetInt("BGM") == null) {
            PlayerPrefs.SetInt("BGM",1);
        }
        if(PlayerPrefs.GetInt("BGM") == 0) {
            MainBGM.Stop();
        }
        WallGroup.SetActive(true);
        if(SaveManager.instance.ReturnScore() == 0) { 
            BallGroup.SetActive(false);
            BrickGroup.SetActive(false);
        } else {                
            Destroy(Instantiate(ParticleMain,new Vector3(0,0,0), Quaternion.identity),1);
            BallGroup.SetActive(true);
            BrickGroup.SetActive(true);
            MainBall.GetComponent<MainBall>().isShot = true;    
            MainBall.GetComponent<MainBall>().Shot();

        }
    }

    public void ToggleBGM() {
        if( MainBGM.isPlaying) {
            MainBGM.Stop();
            PlayerPrefs.SetInt("BGM",0);
        } else {
            MainBGM.Play();
            PlayerPrefs.SetInt("BGM",1);
        }
    }


}
