namespace PollenModule
{
    ﻿using UnityEngine;

    public class SimpleFPSController : MonoBehaviour
    {
        public float speed = 5f;
        public float mouseSensitivity = 2f;
        private float yRotation = 0f;

        void Update()
        {
            // Movimiento
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            transform.position += move * speed * Time.deltaTime;

            // Rotación horizontal
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            transform.Rotate(Vector3.up * mouseX);

            // Rotación vertical
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
            yRotation -= mouseY;
            yRotation = Mathf.Clamp(yRotation, -80f, 80f);

            Camera.main.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        }
    }
}
