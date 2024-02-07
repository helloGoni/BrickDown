using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBall : MonoBehaviour
{
    public Rigidbody2D RB2D;
    public int speed;
    public MainManager MM;
    public bool isShot;

    void Start()
    {
        speed = SaveManager.instance.ReturnScore()*10 + 1000;
        speed = 3000;
    }

    void Update() {
        if(isShot == true && (RB2D.velocity.magnitude < new Vector2(0.1f,0.1f).magnitude)) Shot();
    }

    public void Shot() {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        Vector3 pos = new Vector3(x,y,0).normalized;
        RB2D.AddForce(pos*speed);
    }

    void OnCollisionEnter2D(Collision2D col2D) {
        GameObject colObj = col2D.gameObject;
        if(colObj.CompareTag("Brick")) {
            colObj.GetComponent<Brick>().HitBrick(1);
        }
    }
}
