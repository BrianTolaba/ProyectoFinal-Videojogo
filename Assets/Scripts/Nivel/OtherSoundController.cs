using UnityEngine;

public class OtherSoundController : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float variacionPitch = 0.05f;

    [Header("Money")]
    [SerializeField] private AudioClip soundMoney;

    [Header("Enemy")]
    [SerializeField] private AudioClip soundEnemyAttack;
    [SerializeField] private AudioClip soundEnemyVoice;
    [SerializeField] private AudioClip soundEnemyDamage;
    [SerializeField] private AudioClip soundEnemyDead;

    [Header("Monk")]
    [SerializeField] private AudioClip soundHealing;
    [SerializeField] private AudioClip soundUpLife;

    [Header("Blacksmith")]
    [SerializeField] private AudioClip soundBlacksmithWorking;

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
