using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrossBall : MonoBehaviour
{
    GameManager GM;
    public Rigidbody2D RB2D;
    public bool isMoving;

    private int damage;

    void Start() {
        GM = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        damage = GM.upgradeWeapon[3];
    }

    public void Shot(Vector3 pos) {
        GM.shotTrigger = true;
        isMoving = true;
        RB2D.AddForce(pos * 7000);
    }

    IEnumerator OnCollisionEnter2D(Collision2D col2D) {
        GameObject obj = col2D.gameObject;
        Vector2 colPoint = new Vector2(col2D.contacts[0].point.x, col2D.contacts[0].point.y);

        if(obj.CompareTag("Ground")) {
            RB2D.velocity = Vector2.zero;
            transform.position = new Vector2(col2D.contacts[0].point.x, GM.lifelineY);
            GM.SetStartPos(transform.position);
            isMoving = false;
        }


        if(obj.CompareTag("Brick")) {
            //obj.transform.position
            Destroy(Instantiate(GM.ParticleBoom_P,colPoint, Quaternion.identity),1);


            RaycastHit2D[] hitDown = Physics2D.RaycastAll(colPoint,Vector2.down, 100);
            RaycastHit2D[] hitUp = Physics2D.RaycastAll(colPoint,Vector2.up, 100);
            RaycastHit2D[] hitRight = Physics2D.RaycastAll(colPoint,Vector2.right, 100);
            RaycastHit2D[] hitLeft = Physics2D.RaycastAll(colPoint, Vector2.left, 100);


/*
            foreach(Collider2D boomBrick in boomBricks) {
                if(boomBrick.CompareTag("Brick")) {
                    TMP_Text hitBrickText = boomBrick.transform.GetChild(0).GetComponentInChildren<TMP_Text>();
                    int hitBrickValue = int.Parse(hitBrickText.text) - 1;
                    if( hitBrickValue > 0) {
                        hitBrickText.text = hitBrickValue.ToString();
                        obj.GetComponent<Animator>().SetTrigger("Shock");
                    } else {
                        Destroy(obj);
                        GM.money += GM.score;
                        Destroy(Instantiate(GM.ParticleRed_P, obj.transform.position, Quaternion.identity),1);
                    }
                }

            }            
*/
            RB2D.velocity = Vector2.zero;
            gameObject.GetComponent<TrailRenderer>().enabled = false;
            transform.position = new Vector2(col2D.contacts[0].point.x, GM.lifelineY);
            GM.SetStartPos(transform.position);
            isMoving = false;
            gameObject.GetComponent<TrailRenderer>().enabled = true;



        }

        yield return null;
    }
    


}