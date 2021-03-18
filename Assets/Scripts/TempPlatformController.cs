using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlatformController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        switch (other.gameObject.tag) {
            case "Player":
                StartCoroutine(DestroyPlatform());
                break;
        }
    }

    public IEnumerator DestroyPlatform() {
        yield return new WaitForSeconds(1.5f);
        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(5);
        gameObject.GetComponent<Renderer>().enabled = true;
        gameObject.GetComponent<Collider2D>().enabled = true;

    }
}
