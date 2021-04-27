using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe que gerencia a o movimenyo do player e as animações de movimento
/// </summary>

public class MovimentaPlayer : MonoBehaviour
{

    public float VelocidadeMovimento = 3.0f; // equivale ao momento (impulso) a ser dado ao player
    Vector2 Movimento = new Vector2(); // detectar movimento pelo teclado

    Animator animator; // guarda a componente do Controlador de Animação
    //string estadoAnimacao = "EstadoAnimacao";         // Guarda o nome do parâmetro de Animação       // Desnecessario com o Blend Tree 

    Rigidbody2D rb2D; // guarda a componente CorpoRígido do Player

  /*  enum EstadosCaractere                     // Desnecessario com o Blend Tree 
    {
        andaLeste = 1,
        andaOeste = 2,
        andaNorte = 3,
        andaSul = 4,
        idle = 5
    }*/

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();                        // Obtem o Animator do player
        rb2D = GetComponent<Rigidbody2D>();                         // Obtem o componente Corpo Rigido (RigidBody) do player
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEstado();                                         // Atualiza a animação do player
    }

    private void FixedUpdate()
    {
        MoveCaractere();                                        // Move o corpo rígido do player
    }

    // Funçao que recebe os inputs de movimento e movimenta o player
    private void MoveCaractere()
    {
        Movimento.x = Input.GetAxisRaw("Horizontal");            // Pega o input de direçao x (A/D ou LArrow/RArrow)
        Movimento.y = Input.GetAxisRaw("Vertical");              // Pega o input de direçao y (W/S ou UpArrow/DownArrow)
        Movimento.Normalize();                                   // Normaliza o vetor resultante
        rb2D.velocity = Movimento * VelocidadeMovimento;        // Atribui ao vetor de  velocidade um vetor na direçao de movimento de modulo velocidadeMovimento
    }

    // Funçao que atualiza os parametros da blend Tree de Movimento do player 
    private void UpdateEstado()
    {
        if(Mathf.Approximately(Movimento.x,0) && Mathf.Approximately(Movimento.y, 0))   // Se movimento ~ (0,0)
        {
            animator.SetBool("Caminhando",false);            // Altero o bool "Caminhando" do Animator para false 
        }
        else                                                 // Caso contrario...
        {
            animator.SetBool("Caminhando", true);           // Altero o bool "Caminhando" do Animator para true 
        }
        animator.SetFloat("dirX",Movimento.x);        // Atribui o valor de movimento.x ao parametro de direção X do Animator 
        animator.SetFloat("dirY", Movimento.y);       // Atribui o valor de movimento.y ao parametro de direção Y do Animator
    }

    /*
    private void UpdateEstado()
    {
        
        if(Movimento.x > 0)
        {
            animator.SetInteger(estadoAnimacao, (int)EstadosCaractere.andaLeste);
        }
        else if(Movimento.x < 0)
        {
            animator.SetInteger(estadoAnimacao, (int)EstadosCaractere.andaOeste);
        }
        else if (Movimento.y > 0)
        {
            animator.SetInteger(estadoAnimacao, (int)EstadosCaractere.andaNorte);
        }
        else if (Movimento.y < 0)
        {
            animator.SetInteger(estadoAnimacao, (int)EstadosCaractere.andaSul);
        }
        else
        {
            animator.SetInteger(estadoAnimacao, (int)EstadosCaractere.idle);
        }
    }*/
}
