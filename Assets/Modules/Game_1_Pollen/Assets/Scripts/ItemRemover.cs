using System.Collections.Generic;
using UnityEngine;

namespace PollenModule
{
    ﻿using System.Collections;

    public class ItemRemover : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.tag == "Pickable")
            {
            if(other.gameObject.TryGetComponent<PickableHandler>(out PickableHandler pickable)){
                pickable.ReLocate();
                }
                other.gameObject.GetComponentInParent<PickableHandler>().ReLocate();
            }
        }
    }
}
