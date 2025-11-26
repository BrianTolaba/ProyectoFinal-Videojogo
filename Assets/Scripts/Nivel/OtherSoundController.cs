using UnityEngine;

public class OtherSoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sonidoDinero;
    public AudioClip sonidoAtaqueEnemigo;
    public AudioClip sonidoEnemigo;
    public AudioClip sonidoCura;
    public AudioClip sonidoUpVida;
    public AudioClip sonidoHerrero;
    public AudioClip sonidoSacerdote;
    public AudioClip sonidoGritoEnemigo;
    public AudioClip muerteEnemigo; 

    public void PlayDineroSound()
    {
        audioSource.PlayOneShot(sonidoDinero);
    }
    public void PlayAtaqueEnemigoSound()
    {
        audioSource.PlayOneShot(sonidoAtaqueEnemigo);
    }
    public void PlayEnemigoSound()
    {
        audioSource.PlayOneShot(sonidoEnemigo);
    }
    public void PlayGritoEnemigoSound()
    {
        audioSource.PlayOneShot(sonidoGritoEnemigo);
    }
    public void PlayMuerteEnemigoSound()
    {
        audioSource.PlayOneShot(muerteEnemigo);
    }
    public void PlayCuraSound()
    {
        audioSource.PlayOneShot(sonidoCura);
    }
    public void PlayUpVidaSound()
    {
        audioSource.PlayOneShot(sonidoUpVida);
    }
    public void PlaySacerdoteSound()
    {
        audioSource.PlayOneShot(sonidoSacerdote);
    }
    public void PlayHerreroSound()
    {
        audioSource.PlayOneShot(sonidoHerrero);
    }


}
