using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    [SerializeField] private GameObject[] levelPrefabs;
    [SerializeField] private Transform player;
    private GameObject currentLevel;
    private readonly Vector3 playerStartingPosition = new Vector3(0f, 2.5f, 0f);

    private int levelIndex = 0;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        LoadCurrentLevel();
    }
    private void DestroyCurrentLevel()
    {
        Destroy(currentLevel);
    }
    private void LoadCurrentLevel()
    {
        currentLevel = Instantiate(levelPrefabs[levelIndex]);
    }
    public void LoadNextLevel()
    {
        DestroyCurrentLevel();
        levelIndex++;
        LoadCurrentLevel();
        player.position = playerStartingPosition;
    }
    public void RetryLevel()
    {
        DestroyCurrentLevel();
        LoadCurrentLevel();
        player.position = playerStartingPosition;
    }
}
