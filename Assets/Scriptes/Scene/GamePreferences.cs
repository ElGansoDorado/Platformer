using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class GamePreferences : MonoBehaviour
{
    
    private const string LVL = "COMPLETED_LEVELS";
    private const string COINS = "COINS";
    private const string GEMS = "GEMS";


    public int Levels
    {
        get => PlayerPrefs.HasKey(LVL) ? PlayerPrefs.GetInt(LVL) : 0;
        set
        {
            if (!IsLevel() || Levels < value)
            {
                PlayerPrefs.SetInt(LVL, value);
            }
        }
    }

    public int Coins
    {
        get => PlayerPrefs.HasKey(COINS) ? PlayerPrefs.GetInt(COINS) : 0;
        set
        {
            if (IsCoins())
            {
                PlayerPrefs.SetInt(COINS, Coins + value);
            }
            else
            {
                PlayerPrefs.SetInt(COINS, value);
            }
        }
    }

    public int Gems
    {
        get => PlayerPrefs.HasKey(GEMS) ? PlayerPrefs.GetInt(GEMS) : 0;
        set
        {
            string gemsThisLvl = GEMS + SceneManager.GetActiveScene().buildIndex.ToString();

            if (PlayerPrefs.HasKey(gemsThisLvl))
            {
                if (PlayerPrefs.GetInt(gemsThisLvl) > value)
                {
                    PlayerPrefs.SetInt(gemsThisLvl, value);
                    PlayerPrefs.SetInt(GEMS, Gems + Math.Abs(value - PlayerPrefs.GetInt(gemsThisLvl)));
                }
            }
            else
            {
                PlayerPrefs.SetInt(gemsThisLvl, value);

                if (IsGems())
                {
                    PlayerPrefs.SetInt(GEMS, Gems + value);
                }
                else
                {
                    PlayerPrefs.SetInt(GEMS, value);
                }
            }
        }
    }


    public bool IsLevel() => PlayerPrefs.HasKey(LVL);

    public bool IsCoins() => PlayerPrefs.HasKey(COINS);

    public bool IsGems() => PlayerPrefs.HasKey(GEMS);
    
}
