using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float proyectileSpreadMax = 0;
    public float lifetime = 0.02f;
    public int Range = 10;
    public Transform firePoint;
    public int damage = 40;
    public GameObject impactEffect;
    public LineRenderer lineRenderer;
    public Transform weaponslot;
    private Transform ActualPosition;
    public bool IsGrabbed = true;
    // Update is called once per frame
    private void Awake()
    {
        ActualPosition = GetComponent<Transform>();
    }

    void Update()
    {
        if (IsGrabbed)
        {
            ActualPosition = weaponslot;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(Shoot());
        }
          
    }

    IEnumerator Shoot()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);
        Instantiate(impactEffect, firePoint.position, Quaternion.identity);

        if (hitInfo)
        {
            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            if (!enemy)
            {
                enemy.TakeDamage(damage);
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