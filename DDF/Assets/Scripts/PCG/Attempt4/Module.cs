using System.Linq;
using UnityEngine;

public class Module : MonoBehaviour
{
	public string[] Tags;
	public bool isCollude = false;

	public ModuleConnector[] GetExits()
	{
		return GetComponentsInChildren<ModuleConnector>();
	}

	public void RemoveTags(string tag)
	{

		var temp=GetComponentsInChildren<ModuleConnector>();

		Tags = Tags.Where(w => w != tag).ToArray();

		foreach (var item in temp)
		{
			item.RemoveTags(tag);
		}
	}
}
