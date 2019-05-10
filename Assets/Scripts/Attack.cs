using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    public float Recovery;
    public float TiempoInicio;

    public Transform posAtacc;
    public LayerMask QueEnemigo;
    public float attacRange;
    public int daño;
    public GameObject efecto;

    // Update is called once per frame

    void FixedUpdate ()
    {
        if (Recovery <= 0)
        {            
            if (Input.GetKey(KeyCode.X))
            {
                Recovery = TiempoInicio;
                Collider2D[] Enemigos = Physics2D.OverlapCircleAll(posAtacc.position, attacRange, QueEnemigo);
                Instantiate(efecto, posAtacc.position, Quaternion.identity);
                for (int i = 0 ; i < Enemigos.Length; i++)
                {
                    Enemigos[i].GetComponent<Enemy>().TakeDamage(daño);
                    
                }
            }
        }
        else
        {
            Recovery -= Time.deltaTime;
        }
	}

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(posAtacc.position, attacRange);
    }
}
