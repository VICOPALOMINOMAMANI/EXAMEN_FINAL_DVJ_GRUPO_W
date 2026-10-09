using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Objetivos")]
    public Transform thirdPersonTarget;
    public Transform firstPersonTarget;

    [Header("Tercera Persona")]
    public float thirdPersonDistance = 5f;

    [Header("Camara")]
    public float mouseSensitivity = 6f;

    [Header("Limites verticales")]
    public float minVerticalAngle = -20f;
    public float maxVerticalAngle = 60f;

    private float yaw;
    private float pitch = 20f;

    // false = primera persona
    // true = tercera persona
    private bool thirdPerson = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // Cambiar camara con F
        if (Input.GetKeyDown(KeyCode.F))
        {
            thirdPerson = !thirdPerson;
        }

        // Movimiento del mouse
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        pitch = Mathf.Clamp(
            pitch,
            minVerticalAngle,
            maxVerticalAngle
        );

        Quaternion rotation =
            Quaternion.Euler(pitch, yaw, 0f);

        // -------------------------
        // TERCERA PERSONA
        // -------------------------

        if (thirdPerson)
        {
            if (thirdPersonTarget == null)
                return;

            Vector3 desiredPosition =
                thirdPersonTarget.position +
                rotation * new Vector3(
                    0f,
                    0f,
                    -thirdPersonDistance
                );

            transform.position = desiredPosition;

            transform.LookAt(thirdPersonTarget);
        }

        // -------------------------
        // PRIMERA PERSONA
        // -------------------------

        else
        {
            if (firstPersonTarget == null)
                return;

            transform.position =
                firstPersonTarget.position;

            transform.rotation = rotation;
        }
    }
}