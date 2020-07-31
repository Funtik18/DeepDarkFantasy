﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class DeleteThis : MonoBehaviour
{
    float t=0;
    public bool isCollide=false;

    private void Start()
    {

            Destroy(GetComponent<DeleteThis>(),0.01f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "room" )
        {
            print(this.name + " удален из за " + other.name);
            //this.gameObject.SetActive(false);
            Destroy(this.gameObject);
            //Destroy(GetComponent<DeleteThis>());   
            //this.gameObject.
            
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        isCollide = false;
    }
}