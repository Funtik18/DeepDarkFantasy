using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{

    public Transform Enter;
    public Transform Exit;

    public Material[] FloorMaterials;

    // Start is called before the first frame update
    

    void Start()
    {
        foreach (var item in GetComponentsInChildren<MeshRenderer>())
        {
            //print(item.GetComponent<MeshRenderer>().material + " " + FloorMaterials[0]);
            if (item.tag=="Floor")
            {
                item.GetComponent<MeshRenderer>().material = FloorMaterials[Random.Range(0, FloorMaterials.Length)];
                item.transform.rotation = Quaternion.Euler(0, 90 * Random.Range(0, 4),0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
