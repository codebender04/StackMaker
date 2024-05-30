using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotSlide : MonoBehaviour
{
    [SerializeField] private GameObject yellowPrefab;
    [SerializeField] private GameObject whitePrefab;
    
    private Player player;
    private bool slided = false;
    public bool Slided => slided;

    private const string PLAYER = "Player";
    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER) && player.BrickList.Count >= 1)
        {
            yellowPrefab.SetActive(true);
            whitePrefab.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PLAYER) && player.BrickList.Count >= 1)
        {
            slided = true;
        }
    }
}
