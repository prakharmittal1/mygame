using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hostage : MonoBehaviour
{
    public GameObject winScreen;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnGUI()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("ChargedShot") || other.gameObject.tag.Equals("Bullet"))
        {
            Destroy(gameObject);
            UnityEngine.SceneManagement.SceneManager.LoadScene(3);
        }
        else if (other.gameObject.tag.Equals("play"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
