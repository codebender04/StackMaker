using UnityEngine;

public class PlayerBrick : MonoBehaviour
{
    private const float BRICK_HEIGHT = 0.5f;
    public void OnInit(Player player)
    {
        Vector3 offset = new(0f, 0.5f * player.BrickList.Count, 0f);
        transform.position += offset;
        player.playerVisual.transform.position += new Vector3(0f, BRICK_HEIGHT, 0f);
        transform.SetParent(player.transform);
        player.BrickList.Add(this);
    }
    public void OnDespawn(Player player)
    {
        player.playerVisual.transform.position += new Vector3(0f, -BRICK_HEIGHT, 0f);
        player.BrickList.RemoveAt(player.BrickList.Count - 1);
        Destroy(gameObject);
    }
}
