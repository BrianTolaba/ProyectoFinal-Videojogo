using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectorDeEscenas : MonoBehaviour
{
    public GameObject nivelButtonPrefab;
    public Transform buttonContainer;
    public int totalEscenas = 10;
    void Start()
    {
        GenerateLevelButtons();
    }
    void GenerateLevelButtons()
    {
        for (int i = 1; i <= totalEscenas; i++)
        {
            GameObject buttonObj = Instantiate(nivelButtonPrefab, buttonContainer);
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = "Nivel " + i;

            int levelIndex = i;
            buttonObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene("Nivel_" + levelIndex);
            });
        }

    }
}
