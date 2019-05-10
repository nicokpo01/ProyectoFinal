using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public float speed;
    public float lifeTime;
    public float distance;
    public int damage;
    public LayerMask whatIsSolid;
    public Transform FirePosition;
    public GameObject ImpactEffect;
    public LineRenderer linerenderer;

    void FixedUpdate()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            shoot();
        }
    }

    void shoot()
    {

        RaycastHit2D hitInfo = Physics2D.Raycast(FirePosition.position, transform.right, distance, whatIsSolid);
        if (hitInfo)
        {
            enemy target = hitInfo.transform.GetComponent<enemy>();
            if (target != null)
            {
                target.RecibirDaño(10);

            }

            Instantiate(ImpactEffect, hitInfo.point, Quaternion.identity);
            linerenderer.SetPosition(0, FirePosition.position);
            linerenderer.SetPosition(1, hitInfo.point);

        }
        else
        {
            linerenderer.SetPosition(0, FirePosition.position);
            linerenderer.SetPosition(1, FirePosition.position + FirePosition.right * distance);
        }


        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
}