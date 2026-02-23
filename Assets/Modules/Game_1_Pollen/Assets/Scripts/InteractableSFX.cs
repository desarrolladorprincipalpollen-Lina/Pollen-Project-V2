using System.Collections.Generic;
using UnityEngine;

namespace PollenModule
{
    ﻿using System.Collections;

    public class InteractableSFX : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        public void Play(){
            gameObject.GetComponent<ParticleSystem>().Play();
            gameObject.GetComponent<AudioSource>().Play();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
