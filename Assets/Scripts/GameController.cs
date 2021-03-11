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

    //main objective
    public static int carrotCount;

    public static bool isInputEnable;
    public static bool isEndingLevel;

    public enum InteractableType {DOOR, LEVER};

    //list of all scenes
    List<string> scenesInBuild;

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
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        GameController.isInputEnable = true;
        InitCarrotCount(Int32.Parse(scene.name.Substring(5)));
    }

    // Instance method, this method can be accessed through the //singleton instance
    public void InitCarrotCount(int level)
    {
        switch (level)
        {
            case 1:
                carrotCount = 1;
                break;
            case 2:
                carrotCount = 2;
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
        }
        
    }
}
