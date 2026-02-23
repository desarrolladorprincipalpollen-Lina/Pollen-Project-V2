namespace PollenModule
{
    ﻿using UnityEngine;

    public class MouseInteraction : MonoBehaviour
    {
        public float rayDistance = 100f;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, rayDistance))
                {
                    Debug.Log("Objeto clickeado: " + hit.collider.gameObject.name);

                    // Si el objeto tiene componente interactuable
                    IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                    if (interactable != null)
                    {
                        interactable.Interact();
                    }
                }
            }
        }
    }
}
