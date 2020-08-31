using UnityEngine;
using System.Linq;

public class ModuleConnector : MonoBehaviour
{
    public string[] Tags;
    public bool IsDefault;

    void OnDrawGizmos()
    {
        var scale = 1.0f;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * scale);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (transform.right));
        //Gizmos.DrawLine(transform.position, transform.position - (transform.right * (transform.parent.GetComponent<BoxCollider>().size.z)));

        //Gizmos.DrawLine(transform.position + transform.parent.GetComponent<BoxCollider>().size.z * transform.parent.forward / 2, (transform.position + transform.parent.GetComponent<BoxCollider>().size.z * transform.parent.forward / 2) + transform.forward * (transform.parent.GetComponent<BoxCollider>().size.x / 2));
        //Gizmos.DrawLine(transform.position + transform.parent.GetComponent<BoxCollider>().size.z * transform.parent.forward / 2, (transform.position + transform.parent.GetComponent<BoxCollider>().size.z * transform.parent.forward / 2) - transform.forward * (transform.parent.GetComponent<BoxCollider>().size.x / 2));


        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * scale);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.125f);
    }
    
    public void RemoveTags(string tag)
    {
        Tags = Tags.Where(w => w != tag).ToArray();
    }

}
