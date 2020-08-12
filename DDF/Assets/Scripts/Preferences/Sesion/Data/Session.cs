using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Session {
    public string id = System.Guid.NewGuid().ToString();
    public string name = "Test";


    public string GetSession() {
        return id + "-" + name;
	}
}
