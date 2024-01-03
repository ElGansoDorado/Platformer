using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;

public class Main : MonoBehaviour
{

    #region CONSTANS

    private const string LVL = "COMPLETED_LEVELS";
    private const string COINS = "Coins";
    private const string GEMS = "Gems";

    #endregion


    [SerializeField] private Player player;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite isLife, nonLife;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;


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
        if (!PlayerPrefs.HasKey(LVL) || PlayerPrefs.GetInt(LVL) < SceneManager.GetActiveScene().buildIndex)
        {
            PlayerPrefs.SetInt(LVL, SceneManager.GetActiveScene().buildIndex);
        }

        if (PlayerPrefs.HasKey(COINS))
        {
            PlayerPrefs.SetInt(COINS, PlayerPrefs.GetInt(COINS) + player.coins);
        }
        else
        {
            PlayerPrefs.SetInt(COINS, player.coins);
        }

        string gemsThisLvl = GEMS + SceneManager.GetActiveScene().buildIndex.ToString();
        if (PlayerPrefs.HasKey(gemsThisLvl))
        {
            if (PlayerPrefs.GetInt(gemsThisLvl) > player.gems)
            {
                PlayerPrefs.SetInt(gemsThisLvl, player.gems);
                PlayerPrefs.SetInt(GEMS, PlayerPrefs.GetInt(GEMS) + Math.Abs(player.gems - PlayerPrefs.GetInt(gemsThisLvl)));
            }
        }
        else
        {
            PlayerPrefs.SetInt(gemsThisLvl, player.gems);

            if (PlayerPrefs.HasKey(GEMS))
            {
                PlayerPrefs.SetInt(GEMS, PlayerPrefs.GetInt(GEMS) + player.gems);
            }
            else
            {
                PlayerPrefs.SetInt(GEMS, player.gems);
            }
        }
    }
}
