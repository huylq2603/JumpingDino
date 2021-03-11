using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableAlert : MonoBehaviour
{
    public GameObject FloatingTextPrefab;
    public float offsetY;
    private GameObject textInstance;
    public bool canInteract;
    public bool isNoCarrotRelated;
    public int interactableType;

    public Camera cam;
    public Transform camTransform;

    void Start()
    {
        canInteract = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(isNoCarrotRelated && GameController.carrotCount != 0){
            return;
        }
        gameObject.transform.localScale = new Vector2(1.2f, 1.2f);
        gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + offsetY);
        textInstance = Instantiate(FloatingTextPrefab, new Vector2(transform.position.x, transform.position.y + 3), Quaternion.identity, transform);
        canInteract = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(isNoCarrotRelated && GameController.carrotCount != 0){
            return;
        }
        gameObject.transform.localScale = new Vector2(1, 1);
        gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - offsetY);
        canInteract = false;
        DestroyGuideText();
    }

    public void DestroyGuideText(){
        Destroy(textInstance);
    }

    public IEnumerator DoInteraction(){
        switch(interactableType){
            case (int) GameController.InteractableType.DOOR:
                Debug.Log("door");
                GameController.isInputEnable = false;
                Vector3 camPosition = camTransform.position;
                GameController.isEndingLevel = true;
                yield return new WaitForSeconds(1.5f);

                StartCoroutine(ZoomOutCamera(8, 16, new Vector3(1, 1, 1), new Vector3(2, 2, 1), camPosition, new Vector3(0, 0, -10), 1));

                yield return new WaitForSeconds(5);

                GameController.Instance.LoadNextScene();
                break;
            case (int) GameController.InteractableType.LEVER:
                Debug.Log("lever");
                break;
            default:
                break;
        }
    }

    private IEnumerator ZoomOutCamera(float oldSize, float newSize, Vector3 oldScale, Vector3 newScale, Vector3 oldPosition, Vector3 newPosition, float time)
    {
        float elapsed = 0;
        while (elapsed <= time)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / time);
    
            cam.orthographicSize = Mathf.Lerp(oldSize, newSize, t);
            camTransform.localScale = Vector3.Lerp(oldScale, newScale, t);
            camTransform.position = Vector3.Lerp(oldPosition, newPosition, t);
            yield return null;
        }
    }
}
