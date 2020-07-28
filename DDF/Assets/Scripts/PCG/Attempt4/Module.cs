using UnityEngine;

public class Module : MonoBehaviour
{
	public string[] Tags;
	public bool isCollude = false;

	public ModuleConnector[] GetExits()
	{
		return GetComponentsInChildren<ModuleConnector>();
	}

}
