using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
public class Perambular : MonoBehaviour
{
    public float velocidadePerseguicao; // velocidade do inimigo na area de spot
    public float velocidadePerambular; // velocidade do inimigo passeando
    float velocidadeCorrente; // velocidade do inimigo atribuida

    public float intervaloMudancaDirecao; // tempo para alterar direcao
    public bool perseguePlayer;       // indicador de se o caractere � perseguidor ou nao
    public bool movimentoOrtogonal;   // indicador de se o caractere se movimenta apenas em dire�oes ortogonais ou nao

    Coroutine MoverCoroutine;               // Corrotina que controla o movimento do inimigo
    Coroutine PerambularCoroutine;
    Coroutine VoltarCoroutine;

    Rigidbody2D rb2D;                      // atmazena o componente rigidbody2D
    Animator animator;                     // armazena o componente Animator

    Transform alvoTransform = null;        // armazena o componente transform do alvo (player ou outro)
    Vector3 posicaoFinal;
    float anguloAtual = 0;

    CircleCollider2D circleCollider;      // armazena circulo de spot

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();                            // Obtem Animator do caractere
        velocidadeCorrente = velocidadePerambular;                      // Inicializa a velocidade atual como a vel. de perambular
        rb2D = GetComponent<Rigidbody2D>();                             // Obtem rigidbody do caractere
        PerambularCoroutine = StartCoroutine(RotinaPerambular());       // Inicializa a corrotina de perambular
        circleCollider = GetComponent<CircleCollider2D>();              // Obtem o componente CircleCollider2D
        //posicaoFinal = transform.position;
    }

    private void OnDrawGizmos()
    {
        if(circleCollider != null)                                              // Se o CircleCollider n�o � nulo...
        {
            Gizmos.DrawWireSphere(transform.position,circleCollider.radius); // Desenha circunferencia com o radio do CircleCollider
        }
    }

    // Corrotina que controla o movimento de perambular
    public IEnumerator RotinaPerambular()
    {
        while (true)
        {
            EscolhaNovoPontoFinal();                    // Escolhe novo ponto final aleat�rio
            if (movimentoOrtogonal)                     // Se o caractere se movimenta de maneira ortogonal (ex.: NPC)
            {
                AtualizaDirecaoAnimacao(anguloAtual);   // Atualiza a anima�ao do caractere para coincidir com a dire�ao de movimento
            }
            if(MoverCoroutine != null)                  // Se a corrotina de movimento est� sendo executada...
            {
                StopCoroutine(MoverCoroutine);            // Para a corrotina de movimento
            }
            MoverCoroutine = StartCoroutine(Mover(rb2D,velocidadeCorrente));        // Inicia a corrotina de movimento com a velocidade atual
            yield return new WaitForSeconds(intervaloMudancaDirecao);               // Aguarda um intervalo
        }
    }

    // Metodo que escolhe um novo angulo aleatorio para o qual o caractere caminhar� se ele estiver no modo de perambular

    public virtual void EscolhaNovoPontoFinal()
    {
        EscolhaNovoAngulo(movimentoOrtogonal);      // Escolhe 
        posicaoFinal = transform.position + Vetor3ParaAngulo(anguloAtual); // Alterado: vetor posi�ao randomico sempre parte da posi�ao do Inimigo
    }

    void EscolhaNovoAngulo(bool movimentoOrtogonal)
    {
        if (movimentoOrtogonal)                             // Se o caractere se movimenta ortogonalmente
        {
            anguloAtual = (int)(Random.Range(0, 4)) * 90f;     // Angulo atual recebe um novo angulo multiplo de 90�
        }
        else                                                // Caso contrario...
        {
            anguloAtual += Random.Range(0, 360);            // Angulo atual recebe um incremento de 0 a 360� 
            anguloAtual = Mathf.Repeat(anguloAtual, 360);   // Repete-se at� que o angulo estja entre o angulo atual e 360�
        }
        //print(anguloAtual);
    }

    // Metodo que recebe a dire�ao (angulo) do caractere e atualiza a anima�ao do caractere dependendo da dire�ao 
    void AtualizaDirecaoAnimacao(float angulo)
    {
        float anguloRad = angulo * Mathf.Deg2Rad;
        float dirX = Mathf.Cos(anguloRad);
        float dirY = Mathf.Sin(anguloRad);
        animator.SetFloat("dirX",dirX);
        animator.SetFloat("dirY", dirY);
    }

    // Recebe um angulo e retorna um vetor com este angulo 
    Vector3 Vetor3ParaAngulo(float anguloEntradaGraus)
    {
        float anguloEntradaRadianos = anguloEntradaGraus * Mathf.Deg2Rad;   // Transforma o angulo de graus para radianos 
        return new Vector3(Mathf.Cos(anguloEntradaRadianos),Mathf.Sin(anguloEntradaRadianos),0); // Retorna um vetor com o angulo de entrada
    }

    // Corrotina que gerencia o movimento do player
    public IEnumerator Mover(Rigidbody2D rbParaMover,float velocidade)
    {
        float distanciaFaltante = (transform.position - posicaoFinal).sqrMagnitude;     // Calcula distance faltante at� o destino
        while(distanciaFaltante > float.Epsilon)                         // Enquanto a ditsancia faltante for maior que zero
        {
            if (alvoTransform != null)                          // Se o transform do alvo n�o for nulo...
            {
                posicaoFinal = alvoTransform.position;          // posi��o final recebe a posi��o do alvo
            }
            if(rbParaMover != null)                             // Se o rigidBody do inimigo nao for nulo...
            {
                animator.SetBool("Caminhando",true);            // Alterar flag "Caminhando" do Animator para true
                // Nova posi��o recebe uma posi�ao intermediaria entre a posi��o do caractere e o destino
                Vector3 novaPosicao = Vector3.MoveTowards(rbParaMover.position,posicaoFinal,velocidade*Time.deltaTime); 
                rb2D.MovePosition(novaPosicao);                             // Move o rigidbody do inimigo at� a nova posi��o
                distanciaFaltante = (transform.position - posicaoFinal).sqrMagnitude;       // Atualiza a distancia faltante
            }
            yield return new WaitForFixedUpdate();                          // Aguarda o FixedUpdate
        }
        animator.SetBool("Caminhando",false);               // Muda a flag Caminhando para false
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && perseguePlayer)     // Se colidiu com o player e � do tipo que persegue o player
        {
            velocidadeCorrente = velocidadePerseguicao;             // Velocidade atual recebe a velocidade de persegui�ao
            alvoTransform = collision.gameObject.transform;         // Transform do alvo recebe o transform do player
            if (MoverCoroutine != null)                             // Se a corrotina de movimento for nula...
            {
                StopCoroutine(MoverCoroutine);                      // Para corrotina de movimento
            }
            MoverCoroutine = StartCoroutine(Mover(rb2D,velocidadePerseguicao)); // Inicia corrotina de movimento
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))              // Se colidiu com o player...
        {
            animator.SetBool("Caminhando",false);                   // Muda flag "caminhando" do Animator para false
            velocidadeCorrente = velocidadePerambular;              // Velocidade atual recebe velocidade de perambular
            if (MoverCoroutine != null)                             // Se a corrotina de movimento for nula...
            {
                StopCoroutine(MoverCoroutine);                      // Para corrotina de movimento
            }
            alvoTransform = null;                                   // Tranform do alvo recebe null (sem alvo)
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(rb2D.position,posicaoFinal,Color.red);   // Desenha linha entre a posi�ao do inimigo e a posi�ao final
    }
}
