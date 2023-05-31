using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LineRenderer[] lineRenderers;
    public Transform[] linePositions;
    public Transform center;
    public Transform idlePosition;

    public Transform idleCamera;
    public Transform TankFireCamera;

    public Vector3 CurrentPosition;
    public float BottomBoundary;

    public float maxLength;
    public float TankPositionOffset;
    bool SetGameOver = false;

    public GameObject Tank;
    private Rigidbody TankRB;
    private Collider TankCollider;

    public int TankShots { get; set; } = 10;

    public float Tankforce;

    bool isMouseDown = false;

    // Camera variables
    private Camera mainCamera;
    private Vector3 cameraOffset;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderers[0].positionCount = 2;
        lineRenderers[1].positionCount = 2;

        // Set line position of the string
        lineRenderers[0].SetPosition(0, linePositions[0].position);
        lineRenderers[1].SetPosition(0, linePositions[1].position);

        // Initialize tank and camera
        CreateTank();
        mainCamera = Camera.main;
        cameraOffset = mainCamera.transform.position - Tank.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Call game over
        if (TankShots <= 0)
        {
            Invoke("GameOver", 10);
        }
        if (isMouseDown && TankShots > 0)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 0;

            CurrentPosition = Camera.main.ViewportToScreenPoint(mousePosition);
            CurrentPosition = center.position + Vector3.ClampMagnitude(CurrentPosition - center.position, maxLength);

            CurrentPosition = ClampBoundary(CurrentPosition);

            setLines(CurrentPosition);

            if (TankCollider)
            {
                TankCollider.enabled = true;
            }
        }
        else
        {
            ResetLine();
        }
    }

    public void GameOver()
    {
        SetGameOver = true;
        Debug.Log("Game Over");
    }

    public void CreateTank()
    {
        TankRB = Instantiate(Tank).GetComponent<Rigidbody>();
        TankCollider = TankRB.GetComponent<BoxCollider>();
        TankCollider.enabled = false;
        TankRB.isKinematic = true;
    }

    private void OnMouseDown()
    {
        isMouseDown = true;
    }

    public void OnMouseUp()
    {
        isMouseDown = false;
        shoot();
        TankShots -= 1;
    }

    void shoot()
    {
        ChangeCameraMobile();
        if (Tank)
        {
            TankRB.isKinematic = false;
            Vector3 Force = (CurrentPosition - center.position) * Tankforce * -1;

            TankRB.velocity = Force;

            TankRB = null;
            TankCollider = null;

            Invoke("CreateTank", 2);
            Invoke("ChangeCameraIdle", 5);
        }
    }

    public void ResetLine()
    {
        CurrentPosition = idlePosition.position;
        setLines(CurrentPosition);
    }

    public void setLines(Vector3 position)
    {
        lineRenderers[0].SetPosition(1, position);
        lineRenderers[1].SetPosition(1, position);

        if (TankRB)
        {
            Vector3 direction = position - center.position;
            TankRB.transform.position = position + direction.normalized * TankPositionOffset;
            TankRB.transform.right = -direction.normalized;
        }
    }

    public void ChangeCameraMobile()
    {
        mainCamera.transform.position = TankFireCamera.transform.position;
        mainCamera.transform.rotation = TankFireCamera.transform.rotation;
        Debug.Log("CameraMobile");
    }

    public void ChangeCameraIdle()
    {
        mainCamera.transform.position = idleCamera.transform.position;
        mainCamera.transform.rotation = idleCamera.transform.rotation;
        Debug.Log("CameraIdle");
    }

    Vector3 ClampBoundary(Vector3 Position)
    {
        Position.y = Mathf.Clamp(Position.y, BottomBoundary, 4000);
        return Position;
    }
}
