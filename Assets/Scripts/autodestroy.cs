﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autodestroy : MonoBehaviour {

    public float time = 5;
    private float t;
	// Use this for initialization
	void Start () {
        t = time;	}

    // Update is called once per frame
    void Update()
    {
        if (t <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            t -= Time.deltaTime;
        }
    }
}
