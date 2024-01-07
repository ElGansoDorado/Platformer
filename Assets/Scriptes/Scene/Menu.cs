using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{

    [SerializeField] TMP_Text coinsText; 
    [SerializeField] TMP_Text gemsText; 

    public Button[] Lvls;

    private GamePreferences gp;


    private void Start()
    {
        gp = new GamePreferences();

        if (gp.IsLevel())
        {
            for (int i = 0; i < Lvls.Length; i++)
            {
                if (i <= gp.Levels)
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

    private void Update() 
    {
        coinsText.text = gp.Coins.ToString();
        gemsText.text = gp.Gems.ToString();
    }


    public void OpenScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void DeleteKeys()
    {
        PlayerPrefs.DeleteAll();
    }
}
