using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    void Start()
    {
        
    }

    public IEnumerator MoveUpStar() {

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
        if(gameObject.transform.position.y > 44) {
            DeleteStar();
        }
    }

    public void DeleteStar() {
        Destroy(gameObject);
    }

    public void Check() {
        
    }


}
