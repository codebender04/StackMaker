using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private Player player;
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
    }
    public void CompleteLevel()
    {
        UIManager.Instance.LoadCompleteLevelUI();
        UIManager.Instance.SetScore(0);
    }
    public void NextLevel()
    {
        player.ClearBrick();
        LevelManager.Instance.LoadNextLevel();
        UIManager.Instance.UnloadCompleteLevelUI();
    }
    public void RetryLevel()
    {
        player.ClearBrick();
        LevelManager.Instance.RetryLevel();
        UIManager.Instance.UnloadCompleteLevelUI();
    }
    
}
