using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Brick : MonoBehaviour
{
    public int brickValue;
    public TMP_Text brickText;


    public void SetBrick(int value) {
        brickValue = value;
    }

    public IEnumerator MoveUpBrick() {
        yield return new WaitForSeconds(0.2f);

        Vector3 targetPos = gameObject.transform.position + new Vector3(0, 12.8f, 0);

        if(targetPos.y > 50) {
            if(transform.CompareTag("Brick")) {
                //isDie = true;
                //Gameover();
            }
            //for(int i = 0 ; i < BallGroup.childCount ; i++) {
                //BallGroup.GetChild(i).GetComponent<CircleCollider2D>().enabled = false;
            //}
        }


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
            brickText.text = brickValue.ToString();
            gameObject.GetComponent<Animator>().SetTrigger("Shock");
        } else {
            Destroy(gameObject);
        }

    }



}
