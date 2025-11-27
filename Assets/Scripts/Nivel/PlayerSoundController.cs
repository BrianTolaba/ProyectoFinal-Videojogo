using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float variacionPitch = 0.05f;

    [Header("Clips de Audio")]
    [SerializeField] private AudioClip soundMovement;
    [SerializeField] private AudioClip soundAttackVoice;
    [SerializeField] private AudioClip soundAttackSword;
    [SerializeField] private AudioClip soundDamage;
    [SerializeField] private AudioClip soundDead;

    public void PlayMovementSound()
    {
        audioSource.pitch = Random.Range(1f - variacionPitch, 1f + variacionPitch);
        audioSource.PlayOneShot(soundMovement);
    }
    public void PlayAttackVoiceSound()
    {
        audioSource.pitch = Random.Range(1f - variacionPitch, 1f + variacionPitch);
        audioSource.PlayOneShot(soundAttackVoice);
    }
    public void PlayAttackSwordSound()
    {
        audioSource.pitch = Random.Range(1f - variacionPitch, 1f + variacionPitch);
        audioSource.PlayOneShot(soundAttackSword);
    }
    public void PlayDamageSound()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(soundDamage);
    }
    public void PlayDeadSound()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(soundDead);
    }
    public void StopSound()
    {
        audioSource.Stop();
    }
}
