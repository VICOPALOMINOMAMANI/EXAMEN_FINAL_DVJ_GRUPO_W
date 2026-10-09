using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    [Header("Salto")]
    public float jumpHeight = 2f;
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.15f;

    [Header("Gravedad")]
    public float gravity = -9.81f;

    [Header("Camara")]
    public Transform cameraTransform;

    private CharacterController controller;
    private Animator animator;

    private float verticalVelocity;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // -------------------------
        // INPUT
        // -------------------------

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Guardar brevemente la pulsacion de salto
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // -------------------------
        // DETECCION DEL SUELO
        // -------------------------

        if (controller.isGrounded)
        {
            coyoteTimeCounter = coyoteTime;

            if (verticalVelocity < 0f)
            {
                verticalVelocity = -2f;
            }
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // -------------------------
        // SALTO
        // -------------------------

        if (jumpBufferCounter > 0f &&
            coyoteTimeCounter > 0f)
        {
            verticalVelocity =
                Mathf.Sqrt(jumpHeight * -2f * gravity);

            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }

        // -------------------------
        // ANIMACION
        // -------------------------

        float speed =
            new Vector2(horizontal, vertical).magnitude;

        if (animator != null)
        {
            animator.SetFloat("Speed", speed);
        }

        // -------------------------
        // MOVIMIENTO
        // -------------------------

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 direction =
            cameraForward * vertical +
            cameraRight * horizontal;

        direction.Normalize();

        if (direction.magnitude >= 0.1f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // -------------------------
        // GRAVEDAD
        // -------------------------

        verticalVelocity += gravity * Time.deltaTime;

        // Movimiento horizontal + vertical en una sola llamada
        Vector3 movement =
            direction * moveSpeed +
            Vector3.up * verticalVelocity;

        controller.Move(
            movement * Time.deltaTime
        );
    }
}