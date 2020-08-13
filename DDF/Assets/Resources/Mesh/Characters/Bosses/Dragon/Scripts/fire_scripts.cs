using DDF.Character.Stats;
using UnityEngine;

public class fire_scripts : MonoBehaviour
{
    public int dmg = 10;
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other) {
        CharacterStats stat = other.GetComponent<CharacterStats>();
        if(stat != null){
            stat.TakeDamage(dmg,gameObject.transform.root.gameObject);
        }
        Debug.Log("Hit "+other.name);
    }
    
}
