using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed;
    public GameObject ammo;
    public GameObject ammoSpawn;
    float moveX;

    public float fireMaxCounter;
    public float fireCounter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Horizontal") != 0)
        {
            if (gameObject.transform.position.x > 4 && Input.GetAxis("Horizontal") >= 0)
            {
                moveX = 0;
            }
            else if (gameObject.transform.position.x < -4 && Input.GetAxis("Horizontal") <= 0)
            {
                moveX = 0;
            }
            else
            {
                moveX = Input.GetAxis("Horizontal");
                transform.Translate(moveX * moveSpeed * Time.deltaTime, 0, 0);
            }
        }

        if(fireCounter < fireMaxCounter)
        {
            fireCounter += Time.deltaTime;
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                Instantiate(ammo, ammoSpawn.transform.position, Quaternion.identity);
                fireCounter = 0;   
            }
        }


    }
}
