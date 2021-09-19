using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public Transform playerTransform;
    public bool lookingUp;

    int level;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerController>().IsGrounded() && !Input.GetKey(KeyCode.Space))
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                transform.position = new Vector3(0f, transform.position.y + 10f, -10f);
                lookingUp = true;
            }
            else if (Input.GetKeyUp(KeyCode.Tab) && !((int)playerTransform.position.y >= transform.position.y - 5f || (int)playerTransform.position.y >= transform.position.y + 5f))
            {
                transform.position = new Vector3(0f, transform.position.y - 10f, -10f);
                lookingUp = false;
            }
        }

    if((int) playerTransform.position.y % 5 == 0 && (int) playerTransform.position.y % 10 != 0 && !Input.GetKey(KeyCode.Tab))
    {
        transform.position = new Vector3(0f, (int)playerTransform.position.y - 5f, -10f);
    }
    if((int) playerTransform.position.y % 5 - 1 == 0 && (int) playerTransform.position.y % 10 - 1 != 0 && !Input.GetKey(KeyCode.Tab))
    {
        transform.position = new Vector3(0f, (int)playerTransform.position.y + 5f, -10f);
    }

        level = (int)playerTransform.position.y / 10;

        /*
        Vector3 temp = transform.position;

        temp.y = playerTransform.position.y;

        transform.position = temp;
        */
    }
}
