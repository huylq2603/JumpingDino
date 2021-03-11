using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Rigidbody2D player;

    public float offSetY;

    // Update is called once per frame
    void Update()
    {
        if (!GameController.isEndingLevel) {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + offSetY, transform.position.z);
        }
    }
}
