using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public Transform PosSlot;
    public float DistanceRayCast = 0.5f;
    public float GrabRange;
    private bool Grabbing = false;
    public int multiplier;
    public float ForceThrowUp = 0.2f;
    public float ForceThrowRight = 0.7f;
    public float ThrowTime = 0.05f;

    public float fallmultiplier = 2.5f;
    public float lowjumpmultiplier = 2f;

    Rigidbody2D rb;

    public CharacterController2D control;

    private Animator Anim;
    public float VelocidadMovimiento;
    public float MovimientoHorizontal;

    bool salto = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        MovimientoHorizontal = Input.GetAxisRaw("Horizontal") * VelocidadMovimiento;
        if (Input.GetButtonDown("Jump"))
        {
            salto = true;
            Anim.SetTrigger("Salto");
        }


    }

    private void FixedUpdate()
    {
        if (control.m_FacingRight)
        {
            multiplier = 1;
        }
        else
        {
            multiplier = -1;
        }
        control.Move(MovimientoHorizontal * Time.fixedDeltaTime, false, salto);
        salto = false;

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallmultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowjumpmultiplier - 1) * Time.fixedDeltaTime;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (Grabbing == false)
            {
                Vector3 PosArriba = PosSlot.position;
                PosArriba.y = PosSlot.position.y + DistanceRayCast;
                Vector3 PosAbajo = PosSlot.position;
                PosAbajo.y = PosSlot.position.y - DistanceRayCast;

                RaycastHit2D hitInfoArriba = Physics2D.Raycast(PosArriba, PosSlot.right * multiplier, GrabRange);
                RaycastHit2D hitInfoMedio = Physics2D.Raycast(PosSlot.position, PosSlot.right * multiplier, GrabRange);
                RaycastHit2D hitInfoAbajo = Physics2D.Raycast(PosAbajo, PosSlot.right * multiplier, GrabRange);
                if (hitInfoMedio)
                {
                    Weapon weapon = hitInfoMedio.transform.GetComponent<Weapon>();
                    if (weapon != null)
                    {
                        Vector3 Scale = weapon.transform.localScale;
                        if (control.m_FacingRight)
                        {
                            if (Mathf.Sign(weapon.transform.localScale.x) < 0)
                            { Scale.x *= -1; }
                        }
                        else
                        {
                            if (Mathf.Sign(weapon.transform.localScale.x) > 0)
                            { Scale.x *= -1; }
                        }

                        weapon.transform.localScale = Scale;
                        weapon.control = control;
                        control.weapon = weapon;
                        Grabbing = true;
                        weapon.IsGrabbed = true;
                        weapon.weaponslot = PosSlot;
                    }
                }
                else
                {
                    if (hitInfoArriba)
                    {
                        Weapon weapon = hitInfoArriba.transform.GetComponent<Weapon>();
                        if (weapon != null)
                        {
                            Vector3 Scale = weapon.transform.localScale;
                            if (control.m_FacingRight)
                            {
                                if (Mathf.Sign(weapon.transform.localScale.x) < 0)
                                { Scale.x *= -1; }
                            }
                            else
                            {
                                if (Mathf.Sign(weapon.transform.localScale.x) > 0)
                                { Scale.x *= -1; }
                            }

                            weapon.transform.localScale = Scale;

                            weapon.control = control;
                            control.weapon = weapon;
                            Grabbing = true;
                            weapon.IsGrabbed = true;
                            weapon.weaponslot = PosSlot;
                        }
                    }
                    else if (hitInfoAbajo)
                    {
                        Weapon weapon = hitInfoAbajo.transform.GetComponent<Weapon>();
                        if (weapon != null)
                        {
                            Vector3 Scale = weapon.transform.localScale;
                            if (control.m_FacingRight)
                            {
                                if (Mathf.Sign(weapon.transform.localScale.x) < 0)
                                { Scale.x *= -1; }
                            }
                            else
                            {
                                if (Mathf.Sign(weapon.transform.localScale.x) > 0)
                                { Scale.x *= -1; }
                            }
                            weapon.transform.localScale = Scale;
                            weapon.control = control;
                            control.weapon = weapon;
                            Grabbing = true;
                            weapon.IsGrabbed = true;
                            weapon.weaponslot = PosSlot;
                        }
                    }
                }

            }
            else
            {                
                RaycastHit2D hitInfo = Physics2D.Raycast(PosSlot.position, PosSlot.right);
                Weapon weapon = hitInfo.transform.GetComponent<Weapon>();
                weapon.control = null;
                control.weapon = null;
                Grabbing = false;
                weapon.IsGrabbed = false;
                weapon.Throw(ForceThrowRight * multiplier, ForceThrowUp, ThrowTime);
            }
        }
    }
}
