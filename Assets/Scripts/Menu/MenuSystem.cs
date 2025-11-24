using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public void Jugar() 
    {
        SceneManager.LoadScene("Nivel_1");
    }
    public void Salir()
    {
        Debug.Log("JuegoCerrado...");
        Application.Quit();
    }

}
