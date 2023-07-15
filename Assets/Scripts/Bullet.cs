using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class Bullet : MonoBehaviour
{

    public Teams team;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
