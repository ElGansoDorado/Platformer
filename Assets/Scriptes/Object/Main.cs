using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Microsoft.Unity.VisualStudio.Editor;

public class Main : MonoBehaviour
{
    public Player player;
    public TMP_Text coinText;
    public Image[] hearts;
    public Sprite isLife, nonLife;

    public void Update()
    {
        coinText.text = player.coins.ToString();
    }

    public void Lose()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
