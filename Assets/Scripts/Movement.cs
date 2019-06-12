using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public CharacterController2D control;

    public LayerMask MaskWeapon;

    public GameObject dummy;

    public Transform TopTransform;
    public Transform BottomTransform;
    public Transform PosSlot;
    private Animator Anim;
    Rigidbody2D rb;

    private bool Grabbing = false;
    bool salto = false;

    public int multiplier;

    private Weapon last_weapon;

    public int Healt = 3;

    public float tilt;
    public float ForceThrowUp = 0.2f;
    public float ForceThrowRight = 0.7f;
    public float ThrowTime = 0.05f;

    public float GrabRange;

    //public float fallmultiplier = 2.5f;
    public float lowjumpmultiplier = 2f;

    public float VelocidadMovimiento;
    public float MovimientoHorizontal;

    private string StrJump = "Jump";
    private string StrHorizontal = "Horizontal";
    private string StrGrab = "Grab/interact";


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (gameObject.tag == "Player 2")
        {
            StrHorizontal += " 2";
            StrJump += " 2";
            StrGrab += " 2";
        }
        Anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        MovimientoHorizontal = Input.GetAxisRaw(StrHorizontal) * VelocidadMovimiento;
        if (Input.GetButtonDown(StrJump))
        {
            salto = true;
            Anim.SetTrigger("Salto");
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

        /*
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallmultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {*/
        rb.velocity += Vector2.up * Physics2D.gravity.y * (lowjumpmultiplier - 1) * Time.fixedDeltaTime;
       // }
        

        if (Input.GetButtonDown(StrGrab))
        {
            if (Grabbing == false)
            {
                last_weapon = null;
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
                DropWeapon(true);
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
        weapon.objectTag = gameObject.tag;
        last_weapon = weapon;
    }

    public void DropWeapon(bool withDrop)
    {
        if (last_weapon != null)
        {
            last_weapon.control = null;
            control.weapon = null;
            Grabbing = false;
            last_weapon.IsGrabbed = false;
            if (withDrop)
            {
                last_weapon.Throw(ForceThrowRight * multiplier, ForceThrowUp, ThrowTime);
            }
        }    
    }

    public void TakeDamage()
    {
     //   Instantiate(EfectoHit, transform.position, Quaternion.identity);
        Healt -= Healt;
        DropWeapon(false);
        //DEATH
        GameObject @object = Instantiate(dummy, transform.position, Quaternion.Euler(0,0,tilt * multiplier));
        @object.GetComponent<Rigidbody2D>().velocity = rb.velocity;
        @object.GetComponent<Rigidbody2D>().AddForce(rb.velocity);
        

        Destroy(gameObject);

        if (Healt > 0)
        {
            //still
        }
        else
        {
            //Lose
        }
    }

    public GameObject TakeDamage(bool yes)
    {
        //   Instantiate(EfectoHit, transform.position, Quaternion.identity);
        Healt -= Healt;
        DropWeapon(false);
        //DEATH
        GameObject retorno = Instantiate(dummy, transform.position, Quaternion.Euler(0,0,tilt * multiplier));
        Destroy(gameObject);

        if (Healt > 0)
        {
            //still
        }
        else
        {
            //Lose
        }
        return retorno;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(TopTransform.position, GrabRange);
        Gizmos.DrawWireSphere(BottomTransform.position, GrabRange);
    }
}

