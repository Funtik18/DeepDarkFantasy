using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTheDoor : MonoBehaviour
{

    public string AnimName;
    public GameObject Player;

    public GameObject door;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player.gameObject.GetComponent<Animator>().SetBool("open", true);
            Player.gameObject.GetComponent<PlayerMovement>().freezMovement = true;
            door.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }


}
