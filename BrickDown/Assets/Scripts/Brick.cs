using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Brick : MonoBehaviour
{
    public int brickType; //0이면 기본 1이면 물음표
    public int brickValue;
    public TMP_Text brickText;
    public GameManager GM;
    public SpriteRenderer SR;

    void Start() {
        GM = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    public void SetBrick(int value, int type) {
        brickType = type;
        brickValue = value;
        if(brickType == 0)
            brickText.text = brickValue.ToString();
        else    brickText.text = "?";
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
            if(brickType == 1) brickText.text = "?";
            else brickText.text = brickValue.ToString();
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
            SR.color = new Color32(165, 100, 255, 255);
        } else {
            if(brickValue >= 495) {
                SR.color = new Color32(255, 30, 81, 255);
            } else {
                int colorValue = 9*((495-brickValue)/55) + 30;
                SR.color = new Color(255f / 255f, (float)colorValue / 255f, 81 / 255f, 255 / 255f);
            } 
            //255 65 81
            //30 45 60 75 90 105 120
            //15씩 더해서 30 ~ 120
        }
    }

}
