using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private float Recovery;
    public float TiempoInicio;
    public float proyectileSpreadMax = 0;
    public float lifetime = 0.02f;
    public int Range = 10;
    public Transform firePoint;
    public int damage = 40;
    public GameObject impactEffect;
    public LineRenderer lineRenderer;
    public Transform weaponslot;
    public bool IsGrabbed = true;
    // Update is called once per frame

    void FixedUpdate()
    {
        if (IsGrabbed)
        {
            Vector3 Pos = weaponslot.position;
            transform.position = Pos;
            if (Recovery <= 0)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Recovery = TiempoInicio;
                    StartCoroutine(Shoot());
                }
            }
            else
            {
                Recovery -= Time.deltaTime;
            }
        }
        else {
            Recovery = 0;
        }

        //Temp it shouldd


    }

    IEnumerator Shoot()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right, Range);
        Instantiate(impactEffect, firePoint.position, Quaternion.identity);

        if (hitInfo)
        {
            Debug.Log(hitInfo.transform.name);
            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Naisu");
            }
            
            float i = Random.Range(-proyectileSpreadMax, proyectileSpreadMax);
            Vector2 pos;
            pos.x = hitInfo.point.x;
            pos.y = hitInfo.point.y + i;
            Instantiate(impactEffect, pos, Quaternion.identity);            
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
           
        }
        else
        {
            float i = Random.Range(-proyectileSpreadMax, proyectileSpreadMax);
            Vector3 pos = firePoint.position;
            pos.y = hitInfo.point.y + i;
            pos.x += Range;
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + pos);
        }

        lineRenderer.enabled = true;

        yield return new WaitForSeconds(lifetime);

        lineRenderer.enabled = false;
    }

}