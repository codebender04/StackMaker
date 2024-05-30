using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private GameObject brickPrefab;
    
    public GameObject playerVisual;
    private List<PlayerBrick> brickList = new();
    public List<PlayerBrick> BrickList { get { return brickList; } }

    private enum PushPivotDirection { Forward, Right }
    private PushPivotDirection pushPivotDirection;
    private Vector2 startMousePos;
    private Vector2 endMousePos;
    private bool reachedTarget = true;
    private IEnumerator moveCoroutine;
    
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
                        moveCoroutine = Move(Vector3.right);
                    }
                    if (startMousePos.x < endMousePos.x)
                    {
                        moveCoroutine = Move(Vector3.left);
                    }
                }
                else
                {
                    if (startMousePos.y > endMousePos.y)
                    {
                        moveCoroutine = Move(Vector3.forward);
                    }
                    if (startMousePos.y < endMousePos.y)
                    {
                        moveCoroutine = Move(Vector3.back);
                    }
                }
                StartCoroutine(moveCoroutine);
            }
        }
    }
    private IEnumerator Move(Vector3 direction)
    {
        reachedTarget = false;
        if (Physics.Raycast(transform.position, direction, out RaycastHit hitInfo, 100f, wallLayer)) //If hit a wall
        {
            CheckPushPivotDirection(hitInfo);
            Vector3 offset = - direction;
            
            while (Vector3.Distance(transform.position, hitInfo.transform.position + offset) > 0.1f) //Moving to the target
            {
                transform.position = Vector3.MoveTowards(transform.position, hitInfo.transform.position + offset, speed * Time.deltaTime);
                yield return null;            
            }
            transform.position = hitInfo.transform.position + offset;
        }
        reachedTarget = true;
    }
    private void CheckPushPivotDirection(RaycastHit hitInfo)
    {
        if (hitInfo.collider.CompareTag("WallForward"))
        {
            pushPivotDirection = PushPivotDirection.Forward;
        }
        if (hitInfo.collider.CompareTag("WallRight"))
        {
            pushPivotDirection = PushPivotDirection.Right;
        }
    }
    private void AddBrick()
    {
        PlayerBrick playerBrick = Instantiate(brickPrefab, transform.position, Quaternion.Euler(-90, 0, 180)).GetComponent<PlayerBrick>();
        playerBrick.OnInit(this);
        UIManager.Instance.SetScore(brickList.Count);

    }
    private void RemoveBrick()
    {    
        brickList[^1].OnDespawn(this);
        UIManager.Instance.SetScore(brickList.Count);
    }
    public void ClearBrick()
    {
        foreach (PlayerBrick brick in brickList.ToList())
        {
            brick.OnDespawn(this);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PivotBrick>() != null)
        {
            AddBrick();
        }
        if (other.TryGetComponent<PivotSlide>(out PivotSlide pivotSlide))
        {
            if (brickList.Count > 0 && !pivotSlide.Slided)
            {
                RemoveBrick();
                UIManager.Instance.SetScore(brickList.Count);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Pivot Push") && reachedTarget)
        {
            if (pushPivotDirection == PushPivotDirection.Forward)
            {
                StartCoroutine(Move(-other.transform.forward));
            }
            else if (pushPivotDirection == PushPivotDirection.Right)
            {
                StartCoroutine(Move(-other.transform.right));
            }
        }
    }
}
