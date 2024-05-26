using UnityEngine;

public class PivotBrick : MonoBehaviour
{
    private const string PLAYER = "Player";
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER))
        {
            gameObject.SetActive(false);
        }
    }
}
