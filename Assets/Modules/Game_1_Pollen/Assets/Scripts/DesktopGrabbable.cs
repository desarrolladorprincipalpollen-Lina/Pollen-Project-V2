namespace PollenModule
{
    ﻿using UnityEngine;

    public class DesktopGrabbable : MonoBehaviour
    {
        private bool isDragging = false;
        private float distance;

        void OnMouseDown()
        {
            distance = Vector3.Distance(transform.position, Camera.main.transform.position);
            isDragging = true;
        }

        void OnMouseUp()
        {
            isDragging = false;
        }

        void Update()
        {
            if (isDragging)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 point = ray.GetPoint(distance);
                transform.position = point;
            }
        }
    }
}
