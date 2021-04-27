using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe que gerencia os disparos e a anima��o de atirar
/// </summary>

[RequireComponent(typeof(Animator))]
public class Armas : MonoBehaviour
{
    public GameObject municaoPrefab;                // armazena o prefab de municao
    static List<GameObject> municaoPiscina;         // pool de municoes
    public int tamanhoPiscina;                      // tamanho do pool de muni�oes
    public float velocidadeArma;                          // Velocidade Arma

    bool atirando;                                      // Flag que indica se esta atirando
    [HideInInspector]
    public Animator animator;                           // Armazena Animator do player

    Camera cameraLocal;                                 // Armazena a camera local

    float slopePositivo;        // Armazena coef. angular da reta que cruza os cantos superior direito e inferior esquerdo
    float slopeNegativo;        // Armazena coef. angular da reta que cruza os cantos superior esquerdo e inferior direito

    // Enum de quadrantes
    enum Quadrante { 
        Leste,
        Sul,
        Oeste,
        Norte
    }

    private void Start()
    {
        animator = GetComponent<Animator>();                            // Obtem o Animator do player
        atirando = false;                                               // Inicializa "atirando" como false
        cameraLocal = Camera.main;
        Vector2 abaixoEsquerda = cameraLocal.ScreenToWorldPoint(new Vector2(0,0)); // Posi�ao do canto inferior esquerdo
        Vector2 acimaDireita = cameraLocal.ScreenToWorldPoint(new Vector2(Screen.width,Screen.height)); // Posi�ao do canto superior direito
        Vector2 acimaEsquerda = cameraLocal.ScreenToWorldPoint(new Vector2(0, Screen.height)); // Posi�ao do canto superior esquerdo 
        Vector2 abaixoDireita = cameraLocal.ScreenToWorldPoint(new Vector2(Screen.width, 0));  // Posi�ao do canto inferior direito
        slopePositivo = PegaSlope(abaixoEsquerda,acimaDireita);         // Obtem coef. angular positivo
        slopeNegativo = PegaSlope(acimaEsquerda,abaixoDireita);         // Obtem coef. angular negativo
    }

    // Metodo que retorna true se o player est� acima do slope positivo
    bool AcimaSlopePositivo(Vector2 posicaoEntrada)
    {
        Vector2 posicaoPlayer = gameObject.transform.position;                          // Pega posi�ao do player
        Vector2 posicaoMouse = cameraLocal.ScreenToWorldPoint(posicaoEntrada);          // Pega posi�ao do mouse
        float interseccaoY = posicaoPlayer.y - (slopePositivo * posicaoPlayer.x);       // Inteseccao da pos. do player com o slope positivo
        float entradaInterseccao = posicaoMouse.y - (slopePositivo * posicaoMouse.x);   // Interseccao da pos. do mouse com o slope positivo
        return entradaInterseccao > interseccaoY; // Retorna se a interseccao do mouse > intreseccao do player 
    }

    // Metodo que retorna true se o player est� acima do slope negativo
    bool AcimaSlopeNegativo(Vector2 posicaoEntrada)
    {
        Vector2 posicaoPlayer = gameObject.transform.position;                          // Pega posi�ao do player
        Vector2 posicaoMouse = cameraLocal.ScreenToWorldPoint(posicaoEntrada);          // Pega posi�ao do mouse
        float interseccaoY = posicaoPlayer.y - (slopeNegativo * posicaoPlayer.x);       // Inteseccao da pos. do player com o slope negativo
        float entradaInterseccao = posicaoMouse.y - (slopeNegativo * posicaoMouse.x);   // Interseccao da pos. do mouse com o slope negativo
        return entradaInterseccao > interseccaoY;       // Retorna se a interseccao do mouse > intreseccao do player
    }

    Quadrante PegaQuadrante()
    {
        Vector2 posicaoMouse = Input.mousePosition;                     // Pega a posi�ao do mouse
        Vector2 posicaoPlayer = transform.position;                     // Pega a posi�ao do player
        bool acimaSlopePositivo = AcimaSlopePositivo(posicaoMouse);     // Checa se o mouse esta acima do slope positivo 
        bool acimaSlopeNegativo = AcimaSlopeNegativo(posicaoMouse);     // Checa se o mouse esta acima do slope negativo
        // Retorna o quadrante em que o mouse est� com base em se o mouse esta acima/ baixo do slope negetivo/positivo
        if (!acimaSlopePositivo && acimaSlopeNegativo)
        {
            return Quadrante.Leste;
        }
        else if (!acimaSlopePositivo && !acimaSlopeNegativo)
        {
            return Quadrante.Sul;
        }
        else if (acimaSlopePositivo && !acimaSlopeNegativo)
        {
            return Quadrante.Oeste;
        }
        else
        {
            return Quadrante.Norte;
        }
    }

    // Atualiza o estado de anima��o do player durante o disparo
    void UpdateEstado()
    {
        if (atirando)                                           // Se esta atirando
        {
            Vector2 vetorQuadrante;                             
            Quadrante quadranteEnum = PegaQuadrante();      // Pega o quadrante em que se encontra o mouse
            // Monta um vetor de dire�oes (x,y) com a dire�ao para qual o player virar� 
            switch (quadranteEnum)                      
            {
                case Quadrante.Leste:
                    vetorQuadrante = new Vector2(1f,0f);         // 
                    break;
                case Quadrante.Sul:
                    vetorQuadrante = new Vector2(0f, -1f);
                    break;
                case Quadrante.Oeste:
                    vetorQuadrante = new Vector2(-1f, 0f);
                    break;
                case Quadrante.Norte:
                    vetorQuadrante = new Vector2(0f, 1f);
                    break;
                default:
                    vetorQuadrante = new Vector2(0f, 0f);
                    break;
            }
            animator.SetBool("Atirando",true);                              // atirando passa para true
            animator.SetFloat("AtiraX",vetorQuadrante.x);         // Ajusta o parametro do Animator "atiraX" de acordo com o vetorQuadrante
            animator.SetFloat("AtiraY", vetorQuadrante.y);        // Ajusta o parametro do Animator "atiraY" de acordo com o vetorQuadrante
            atirando = false;                                     // Muda flag atirando para false
        }
        else
        {
            animator.SetBool("Atirando",false);                   // P�ra anima��o de atirar
        }
    }

    public void Awake()
        {
            if(municaoPiscina == null)                                  // Se a muni�ao for nula...
            {
                municaoPiscina = new List<GameObject>();                // Cria um pool vazio de municao
            }
            for (int i=0;i<tamanhoPiscina;i++)                          // Para cada muni�ao na piscina...
            {
                GameObject municaoO = Instantiate(municaoPrefab);       // Instancia um projetil
                municaoO.SetActive(false);                              // Desativa o projetil
                municaoPiscina.Add(municaoO);                           // Adiciona o projetil ao pool
            }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))                            // Se o mouse for clicado...
        {
            atirando = true;                                        // Muda "atirando" para true
            DisparaMunicao();                                       // Chama fun�a de instanciar o projetil
        }
        UpdateEstado();                                             // Atualiza estado da anima�ao do player
    }

    // Retorna o coef. angular da reta que crua os dois ponros passados
    float PegaSlope(Vector2 ponto1,Vector2 ponto2)
    {
        return (ponto1.y - ponto2.y) / (ponto1.x - ponto2.x);
    }

    // Instancia os projeteis
    GameObject SpawnMunicao(Vector3 posicao)
    {
        foreach(GameObject municao in municaoPiscina)                   // Para cada projetil no pool de muni�ao
        {
            if(municao.activeSelf == false)                             // Se o gameobject do projetil esta desativado...
            {
                municao.SetActive(true);                                // Ativa objeto do projetil
                municao.transform.position = posicao;                   // Muda posi�ao do projetil para a pos~i�ao recebida
                return municao;                                         // retorna a muni��o instanciada
            }
        }
        return null;
    }

    // Metodo que verifica a posi�ao do mouse (destino) e inicia a corrotina de calcular a trajetoria do projetil at� o destino 
    void DisparaMunicao()
    {
        Vector3 posicaoMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Pega a pois�ao de destino
        GameObject municao = SpawnMunicao(transform.position);  // Instancia o projetil na posi�ao do player
        if(municao != null)                                     // Se o projetil nao � nulo...
        {
            Arco arcoScript = municao.GetComponent<Arco>();     // Pega o componente Arco do projetil
            float duracaoTrajetoria = 1f / velocidadeArma;      // Calcula a dura�ao da trajetoria
            StartCoroutine(arcoScript.arcoTrajetoria(posicaoMouse,duracaoTrajetoria));  // Inicia a corrotina que controla a trajetoria do projetil
        }
    }

    // Ao ser destruido
    private void OnDestroy()
    {
        municaoPiscina = null;          // Atribui null ao projetil
    }
}
