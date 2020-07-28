using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooms : MonoBehaviour
{
    public Material[] FloorMaterials;

    public GameObject Door1;//Дверь сверху
    public GameObject Door2;//Дверь справа
    public GameObject Door3;//Дверь снизу
    public GameObject Door4;//Дверь слева

    [Range(0,1)]
    public float ChanceOfItem=0.5f;
    public GameObject[] Items;

    void Start()
    {
        foreach (var item in GetComponentsInChildren<MeshRenderer>())
        {

            if (item.tag == "Floor")
            {
                item.GetComponent<MeshRenderer>().material = FloorMaterials[Random.Range(0, FloorMaterials.Length)];
                item.transform.rotation = Quaternion.Euler(0, 90 * Random.Range(0, 4), 0);
                if (Random.value < ChanceOfItem && Items.Length>0)
                {
                    Instantiate(Items[Random.Range(0,Items.Length)], item.transform);
                }
            }

        }
    }

    public void RotateRandom()
    {
        int count = Random.Range(0, 4);

        for (int i = 0; i < count; i++)
        {
            transform.Rotate(0, 90, 0);
            GameObject tmp = Door4;
            Door4 = Door3;
            Door3 = Door2;
            Door2 = Door1;
            Door1 = tmp;

        }
    }
}
