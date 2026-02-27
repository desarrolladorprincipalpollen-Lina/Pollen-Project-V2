using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInteracion : MonoBehaviour
{
    private GameObject selectedObject;
    private Vector3 offset;
    private float baseHeight; // altura del objeto respecto a la mesa

    public LayerMask surfaceLayer; // capa de la mesa

    void Update()
    {
        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TrySelect();
        }

        if (Mouse.current.leftButton.isPressed && selectedObject != null)
        {
            Drag();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            selectedObject = null;
        }
    }

    void TrySelect()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Draggable"))
            {
                selectedObject = hit.collider.gameObject;

                // Offset entre punto clic y pivot del objeto
                offset = selectedObject.transform.position - hit.point;

                // Guardar la altura base (para que no flote)
                baseHeight = selectedObject.transform.position.y;
            }
        }
    }

    void Drag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, surfaceLayer))
        {
            // Evitamos engancharse a sí mismo
            if (!hit.collider.CompareTag("Draggable"))
            {
                Vector3 targetPos = hit.point + offset;
                // Mantener la altura base para que no flote
                targetPos.y = baseHeight;
                selectedObject.transform.position = targetPos;
            }
        }
    }
}