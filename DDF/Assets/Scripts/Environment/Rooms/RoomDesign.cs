using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Environment {
    public class RoomDesign : MonoBehaviour {

		public FloorDesign floors;
		public WallDesign walls;

		public void SetRoom( Material materialFloor, Material materialWall) {
			floors.SetFloor(materialFloor);
			walls.SetWalls(materialWall);
		}
	}
}
