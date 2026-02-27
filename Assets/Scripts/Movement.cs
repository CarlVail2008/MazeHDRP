using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
	public float moveSpeed = 3.5f;
	public float dashSpeed = 7f;
	public float mouseSensitivity = 0.5f;
	[Range(0, 90)] public float lookLimit = 30f;

	private Rigidbody rb;
	private Transform playerCamera;

	private float verticalRotation = 0f;
	private float horizontalRotation = 0f;

	Vector2 moveInput;
	bool isDashing;
	Vector2 mouseDelta;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		playerCamera = GetComponentInChildren<Camera>().transform;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update()
	{
		// Handle movement

		float currentSpeed = isDashing ? dashSpeed : moveSpeed;

		Vector3 camForward = playerCamera.forward;
		Vector3 camRight = playerCamera.right;

		camForward.y = 0;
		camRight.y = 0;
		camForward.Normalize();
		camRight.Normalize();

		Vector3 moveDirection = (camForward * moveInput.y + camRight * moveInput.x).normalized;

		rb.linearVelocity = new Vector3(
			moveDirection.x * currentSpeed,
			rb.linearVelocity.y,
			moveDirection.z * currentSpeed
		);



		// Handle camera rotation

		horizontalRotation += mouseDelta.x;
		verticalRotation -= mouseDelta.y;
		verticalRotation = Mathf.Clamp(verticalRotation, -lookLimit, lookLimit);

		playerCamera.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
	}

	public void Move(InputAction.CallbackContext ctx)
	{
		moveInput = ctx.ReadValue<Vector2>();
	}

	public void Run(InputAction.CallbackContext ctx)
	{
		isDashing = !ctx.canceled;
	}

	public void Turn(InputAction.CallbackContext ctx)
	{
		mouseDelta = ctx.ReadValue<Vector2>() * mouseSensitivity;
	}
}
