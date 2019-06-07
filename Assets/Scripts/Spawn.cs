using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public float time = 5;
    public float size = 1;

    public List<GameObject> players;
    public List<Transform> pos;
    private List<GameObject> Find;
    private List<bool> doing;

    // Use this for initialization
    void Start()
    {
        Find = new List<GameObject>();
        doing = new List<bool>();

        int i = 1;
        foreach (GameObject a in players)
        {
            doing.Add(false);
            string str = "Player " + i;
            Find.Add(GameObject.FindGameObjectWithTag(str));
            i++;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        int cont = 1;
        int i = 0;
        foreach (GameObject a in players)
        {
            string str = "Player " + cont;
            Find[i] = GameObject.FindGameObjectWithTag(str);
            if (Find[i] != null)
            {
                doing[i] = false;
            }

            if (Find[i] == null && !doing[i])
            {
                StartCoroutine(spawn(players[i], i));
                doing[i] = true;
            }
            cont++;
            i++;
        }
    }

    IEnumerator spawn(GameObject @object, int i)
    {
        yield return new WaitForSeconds(time);

        GameObject obj = Instantiate(players[i], pos[i].position, Quaternion.identity);
        Vector3 scale = obj.transform.localScale;
        scale.x = size;
        scale.y = size;
        obj.transform.localScale = scale;

        doing[i] = false;
    }
}
