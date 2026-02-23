using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


namespace PollenModule
{
    ﻿using System.Collections;

    public class PickableHandler : MonoBehaviour
    {
        [SerializeField] public string determinant;
        [SerializeField] public string pickableName;
        [SerializeField] public string pickableColor;
        [SerializeField] public string pickableCharacteristic;

        private Vector3 initialPos;
        private Quaternion initialRot;
        // Start is called before the first frame update
        void Start()
        {
            initialPos = transform.position;
            initialRot = transform.rotation;
            //TODO Load the poof SFX
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void ReLocate(){
            var grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            if (grabInteractable != null && grabInteractable.isSelected)
            {
                var beingHeldBy = grabInteractable.firstInteractorSelecting;
                Debug.Log($"{this} is being held by {beingHeldBy}");
                grabInteractable.interactionManager.SelectCancel(beingHeldBy, grabInteractable);
            }
            //TODO play the poof SFX
            transform.position = initialPos;
            transform.rotation = initialRot;
        }


    }
}
