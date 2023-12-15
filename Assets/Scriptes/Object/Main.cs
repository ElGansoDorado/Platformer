using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Main : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite isLife, nonLife;
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private GameObject LosePanel;

    public void Update()
    {
        coinText.text = player.coins.ToString();

        for (int i = 0; i < hearts.Length; i++)
        {
            if (player.curHp > i)
             {
                hearts[i].sprite = isLife;
            }
            else
            {
                hearts[i].sprite = nonLife;
            }
        }
    }

    public void ReloadLevel()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseOn()
    {
        Time.timeScale = 0f;
        player.enabled = false;
        PausePanel.SetActive(true);
    }

    public void PauseOff()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        PausePanel.SetActive(false);
    }

    public void Win()
    {
        Time.timeScale = 0f;
        player.enabled = false;
        WinPanel.SetActive(true);
    }

    public void Lose()
    {
        Time.timeScale = 0f;
        player.enabled = false;
        LosePanel.SetActive(true);
    }

    public void MenuLevel()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene("Menu");
    }

    public void Nextlevel()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
