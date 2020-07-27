using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithAChance : MonoBehaviour
{
    [Range(0, 1)]
    public float ChanceOfStaying = 0.5f;

    void Start()
    {
        if (Random.value < ChanceOfStaying)
            Destroy(gameObject);
    }


}
