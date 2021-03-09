using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    void Start()
    {
        ParticleSystem.MainModule breakingEffect = GetComponent<ParticleSystem>().main;
        Destroy(gameObject, breakingEffect.duration);
    }
}
