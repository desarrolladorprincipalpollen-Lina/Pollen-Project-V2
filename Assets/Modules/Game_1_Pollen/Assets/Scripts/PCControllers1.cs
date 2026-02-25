using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PCControllers1 : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;
    public float verticalSpeed = 3f;

    [Header("Mouse")]
    public float mouseSensitivity = 5f;

    private float rotationX = 0f;
    private Transform xrOrigin;
    private Camera cam;

    void Start()
    {
        var originComponent = GetComponentInParent<XROrigin>();
        if (originComponent != null)
            xrOrigin = originComponent.transform;
        else
            xrOrigin = transform.parent;

        cam = GetComponent<Camera>();
        if (cam == null) cam = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Script activo. XR Origin: " + (xrOrigin ? xrOrigin.name : "null") + " | Cam: " + (cam ? cam.name : "null"));
    }

    void Update()
    {
        if (Keyboard.current == null || Mouse.current == null) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        MouseLook();
        Move();
        Interact();
    }

    void MouseLook()
    {
        if (Cursor.lockState != CursorLockMode.Locked) return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        if (xrOrigin != null)
            xrOrigin.Rotate(0, mouseX, 0);

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -80f, 80f);

        if (cam != null)
            cam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }

    void Move()
    {
        if (Keyboard.current == null) return;

        float speed = Keyboard.current.leftShiftKey.isPressed ? sprintSpeed : moveSpeed;

        // Movimiento horizontal
        float x = 0f;
        if (Keyboard.current.aKey.isPressed) x -= 1f;
        if (Keyboard.current.dKey.isPressed) x += 1f;

        float z = 0f;
        if (Keyboard.current.sKey.isPressed) z -= 1f;
        if (Keyboard.current.wKey.isPressed) z += 1f;

        // Movimiento vertical con Q y E
        float y = 0f;
        if (Keyboard.current.eKey.isPressed) y += 1f;
        if (Keyboard.current.qKey.isPressed) y -= 1f;

        Transform moveTransform = cam != null ? cam.transform : transform;

        Vector3 forward = moveTransform.forward;
        Vector3 right = moveTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 move = right * x + forward * z + Vector3.up * y;

        if (xrOrigin != null)
            xrOrigin.position += move * speed * Time.deltaTime;
        else
            transform.position += move * speed * Time.deltaTime;
    }

    void Interact()
    {
        if (Mouse.current == null) return;

        Transform rayOrigin = cam != null ? cam.transform : transform;
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        RaycastHit hit;

        // Click izquierdo — interactuar / agarrar objetos y carteles
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (Physics.Raycast(ray, out hit, 100f))
            {
                Debug.Log("[Click Izquierdo] Tocaste: " + hit.collider.gameObject.name);

                // Interactuar con carteles (Canvas en el mundo)
                var canvas = hit.collider.GetComponentInChildren<Canvas>();
                if (canvas == null)
                    canvas = hit.collider.GetComponentInParent<Canvas>();

                if (canvas != null)
                {
                    Debug.Log("Cartel encontrado: " + canvas.gameObject.name);

                    // Simular click en botones del canvas
                    var buttons = canvas.GetComponentsInChildren<UnityEngine.UI.Button>();
                    foreach (var button in buttons)
                    {
                        // Verificar si el raycast apunta al botón
                        RectTransform rect = button.GetComponent<RectTransform>();
                        if (RectTransformUtility.RectangleContainsScreenPoint(rect, cam.WorldToScreenPoint(hit.point), cam))
                        {
                            button.onClick.Invoke();
                            Debug.Log("Botón presionado: " + button.gameObject.name);
                            break;
                        }
                    }
                }

                // Interactuar con objetos XR
                var interactable = hit.collider.GetComponent<XRBaseInteractable>();
                if (interactable != null)
                    Debug.Log("Objeto XR interactuable: " + interactable.name);
            }
        }

        // Click derecho — agarrar / grip
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (Physics.Raycast(ray, out hit, 100f))
            {
                Debug.Log("[Click Derecho] Agarraste: " + hit.collider.gameObject.name);

                var grabbable = hit.collider.GetComponent<XRGrabInteractable>();
                if (grabbable != null)
                    Debug.Log("Objeto agarrable encontrado: " + grabbable.name);
            }
        }
    }
}