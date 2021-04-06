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
    public bool perseguePlayer;           // indicador de se o caractere é perseguidor ou nao

    Coroutine MoverCoroutine;               // Corrotina que controla o movimento do inimigo

    Rigidbody2D rb2D;                      // atmazena o componente rigidbody2D
    Animator animator;                     // armazena o componente Animator

    Transform alvoTransform = null;        // armazena o componente transform do alvo (player ou outro)
    Vector3 posicaoFinal;
    float anguloAtual = 0;

    CircleCollider2D circleCollider;      // armazena circulo de spot

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        velocidadeCorrente = velocidadePerambular;
        rb2D = GetComponent<Rigidbody2D>();
        StartCoroutine(RotinaPerambular());
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void OnDrawGizmos()
    {
        if(circleCollider != null)
        {
            Gizmos.DrawWireSphere(transform.position,circleCollider.radius);
        }
    }

    public IEnumerator RotinaPerambular()
    {
        while (true)
        {
            EscolhaNovoPontoFinal();
            if(MoverCoroutine != null)
            {
                StopCoroutine(MoverCoroutine);
            }
            MoverCoroutine = StartCoroutine(Mover(rb2D,velocidadeCorrente));
            yield return new WaitForSeconds(intervaloMudancaDirecao);
        }
    }

    void EscolhaNovoPontoFinal()
    {
        anguloAtual += Random.Range(0, 360);
        anguloAtual = Mathf.Repeat(anguloAtual,360);
        posicaoFinal = Vetor3ParaAngulo(anguloAtual);
    }

    Vector3 Vetor3ParaAngulo(float anguloEntradaGraus)
    {
        float anguloEntradaRadianos = anguloEntradaGraus * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(anguloEntradaRadianos),Mathf.Sin(anguloEntradaRadianos),0);
    }

    public IEnumerator Mover(Rigidbody2D rbParaMover,float velocidade)
    {
        float distanciaFaltante = (transform.position - posicaoFinal).sqrMagnitude;
        while(distanciaFaltante > float.Epsilon)
        {
            if (alvoTransform != null)
            {
                posicaoFinal = alvoTransform.position;
            }
            if(rbParaMover != null)
            {
                animator.SetBool("Caminhando",true);
                Vector3 novaPosicao = Vector3.MoveTowards(rbParaMover.position,posicaoFinal,velocidade*Time.deltaTime);
                rb2D.MovePosition(novaPosicao);
                distanciaFaltante = (transform.position - posicaoFinal).sqrMagnitude;
            }
            yield return new WaitForFixedUpdate();
        }
        animator.SetBool("Caminhando",false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && perseguePlayer)
        {
            velocidadeCorrente = velocidadePerseguicao;
            alvoTransform = collision.gameObject.transform;
            if(MoverCoroutine != null)
            {
                StopCoroutine(MoverCoroutine);
            }
            MoverCoroutine = StartCoroutine(Mover(rb2D,velocidadePerseguicao));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("Caminhando",false);
            velocidadeCorrente = velocidadePerambular;
            if(MoverCoroutine != null)
            {
                StopCoroutine(MoverCoroutine);
            }
            alvoTransform = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(rb2D.position,posicaoFinal,Color.red);
    }
}
