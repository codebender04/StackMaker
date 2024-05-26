using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private GameObject playerVisual;

    private Vector2 startMousePos;
    private Vector2 endMousePos;
    private bool reachedTarget = true;
    private List<PlayerBrick> brickList = new List<PlayerBrick>();

    private void Update()
    {
        if (reachedTarget)
        {
            HandleMovement();
        }
    }
    private void HandleMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            endMousePos = Input.mousePosition;
            float swipeLength = (startMousePos - endMousePos).magnitude;

            if (swipeLength > 100f)
            {
                if (Mathf.Abs(startMousePos.x - endMousePos.x) > Mathf.Abs(startMousePos.y - endMousePos.y))
                {
                    if (startMousePos.x > endMousePos.x)
                    {
                        StartCoroutine(Move(Vector3.right, Vector3.left));
                    }
                    if (startMousePos.x < endMousePos.x)
                    {
                        StartCoroutine(Move(Vector3.left, Vector3.right));
                    }
                }
                else
                {
                    if (startMousePos.y > endMousePos.y)
                    {
                        StartCoroutine(Move(Vector3.forward, Vector3.back));
                    }
                    if (startMousePos.y < endMousePos.y)
                    {
                        StartCoroutine(Move(Vector3.back, Vector3.forward));
                    }
                }
            }
        }
    }
    private IEnumerator Move(Vector3 direction, Vector3 offset)
    {
        reachedTarget = false;
        if (Physics.Raycast(transform.position, direction, out RaycastHit hitInfo, 100f, wallLayer))
        {
            while (Vector3.Distance(transform.position, hitInfo.transform.position + offset) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, hitInfo.transform.position + offset, speed * Time.deltaTime);
                yield return new WaitForSeconds(Time.deltaTime);            
            }
        }
        reachedTarget = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PivotBrick>() != null)
        {
            AddBrick();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (reachedTarget && other.CompareTag("Pivot Push"))
        {
            StartCoroutine(Move(-other.transform.forward, other.transform.forward));
        }
    }
    private void AddBrick()
    {
        Vector3 offset = new(0f, 0.5f * brickList.Count, 0f);
        PlayerBrick playerBrick = Instantiate(brickPrefab, transform.position + offset, Quaternion.Euler(-90, 0, 180)).GetComponent<PlayerBrick>();
        playerBrick.OnInit(playerVisual.transform);
        brickList.Add(playerBrick);
        playerBrick.transform.SetParent(transform);
    }
    private void RemoveBrick()
    {
        brickList.RemoveAt(brickList.Count - 1);
    }
    private void ClearBrick()
    {

    }
}
