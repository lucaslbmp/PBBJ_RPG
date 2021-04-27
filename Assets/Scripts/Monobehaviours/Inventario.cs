using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe que gerencia o inventario
/// </summary>

public class Inventario : MonoBehaviour
{
    public GameObject slotPrefab; // objeto que recebe o prefab slot
    public const int numSlots = 5; //numero fixo de slots
    Image[] itemImagens = new Image[numSlots]; // array de imagens
    [HideInInspector] public Item[] itens = new Item[numSlots]; // array de itens
    GameObject[] slots = new GameObject[numSlots]; // array de Slots

    public Dictionary<string, int> itensColetados = new Dictionary<string, int>(); // dicionario de itens coletados pelo player na cena atual

    void Start()
    {
        CriaSlots();                                // Cria slots vazios
        ResetInventario();                          // reseta inventario
    }

    // Metodo que cria slots vazios
    public void CriaSlots()
    {
        if(slotPrefab != null)                                  // Se o prefab do slot nao é nulo...
        {
            for(int i = 0; i < numSlots; i++)                   // Para cada slot...
            {
                GameObject novoSlot = Instantiate(slotPrefab);  // Instancia um novo slot
                novoSlot.name = "ItemSlot_" + i;                // Nomeia o novo slot 
                novoSlot.transform.SetParent(gameObject.transform.GetChild(0).transform); // Configura o gameObject Fundo do Inentario como pai do slot
                slots[i] = novoSlot;                        // Associa o novo slot ao array de slots
                itemImagens[i] = novoSlot.transform.GetChild(1).GetComponent<Image>();      // Associa a imagem do slot ao array de imagens de itens 
            }
        }
    }

    // Adiciona um item ao Inventario
    public bool AddItem(Item itemToAdd)
    {
        for(int i=0; i<itens.Length; i++)                               // Para cada item do array itens...
        {
            if(itens[i]!=null && itens[i].tipoItem == itemToAdd.tipoItem && itemToAdd.empilhavel == true)
             // Se o item nao é nulo (já tem no inventario), tem o mesmo tipo do item a ser adicionado e é empilhavel...
            {
                itens[i].quantidade = itens[i].quantidade + 1;                  // Incrementa a quantidade do item
                Slot slotScript = slots[i].gameObject.GetComponent<Slot>();     // Pega o componente slot do elemento correspondente no array de slots
                Text quantidadeTexto = slotScript.qtdTexto;     // Pega o texto de quantidade de itens do Slot
                quantidadeTexto.enabled = true;                 // Habilita o texto
                quantidadeTexto.text = itens[i].quantidade.ToString("00");      // Atualiza o texto com a quantidade atual do item
                AtualizarListaColetaveis(itemToAdd.tipoItem.ToString(), itens[i].quantidade);   // Atualiza a lista de itens coletados
                GameManager.ImprimeListaColetaveis(itensColetados);     // Imprime Lista de itens coletados
                return true;                                            // Retorna true (adicionou o item)
            }
            else if(itens[i] == null)                // Caso contrario, chegar a um item null (nao adicionado)... 
            {
                itens[i] = Instantiate(itemToAdd);                          // Instancia o item a ser adicionado
                itens[i].quantidade = 1;                                    // Inicializa a quantidade do item em null
                itemImagens[i].sprite = itemToAdd.sprite;                   // Associa a sprite do item a ser adicionado ao array de imagens de itens
                itemImagens[i].enabled = true;                              // Habilita a imagem do item no array
                Slot slotScript = slots[i].gameObject.GetComponent<Slot>(); // Pega o componete slot do slot de index correpondente
                Text quantidadeTexto = slotScript.qtdTexto;                 // Pega o texto de quantidade do item no slot
                quantidadeTexto.enabled = true;                             // habilia o texto de qtde do item
                quantidadeTexto.text = itens[i].quantidade.ToString("00");  // Atualiza o texto de quantidade do item
                AtualizarListaColetaveis(itemToAdd.tipoItem.ToString());    // Atualiza a lista de itens coletados
                GameManager.ImprimeListaColetaveis(itensColetados);         // Imprime Lista de itens coletados
                return true;                                                // Retorna true (adicionou o item)
            }
        }
        return false;                           // Retorna true (adicionou o item)
    }

    // Remove um item do Inventario
    public void RemoveItem(Item itemToRemove)
    {
        for (int i = 0; i < itens.Length; i++)                                   // Para cada item no array de itens...
        {
            if(itens[i] != null)                                                // Se o item nao for nulo...
            {
                if (itens[i].tipoItem == itemToRemove.tipoItem)                 // Se o tipo do item que deve ser removido é igual ao tipo do item do array...
                {
                    itens[i] = null;                                            // O item recebe null
                    itemImagens[i].enabled = false;                             // Desabilita a imagem do item
                    Slot slotScript = slots[i].gameObject.GetComponent<Slot>(); // Pega script Slot do slot correpondente
                    Text quantidadeTexto = slotScript.qtdTexto;                 // Pega o texto de quantidade de itens do slot selecionado
                    quantidadeTexto.enabled = false;                            // Desabilita o texto de quantidade
                }
            }
        }
    }

    // Atualiza lista de itens coletados
    public void AtualizarListaColetaveis(string tipoColetavel)
    {
        if (!itensColetados.ContainsKey(tipoColetavel))         // Se a lista de itens coletados nao contem uma chave igual a "tipoColetavel"
            itensColetados.Add(tipoColetavel, 1);               // Adiciona elemento no dicionario com tipo coletavel e quantidade=1
        else                                                    // Caso contrario...
            itensColetados.Remove(tipoColetavel);               // Remove o elemento do dicionario com a chave "tipoColetavel"
    }

    // Atualiza lista de itens coletados
    public void AtualizarListaColetaveis(string tipoColetavel, int quantidade)
    {
        if (tipoColetavel != "HEALTH" && itensColetados.ContainsKey(tipoColetavel)) // Se o colotavel nao é vida...
        {
            itensColetados[tipoColetavel] = quantidade;             // Atualizo a quantidade do tipoColetavel na lista
        }
    }

    // Reseta inventario
    public void ResetInventario()
    {
        for (int i = 0; i < itens.Length; i++)          // Para cada item no array de itens...
        {
            itens[i] = null;                            // item recebe null
        }
        itensColetados = new Dictionary<string, int>();     // Reseta dicionario de itens coletados
    }
}
