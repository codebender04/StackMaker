using UnityEngine;

public class PlayerBrick : MonoBehaviour
{
    public void OnInit(Transform playerVisual)
    {
        playerVisual.position += new Vector3(0f, 0.5f, 0f);
    }
}
