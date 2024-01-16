using UnityEngine;

public class Reload : MonoBehaviour
{

    [SerializeField] Main main;


    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            main.ReloadLevel();
        }
    }
}
