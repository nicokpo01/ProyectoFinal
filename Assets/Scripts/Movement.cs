using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
       
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
    void Update ()
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
    }
}
