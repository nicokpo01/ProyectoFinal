﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spring : MonoBehaviour
{

    public float force = 2f;
    public float MultiplierForObjects = 0.5f;

    private float uptime = 1f;
    private GameObject @object;
    // Use this for 

    void OnTriggerEnter2D(Collider2D other)
    {
        @object = other.gameObject;
        Throw(force, uptime);
    }

    public void Throw(float ForceUp, float UpTime)
    {
        Weapon check = @object.GetComponent<Weapon>();
        if (check != null)
        {
            Debug.Log("weapon");
            if (!check.IsGrabbed)
            {
                Debug.Log("Is not grabbed");
                @object.GetComponent<Rigidbody2D>().velocity = new Vector2(0, ForceUp * MultiplierForObjects);
            }
        }
        else
        {
            @object.GetComponent<Rigidbody2D>().velocity = new Vector2(0, ForceUp);
        }
        
    }
}

        
