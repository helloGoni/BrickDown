using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [Header ("[ Audio Source ]")]
    public AudioSource gameOver_AS;
    public AudioSource greenOrb_AS;
    public AudioSource plus_AS;
    public AudioSource[] normalBall_AS;
    public AudioSource laserBall_AS;
    public AudioSource boomBall_AS;
    public AudioSource crossBall_AS;

    public void PlayGameOver() {
        gameOver_AS.Play();
    }

    public void PlayGreenOrb() {
        greenOrb_AS.Play();
    }

    public void PlayPlus() {
        plus_AS.Play();
    }

    public void PlayNormalBall() {
        for(int i = 0 ; i < normalBall_AS.Length ; i++) {
            if(!normalBall_AS[i].isPlaying) {
                normalBall_AS[i].Play();
                break;
            }
        }
    }

    public void PlayLaserBall() {
        laserBall_AS.Play();
    }
    
    public void PlayBoomBall() {
        boomBall_AS.Play();
    }

    public void PlayCrossBall() {
        crossBall_AS.Play();
    }
}
