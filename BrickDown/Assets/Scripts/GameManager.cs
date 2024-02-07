using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{


    #region 자료

    public float lifelineY = 55.489f;
    public GameObject Brick_P, GreenOrb_P;
    public GameObject[] BallPrefabs = new GameObject[10];

    public GameObject ParticleBlue_P, ParticleGreen_P, ParticleRed_P, ParticleViolet_P, ParticleBoom_P, ParticleWhite_P, ParticleCross_P;

    public GameObject GameoverPanel, GameoverNewRecordText;
    public GameObject NormalBallPreview,Arrow;
    public GameObject LeftBtn, RightBtn;
    public GameObject[] weaponSlot = new GameObject[10];
    
    public Camera mainCamera;


    public Transform BrickGroup;
    public Transform[] TotalBallGroup = new Transform[10];
    public GameObject[] BallGroupObj = new GameObject[10];

    public LineRenderer Mouse_LR, Ball_LR;
    public TMP_Text ScoreText_TMP, MoneyText_TMP, EndScoreText_TMP, EndMoneyText_TMP, SoundText_TMP, ImpactText_TMP;
    public TMP_Text[] WeaponSlotValue = new TMP_Text[10];

    public Color[] brickColor;
    public Color greenColor;

    public AudioSource Gameover_AS, GreenOrb_AS, Plus_AS;
    public AudioSource[] NormalBall_AS;
    public AudioSource LaserBall_AS, BoomBall_AS, CrossBall_AS;
    

    public Vector3 firstPos_Mouse, secondPos_Mouse, gapPos, ballStartPos;
    public int score, timerCount, launchIndex, money;
    public bool shotTrigger, shotAble, isDie, isNewRecord, isBrickMoving, timerStart, soundOn, impactOn;

    public float timeDelay;

    public int nowWeapon;
    public int[] upgradeWeapon = new int[10];

    #endregion

    void Awake() {
        Camera camera = Camera.main;
        Rect rect = camera.rect;
        float height = ((float)Screen.width / Screen.height) / ((float)9/16);
        float width = 1f / height;
        if(height < 1) {
            rect.height = height;
            rect.y = ( 1f - height ) / 2f;
        } else {
            rect.width = width;
            rect.x = ( 1f - width ) / 2f;
        }
        camera.rect = rect;

        Init();
    }

    void Update() {
        if(isDie) return;

        if(Input.GetMouseButtonDown(0)) {
            firstPos_Mouse =  Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0,0,10);
        }

        shotAble = true;

        if(!CheckShotAble())
            shotAble = false;
        if(!shotAble) return;

        if(shotTrigger && shotAble) {
            shotTrigger = false;
            MakeBrick();
            timeDelay = 0;
        }

        timeDelay += Time.deltaTime;
        if(timeDelay < 0.1f) return; //너무빨리손떼면 라인 안사라져서 버그수정



        bool isMouse = Input.GetMouseButton(0);

        if(isMouse) {
            secondPos_Mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0,0,10);
            if((secondPos_Mouse - firstPos_Mouse).magnitude < 20) {
                NormalBallPreview.SetActive(!isMouse);
                Arrow.SetActive(!isMouse);
                Mouse_LR.SetPosition(0,Vector3.zero);
                Mouse_LR.SetPosition(1,Vector3.zero);
                Ball_LR.SetPosition(0,Vector3.zero);
                Ball_LR.SetPosition(1,Vector3.zero);
                return;
            }
            Mouse_LR.SetPosition(0,firstPos_Mouse);
            Mouse_LR.SetPosition(1,secondPos_Mouse);

            NormalBallPreview.SetActive(isMouse);
            Arrow.SetActive(isMouse);

            gapPos = (secondPos_Mouse - firstPos_Mouse).normalized;
            gapPos = new Vector3(gapPos.y >= 0 ? (gapPos.x >= 0 ? 1 : -1) : gapPos.x , Mathf.Clamp(gapPos.y, -1f, -0.15f) , 0);

            Arrow.transform.position = ballStartPos;
            Arrow.transform.rotation = Quaternion.Euler(0,0,Mathf.Atan2(gapPos.y,gapPos.x) * Mathf.Rad2Deg);
            //아래 수정 확실히 이해
            NormalBallPreview.transform.position = Physics2D.CircleCast(new Vector2(Mathf.Clamp(ballStartPos.x,-54,54),lifelineY),1.7f,gapPos,10000,
                                                                        1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Brick")).centroid;
            RaycastHit2D hit2D = Physics2D.Raycast(ballStartPos, gapPos, 10000, 1 << LayerMask.NameToLayer("Wall"));
            Ball_LR.SetPosition(0, ballStartPos);
            Ball_LR.SetPosition(1,(Vector3)hit2D.point - gapPos * 1.5f);


        }
        if(Input.GetMouseButtonUp(0)) {
            if((secondPos_Mouse - firstPos_Mouse).magnitude < 20){
                NormalBallPreview.SetActive(isMouse);
                Arrow.SetActive(isMouse);
                Mouse_LR.SetPosition(0,Vector3.zero);
                Mouse_LR.SetPosition(1,Vector3.zero);
                Ball_LR.SetPosition(0,Vector3.zero);
                Ball_LR.SetPosition(1,Vector3.zero);
                return;
            }
            NormalBallPreview.SetActive(isMouse);
            Arrow.SetActive(isMouse);
            Mouse_LR.SetPosition(0,Vector3.zero);
            Mouse_LR.SetPosition(1,Vector3.zero);
            Ball_LR.SetPosition(0,Vector3.zero);
            Ball_LR.SetPosition(1,Vector3.zero);

            ballStartPos = Vector3.zero;
            firstPos_Mouse = Vector3.zero;
            timerStart = true;
            LeftBtn.SetActive(false);
            RightBtn.SetActive(false);
        }
    }

    void FixedUpdate() { // 1초에 50번
    
        if(timerStart && ++timerCount == 3) {
            timerCount = 0;
            switch(nowWeapon) {
                case 0:
                    TotalBallGroup[0].GetChild(launchIndex++).GetComponent<NormalBall>().Shot(gapPos);
                    if(launchIndex == TotalBallGroup[0].childCount) {
                        timerStart = false;
                        launchIndex = 0;
                        timerCount = 0;
                    }
                    break;
                case 1:
                    TotalBallGroup[1].GetChild(0).GetComponent<LaserBall>().Shot(gapPos);
                    timerStart = false;
                    launchIndex = 0;
                    timerCount = 0;
                    break;
                case 2:
                    TotalBallGroup[2].GetChild(0).GetComponent<BoomBall>().Shot(gapPos);
                    timerStart = false;
                    launchIndex = 0;
                    timerCount = 0; 
                    break;
                case 3:
                    TotalBallGroup[3].GetChild(0).GetComponent<CrossBall>().Shot(gapPos);
                    timerStart = false;
                    launchIndex = 0;
                    timerCount = 0;
                    break;
                case 4:
                    TotalBallGroup[4].GetChild(0).GetComponent<BounceBall>().Shot(gapPos);
                    timerStart = false;
                    launchIndex = 0;
                    timerCount = 0;
                    break;
            }
        } 
    }
    
    #region 게임시작 / 종료 / 재화

    void Init() {
        score = 0;
        money = 0;
        upgradeWeapon = SaveManager.instance.ReturnUpgrade();
        Vector3 start = new Vector3(0,54.87391f,0);
        SetStartPos(start);
        SetWeapon();
        StartBrick();
        RenewMoney();
        if(PlayerPrefs.GetInt("Sound") == null || PlayerPrefs.GetInt("Sound") == 1) {
            soundOn = true;
            PlayerPrefs.SetInt("Sound",1);
            SoundText_TMP.text = "ON";
        } else {
            soundOn = false;
            PlayerPrefs.SetInt("Sound",0);
            SoundText_TMP.text = "OFF";
        }
        if(PlayerPrefs.GetInt("Impact") == null || PlayerPrefs.GetInt("Impact") == 1) {
            impactOn = true;
            PlayerPrefs.SetInt("Impact",1);
            ImpactText_TMP.text = "ON";
        } else {
            impactOn = false;
            PlayerPrefs.SetInt("Impact",0);
            ImpactText_TMP.text = "OFF";
        }
    
    }

    public void ToMainScene() {
        SceneManager.LoadScene("MainScene");
    }

    public void Gameover() {
        int tempScore = SaveManager.instance.ReturnScore();
        SaveManager.instance.Gameover(score,money);
        GameoverPanel.SetActive(true);
        if( tempScore > score) {
            GameoverNewRecordText.SetActive(false);
        }
    }

    public void SetWeapon() {
        nowWeapon = 0;
        for(int i = 0 ; i < upgradeWeapon[0] ; i++) {
            Instantiate(BallPrefabs[0], ballStartPos,Quaternion.identity).transform.SetParent(TotalBallGroup[0]);
        }
        for(int i = 1 ; i < 10 ; i++) {
            if(upgradeWeapon[i] > 0) {
                Instantiate(BallPrefabs[i], ballStartPos, Quaternion.identity).transform.SetParent(TotalBallGroup[i]);
            }   
        }
        if(upgradeWeapon[1] == 0) {
            LeftBtn.SetActive(false);
            RightBtn.SetActive(false);
        }
        for(int i = 0 ; i < 10 ; i++) {
            if(weaponSlot[i] != null) {
                WeaponSlotValue[i].text = upgradeWeapon[i].ToString();
            }
        }
        RenewWeapon();
    }

    public void EarnMoney(int amount) {
        money += amount;
        RenewMoney();
    }
    #endregion

    #region 블록

    IEnumerator NextRound() {
        for(int i = 0 ; i < BrickGroup.childCount ; i++) {
            StartCoroutine(BrickGroup.GetChild(i).GetComponent<Brick>().MoveUpBrick());         
            if(BrickGroup.GetChild(i).position.y > 35) {
              isDie = true;
              Gameover();
            }  
        }
        isBrickMoving = false;
        yield return null;
    }

    void MakeBrick() {
        
        isBrickMoving = true;
        StartCoroutine(NextRound());

        //아래는 난이도
        ++score;
        RenewScore();
        int count;
        int randomBrick = Random.Range(0,24);
        if(score <= 10) count = randomBrick < 16 ? 1 : 2;
        else if(score <= 20) count = randomBrick < 8 ? 1 : (randomBrick < 16 ? 2 : 3);
        else if(score <= 40) count = randomBrick < 9 ? 2 : (randomBrick < 18 ? 3 : 4);
        else count = randomBrick < 8 ? 2 : (randomBrick < 16 ? 3 : (randomBrick < 20 ? 4 : 5));

        //실 생성 코드
        List<Vector3> spawnList = new List<Vector3>();
        for(int i = 0 ; i < 6 ; i++)
            spawnList.Add(new Vector3(-46.7f + i * 18.68f, -51.2f, 0)); // y 원래 51.2
        for(int i = 0 ; i < count ; i++) {
            int random = Random.Range(0, spawnList.Count);

            GameObject brick = Instantiate(Brick_P, spawnList[random], Quaternion.identity);
            int type = Random.Range(0,10);
            
            if(type == 5) {
                int temp = Random.Range(0,5);
                brick.GetComponent<Brick>().SetBrick(score+temp,1);
            }
            else
                brick.GetComponent<Brick>().SetBrick(score,0);
            brick.transform.SetParent(BrickGroup);

            spawnList.RemoveAt(random);
        }

        if(upgradeWeapon[1] > 0) {
            LeftBtn.SetActive(true);
            RightBtn.SetActive(true);
        }


    }
    
    void StartBrick() {
        //아래는 난이도
        ++score;
        RenewScore();
        int count;
        int randomBrick = Random.Range(0,24);
        if(score <= 10) count = randomBrick < 16 ? 1 : 2;
        else if(score <= 20) count = randomBrick < 8 ? 1 : (randomBrick < 16 ? 2 : 3);
        else if(score <= 40) count = randomBrick < 9 ? 2 : (randomBrick < 18 ? 3 : 4);
        else count = randomBrick < 8 ? 2 : (randomBrick < 16 ? 3 : (randomBrick < 20 ? 4 : 5));

        //실 생성 코드
        List<Vector3> spawnList = new List<Vector3>();
        for(int i = 0 ; i < 6 ; i++)
            spawnList.Add(new Vector3(-46.7f + i * 18.68f, -51.2f, 0)); // y 원래 51.2
        for(int i = 0 ; i < count ; i++) {
            int random = Random.Range(0, spawnList.Count);

            GameObject brick = Instantiate(Brick_P, spawnList[random], Quaternion.identity);
            brick.GetComponent<Brick>().SetBrick(score,0);
            brick.transform.SetParent(BrickGroup);

            spawnList.RemoveAt(random);
        }
    }


    #endregion

    #region 연출 / 표현 / 설정

    void RenewMoney() {
        MoneyText_TMP.text = money.ToString();
        EndMoneyText_TMP.text = "+ " + money.ToString();
    }

    void RenewScore() {
        ScoreText_TMP.text = "<size=140>" + (score).ToString() + "</size> Round";
        EndScoreText_TMP.text = (score).ToString() + " Round";
    }

    void RenewWeapon() {
        for(int i = 0 ; i < 10 ; i++) {
            if(weaponSlot[i] != null) {
                weaponSlot[i].SetActive(false);
            }
            if(BallGroupObj[i] != null) {
                BallGroupObj[i].SetActive(false);
            }
        }
        weaponSlot[nowWeapon].SetActive(true);
        BallGroupObj[nowWeapon].SetActive(true);

        for(int i = 0 ; i < TotalBallGroup[nowWeapon].childCount ; i++) {
            TotalBallGroup[nowWeapon].GetChild(i).transform.position = ballStartPos;
        }

    }     

    public void ToggleSound() {
        if(PlayerPrefs.GetInt("Sound") == 0) {
            PlayerPrefs.SetInt("Sound",1);
            soundOn = true;
            SoundText_TMP.text = "ON";
        } else {
            PlayerPrefs.SetInt("Sound",0);
            soundOn = false;
            SoundText_TMP.text = "OFF";
        }
    }

    public void ToggleImpact() {
        if(PlayerPrefs.GetInt("Impact") == 0) {
            PlayerPrefs.SetInt("Impact",1);
            impactOn = true;
            ImpactText_TMP.text = "ON";
        } else {
            PlayerPrefs.SetInt("Impact",0);
            impactOn = false;
            ImpactText_TMP.text = "OFF";
        }
    }

    #endregion

    #region 전체 Shot 관련

    public void SetStartPos(Vector3 pos) { //볼 출발지점 세팅
        if(ballStartPos == Vector3.zero) {
            ballStartPos = pos;
        }
    }
    
    public bool CheckShotAble() { // ShotAble 체크함수
        if(isBrickMoving)
            return false;    

        for(int i = 0 ; i < TotalBallGroup[0].childCount; i++) {
            if(TotalBallGroup[0].GetChild(i).GetComponent<NormalBall>().isMoving)
                return false;
        }
        
        if(upgradeWeapon[1] > 0 && TotalBallGroup[1].GetChild(0).GetComponent<LaserBall>().isMoving)
            return false;
        if(upgradeWeapon[2] > 0 && TotalBallGroup[2].GetChild(0).GetComponent<BoomBall>().isMoving)
            return false;
        if(upgradeWeapon[3] > 0 && TotalBallGroup[3].GetChild(0).GetComponent<CrossBall>().isMoving)
            return false;
        if(upgradeWeapon[4] > 0 && TotalBallGroup[4].GetChild(0).GetComponent<BounceBall>().isMoving)
            return false;

        return true;
    }


    #endregion

    #region Weapon Chanage

    public void NextWeapon() {
        nowWeapon++;
        if(upgradeWeapon[nowWeapon] < 1) {
            nowWeapon = 0;
        }
        RenewWeapon();
    }
    
    public void PreviousWeapon() {
        nowWeapon--;
        if(nowWeapon<0) {
            nowWeapon = 9;
            while(upgradeWeapon[nowWeapon] <= 0) {
                nowWeapon--;
            }
        }
        RenewWeapon();
    }

    #endregion

}
