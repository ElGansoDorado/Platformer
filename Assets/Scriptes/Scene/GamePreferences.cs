using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class GamePreferences : MonoBehaviour
{
    
    private const string LVL = "COMPLETED_LEVELS";
    private const string COINS = "COINS";
    private const string GEMS = "GEMS";
    private const string HEART = "CONSUMABLE_HEART";
    private const string BLUE_GEM = "CONSUMABLE_BLUE_GEM";
    private const string GREEN_GEM = "CONSUMABLE_GREEN_GEM";


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

    public int Hearts
    {
        get => PlayerPrefs.HasKey(HEART) ? PlayerPrefs.GetInt(HEART) : 0;
        set => PlayerPrefs.SetInt(HEART, value);
    }

    public int BlueGems
    {
        get => PlayerPrefs.HasKey(BLUE_GEM) ? PlayerPrefs.GetInt(BLUE_GEM) : 0;
        set => PlayerPrefs.SetInt(BLUE_GEM, value);
    }

    public int GreenGems
    {
        get => PlayerPrefs.HasKey(GREEN_GEM) ? PlayerPrefs.GetInt(GREEN_GEM) : 0;
        set => PlayerPrefs.SetInt(GREEN_GEM, value);
    }

    public bool IsLevel() => PlayerPrefs.HasKey(LVL);

    public bool IsCoins() => PlayerPrefs.HasKey(COINS);

    public bool IsGems() => PlayerPrefs.HasKey(GEMS);

    public bool IsHeart() => PlayerPrefs.HasKey(HEART);

    public bool IsBlueGem() => PlayerPrefs.HasKey(BLUE_GEM);

    public bool IsGreenGem() => PlayerPrefs.HasKey(GREEN_GEM);
    
}
