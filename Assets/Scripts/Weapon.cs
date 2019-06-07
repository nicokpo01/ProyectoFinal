using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public CharacterController2D control;
    public int MaxAmmo;
    public int Ammo;
    public int multiplier;
    public int Range = 10;
    public int damage = 40;

    private float Recovery;
    private float LineRecovery;
    public float TiempoInicio;
    public float proyectileSpreadMax = 0;

    public float timeforDespawn = 30;
    private float recoverytimedespawn ;

    public float lifetime = 0.02f;
    public float multiplierLine = 1f;
    
    public Transform firePoint;
    public Transform weaponslot;
    public GameObject impactEffect;
    public LineRenderer lineRenderer;

    private LayerMask Whatissolid;

    public float linerendererLength;
    public float linerendererLengthEnd;

    public bool wantlineeffect = false;
    private bool equalLine;
    public bool IsGrabbed = false;
    public bool onStand = false;
    private bool Facing = true;

    private Rigidbody2D Gravity;

    public string objectTag = "Player 1";
    private string StrFire = "Fire";

    private void Awake()
    {
        Gravity = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        recoverytimedespawn = timeforDespawn;

        Whatissolid = LayerMask.GetMask("Player", "Platform");

        if (lifetime > TiempoInicio)
        {
            lifetime = TiempoInicio;
        }
        Ammo = MaxAmmo;
        linerendererLength = lineRenderer.startWidth;
        linerendererLengthEnd = lineRenderer.endWidth;
        if (linerendererLength == linerendererLengthEnd)
        {
            equalLine = true;
        }
    }
    //
    private void Update()
    {
        if (IsGrabbed)
        {
            transform.position = weaponslot.position;
        }
    }

    void FixedUpdate()
    {
        if (objectTag == "Player 2")
        {
            StrFire = "Fire 2";
        }
        else
        {
            StrFire = "Fire";
        }

        if (onStand)
        {
            transform.rotation = Quaternion.identity;
            Gravity.gravityScale = 0;
            recoverytimedespawn = timeforDespawn;
        }

        if (IsGrabbed)
        {
            recoverytimedespawn = timeforDespawn;
            onStand = false;
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
            
            if (Recovery <= 0)
            {
                
                if (Input.GetButtonDown(StrFire))
                {
                    lineRenderer.startWidth = linerendererLength;
                    lineRenderer.endWidth = linerendererLengthEnd;
                    LineRecovery = lifetime;

                    Recovery = TiempoInicio;
                    StartCoroutine(Shoot());
                }
            }
            else
            {                
                Recovery -= Time.deltaTime;
            }
            ///////////////////ahora line
            
            if (LineRecovery > 0)            
            {
                if (wantlineeffect)
                {
                    if (!equalLine)
                    {
                        if (lineRenderer.startWidth >= linerendererLengthEnd)
                        {
                            lineRenderer.startWidth -= linerendererLengthEnd * multiplierLine;
                        }
                    }
                    else
                    {
                        if (lineRenderer.startWidth >= 0)
                        {
                            lineRenderer.startWidth -= 1 * multiplier;
                            lineRenderer.endWidth -= 1 * multiplier;
                        }
                        
                    }
                }
                LineRecovery -= Time.deltaTime;
            }
        }
        else {            
            if (!onStand)
            {
                recoverytimedespawn -= Time.deltaTime;
                Gravity.gravityScale = 2.01f;
            }            
            Recovery = 0;
            if (recoverytimedespawn <= 0)
            {
                Destroy(gameObject);
            }
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
        if (Ammo > 0)
        {
            Ammo--;
            float random = Random.Range(-proyectileSpreadMax, proyectileSpreadMax);
            Vector2 direction = GetDirectionVector2D(random);

            RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, direction, multiplier * Range, Whatissolid);
            Instantiate(impactEffect, firePoint.position, Quaternion.identity);
            if (hitInfo)
            {
                Movement enemy = hitInfo.transform.GetComponent<Movement>();
                if (enemy != null)
                {
                    enemy.TakeDamage();
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

            float ftime = lifetime;
            if (lifetime > TiempoInicio)
            {
                ftime = TiempoInicio;
            }
            yield return new WaitForSeconds(ftime);

            lineRenderer.enabled = false;
        }
        //else
        //{
            //Sin municion
        //}
    }

    public Vector2 GetDirectionVector2D(float angle)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
    }

}