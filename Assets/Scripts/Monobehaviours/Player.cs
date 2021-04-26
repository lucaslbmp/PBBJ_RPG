using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Caractere
{
    public Inventario inventarioPrefab; // referência ao objeto prefab criado do inventário
    [HideInInspector] public Inventario inventario;
    public HealthBar healthBarPrefab; // referência ao objeto prefab criado da HealthBar
    HealthBar healthBar;

    public PontosDano pontosDano; // tem o valor da "saúde" do player

    public ItensParaColetar itensParaColetarPrefab;
    ItensParaColetar itensParaColetar;

    private void Start()
    {
        itensParaColetar = Instantiate(itensParaColetarPrefab);
        itensParaColetar.AtualizarCanvas();
        inventario = Instantiate(inventarioPrefab);
        pontosDano.valor = inicioPontosDano;
        healthBar = Instantiate(healthBarPrefab);
        healthBar.caractere = this;
    }

    public override IEnumerator DanoCaractere(int dano, float intervalo)
    {
        while (true)
        {
            StartCoroutine(FlickerCaractere());
            pontosDano.valor = pontosDano.valor - dano;

            if(pontosDano.valor <= float.Epsilon)
            {
                KillCaractere();
                break;
            }

            if(intervalo > float.Epsilon)
            {
                yield return new WaitForSeconds(intervalo);
            }
            else
            {
                break;
            }
        }
    }

    public override void KillCaractere()
    {
        base.KillCaractere();
        Destroy(healthBar.gameObject);
        Destroy(inventario.gameObject);
    }

    public override void ResetCaractere()
    {
        inventario = Instantiate(inventarioPrefab);
        healthBar = Instantiate(healthBarPrefab);
        healthBar.caractere = this;
        pontosDano.valor = inicioPontosDano;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coletavel"))
        {
            Item DanoObjeto = collision.gameObject.GetComponent<Consumable>().item;
            if(DanoObjeto != null)
            {
                bool DeveDesaparecer = false; // Flag que sinaliza se o objeto com o qual se colidiu deve ser destruído ao final do evento de colisão
                //print("Acertou: " + DanoObjeto.NomeObjeto);
                switch (DanoObjeto.tipoItem) // confere o tipo de coletável com o qual o player colidiu
                {
                    //case Item.TipoItem.MOEDA:
                    //    // DeveDesaparecer = true;
                    //    DeveDesaparecer = inventario.AddItem(DanoObjeto); // adicionar moeda ao inventario
                    //    break;
                    case Item.TipoItem.MOEDA:
                    case Item.TipoItem.DIAMANTE:
                    case Item.TipoItem.CHAVE:
                    case Item.TipoItem.LIVRO:
                    case Item.TipoItem.PERGAMINHO:
                    case Item.TipoItem.COROA:
                        DeveDesaparecer = inventario.AddItem(DanoObjeto); // adicionar moeda ao inventario
                        break;

                    case Item.TipoItem.HEALTH:
                        DeveDesaparecer = AjustePontosDano(DanoObjeto.quantidade); // ajustar saude do player
                        break;
                    //case Item.TipoItem.DIAMANTE:
                    //    DeveDesaparecer = inventario.AddItem(DanoObjeto); // adicionar diamante ao inventario
                    //    break;
                    //case Item.TipoItem.CHAVE:
                    //    DeveDesaparecer = inventario.AddItem(DanoObjeto); // adicionar chave ao inventario
                    //    break;
                    //case Item.TipoItem.PERGAMINHO:
                    //    DeveDesaparecer = inventario.AddItem(DanoObjeto); // adicionar pergaminho ao inventario
                    //    break;
                    default:
                        break;
                }
                if (DeveDesaparecer) 
                {
                    collision.gameObject.SetActive(false); // desabilita (remove) o gameobject com o qual o player colidiu
                }
                GameManager.itensColetados = inventario.itensColetados;   // atualizando dicionario de itens coletados em GameManager
                itensParaColetar.AtualizarCanvas();
            }
        }
    }

    public bool AjustePontosDano(int quantidade)
    {
        if (pontosDano.valor < MaxPontosDano)
        {
            pontosDano.valor = pontosDano.valor + quantidade;
            print("Ajustando PD por: " + quantidade + ". Novo Valor = " + pontosDano.valor);
            return true;
        }
        else
            return false;
    }

    public Inventario GetInventario()
    {
        return inventario;
    }
}
