using UnityEngine;
using UnityEngine.UI;

public class ObstacleWithTimer : MonoBehaviour
{
    
    [SerializeField] private Image countdown;
    [SerializeField] private float timer;

    private float hitTimer = 0;


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            hitTimer += Time.deltaTime;

            if (hitTimer >= timer)
            {
                hitTimer = 0f;
                countdown.fillAmount = 1f;
                other.gameObject.GetComponent<Player>().RecountHp(-1);
            }
            else
            {
                countdown.fillAmount = 1 - (hitTimer / timer);
            }
        }    
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            hitTimer = 0f;
            countdown.fillAmount = 0f;
        }
    }
}
