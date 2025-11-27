using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip soundMovement;
    public AudioClip soundAttackVoice;
    public AudioClip soundAttackSword;
    public AudioClip soundDamage;
    public AudioClip soundDead;
    private float variacionPitch = 0.05f;

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
