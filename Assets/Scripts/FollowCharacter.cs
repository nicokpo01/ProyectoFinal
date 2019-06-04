using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharacter : MonoBehaviour
{

    public float Xmax;
    public float Xmin;
    public float Ymax;
    public float Ymin;

    public float DistanceLimit = 2f;
    public float limit_between = 2f;

    public float movementspeed = 1f;

    private float distance;
    //private Transform final;
    private Transform Target;
    private Transform Target_2;

    Vector3 nextposition; 

    private Camera Cam;
    // Use this for initialization
    void Start()
    {
        Cam = GetComponent<Camera>();
        Target = GameObject.FindGameObjectWithTag("Player 1").transform;
        Target_2 = GameObject.FindGameObjectWithTag("Player 2").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector2 final = new Vector2((Target.position.x + Target_2.position.x) * 0.5f, (Target.position.y + Target_2.position.y) * 0.5f);
        Vector2 Xa = new Vector2(Target.position.x, 1);
        Vector2 Xb = new Vector2(Target_2.position.x, 1);
        distance = Vector2.Distance(Xa, Xb);
        if (distance < limit_between)
        {
            Cam.orthographicSize = limit_between;
        }
        else
        {
            if (distance > DistanceLimit)
            {
                Cam.orthographicSize = DistanceLimit;
            }
            else
            {
                Cam.orthographicSize = distance;
            }
        }

        nextposition = new Vector3(Mathf.Clamp(final.x, Xmin, Xmax), Mathf.Clamp(final.y, Ymin, Ymax), transform.position.z);
        //transform.position = new Vector3(Mathf.Clamp(final.x, Xmin, Xmax), Mathf.Clamp(final.y, Ymin, Ymax), transform.position.z);
        transform.position = Vector3.Lerp(transform.position, nextposition, Time.deltaTime * movementspeed);
    }
}
