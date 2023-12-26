using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GameWinTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            GameManager.Instance.OnWin();
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
