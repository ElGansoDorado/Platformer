using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro.Examples;
using System;

public class Main : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite isLife, nonLife;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

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

    private void Status(bool isPause)
    {
        Time.timeScale = isPause ? 1f : 0f;
        player.enabled = isPause;
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

    private void SaveData()
    {
        if (!PlayerPrefs.HasKey("Lvl") || PlayerPrefs.GetInt("Lvl") < SceneManager.GetActiveScene().buildIndex)
        {
            PlayerPrefs.SetInt("Lvl", SceneManager.GetActiveScene().buildIndex);
        }

        if (PlayerPrefs.HasKey("Coins"))
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + player.coins);
        }
        else
        {
            PlayerPrefs.SetInt("Coins", player.coins);
        }

        string gemsThisLvl = "Gems" + SceneManager.GetActiveScene().buildIndex.ToString();
        print("я тут в " + gemsThisLvl);
        print(player.gems);

        if (PlayerPrefs.HasKey(gemsThisLvl))
        {
            if (PlayerPrefs.GetInt(gemsThisLvl) > player.gems)
            {
                PlayerPrefs.SetInt(gemsThisLvl, player.gems);
                PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") + Math.Abs(player.gems - PlayerPrefs.GetInt(gemsThisLvl)));
            }
        }
        else
        {
            PlayerPrefs.SetInt(gemsThisLvl, player.gems);

            if (PlayerPrefs.HasKey("Gems"))
            {
                PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") + player.gems);

                print(PlayerPrefs.GetInt("Gems"));
                print(Math.Abs(player.gems - PlayerPrefs.GetInt(gemsThisLvl)));
                print(PlayerPrefs.GetInt("Gems") + Math.Abs(player.gems - PlayerPrefs.GetInt(gemsThisLvl)));
            }
            else
            {
                PlayerPrefs.SetInt("Gems", player.gems);
            }
        }
    }
}
