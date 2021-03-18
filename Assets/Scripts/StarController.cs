using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour
{
    public Vector3 pos1Offset;
    public Vector3 pos2Offset;
    public bool isHorizontal;
    public float speed;

    private Vector3 pos1;
    private Vector3 pos2;
    private bool isTurnback;
    // Start is called before the first frame update
    void Start()
    {
        pos1 = transform.position + pos1Offset;
        pos2 = transform.position + pos2Offset;
        isTurnback = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHorizontal) {
            if(transform.position.x >= pos1.x) {
                isTurnback = true;
            }
            if(transform.position.x <= pos2.x) {
                isTurnback = false;
            }
        } else {
            if(transform.position.y >= pos1.y) {
                isTurnback = true;
            }
            if(transform.position.y <= pos2.y) {
                isTurnback = false;
            }
        }
        if (isTurnback) {
            transform.position = Vector3.MoveTowards(transform.position, pos2, speed * Time.deltaTime);
        } else {
            transform.position = Vector3.MoveTowards(transform.position, pos1, speed * Time.deltaTime);
        }
    }
}
