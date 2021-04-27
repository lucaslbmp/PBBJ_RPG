using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe que gerencia atributos e metodos do player e instancia objetos do player (HealthBar e Inventario)
/// </summary>
/// 
public class Player : Caractere
{
    public Inventario inventarioPrefab; // referência ao objeto prefab criado do inventário
    [HideInInspector] public Inventario inventario;
    public HealthBar healthBarPrefab; // referência ao objeto prefab criado da HealthBar
    HealthBar healthBar;

    public PontosDano pontosDano; // tem o valor da "saúde" do player

    public ItensParaColetar itensParaColetarPrefab;         // Prefab de canvas de itens para coletar
    ItensParaColetar itensParaColetar;                      // Armazena um objeto ItensParaColetar

    private void Start()
    {
        itensParaColetar = Instantiate(itensParaColetarPrefab);         // Instancia canvas de itens para coletar
        itensParaColetar.AtualizarCanvas();                             // Atualiza canvas de itens para coletar 
        inventario = Instantiate(inventarioPrefab);                     // Instancia inventario
        pontosDano.valor = inicioPontosDano;                            // Inicializa pontos de dano 
        healthBar = Instantiate(healthBarPrefab);                       // Instancia HealthBar
        healthBar.caractere = this;                                     // Associa o player ao campo caractere do HealthBar 
    }

    // Corrotina de dano do player
    public override IEnumerator DanoCaractere(int dano, float intervalo)
    {
        while (true)
        {
            StartCoroutine(FlickerCaractere());                 // Inicia corrotina de "piscar"
            pontosDano.valor = pontosDano.valor - dano;         // Decrementa "dano" de "pontos de dano"

            if(pontosDano.valor <= float.Epsilon)               // Se pontos de dano <= 0 ...
            {
                KillCaractere();                                // Mata player
                break;
            }

            if(intervalo > float.Epsilon)                       // Se intervalo > 0 ...
            {
                yield return new WaitForSeconds(intervalo);     // Aguarda "intervalo"
            }
            else
            {
                break;
            }
        }
    }

    // Metodo que gerencia a morte do player
    public override void KillCaractere()
    {
        base.KillCaractere();               // Executa o metodo base (de Character)
        Destroy(healthBar.gameObject);      // Destroi a Healthbar
        Destroy(inventario.gameObject);     // Destroi o inventario
        GameManager.CarregarGameOver();     // Carregar tela de game over
    }

    // Metodo que remove o player
    public void RemoveCaractere()
    {
        base.KillCaractere();               // Executa o metodo base (de Character)
        Destroy(healthBar.gameObject);      // Destroi a Healthbar
        Destroy(inventario.gameObject);     // Destroi o inventario
    }

    // Metodo que reseta o player
    public override void ResetCaractere()
    {
        inventario = Instantiate(inventarioPrefab);         // Instancia inventario 
        healthBar = Instantiate(healthBarPrefab);           // Instancia HealthBar
        healthBar.caractere = this;                         // Associa o player ao caractere da Healthbar
        pontosDano.valor = inicioPontosDano;                // Inicializa pontos de dano do player
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coletavel"))                   // Se colidiu com um coletavel...
        {
            Item DanoObjeto = collision.gameObject.GetComponent<Consumable>().item; // Obtem o item do objeto coletavel...
            if(DanoObjeto != null)                               // Se o item nao for nulo...
            {
                bool DeveDesaparecer = false; // Flag que sinaliza se o objeto com o qual se colidiu deve ser destruído ao final do evento de colisão
                //print("Acertou: " + DanoObjeto.NomeObjeto);
                switch (DanoObjeto.tipoItem) // confere o tipo de coletável com o qual o player colidiu
                {
                    case Item.TipoItem.MOEDA:               // Se o tipo do item for moeda...
                    case Item.TipoItem.DIAMANTE:            // Se o tipo do item for diamante...
                    case Item.TipoItem.CHAVE:               // Se o tipo do item for chave...
                    case Item.TipoItem.LIVRO:               // Se o tipo do item for livro...
                    case Item.TipoItem.PERGAMINHO:          // Se o tipo do item for pergaminho...
                    case Item.TipoItem.COROA:               // Se o tipo do item for coroa...
                        DeveDesaparecer = inventario.AddItem(DanoObjeto); // adicionar moeda ao inventario
                        break;

                    case Item.TipoItem.HEALTH:
                        DeveDesaparecer = AjustePontosDano(DanoObjeto.quantidade); // ajustar saude do player
                        break;
                    default:
                        break;
                }
                if (DeveDesaparecer) 
                {
                    collision.gameObject.SetActive(false); // desabilita (remove) o gameobject com o qual o player colidiu
                }
                GameManager.itensColetados = inventario.itensColetados;   // atualizando dicionario de itens coletados em GameManager
                itensParaColetar.AtualizarCanvas();     // Atualiza o canvas de itens para coletar
            }
        }
    }

    // Metodo que incrementa a saude do player por uma quantidade
    public bool AjustePontosDano(int quantidade)
    {
        if (pontosDano.valor < MaxPontosDano)   // Se a saude é inferior à saude maxima do player
        {
            pontosDano.valor = pontosDano.valor + quantidade;   // Aumenta os pontos de vida do player de "quantidade"
            print("Ajustando PD por: " + quantidade + ". Novo Valor = " + pontosDano.valor);
            return true;        // Retorna true (aumentou saude do player)
        }
        else
            return false;       // Retorna false (nao aumentou saude do player)
    }

    // Retorna inventario
    public Inventario GetInventario()
    {
        return inventario;
    }
}
