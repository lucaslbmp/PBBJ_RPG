using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instanciaCompartilhada = null;
    public RPGCameraManager cameraManager;

    public PontoSpawn playerPontoSpawn;

    [HideInInspector]public static Dictionary<string, int> dicionarioColetaveis;
    [HideInInspector]public static Dictionary<string, int> itensColetados;

    public MensagemCanvas mensagemCanvasPrefab;
    [HideInInspector] public static MensagemCanvas mensagemCanvas;

    public enum Nivel
    {
        None,
        Rpg_lvl_01,
        Rpg_lvl_02,
        Rpg_lvl_03
    }
    public Nivel nivelAtual; 

    private void Awake()
    {
        if(instanciaCompartilhada != null && instanciaCompartilhada != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instanciaCompartilhada = this;
        }
        nivelAtual = GetNivelAtual();
        //DontDestroyOnLoad(this.gameObject);
        dicionarioColetaveis = new Dictionary<string, int>();
        itensColetados = new Dictionary<string, int>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ContarColetaveis();
        SetupScene();
    }

    public void SetupScene()
    {
        /// Configura a cena
        mensagemCanvas = Instantiate(mensagemCanvasPrefab);
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        /// Instancia o player
        if(playerPontoSpawn != null)
        {
            GameObject player = playerPontoSpawn.SpawnO();
            cameraManager.virtualCamera.Follow = player.transform;
        }
    }

    Nivel GetNivelAtual()
    {
        /// Retorna o nível correspondete à cena atual
        string nomeCenaCorrente = SceneManager.GetActiveScene().name;
        foreach (Nivel nivel in Enum.GetValues(typeof(Nivel)))
        {
            //print(nivel);
            if (nivel.ToString().Equals(nomeCenaCorrente))
            {
                return nivel;
            }
        }
        return Nivel.None;
    }

    void ContarColetaveis()
    {
        /// Função que conta quantos coletáveis existem na cena
        GameObject[] coletaveis = GameObject.FindGameObjectsWithTag("Coletavel");
        foreach (GameObject c in coletaveis)
        {
            Item item = c.GetComponent<Consumable>().item;
            string tipoColetavel = item.tipoItem.ToString();
            if (tipoColetavel != "HEALTH")
            {
                if (dicionarioColetaveis.ContainsKey(tipoColetavel))
                {
                    dicionarioColetaveis[tipoColetavel]++;
                }
                else
                {
                    dicionarioColetaveis.Add(tipoColetavel, 1);
                }
            }
        }
        //ImprimeListaColetaveis(dicionarioColetaveis);
    }

    public static void ImprimeListaColetaveis(Dictionary<string,int> coletaveis)
    {
        /// Função que exibe um dicionario de coletáveis com suas respectivas quantidades
        foreach (KeyValuePair<string, int> c in coletaveis)
        {
            print(c);
        }
    }

    public bool PegouColetaveisTodos() 
    {
        /// Funçao que detecta se o player pegou todos os coletaveis da cena
        return dicionarioColetaveis.Count == itensColetados.Count && 
            !dicionarioColetaveis.Except(itensColetados).Any() &&
            dicionarioColetaveis.Count > 0 && itensColetados.Count > 0; // compara os dicionarios de itens coletados e itens na cena
    }

    public static void ExibirMensagem(string mensagem)
    {
        mensagemCanvas.AtualizarCanvas(mensagem);
    }

    // Update is called once per frame
    void Update()
    {
        //ImprimeListaColetaveis(dicionarioColetaveis);
        //inventario = player.GetInventario();
        //ImprimeListaColetaveis(itensColetados);
        if (PegouColetaveisTodos()) // Se o player pegou todos os coletaveis na cena
        {
            nivelAtual++; // altera para próxima fase
            //print(nivelAtual);
            itensColetados = new Dictionary<string, int>(); // reseta o dicionario de itens coletados
            Destroy(GameObject.Find("PlayerPontoSpawn")); // destroi o spawnpoint do player 
            SceneManager.LoadScene(nivelAtual.ToString()); // carrega próxima fase
        }
    }
}
