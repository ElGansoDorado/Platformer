using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffector : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip jumpSound, coinSound, winSound, loseSound;


    public void PlayJumpSound() => audioSource.PlayOneShot(jumpSound);

    public void PlayCoinSound() => audioSource.PlayOneShot(coinSound);

    public void PlayWinSound() => audioSource.PlayOneShot(winSound);

    public void PlayLoseSound() => audioSource.PlayOneShot(loseSound);

}
