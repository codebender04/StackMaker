using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private GameObject nextLevelUI;
    [SerializeField] private TextMeshProUGUI levelScore;
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
    public void SetScore(int brick)
    {
        score.text = brick.ToString();
    }
    public void LoadCompleteLevelUI()
    {
        levelScore.text = "Score: " + score.text;
        nextLevelUI.SetActive(true);
    }
    public void UnloadCompleteLevelUI()
    {
        nextLevelUI.SetActive(false);
    }
}
