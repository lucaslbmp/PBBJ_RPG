using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armas : MonoBehaviour
{
    public GameObject municaoPrefab;                // armazena o prefab de municao
    static List<GameObject> municaoPiscina;         // pool de municoes
    public int tamanhoPiscina;                      // tamanho do pool de muniçoes
    public float velocidadeArma;                          // Velocidade Arma

    public void Awake()
    {
        if(municaoPiscina == null)
        {
            municaoPiscina = new List<GameObject>();
        }
        for (int i=0;i<tamanhoPiscina;i++)
        {
            GameObject municaoO = Instantiate(municaoPrefab);
            municaoO.SetActive(false);
            municaoPiscina.Add(municaoO);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DisparaMunicao();
        }
    }

    GameObject SpawnMunicao(Vector3 posicao)
    {
        foreach(GameObject municao in municaoPiscina)
        {
            if(municao.activeSelf == false)
            {
                municao.SetActive(true);
                municao.transform.position = posicao;
                return municao;
            }
        }
        return null;
    }

    void DisparaMunicao()
    {
        Vector3 posicaoMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject municao = SpawnMunicao(transform.position);
        if(municao != null)
        {
            Arco arcoScript = municao.GetComponent<Arco>();
            float duracaoTrajetoria = 1f / velocidadeArma;
            StartCoroutine(arcoScript.arcoTrajetoria(posicaoMouse,duracaoTrajetoria));
        }
    }

    private void OnDestroy()
    {
        municaoPiscina = null;
    }
}
