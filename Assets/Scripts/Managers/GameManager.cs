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
        Rpg_lvl_03,
        Rpg_Congratulations
    }
    public static Nivel nivelAtual; 

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
        /// Retorna o n�vel correspondete � cena atual
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
        /// Fun��o que conta quantos colet�veis existem na cena
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
        /// Fun��o que exibe um dicionario de colet�veis com suas respectivas quantidades
        foreach (KeyValuePair<string, int> c in coletaveis)
        {
            print(c);
        }
    }

    public bool PegouColetaveisTodos() 
    {
        /// Fun�ao que detecta se o player pegou todos os coletaveis da cena
        return dicionarioColetaveis.Count == itensColetados.Count && 
            !dicionarioColetaveis.Except(itensColetados).Any() &&
            dicionarioColetaveis.Count > 0 && itensColetados.Count > 0; // compara os dicionarios de itens coletados e itens na cena
    }

    public static void ExibirMensagem(string mensagem)
    {
        mensagemCanvas.AtualizarCanvas(mensagem);
    }

    public void  CarregarInicio()
    {
        SceneManager.LoadScene("Lab5_RPGSetup");
    }

    public void CarregarFaseUm()
    {
        SceneManager.LoadScene(Enum.GetName(typeof(Nivel),1));
    }

    public void CarregarCreditos()
    {
        SceneManager.LoadScene("RPG_Creditos");
    }

    public static void CarregarGameOver()
    {
        SceneManager.LoadScene("RPG_GameOver");
    }

    // Update is called once per frame
    void Update()
    {
        if (PegouColetaveisTodos())                             // Se o player pegou todos os coletaveis na cena
        {
            Nivel novoNivel = ++nivelAtual;                    // novoNivel recebe o pr�ximo n�vel
            //if (!Enum.IsDefined(typeof(Nivel), novoNivel+1))   // Checa se o n�vel subsequente ao proximo n�vel nao existe, i. e., se o proximo nivel � o ultimo
            //{
            //    print("Entrei");
            //    Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();      // Encontra o player
            //    player.RemoveCaractere();                                                               // Remove o player e seus objetos instanciados
            //}
            itensColetados = new Dictionary<string, int>();                                             // reseta o dicionario de itens coletados           
            SceneManager.LoadScene(novoNivel.ToString());                                               // carrega pr�xima fase
        }
    }
}
