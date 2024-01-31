using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NormalBall : MonoBehaviour
{
    GameManager GM;

    public Rigidbody2D RB2D;
    public bool isMoving;

    void Start() {
        GM = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    public void Shot(Vector3 pos) {
        GM.shotTrigger = true;
        isMoving = true;
        RB2D.AddForce(pos * 7000);
    }

    IEnumerator OnCollisionEnter2D(Collision2D col2D) {
        GameObject obj = col2D.gameObject;
        Physics2D.IgnoreLayerCollision(2,2);

        if(obj.CompareTag("Ground")) {
            RB2D.velocity = Vector2.zero;
            transform.position = new Vector2(col2D.contacts[0].point.x, GM.lifelineY);
            GM.SetStartPos(transform.position);

            while(true) {
                yield return null;
                transform.position = Vector3.MoveTowards(transform.position, GM.ballStartPos, 4);
                if(transform.position == GM.ballStartPos) {
                    isMoving = false;
                    yield break;
                }
            }

        }

        if(obj.CompareTag("Brick")) {
            TMP_Text brickText = obj.transform.GetChild(0).GetComponentInChildren<TMP_Text>();
            int brickValue = int.Parse(brickText.text) - 1;

            //for(int i = 0 ; i < GM.B) // 사운드

            if( brickValue > 0) {
                brickText.text = brickValue.ToString();
                obj.GetComponent<Animator>().SetTrigger("Shock");
            } else {
                Destroy(obj);
                GM.money += GM.score;
                Destroy(Instantiate(GM.ParticleRed_P, obj.transform.position, Quaternion.identity),1);
            }
        }
        //가로로만 움직이는 경우
        Vector2 pos = RB2D.velocity.normalized;
        if(pos.magnitude != 0 && pos.y < 0.15f && pos.y > -0.15f) {
            RB2D.velocity = Vector2.zero;
            RB2D.AddForce(new Vector2(pos.x > 0 ? 1 : -1, -0.2f).normalized * 7000);
        }
    }
}
