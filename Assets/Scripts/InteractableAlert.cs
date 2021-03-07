using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableAlert : MonoBehaviour
{
    public GameObject FloatingTextPrefab;
    public float offsetY;
    private GameObject textInstance;

    void Start()
    {

    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        gameObject.transform.localScale = new Vector2(1.2f, 1.2f);
        gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + offsetY);
        textInstance = Instantiate(FloatingTextPrefab, new Vector2(transform.position.x, transform.position.y + 3), Quaternion.identity, transform);
    }

    private void OnTriggerExit2D(Collider2D other) {
        gameObject.transform.localScale = new Vector2(1, 1);
        gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - offsetY);
        DestroyGuideText();
    }

    public void DestroyGuideText(){
        Destroy(textInstance);
    }
}
