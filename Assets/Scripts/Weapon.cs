using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public CharacterController2D control;
    public int multiplier;
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
    public bool IsGrabbed = false;
    private bool Facing = true;

    private Rigidbody2D Gravity;

    private void Awake()
    {
        Gravity = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (IsGrabbed)
        {
            if (control.m_FacingRight)
            {
                multiplier = 1;
            }
            else
            {
                multiplier = -1;
            }
            transform.rotation = new Quaternion(0,0,0,0);
            Gravity.gravityScale = 0;
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
            Gravity.gravityScale = 2.01f;
            Recovery = 0;
        }

        //Temp it shouldd


    }

    public void Flip()
    {
        Facing = !Facing;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public IEnumerator FinalThrow(float ForceRight, float ForceUp, float UpTime)
    {
        Gravity.velocity = new Vector2(ForceRight, ForceUp);
        yield return new WaitForSeconds(UpTime);
        Gravity.velocity = new Vector2(0, 0);
    }

    public void Throw(float ForceRight, float ForceUp, float UpTime)
    {
        StartCoroutine(FinalThrow(ForceRight, ForceUp, UpTime));
    }

    IEnumerator Shoot()
    {
        float random = Random.Range(-proyectileSpreadMax, proyectileSpreadMax);
        Vector2 direction = GetDirectionVector2D(random);

        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, direction, multiplier * Range);
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
            
            Instantiate(impactEffect, hitInfo.point, Quaternion.identity);            
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
           
        }
        else
        {
            Vector3 pos = firePoint.position;
            pos.y = hitInfo.point.y + random;
            pos.x = hitInfo.point.x + multiplier * Range;
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + pos);
        }

        lineRenderer.enabled = true;

        yield return new WaitForSeconds(lifetime);

        lineRenderer.enabled = false;
    }

    public Vector2 GetDirectionVector2D(float angle)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
    }

}