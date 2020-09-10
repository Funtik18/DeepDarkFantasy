using System.Collections;
using System.Collections.Generic;
using DDF.Atributes;
using UnityEngine;

public class RayScan : MonoBehaviour {
	
	public bool isee;
	[ReadOnly] public string targetTag = "Player";
	public int rays = 6;
	public int distance = 15;
	public float angle = 20;
	public Vector3 offset;
	private Transform target;
	public List<GameObject> inview = new List<GameObject>();

	void Start () 
	{
		//target = GameObject.FindGameObjectWithTag(targetTag).transform;
    }

	bool GetRaycast(Vector3 dir)
	{
		bool result = false;
		RaycastHit hit = new RaycastHit();
		Vector3 pos = transform.position + offset;
		if (Physics.Raycast (pos, dir, out hit, distance))
		{
			if(hit.collider.tag.Equals(targetTag))
			{
				result = true;
				add_object(hit.collider.gameObject);
				Debug.DrawLine(pos, hit.point, Color.green);
			}
			else
			{
                add_object(hit.collider.gameObject);
				Debug.DrawLine(pos, hit.point, Color.blue);
				//Debug.Log(hit.collider.name);
			}
		}
		else
		{
			Debug.DrawRay(pos, dir * distance, Color.red);
		}
		return result;
	}
	
	bool RayToScan () 
	{
		bool result = false;
		bool a = false;
		bool b = false;
		float j = 0;
		for (int i = 0; i < rays; i++)
		{
			var x = Mathf.Sin(j);
			var y = Mathf.Cos(j);

			j += angle * Mathf.Deg2Rad / rays;

			Vector3 dir = transform.TransformDirection(new Vector3(x, 0, y));
			if(GetRaycast(dir)) a = true;

			if(x != 0) 
			{
				dir = transform.TransformDirection(new Vector3(-x, 0, y));
				if(GetRaycast(dir)) b = true;
			}
		}
	
		if(a || b) result = true;
		return result;
	}

	void Update ()
	{	
		//if(Vector3.Distance(transform.position, target.position) < distance)
		//{
			if(RayToScan())
			{
				// Контакт с целью
				isee = true;
			}
			else
			{
				isee = false;
				// Поиск цели...
			}
		//}
	}

	private void add_object(GameObject other){
		bool have = false;
		foreach(GameObject g in inview){
			if(g==other){
				have = true;
			}
		}
		if(!have){
			inview.Add(other);
		}
	}
}
