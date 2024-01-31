using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoomBall : MonoBehaviour
{
    GameManager GM;
    public Rigidbody2D RB2D;
    public bool isMoving;

    private float radius;
    private int damage;

    void Start() {
        GM = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        damage = GM.upgradeWeapon[2];
        radius = 20f + (float)damage*0.005f;
    }

    public void Shot(Vector3 pos) {
        GM.shotTrigger = true;
        isMoving = true;
        RB2D.AddForce(pos * 7000);
    }

    IEnumerator OnCollisionEnter2D(Collision2D col2D) {
        GameObject colObj = col2D.gameObject;
        Vector2 colPoint = new Vector2(col2D.contacts[0].point.x, col2D.contacts[0].point.y);

        if(colObj.CompareTag("Ground")) {
            RB2D.velocity = Vector2.zero;
            transform.position = new Vector2(col2D.contacts[0].point.x, GM.lifelineY);
            GM.SetStartPos(transform.position);
            isMoving = false;
        }

        if(colObj.CompareTag("Brick")) {

            Destroy(Instantiate(GM.ParticleBoom_P,colPoint, Quaternion.identity),1);
            Collider2D[] boomBricks = Physics2D.OverlapCircleAll(colPoint, radius);

            foreach(Collider2D boomBrick in boomBricks) {
                if(boomBrick.CompareTag("Brick")) {
                    TMP_Text hitBrickText = boomBrick.transform.GetChild(0).GetComponentInChildren<TMP_Text>();
                    int hitBrickValue = int.Parse(hitBrickText.text) - damage;
                    if( hitBrickValue > 0) {
                        hitBrickText.text = hitBrickValue.ToString();
                        colObj.GetComponent<Animator>().SetTrigger("Shock");
                    } else {
                        Destroy(colObj);
                        GM.money += GM.score;
                        Destroy(Instantiate(GM.ParticleRed_P, colObj.transform.position, Quaternion.identity),1);
                    }
                }

            }            

            RB2D.velocity = Vector2.zero;
            gameObject.GetComponent<TrailRenderer>().enabled = false;
            transform.position = new Vector2(col2D.contacts[0].point.x, GM.lifelineY);
            GM.SetStartPos(transform.position);
            isMoving = false;
            gameObject.GetComponent<TrailRenderer>().enabled = true;
        }

        yield return null;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,radius);
    }
    

}
