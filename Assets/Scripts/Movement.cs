using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public CharacterController2D control;

    public LayerMask MaskWeapon;

    public Transform TopTransform;
    public Transform BottomTransform;
    public Transform PosSlot;
    private Animator Anim;
    Rigidbody2D rb;

    private bool Grabbing = false;
    bool salto = false;

    public int multiplier;

    //Testing
    public Weapon Equip;
    //Testing

    public float ForceThrowUp = 0.2f;
    public float ForceThrowRight = 0.7f;
    public float ThrowTime = 0.05f;
    public float GrabRange;
    public float fallmultiplier = 2.5f;
    public float lowjumpmultiplier = 2f;
    public float VelocidadMovimiento;
    public float MovimientoHorizontal;


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
        if (Equip != null)
        {
            EquipWeapon(Equip);
            Equip = null;
        }

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
                Weapon weapon;
                weapon = null;

                Collider2D[] weaponsTop = Physics2D.OverlapCircleAll(TopTransform.position, GrabRange, MaskWeapon);
                Collider2D[] weaponsBot = Physics2D.OverlapCircleAll(BottomTransform.position, GrabRange, MaskWeapon);

                int i;
                
                if (weaponsBot.Length != 0)
                {
                    i = Random.Range(0, weaponsBot.Length);
                    weapon = weaponsBot[i].GetComponent<Weapon>();
                }
                if (weaponsTop.Length != 0)
                {
                    i = Random.Range(0, weaponsTop.Length);
                    weapon = weaponsTop[i].GetComponent<Weapon>();
                }

                if (weapon != null)
                {
                    EquipWeapon(weapon);
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


    public void EquipWeapon(Weapon weapon)
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

        weapon.onStand = false;
        weapon.transform.localScale = Scale;
        weapon.control = control;
        control.weapon = weapon;
        Grabbing = true;
        weapon.IsGrabbed = true;
        weapon.weaponslot = PosSlot;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(TopTransform.position, GrabRange);
        Gizmos.DrawWireSphere(BottomTransform.position, GrabRange);
    }
}

