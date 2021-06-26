using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed = 5;
    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Map")
        {
            Destroy(gameObject);
        }
        else if(other.gameObject.CompareTag("Player") && gameObject.CompareTag("Lazer"))
        {
            ThirdPersonController controller = other.gameObject.GetComponent<ThirdPersonController>();
            controller.playerHealth--;
            controller.UpdateHealth();
            Destroy(gameObject);
        }
        else if(other.gameObject.CompareTag("Enemy") && gameObject.CompareTag("PlayerLazer"))
        {
            other.gameObject.GetComponent<EnemyActions>().enemyHealth--;
            Destroy(gameObject);
        }
    }
}
