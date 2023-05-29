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

	public Vector3 CurrentPosition;
	public float BottomBoundary;

	public float maxLength;
	public float TankPositionOffset;

	public GameObject Tank;
	private Rigidbody TankRB;
	private Collider TankCollider;

	public float Tankforce;

	bool isMouseDown = false;

	// Start is called before the first frame update
	void Start()
	{
		lineRenderers[0].positionCount = 2;
		lineRenderers[1].positionCount = 2;

		//set line position of the string
		lineRenderers[0].SetPosition(0, linePositions[0].position);
		lineRenderers[1].SetPosition(0, linePositions[1].position);

		CreateTank();
	}

	// Update is called once per frame
	void Update()
	{
		if (isMouseDown)
		{
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.z = 0;

			CurrentPosition = Camera.main.ScreenToWorldPoint(mousePosition);
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

	public void CreateTank()
	{
		TankRB = Instantiate(Tank).GetComponent<Rigidbody>();
		TankCollider = TankRB.GetComponent<SphereCollider>();
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
	}

	void shoot()
	{
		TankRB.isKinematic = false;
		Vector3 Force = (CurrentPosition - center.position) * Tankforce * -1;

		TankRB.velocity = Force;

		TankRB = null;
		TankCollider = null;

		Invoke("CreateTank", 2);
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

	Vector3 ClampBoundary(Vector3 Position)
	{
		Position.y = Mathf.Clamp(Position.y, BottomBoundary, 4000);
		return Position;
	}
}
