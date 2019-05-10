using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour {

    public Transform posAtacc;
    public LayerMask QueObjeto;
    public float attacRange;
    public int daño;
    public GameObject efecto;
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.C))
        {
            Collider2D[] Weapons = Physics2D.OverlapCircleAll(posAtacc.position, attacRange, QueObjeto);
            int largo = Weapons.Length;
            var i = Random.Range(0, Weapons.Length);
            Weapon Item = Weapons[i].GetComponent<Weapon>();
            Item.IsGrabbed = true;
            Item.weaponslot = posAtacc;
        }
            /*
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
         */         
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(posAtacc.position, attacRange);
    }
}
