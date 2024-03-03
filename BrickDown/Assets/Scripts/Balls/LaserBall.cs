using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LaserBall : MonoBehaviour, IBall
{
    GameManager GM;
    public bool isMoving { get; set; }
    public Rigidbody2D RB2D;
    private int damage;
    private const int BALL_SPEED = 7000;

    void Start() {
        GM = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        damage = GM.upgradeWeapon[1];
    }

    public void Shot(Vector3 pos) {
        GM.shotTrigger = true;
        isMoving = true;
        RB2D.AddForce(pos * BALL_SPEED);
    }

    IEnumerator OnCollisionEnter2D(Collision2D col2D) {
        GameObject obj = col2D.gameObject;

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
        if(obj.CompareTag("Brick") && isMoving) {
            obj.GetComponent<Brick>().HitBrick(damage);
            if(GM.soundOn)
                GM.SM.PlayLaserBall();  
        }

        //가로로만 움직이는 경우
        Vector2 pos = RB2D.velocity.normalized;
        if(pos.magnitude != 0 && pos.y < 0.15f && pos.y > -0.15f) {
            RB2D.velocity = Vector2.zero;
            RB2D.AddForce(new Vector2(pos.x > 0 ? 1 : -1, -0.2f).normalized * 7000);
        }
    }
}