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
            Collider2D[] Objetos = Physics2D.OverlapCircleAll(posAtacc.position, attacRange, QueObjeto);
            Instantiate(efecto, posAtacc.position, Quaternion.identity);
            Random rnd = new Random();
            int largo = Objetos.Length;
            var i = Random.Range(0, Objetos.Length);
           
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(posAtacc.position, attacRange);
    }
}
