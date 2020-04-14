using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("ChargedShot") || other.gameObject.tag.Equals("Bullet") || other.gameObject.tag.Equals("Enemy"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        }
    }

    void Update()
    {
        
    }
}
