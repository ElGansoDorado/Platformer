using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

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
    [SerializeField] private TimeWork timeWork;
    [SerializeField] private float countdown;

    private GamePreferences gp;
    private float timer;


    private void Start()
    {
        gp = new GamePreferences();

        player.OnCoinsInfoEvent += OnCoinsInfo;
        player.OnHeartInfoEvent += OnHeartInfo;

        if (timeWork == TimeWork.Timer)
        {
            timer = countdown;
        }

        if (timeWork == TimeWork.None)
        {
            timeText.gameObject.SetActive(false);
        }

        Status(true);
    }

    private void Update()
    {
        if (timeWork == TimeWork.Stopwatch)
        {
            timer += Time.deltaTime;
            timeText.text = ((int)timer / 60).ToString() + ":" + ((int)timer % 60).ToString("D2");
        }
        else if (timeWork == TimeWork.Timer)
        {
            timer -= Time.deltaTime;
            timeText.text = ((int)timer / 60).ToString() + ":" + ((int)timer % 60).ToString("D2");

            if (timer <= 0f)
            {
                Lose();
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
        gp.Levels = SceneManager.GetActiveScene().buildIndex;
        gp.Coins = player.coins;
        gp.Gems = player.gems;
    }

    private void OnCoinsInfo()
    {
        coinText.text = player.coins.ToString();
    }

    private void OnHeartInfo()
    {
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
}

public enum TimeWork
{
    None,
    Stopwatch,
    Timer
}