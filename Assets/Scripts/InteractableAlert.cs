using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableAlert : MonoBehaviour
{
    public GameObject FloatingTextPrefab;
    public float offsetY;
    private GameObject textInstance;
    public bool canInteract;
    private bool isInteracted;
    public bool isNoCarrotRelated;
    public int interactableType;

    void Start()
    {
        canInteract = false;
        isInteracted = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(isInteracted) {
            return;
        }
        if(isNoCarrotRelated && GameController.carrotCount != 0){
            return;
        }
        if((GameController.currentLevel == 3 && GameController.carrotCount > 8) || (GameController.currentLevel == 4 && GameController.carrotCount > 6)){
            return;
        }
        gameObject.transform.localScale = new Vector2(1.2f, 1.2f);
        gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + offsetY);
        textInstance = Instantiate(FloatingTextPrefab, new Vector2(transform.position.x, transform.position.y + 3), Quaternion.identity, transform);
        canInteract = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(isInteracted) {
            return;
        }
        if(isNoCarrotRelated && GameController.carrotCount != 0){
            return;
        }
        if((GameController.currentLevel == 3 && GameController.carrotCount > 8) || (GameController.currentLevel == 4 && GameController.carrotCount > 6)){
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
        isInteracted = true;
        switch(interactableType){
            case (int) GameController.InteractableType.DOOR:
                Debug.Log("door");
                GameController.isInputEnable = false;
                
                GameController.isEndingLevel = true;
                yield return new WaitForSeconds(1.5f);
                GameController.Instance.SetTilemapsActive(true, true);
                StartCoroutine(GameController.Instance.ZoomOutCamera(1));
                yield return new WaitForSeconds(5);
                GameController.Instance.LoadNextScene();
                break;
            case (int) GameController.InteractableType.LEVER:
                Debug.Log("lever");
                yield return new WaitForSeconds(1);
                GameController.Instance.SetTilemapsActive(false, true);
                break;
            default:
                break;
        }
    }
}
