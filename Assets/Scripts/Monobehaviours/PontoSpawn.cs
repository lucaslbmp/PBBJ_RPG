using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe que realiza o spawn de caracteres
/// </summary>

public class PontoSpawn : MonoBehaviour
{
    public GameObject prefabParaSpawn;                  // Prefab para fazer spawn

    public float intervaloRepeticao;                    // Intervalo a cada qual o spawn acontece
   
    void Start()
    {
        if(intervaloRepeticao > 0)                              // Se o intervalo é maior que zero
        {
            //print(this.gameObject);
            InvokeRepeating("SpawnO", 0.0f, intervaloRepeticao);    // Invocar SpawnO a cada "intervaloRepeticao"
        }
    }

    // Metodo que instancia o gameobject do caractere
    public GameObject SpawnO()
    {
        if(prefabParaSpawn != null && ContarObjetosNaCena()<12)   // Se o prefab de caractere nao for nulo e o n° de objetos na cena é menor que 12
        {
            return Instantiate(prefabParaSpawn, transform.position, Quaternion.identity);   // Instancia o gameobject do caractere a ser spawnado
        }
        return null;        // retorna null
    }

    // Metodo que conta os objetos do prefab na cena
    int ContarObjetosNaCena()
    {
        string tag = prefabParaSpawn.tag;                   // Pega o tag do prefab para spawn
        return GameObject.FindGameObjectsWithTag(tag).Length; // retorna quantidade de objetos com a tag do prefab na cena
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
