using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;

public class Main : MonoBehaviour
{

    [SerializeField] private Player player;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite isLife, nonLife;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    private GamePreferences gp;
    private float timer;


    private void Start()
    {
        gp = new GamePreferences();
    }

    private void Update()
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

        timer += Time.deltaTime;
        timeText.text = timer.ToString("F2").Replace(",", ":");
    }
    
    
    public void ReloadLevel()
    {
        Status(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseOn()
    {
        Status(false);
        pausePanel.SetActive(true);
    }

    public void PauseOff()
    {
        Status(true);
        pausePanel.SetActive(false);
    }

    public void Win()
    {
        Status(false);
        winPanel.SetActive(true);
        
        SaveData();
    }

    public void Lose()
    {
        Status(false);
        losePanel.SetActive(true);
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


    private void Status(bool isPause)
    {
        Time.timeScale = isPause ? 1f : 0f;
        player.enabled = isPause;
    }

    private void SaveData()
    {
        gp.Levels = SceneManager.GetActiveScene().buildIndex;
        gp.Coins = player.coins;
        gp.Gems = player.gems;
    }
}
