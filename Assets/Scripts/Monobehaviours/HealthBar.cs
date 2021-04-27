using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe que gerencia a barra de vida
/// </summary>

public class HealthBar : MonoBehaviour
{
    public PontosDano pontosDano; //Objeto de leitura dos dados de quantos pontos tem o player
    public Player caractere; // receberá o objeto do player
    public Image medidorImagem; //recebe a barra de medição
    public Text pdTexto; // recebe os dados de PD
    float maxPontosDano; // armazena a quantidade limite de "saúde" do Player

    //private void Awake()
    //{
    //    DontDestroyOnLoad(this.gameObject);
    //}

    void Start()
    {
        //print(caractere);
        maxPontosDano = caractere.MaxPontosDano;            // Inicializa os pontos de dano do caractere
    }

    // Update is called once per frame
    void Update()
    {
        if(caractere != null)                                           // Se o caractere nao é nulo...
        {
            medidorImagem.fillAmount = pontosDano.valor / maxPontosDano;    // Atualizo o medidor (barra) da HealthBar
            pdTexto.text = "PD: " + (medidorImagem.fillAmount * 100);       // Atualizo o texto que mostra o valor de saude no caractere
        }
        
    }
}
