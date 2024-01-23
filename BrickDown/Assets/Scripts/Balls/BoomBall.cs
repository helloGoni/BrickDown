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
    public float radius = 1f;

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

        if(obj.CompareTag("Ground")) {
            RB2D.velocity = Vector2.zero;

        }

        if(obj.CompareTag("Brick")) {

            Collider2D[] boomBricks = Physics2D.OverlapCircleAll(obj.transform.position, radius);

            foreach(Collider2D boomBrick in boomBricks) {
                if(boomBrick.CompareTag("Brick")) {
                    TMP_Text hitBrickText = boomBrick.transform.GetChild(0).GetComponentInChildren<TMP_Text>();
                    int hitBrickValue = int.Parse(hitBrickText.text) - 1;
                }
            }

        }


        yield return null;
    }

}
