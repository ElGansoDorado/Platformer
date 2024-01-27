using System;
using System.Collections;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    [SerializeField] private GameObject blueGem, greenGem;

    public int coins 
    {
        get => _coins; 
        private set
        {
            _coins = value;
            OnCoinsInfoEvent?.Invoke();
        }
    }
    public int gems {get; private set;} = 0;

    private Player player;
    private int _coins = 0;
    private int gemCount = 0;

    private bool key;
    private bool canTP = true;


    public event Action OnCoinsInfoEvent;


    private void Start()
    {
        player = GetComponent<Player>();
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
                Destroy(other.gameObject);
                key = true;
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
                Destroy(other.gameObject);
                player.RecountHp(1);
                break;

            case "BlueGem":
                gems++;
                Destroy(other.gameObject);
                StartCoroutine(NoHitBonus());
                break;

            case "GreenGem":
                gems++;
                Destroy(other.gameObject);
                StartCoroutine(SpeedBonus());
                break;

            default:
                break;
        }   
    }
}