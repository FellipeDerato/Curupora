using UnityEngine;

public class Camera_Principal : MonoBehaviour
{
    public Jogador_Controles j1;
    public Jogador_Controles j2;

    [Header("Posição da Câmera")]
    public float distanciaCamera = -10;

    [Header("Zoom")]
    public float distanciaMaximaEntrePlayersHorizontal = 15f;
    public float distanciaMaximaEntrePlayersVertical = 8.5f;
    public float tamanhoMinimo = 5f;
    public float tamanhoMaximo = 20f;
    public float zoomSpeed = 3f;

    public float distanciaX;
    public float distanciaY;
    

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        // Centraliza a câmera entre os dois jogadores
        Vector3 media = (j1.transform.position + j2.transform.position) / 2f;
        transform.position = new Vector3(media.x, media.y, distanciaCamera);

        // Calcula distância entre os jogadores
        distanciaX = Mathf.Abs(j1.transform.position.x - j2.transform.position.x);
        distanciaY = Mathf.Abs(j1.transform.position.y - j2.transform.position.y);

        // Define o tamanho ideal da câmera
        float tamanhoIdeal = tamanhoMinimo;
        float tamanhoIdealX = tamanhoMinimo;
        float tamanhoIdealY = tamanhoMinimo;

        if (distanciaX > distanciaMaximaEntrePlayersHorizontal)
        {
            // Quanto maior a distância, mais a câmera abre
            tamanhoIdealX = Mathf.Clamp((distanciaX) / 3, tamanhoMinimo, tamanhoMaximo);
        }
        if (distanciaY > distanciaMaximaEntrePlayersVertical)
        {
            // Quanto maior a distância, mais a câmera abre
            tamanhoIdealY = Mathf.Clamp((distanciaY) / 1.7f, tamanhoMinimo, tamanhoMaximo);
        }

        tamanhoIdeal = Mathf.Max(tamanhoIdealX, tamanhoIdealY);

        // Faz o zoom suave
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, tamanhoIdeal, zoomSpeed * Time.deltaTime);
    }
    void Update()
    {
        
    }
}
