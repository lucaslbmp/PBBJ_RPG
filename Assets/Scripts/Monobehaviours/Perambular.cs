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
    public bool perseguePlayer;       // indicador de se o caractere é perseguidor ou nao
    public bool movimentoOrtogonal;   // indicador de se o caractere se movimenta apenas em direçoes ortogonais ou nao

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
        animator = GetComponent<Animator>();
        velocidadeCorrente = velocidadePerambular;
        rb2D = GetComponent<Rigidbody2D>();
        PerambularCoroutine = StartCoroutine(RotinaPerambular());
        circleCollider = GetComponent<CircleCollider2D>();
        //posicaoFinal = transform.position;
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
            EscolhaNovoAngulo(movimentoOrtogonal);
            EscolhaNovoPontoFinal();
            if (movimentoOrtogonal)
            {
                AtualizaDirecaoAnimacao(anguloAtual);
            }
            if(MoverCoroutine != null)
            {
                StopCoroutine(MoverCoroutine);
            }
            MoverCoroutine = StartCoroutine(Mover(rb2D,velocidadeCorrente));
            yield return new WaitForSeconds(intervaloMudancaDirecao);
        }
    }

    void EscolhaNovoAngulo(bool movimentoOrtogonal)
    {
        if (movimentoOrtogonal)
        {
            anguloAtual = (int)(Random.Range(0,4))*90f;
        }
        else
        {
            anguloAtual += Random.Range(0, 360);
            anguloAtual = Mathf.Repeat(anguloAtual, 360);
        }
        print(anguloAtual);
    }

    public virtual void EscolhaNovoPontoFinal()
    {
        //print("Entrou");
        //anguloAtual += Random.Range(0, 360);
        //anguloAtual = Mathf.Repeat(anguloAtual, 360);
        //print(anguloAtual);
        //if (!perseguePlayer) // Se o Inimigo for o NPC (Ancião)...
        //{
        //    anguloAtual -= (int)anguloAtual % 90; //... arrendondar o angulo para multiplos de 90°
        //    print(anguloAtual);
        //}
        posicaoFinal = transform.position + Vetor3ParaAngulo(anguloAtual); // Alterado: vetor posiçao randomico sempre parte da posiçao do Inimigo
        print(posicaoFinal);
    }
    void AtualizaDirecaoAnimacao(float angulo)
    {
        float anguloRad = angulo * Mathf.Deg2Rad;
        float dirX = Mathf.Cos(anguloRad);
        float dirY = Mathf.Sin(anguloRad);
        print((dirX, dirY));
        animator.SetFloat("dirX",dirX);
        animator.SetFloat("dirY", dirY);
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

    public void Retornar(bool retornar)
    {
        if (retornar)
        {
            if (PerambularCoroutine != null)
            {
                StopCoroutine(PerambularCoroutine);
            }
            //if(VoltarCoroutine != null)
                VoltarCoroutine = StartCoroutine(Voltar());
        }
        else
        {
            if (VoltarCoroutine != null)
            {
                StopCoroutine(VoltarCoroutine);
            }
            if (PerambularCoroutine != null)
                PerambularCoroutine = StartCoroutine(RotinaPerambular());
        }
        
    }

    public IEnumerator Voltar()
    {
        while (true)
        {
            anguloAtual += 180;
            anguloAtual = Mathf.Repeat(anguloAtual, 360);
            posicaoFinal = transform.position + Vetor3ParaAngulo(anguloAtual);
            MoverCoroutine = StartCoroutine(Mover(rb2D, velocidadeCorrente));
            yield return new WaitForSeconds(intervaloMudancaDirecao);
            PerambularCoroutine = StartCoroutine(RotinaPerambular());
        }
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
            if (MoverCoroutine != null)
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
