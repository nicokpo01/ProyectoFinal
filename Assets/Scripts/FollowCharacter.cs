using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharacter : MonoBehaviour {

    public float Xmax;
    public float Xmin;
    public float Ymax;
    public float Ymin;

    private Transform Target;
	// Use this for initialization
	void Start () {
        Target = GameObject.Find("Character").transform;
	}
	 
	// Update is called once per frame
	void LateUpdate () {
        transform.position = new Vector3(Mathf.Clamp(Target.position.x, Xmin, Xmax), Mathf.Clamp(Target.position.y, Ymin, Ymax), transform.position.z);
	}
}
