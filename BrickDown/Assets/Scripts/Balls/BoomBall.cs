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
        radius = 20f + (float)damage*0.01f;
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

        if(colObj.CompareTag("Brick") && isMoving) {

            Collider2D[] boomBricks = Physics2D.OverlapCircleAll(transform.position/*colPoint*/, radius);
            Destroy(Instantiate(GM.ParticleBoom_P,colPoint, Quaternion.identity),1);
            if(GM.impactOn)
                GM.mainCamera.GetComponent<Animator>().SetTrigger("Boom");
            if(GM.soundOn)
                GM.BoomBall_AS.Play();
            RB2D.velocity = Vector2.zero;
            gameObject.GetComponent<TrailRenderer>().enabled = false;


            foreach(Collider2D boomBrick in boomBricks) {
                if(boomBrick.CompareTag("Brick")) {
                    boomBrick.GetComponent<Brick>().HitBrick(damage);
                }

            }            
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
