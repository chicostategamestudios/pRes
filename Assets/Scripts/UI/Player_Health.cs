using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Health : MonoBehaviour {

    [SerializeField]
    private HealthStat Health;

    private void Awake()
    {
        Health.Initialize();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown("q"))
        {
            Health.CurrentVal -= 10;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
           //Health.CurrentVal -= collision.gameObject.GetComponent
        }
    }
}
