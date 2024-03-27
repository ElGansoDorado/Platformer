using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInventory : MonoBehaviour
{

    [SerializeField] private GameObject blueGem, greenGem;

    [SerializeField] private TMP_Text[] itemText;
    [SerializeField] private Image[] itemImage;
    [SerializeField] private Sprite is_hp, no_hp, is_bg, no_bg, is_gg, no_gg, is_key, no_key;


    public int coins 
    {
        get => _coins; 
        private set
        {
            _coins = value;
            OnCoinsInfoEvent?.Invoke();
        }
    }
    public int hp = 0, bg = 0, gg = 0;
    public int gems {get; private set;} = 0;

    private Player player;
    private GamePreferences gp;
    private int _coins = 0;
    private int gemCount = 0;

    private bool key;
    private bool canTP = true;


    public event Action OnCoinsInfoEvent;


    private void Start()
    {
        player = GetComponent<Player>();
        gp = new GamePreferences();

        hp = gp.Hearts;
        bg = gp.BlueGems;
        gg = gp.GreenGems;

        if (gp.Hearts > 0)
        {
            hp = gp.Hearts;
            itemImage[0].sprite = is_hp;
            itemText[0].text = hp.ToString();
        }

        if (gp.BlueGems > 0)
        {
            bg = gp.BlueGems;
            itemImage[1].sprite = is_bg;
            itemText[1].text = bg.ToString();
        }

        if (gp.GreenGems > 0)
        {
            gg = gp.GreenGems;
            itemImage[2].sprite = is_gg;
            itemText[2].text = gg.ToString();
        }
    }


    private void AddHp()
    {
        hp++;
        itemImage[0].sprite = is_hp;
        itemText[0].text = hp.ToString();
    }

    private void AddBg()
    {
        bg++;
        gems++;
        itemImage[1].sprite = is_bg;
        itemText[1].text = bg.ToString();
    }

    private void AddGg()
    {
        gg++;
        gems++;
        itemImage[2].sprite = is_gg;
        itemText[2].text = gg.ToString();
    }

    private void AddKey()
    {
        itemImage[3].sprite = is_key;
        key = true;
    }

    public void UseHp()
    {
        if (hp > 0)
        {
            hp--;
            player.RecountHp(1);
            itemText[0].text = hp.ToString();

            if (hp == 0)
            {
                itemImage[0].sprite = no_hp;
            }
        }
    }

    public void UseBg()
    {
        if (bg > 0)
        {
            bg--;
            StartCoroutine(NoHitBonus());
            itemText[1].text = bg.ToString();

            if (bg == 0)
            {
                itemImage[1].sprite = no_bg;
            }
        }
    }

    public void UseGg()
    {
        if (gg > 0)
        {
            gg--;
            StartCoroutine(SpeedBonus());
            itemText[2].text = gg.ToString();

            if (gg == 0)
            {
                itemImage[2].sprite = no_gg;
            }
        }
    }


    private void CheckGems(GameObject obj)
    {
        if (gemCount == 1)
        {
            obj.transform.localPosition = new Vector3(0f, 0.45f, obj.transform.localPosition.z);
        }
        else if (gemCount == 2)
        {
            blueGem.transform.localPosition = new Vector3(-0.3f, 0.4f, blueGem.transform.localPosition.z);
            greenGem.transform.localPosition = new Vector3(0.3f, 0.4f, greenGem.transform.localPosition.z);
        }
    }


    private IEnumerator TPWait()
    {
        yield return new WaitForSeconds(1f);
        canTP = true;
    }

    private IEnumerator NoHitBonus()
    {
        gemCount++;
        blueGem.SetActive(true);
        CheckGems(blueGem);
        player.canHit = false;

        blueGem.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(5f);
        StartCoroutine(Invis(blueGem.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);

        player.canHit = true;
        CheckGems(blueGem);
        blueGem.SetActive(false);
        gemCount--;
    }

    private IEnumerator SpeedBonus()
    {
        gemCount++;
        greenGem.SetActive(true);
        player.Speed *= 2;
        CheckGems(greenGem);
        
        greenGem.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(5f);
        StartCoroutine(Invis(greenGem.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);
        
        CheckGems(greenGem);
        player.Speed /= 2;
        greenGem.SetActive(false);
        gemCount--;
    }

    private IEnumerator Invis(SpriteRenderer spr, float time)
    {
        spr.color = new Color(1f, 1f, 1f, spr.color.a - time * 2);
        yield return new WaitForSeconds(time);

        if (spr.color.a > 0)
        {
            StartCoroutine(Invis(spr, time));
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Key":
                AddKey();
                Destroy(other.gameObject);
                break;

            case "Door":
                if (other.gameObject.GetComponent<Door>().isOpen && canTP)
                {
                    other.gameObject.GetComponent<Door>().Teleport(gameObject);
                    canTP = false;
                    StartCoroutine(TPWait());
                }
                else if (key)
                {
                    other.gameObject.GetComponent<Door>().Unlock();
                }
                break;

            case "Coin":
                Destroy(other.gameObject);
                coins++;
                break;

            case "Heart":
                AddHp();
                Destroy(other.gameObject);
                break;

            case "BlueGem":
                AddBg();
                Destroy(other.gameObject);
                break;

            case "GreenGem":
                AddGg();
                Destroy(other.gameObject);
                break;

            default:
                break;
        }   
    }
}