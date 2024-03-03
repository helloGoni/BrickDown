using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


//박스때릴때shock적용이 이상
//메인메뉴 박스
//음악X일때 이상

public class MainManager : MonoBehaviour
{
    #region 데이터

    public TMP_Text highScoreText_TMP, moneyText_TMP;
    public GameObject MainBall, ShopPanel, ParticleMain;

    [Header ("Bundle Group")]
    public GameObject ballGroup;
    public GameObject wallGroup;
    public GameObject brickGroup;
    
    [Space (10f)]
    public AudioSource mainBGM_AS;

    #endregion


    void Start() {
        StartMain();
    }

    public void RenewText() {
        highScoreText_TMP.text = SaveManager.instance.ReturnScore().ToString() + " Round";
        moneyText_TMP.text = SaveManager.instance.ReturnMoney().ToString();
    }

    public void PlayGame() {
        SceneManager.LoadScene("GameScene");
    }

    public void PushShopBtn() {
        MainBall.GetComponent<MainBall>().isShot = false;
        ShopPanel.SetActive(true);
        ballGroup.SetActive(false);
        wallGroup.SetActive(false);
        brickGroup.SetActive(false);
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
            mainBGM_AS.Stop();
        }
        wallGroup.SetActive(true);
        if(SaveManager.instance.ReturnScore() == 0) { 
            ballGroup.SetActive(false);
            brickGroup.SetActive(false);
        } else {                
            Destroy(Instantiate(ParticleMain,new Vector3(0,0,0), Quaternion.identity),1);
            ballGroup.SetActive(true);
            brickGroup.SetActive(true);
            MainBall.GetComponent<MainBall>().isShot = true;    
            MainBall.GetComponent<MainBall>().Shot();

        }
    }

    public void ToggleBGM() {
        if( mainBGM_AS.isPlaying) {
            mainBGM_AS.Stop();
            PlayerPrefs.SetInt("BGM",0);
        } else {
            mainBGM_AS.Play();
            PlayerPrefs.SetInt("BGM",1);
        }
    }


}
