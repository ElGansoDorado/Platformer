using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro.Examples;

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

    private void Status(bool b)
    {
        Time.timeScale = b ? 1f : 0f;
        player.enabled = b;
    }
    
    public void ReloadLevel()
    {
        Status(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseOn()
    {
        Status(false);
        PausePanel.SetActive(true);
    }

    public void PauseOff()
    {
        Status(true);
        PausePanel.SetActive(false);
    }

    public void Win()
    {
        Status(false);
        WinPanel.SetActive(true);
    }

    public void Lose()
    {
        Status(false);
        LosePanel.SetActive(true);
    }


    public void MenuLevel()
    {
        Status(true);
        SceneManager.LoadScene("Menu");
    }

    public void Nextlevel()
    {
        Status(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
