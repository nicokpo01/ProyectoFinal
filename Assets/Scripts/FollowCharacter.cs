using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharacter : MonoBehaviour
{

    public float Xmax;
    public float Xmin;
    public float Ymax;
    public float Ymin;

    public Transform blankpoint;
    public float limit_between = 2f;
    public float limit_Max = 20f;

    public float modifierZoom = 1;

    public float z;

    private float last;
    private float distance;

    private Vector3 final;

    private Transform Target;
    private Transform Target_2;

    private GameObject x1;
    private GameObject x2;

    Vector3 nextposition; 

    private Camera Cam;
    // Use this for initialization
    void Start()
    {
        Cam = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {        
        last = Cam.orthographicSize;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        x1 = GameObject.FindGameObjectWithTag("Player 1");
        x2 = GameObject.FindGameObjectWithTag("Player 2");

        if (x1 == null)
        {
            if (x2 == null)
            {
                Target = blankpoint;
                Target_2 = blankpoint;               
            }
            else
            {
                Target = x2.transform;
            }
        }
        else
        {
            Target = x1.transform;
        }
        if (x2 == null)
        {
            if (x1 == null)
            {
                Target = blankpoint;
                Target_2 = blankpoint;
            }
            else
            {
                Target_2 = x1.transform;
            }
        }
        else
        {
            Target_2 = x2.transform;
        }

        final = new Vector3((Target.position.x + Target_2.position.x) * 0.5f, (Target.position.y + Target_2.position.y) * 0.5f, z);
        Cam.transform.position = final;

        Vector2 Xa = new Vector2(Target.position.x, 1);
        Vector2 Xb = new Vector2(Target_2.position.x, 1);
        distance = Vector2.Distance(Xa, Xb);
        float extra = Time.deltaTime * modifierZoom;

        if (distance < limit_between)
        {
            //Cam.orthographicSize = limit_between;
            Cam.orthographicSize = Mathf.Lerp(last, limit_between, extra);
        }
        else
        {
            if (distance > limit_Max)
            {
                Cam.orthographicSize = Mathf.Lerp(last, limit_Max, extra);
                //Cam.orthographicSize = limit_Max;
            }
            else
            {
                //Cam.orthographicSize = Mathf.Lerp(last, distance + CutOff, extra);
                Cam.orthographicSize = Mathf.Lerp(last, distance, extra);
            }           
        }
    }
}
