using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//brickType 0 : 기본 1 : 물음표

public class Brick : MonoBehaviour
{
    private int brickType; 
    private int brickValue;
    public TMP_Text brickText_TMP;
    public GameManager GM;
    public SpriteRenderer brick_SR;

    void Start() {
        GM = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    public void SetBrick(int value, int type) {
        brickType = type;
        brickValue = value;
        if(brickType == 0)
            brickText_TMP.text = brickValue.ToString();
        else    brickText_TMP.text = "?";
        ChangeColor();
    }

    public IEnumerator MoveUpBrick() {

        yield return new WaitForSeconds(0.2f);
        Vector3 targetPos = gameObject.transform.position + new Vector3(0, 12.8f, 0);

        float timeDelta = 1.5f;
        while(true) {
            yield return null;
            timeDelta -= Time.deltaTime * 1.5f;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPos + new Vector3(0,6,0), timeDelta);
            if (gameObject.transform.position == targetPos + new Vector3(0, 6, 0)) break;
        }

        timeDelta = 0.9f;
        while(true) {
            yield return null;
            timeDelta -= Time.deltaTime;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,targetPos,timeDelta);
            if(gameObject.transform.position == targetPos) break;
        }
    }

    public void HitBrick(int damage) {
        brickValue -= damage;

        if(brickValue > 0) {
            if(brickType == 1) brickText_TMP.text = "?";
            else brickText_TMP.text = brickValue.ToString();
            ChangeColor();
            gameObject.GetComponent<Animator>().SetTrigger("Shock");

        } else {
            Destroy(gameObject);
            if(brickType == 1)  {
                GM.EarnMoney(2);
                Destroy(Instantiate(GM.ParticleViolet_P, transform.position, Quaternion.identity),1);
            } else {
                GM.EarnMoney(1);
                Destroy(Instantiate(GM.ParticleRed_P, transform.position, Quaternion.identity),1);
            }
        }

    }

    private void ChangeColor() {
        if(brickType == 1) { 
            brick_SR.color = new Color32(165, 100, 255, 255);
        } else {
            if(brickValue >= 495) {
                brick_SR.color = new Color32(255, 30, 81, 255);
            } else {
                int colorValue = 9*((495-brickValue)/55) + 30;
                brick_SR.color = new Color(255f / 255f, (float)colorValue / 255f, 81 / 255f, 255 / 255f);
            } 
            //255 65 81
            //30 45 60 75 90 105 120
            //15씩 더해서 30 ~ 120
        }
    }

}
