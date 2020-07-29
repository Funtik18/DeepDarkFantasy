using DDF.Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDesignManager : MonoBehaviour {

    public static LevelDesignManager _instance;

	public List<RoomDesign> rooms;

	public Material floorMaterial;
	public Material wallMaterial;

	private void Awake() {
		_instance = this;
	

	}
	private void Start() {
		
	}
	public void RefreshRoom() {
		for(int i = 0; i< rooms.Count; i++) {
			rooms[i].SetRoom(floorMaterial, wallMaterial);
		}
	}


}
