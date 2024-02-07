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


        if(obj.CompareTag("Brick") && isMoving) {

            Destroy(Instantiate(GM.ParticleCross_P,colPoint, Quaternion.identity),1);
            Destroy(Instantiate(GM.ParticleCross_P,colPoint, Quaternion.Euler(new Vector3(0,0,90))),1);
            Destroy(Instantiate(GM.ParticleCross_P,colPoint, Quaternion.Euler(new Vector3(0,0,180))),1);
            Destroy(Instantiate(GM.ParticleCross_P,colPoint, Quaternion.Euler(new Vector3(0,0,270))),1);
            if(GM.soundOn)
                GM.CrossBall_AS.Play();
            Destroy(Instantiate(GM.ParticleWhite_P,colPoint, Quaternion.identity),1);
            RaycastHit2D[] hitDown = Physics2D.RaycastAll(colPoint,Vector2.down, 100);
            RaycastHit2D[] hitUp = Physics2D.RaycastAll(colPoint,Vector2.up, 100);
            RaycastHit2D[] hitRight = Physics2D.RaycastAll(colPoint,Vector2.right, 100);
            RaycastHit2D[] hitLeft = Physics2D.RaycastAll(colPoint, Vector2.left, 100);

            obj.GetComponent<Brick>().HitBrick(damage);

            foreach(RaycastHit2D hitBrick in hitDown) {
                if(hitBrick.collider.CompareTag("Brick") && hitBrick.collider.gameObject != obj) {
                    hitBrick.collider.GetComponent<Brick>().HitBrick(damage);
                }
            }
            foreach(RaycastHit2D hitBrick in hitUp) {
                if(hitBrick.collider.CompareTag("Brick") && hitBrick.collider.gameObject != obj) {
                    hitBrick.collider.GetComponent<Brick>().HitBrick(damage);
                }
            }
            foreach(RaycastHit2D hitBrick in hitRight) {
                if(hitBrick.collider.CompareTag("Brick") && hitBrick.collider.gameObject != obj) {
                    hitBrick.collider.GetComponent<Brick>().HitBrick(damage);
                }
            }
            foreach(RaycastHit2D hitBrick in hitLeft) {
                if(hitBrick.collider.CompareTag("Brick") && hitBrick.collider.gameObject != obj) {
                    hitBrick.collider.GetComponent<Brick>().HitBrick(damage);
                }
            }


            RB2D.velocity = Vector2.zero;
            transform.position = new Vector2(col2D.contacts[0].point.x, GM.lifelineY);
            GM.SetStartPos(transform.position);
            isMoving = false;




        }

        yield return null;
    }
    


}