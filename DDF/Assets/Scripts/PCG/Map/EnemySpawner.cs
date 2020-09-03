using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemys;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(enemys[Random.Range(0, enemys.Length)], this.transform);
    }
}
