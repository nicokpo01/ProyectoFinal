using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharacter : MonoBehaviour
{
    public Transform blankpoint;
    public float limit_between = 2f;
    public float limit_Max = 20f;

    public float cutoff = 0;
    public float modifierZoom = 1;
    public float ExtraModifierZoom = 1.5f;

    public float z;

    private float last;
    private float distance;

    private Vector3 final;

    private Transform Target;
    private Transform Target_2;

    private GameObject x1;
    private GameObject x2;

    private bool allDead = false;

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
                allDead = true;
                Target = blankpoint;
                Target_2 = blankpoint;               
            }
            else
            {
                allDead = false;
                Target = x2.transform;
            }
        }
        else
        {
            allDead = false;
            Target = x1.transform;
        }
        if (x2 == null)
        {
            if (x1 == null)
            {
                allDead = true;
                Target = blankpoint;
                Target_2 = blankpoint;
            }
            else
            {
                allDead = false;
                Target_2 = x1.transform;
            }
        }
        else
        {
            allDead = false;
            Target_2 = x2.transform;
        }

        final = new Vector3((Target.position.x + Target_2.position.x) * 0.5f, (Target.position.y + Target_2.position.y) * 0.5f, z);
        Cam.transform.position = final;

        Vector2 Xa = new Vector2(Target.position.x, 1);
        Vector2 Xb = new Vector2(Target_2.position.x, 1);
        distance = Vector2.Distance(Xa, Xb);
        float extra = Time.deltaTime * modifierZoom;
        float Moreextra = extra * ExtraModifierZoom;

        if(!allDead)
        {
            if (distance < limit_between)
            {
                //Cam.orthographicSize = limit_between;
                Cam.orthographicSize = Mathf.Lerp(last, limit_between + cutoff, extra);
            }
            else
            {
                if (distance > limit_Max)
                {
                    Cam.orthographicSize = Mathf.Lerp(last, limit_Max + cutoff, extra);
                    //Cam.orthographicSize = limit_Max;
                }
                else
                {
                    //Cam.orthographicSize = Mathf.Lerp(last, distance + CutOff, extra);
                    Cam.orthographicSize = Mathf.Lerp(last, distance + cutoff, extra);
                }
            }
        }
        else
        {
            Cam.orthographicSize = Mathf.Lerp(last, limit_Max + cutoff, Moreextra);
            allDead = true;
        }
    }
}
