using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sonidoMov1;
    public AudioClip sonidoVoz;
    public AudioClip sonidoAtaque;
    public AudioClip sonidoRecibirDaño;
    public AudioClip sonidoMuerte;
    public AudioClip sonidoMov2;

    public void PlayMovimientoSound()
    {
        audioSource.PlayOneShot(sonidoMov1);
    }
    public void PlayMovimiento2Sound()
    {
        audioSource.PlayOneShot(sonidoMov2);
    }
    public void PlayVozSound()
    {
        audioSource.PlayOneShot(sonidoVoz);
    }
    public void PlayAtaqueSound()
    {
        audioSource.PlayOneShot(sonidoAtaque);
    }
    public void PlayRecibirDañoSound()
    {
        audioSource.PlayOneShot(sonidoRecibirDaño);
    }
    public void PlayMuerteSound()
    {
        audioSource.PlayOneShot(sonidoMuerte);
    }
    public void StopSound()
    {
        audioSource.Stop();
    }



}
