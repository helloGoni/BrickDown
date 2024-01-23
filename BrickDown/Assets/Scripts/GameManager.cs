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
    public GameObject NormalBall_P, Brick_P, GreenOrb_P;
    public GameObject ParticleBlue_P, ParticleGreen_P, ParticleRed_P;
    public GameObject GameoverPanel;
    public GameObject NormalBallPreview,Arrow;
    public GameObject LeftBtn, RightBtn;
    public GameObject[] weaponSlot = new GameObject[10];
    
    public NormalBall normalBall;

    public Transform BallGroup, BrickGroup;

    public LineRenderer Mouse_LR, Ball_LR;
    public TMP_Text ScoreText_TMP, MoneyText_TMP, EndScoreText_TMP;
    public TMP_Text[] weaponSlotValue = new TMP_Text[10];

    public Color[] brickColor;
    public Color greenColor;
    public AudioSource Gameover_AS, GreenOrb_AS, Plus_AS;
    public AudioSource[] Brick_AS;
    

    public Vector3 firstPos_Mouse, secondPos_Mouse, gapPos, ballStartPos;
    public int score, timerCount, launchIndex, money;
    public bool shotTrigger, shotAble, isDie, isNewRecord, isBrickMoving, timerStart;

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
        RenewMoney();
        if(Input.GetMouseButtonDown(0)) {
            firstPos_Mouse =  Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0,0,10);
        }

        shotAble = true;
        for(int i = 0 ; i < BallGroup.childCount; i++) {
            if(BallGroup.GetChild(i).GetComponent<NormalBall>().isMoving)
                shotAble = false;
        }
        if(isBrickMoving)
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
        NormalBallPreview.SetActive(isMouse);
        Arrow.SetActive(isMouse);
        if(isMouse) {
            secondPos_Mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0,0,10);

            Mouse_LR.SetPosition(0,firstPos_Mouse);
            Mouse_LR.SetPosition(1,secondPos_Mouse);

            if((secondPos_Mouse - firstPos_Mouse).magnitude < 1) return;
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
            Mouse_LR.SetPosition(0,Vector3.zero);
            Mouse_LR.SetPosition(1,Vector3.zero);
            Ball_LR.SetPosition(0,Vector3.zero);
            Ball_LR.SetPosition(1,Vector3.zero);

            ballStartPos = Vector3.zero;
            firstPos_Mouse = Vector3.zero;
            timerStart = true;
        }
    }

    void FixedUpdate() { // 1초에 50번
        if(timerStart && ++timerCount == 3) { // 0.06초 간격
            timerCount = 0;
            BallGroup.GetChild(launchIndex++).GetComponent<NormalBall>().Shot(gapPos);
            if(launchIndex == BallGroup.childCount) {
                timerStart = false;
                launchIndex = 0;
                timerCount = 0;
            }
        }
    }
    




    #region 게임시작 / 종료

    void Init() {
        score = 0;
        upgradeWeapon = SaveManager.instance.ReturnUpgrade();
        SetWeapon();
        Vector3 start = new Vector3(0,54.87391f,0);
        SetStartPos(start);
        StartBrick();
    }

    public void ToMainScene() {
        SceneManager.LoadScene("MainScene");
    }

    public void Gameover() {
        SaveManager.instance.Gameover(score,money);
        GameoverPanel.SetActive(true);
    }

    public void SetWeapon() {
        nowWeapon = 0;
        if(upgradeWeapon[1] == 0) {
            LeftBtn.SetActive(false);
            RightBtn.SetActive(false);
        }
        for(int i = 0 ; i < 10 ; i++) {
            if(weaponSlot[i] != null) {
                weaponSlotValue[i].text = upgradeWeapon[i].ToString();
            }
        }
        RenewWeapon();
    }

    #endregion

    #region 블록

    IEnumerator MoveUpBrick(Transform transform) {
        yield return new WaitForSeconds(0.2f);
        Vector3 targetPos = transform.position + new Vector3(0, 12.8f, 0);
        

        if(targetPos.y > 50) {
            if(transform.CompareTag("Brick")) {
                isDie = true;
                Gameover();
            }
            for(int i = 0 ; i < BallGroup.childCount ; i++) {
                BallGroup.GetChild(i).GetComponent<CircleCollider2D>().enabled = false;
            }
        }


        float timeDelta = 1.5f;
        while(true) {
            yield return null;
            timeDelta -= Time.deltaTime * 1.5f;
            transform.position = Vector3.MoveTowards(transform.position, targetPos + new Vector3(0,6,0), timeDelta);
            if (transform.position == targetPos + new Vector3(0, 6, 0)) break;
        }
        timeDelta = 0.9f;
        while(true) {
            yield return null;
            timeDelta -= Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position,targetPos,timeDelta);
            if(transform.position == targetPos) break;
        }
        isBrickMoving = false;
    }

    void MakeBrick() {
        
        isBrickMoving = true;
        for(int i = 0 ; i < BrickGroup.childCount; i++) {
            StartCoroutine(MoveUpBrick(BrickGroup.GetChild(i)));
        }
        isBrickMoving = false;
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

            Transform transform = Instantiate(Brick_P, spawnList[random], Quaternion.identity).transform;
            transform.SetParent(BrickGroup);
            transform.GetChild(0).GetComponentInChildren<TMP_Text>().text = score.ToString();

            spawnList.RemoveAt(random);
        }


    }

    void ChangeBrickColor() {
        for(int i = 0 ; i < BrickGroup.childCount ; i++) {
            if (BrickGroup.GetChild(i).CompareTag("Brick")) {
                //float per = int.Parse(BlockGroup.GetChild(i).GetChild(0).Get)
            }
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

            Transform transform = Instantiate(Brick_P, spawnList[random], Quaternion.identity).transform;
            transform.SetParent(BrickGroup);
            transform.GetChild(0).GetComponentInChildren<TMP_Text>().text = score.ToString();

            spawnList.RemoveAt(random);
        }
    }


    #endregion

    #region 연출 / 표현 / 설정

    void RenewMoney() {
        MoneyText_TMP.text = money.ToString();
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
        }
        weaponSlot[nowWeapon].SetActive(true);
    }


    #endregion

    #region 전체 Shot 관련

    public void SetStartPos(Vector3 pos) {
        if(ballStartPos == Vector3.zero) {
            ballStartPos = pos;
        }
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

    #region 노말볼 
    #endregion
    
    #region 바운스볼 

    #endregion

    #region 레이저볼
    #endregion

    #region 폭탄볼
    #endregion
}
