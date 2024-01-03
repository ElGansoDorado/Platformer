using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{

    [SerializeField] TMP_Text coinsText; 
    [SerializeField] TMP_Text gemsText; 

    public Button[] Lvls;


    private void Start()
    {
        if (PlayerPrefs.HasKey("Lvl"))
        {
            for (int i = 0; i < Lvls.Length; i++)
            {
                if (i <= PlayerPrefs.GetInt("Lvl"))
                {
                    Lvls[i].interactable = true;
                }
                else
                {
                    Lvls[i].interactable = false;
                }
            }
        }
    }


    public void OpenScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void DeleteKeys()
    {
        PlayerPrefs.DeleteAll();
    }


    private void Update() 
    {
        if (PlayerPrefs.HasKey("Coins"))
        {
            coinsText.text = PlayerPrefs.GetInt("Coins").ToString();
        }

        if (PlayerPrefs.HasKey("Gems"))
        {
            gemsText.text = PlayerPrefs.GetInt("Gems").ToString();
        }
    }
}
