using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PontoSpawn : MonoBehaviour
{
    public GameObject prefabParaSpawn;

    public float intervaloRepeticao;
   
    void Start()
    {
        if(intervaloRepeticao > 0)
        {
            //print(this.gameObject);
            InvokeRepeating("SpawnO", 0.0f, intervaloRepeticao);
        }
    }

    public GameObject SpawnO()
    {
        if(prefabParaSpawn != null && ContarObjetosNaCena()<12)
        {
            return Instantiate(prefabParaSpawn, transform.position, Quaternion.identity);
        }
        return null;
    }

    int ContarObjetosNaCena()
    {
        string tag = prefabParaSpawn.tag;
        return GameObject.FindGameObjectsWithTag(tag).Length; // retorna quantidade de objetos com a tag do prefab na cena
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
