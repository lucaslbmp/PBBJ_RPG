using UnityEngine;
using Cinemachine;

/// <summary>
/// Classe que gerencia a camera
/// </summary>

public class RPGCameraManager : MonoBehaviour
{
    public static RPGCameraManager instanciaCompartilhada = null;       // Inicia instancia compartilhada em null

    [HideInInspector]
    public CinemachineVirtualCamera virtualCamera;      // Recebe Virtual Camera

    private void Awake()
    {
        if(instanciaCompartilhada != null && instanciaCompartilhada != this)        // Se instancia compartilhada nao é null e nao é este script...
        {
            Destroy(gameObject);                                           // Destrua este gameobject
        }
        else                                                                    // Caso contrario...
        {
            instanciaCompartilhada = this;                                      // Instancia compartilhada recebe este gameobject
        }

        GameObject vCamGameObject = GameObject.FindWithTag("Virtual Camera");           // Encontra a VCam
        virtualCamera = vCamGameObject.GetComponent<CinemachineVirtualCamera>();        // Encontra o componente CinemachineVirtualCamera de VCam
        DontDestroyOnLoad(this.gameObject);         // Destroi este gameobject
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
