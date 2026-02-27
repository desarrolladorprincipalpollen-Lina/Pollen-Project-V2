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

    // Variable para rastrear el objeto actualmente agarrado
    private PollenModule.DesktopGrabbable currentHeldObject;
    [Header("Interaction")]
    public Transform holdPoint; // Asigna un objeto vacío hijo de la cámara aquí

    void Interact()
    {
        if (Mouse.current == null) return;

        // Click izquierdo — interactuar / agarrar objetos y carteles
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (currentHeldObject != null)
            {
                // Soltar el objeto
                currentHeldObject.Release();
                currentHeldObject = null;
            }
            else
            {
                Transform rayOrigin = cam != null ? cam.transform : transform;
                Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 3f)) // Distancia de agarre de 3 metros
                {
                    // 1. Intentar agarrar objeto (DesktopGrabbable)
                    PollenModule.DesktopGrabbable grabbable = hit.collider.GetComponent<PollenModule.DesktopGrabbable>();
                    if (grabbable == null) grabbable = hit.collider.GetComponentInParent<PollenModule.DesktopGrabbable>();

                    if (grabbable != null)
                    {
                        if (holdPoint == null)
                        {
                            GameObject hp = new GameObject("HoldPoint");
                            hp.transform.SetParent(cam.transform);
                            hp.transform.localPosition = new Vector3(0.5f, -0.5f, 1f);
                            holdPoint = hp.transform;
                        }

                        currentHeldObject = grabbable;
                        currentHeldObject.Grab(holdPoint);
                        return; // Si agarramos algo, no hacemos nada más
                    }

                    // 2. Interactuar con UI (Canvas)
                    var canvas = hit.collider.GetComponentInChildren<Canvas>();
                    if (canvas == null) canvas = hit.collider.GetComponentInParent<Canvas>();

                    if (canvas != null)
                    {
                        var buttons = canvas.GetComponentsInChildren<UnityEngine.UI.Button>();
                        foreach (var button in buttons)
                        {
                            RectTransform rect = button.GetComponent<RectTransform>();
                            // Proyección simple para world space UI
                            // Nota: Esto es una aproximación básica. Para UI world space precisa se necesita un GraphicRaycaster.
                            // Pero para este caso simple, intentamos invocar si el raycast golpea el collider del botón.
                            if (hit.collider.gameObject == button.gameObject || hit.collider.transform.IsChildOf(button.transform))
                            {
                                button.onClick.Invoke();
                                Debug.Log("Botón presionado: " + button.gameObject.name);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}