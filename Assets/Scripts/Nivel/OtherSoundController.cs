using UnityEngine;

public class OtherSoundController : MonoBehaviour
{
    public AudioSource audioSource;
    [Header("Money")]
    public AudioClip soundMoney;
    [Header("Enemy")]
    public AudioClip soundEnemyAttack;
    public AudioClip soundEnemyVoice;
    public AudioClip soundEnemyDamage;
    public AudioClip soundEnemyDead;
    [Header("Monk")]
    public AudioClip soundHealing;
    public AudioClip soundUpLife;
    [Header("Blacksmith")]
    public AudioClip soundBlacksmithWorking;
    private float variacionPitch = 0.05f;

    public void PlayDineroSound()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(soundMoney);
    }
    public void PlayAtaqueEnemigoSound()
    {
        audioSource.pitch = Random.Range(1f - variacionPitch, 1f + variacionPitch);
        audioSource.PlayOneShot(soundEnemyAttack);
    }
    public void PlayEnemigoSound()
    {
        audioSource.pitch = Random.Range(1f - variacionPitch, 1f + variacionPitch);
        audioSource.PlayOneShot(soundEnemyVoice);
    }
    public void PlayGritoEnemigoSound()
    {
        audioSource.pitch = Random.Range(1f - variacionPitch, 1f + variacionPitch);
        audioSource.PlayOneShot(soundEnemyDamage);
    }
    public void PlayMuerteEnemigoSound()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(soundEnemyDead);
    }
    public void PlayCuraSound()
    {
        audioSource.pitch = Random.Range(1f - variacionPitch, 1f + variacionPitch);
        audioSource.PlayOneShot(soundHealing);
    }
    public void PlayUpVidaSound()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(soundUpLife);
    }
    public void PlayHerreroSound()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(soundBlacksmithWorking);
    }


}
