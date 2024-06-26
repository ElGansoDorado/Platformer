using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Main : MonoBehaviour
{

    [SerializeField] private PlayerInventory inventory;
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
    [SerializeField] private SoundEffector soundEffector;
    [SerializeField] private AudioSource MusicSource, SoundSource;

    private GamePreferences gp;
    private float timer;


    private void Start()
    {
        gp = new GamePreferences();

        inventory.OnCoinsInfoEvent += OnCoinsInfo;
        player.OnHeartInfoEvent += OnHeartInfo;

        MusicSource.volume = (float)gp.Music / 9;
        SoundSource.volume = (float)gp.Audio / 9;

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
        SaveInventory();
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
        soundEffector.PlayWinSound();

        Status(false);
        winPanel.SetActive(true);
        
        SaveData();
    }

    public void Lose()
    {
        soundEffector.PlayLoseSound();

        SaveInventory();
        Status(false);
        losePanel.SetActive(true);
    }

    public void MenuLevel()
    {
        SaveInventory();
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
        gp.Coins = inventory.Coins;
        gp.Gems = inventory.Gems;

        SaveInventory();
    }

    private void OnCoinsInfo()
    {
        coinText.text = inventory.Coins.ToString();
    }

    private void OnHeartInfo()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (player.CurHp > i)
            {
                hearts[i].sprite = isLife;
            }
            else
            {
                hearts[i].sprite = nonLife;
            }
        }
    }

    private void SaveInventory()
    {
        if (inventory.Hp < gp.Hearts)
        {
            gp.Hearts = inventory.Hp;
        }

        if (inventory.Bg < gp.BlueGems)
        {
            gp.BlueGems = inventory.Bg;
        }

        if (inventory.Gg < gp.GreenGems)
        {
            gp.GreenGems = inventory.Gg;
        }
    }
}

public enum TimeWork
{
    None,
    Stopwatch,
    Timer
}