using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameController : MonoBehaviour
{
    // Static singleton property.
    public static GameController Instance { get; private set; }

    //static
    public static int carrotCount;
    public static int currentLevel;
    public static bool isInputEnable;
    public static bool isEndingLevel;
    public enum InteractableType {DOOR, LEVER};

    //public from editor
    public GameObject tilesmapA;
    public GameObject tilesmapB;
    
    public Camera cam;
    public Transform camTransform;

    //list of all scenes
    List<string> scenesInBuild;

    public AudioClip endSound;
    static AudioSource audioSrc;

    void Awake()
    {
        // Save a reference to the AudioManager component as our //singleton instance.
        Instance = this;
    }

    void Start() 
    {
        isInputEnable = true;
        isEndingLevel = false;
        scenesInBuild = new List<string>();
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            int lastSlash = scenePath.LastIndexOf("/");
            scenesInBuild.Add(scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1));
        }
        audioSrc = GetComponent<AudioSource>();
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        GameController.isInputEnable = true;
        currentLevel = Int32.Parse(scene.name.Substring(5));
        InitCarrotCount();
        SetTilemapsActive(true, false);
    }

    // Instance method, this method can be accessed through the //singleton instance
    public void InitCarrotCount()
    {
        switch (currentLevel)
        {
            case 1:
                carrotCount = 1;
                break;
            case 2:
                carrotCount = 3;
                break;
            case 3:
                carrotCount = 16;
                break;
            case 4:
                carrotCount = 12;
                break;
            default:
                carrotCount = 1;
                break;
        }
    }

    public void decreaseCarrot() {
        carrotCount -= 1;
    }

    public void LoadNextScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        int level = Int32.Parse(scene.name.Substring(5));
        if(scenesInBuild.Contains("Level" + (level + 1))) {
            SceneManager.LoadScene("Level" + (level + 1));
        } else {
            Debug.Log("load end game");
            // PlaySound("endSound");
        }   
    }
    
    public void SetTilemapsActive(bool isAActive, bool isBActive) {
        tilesmapA.SetActive(isAActive);
        tilesmapB.SetActive(isBActive);
    }

    public IEnumerator ZoomOutCamera(float time)
    {
        Vector3 oldScale, newScale, newPosition;
        float oldSize, newSize;
        switch (currentLevel)
        {
            case 1:
                oldSize = 8;
                newSize = 16;
                oldScale = new Vector3(1, 1, 1);
                newScale =  new Vector3(2, 2, 1);
                newPosition = new Vector3(0, 0, -10);
                break;
            case 2:
                oldSize = 8;
                newSize = 16;
                oldScale = new Vector3(1, 1, 1);
                newScale =  new Vector3(2, 2, 1);
                newPosition = new Vector3(0, 0, -10);
                break;
            case 3:
                oldSize = 8;
                newSize = 24;
                oldScale = new Vector3(1, 1, 1);
                newScale =  new Vector3(3, 3, 1);
                newPosition = new Vector3(0, 10, -10);
                break;
            case 4:
                oldSize = 8;
                newSize = 40;
                oldScale = new Vector3(1, 1, 1);
                newScale =  new Vector3(5, 5, 1);
                newPosition = new Vector3(0, 5, -10);
                break;
            default:
                oldSize = 8;
                newSize = 16;
                oldScale = new Vector3(1, 1, 1);
                newScale =  new Vector3(2, 2, 1);
                newPosition = new Vector3(0, 0, -10);
                break;
        }
        float elapsed = 0;
        Vector3 oldPosition = camTransform.position;
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

    public void PlaySound(String clip){
        switch(clip) {
            case "endSound":
                audioSrc.PlayOneShot(endSound);
                break;
            default:
                break;
        }
    }
}
