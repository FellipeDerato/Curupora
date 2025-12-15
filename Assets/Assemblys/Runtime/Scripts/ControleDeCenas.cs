using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static PortasSaidas;

public class ControleDeCenas : MonoBehaviour
{
    public static ControleDeCenas Instance;
    [SerializeField] Animator anim_Transicao;
    public float TempoTransicao = .4f;
    private bool cdtransicao = false;
    public float CooldownTransicao = .6f;

    private void Awake()
    {
        anim_Transicao.gameObject.SetActive(true);
        if (Instance == null)
        {

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ProximaSala(string Sala, string NomePorta)
    {
        if (cdtransicao) { return; }

        StartCoroutine(CarregarSala(Sala, NomePorta));

    }

    IEnumerator CarregarSala(string Sala, string NomePorta)
    {
        //começa a animação, bloqueia mais transicoes de cenas
        cdtransicao = true;
        anim_Transicao.SetTrigger("End");

        yield return new WaitForSeconds(TempoTransicao);

        //Troca de cena, Esperar carregar para mostrar a cena
        SceneManager.LoadScene(Sala);
        AsyncOperation carregamento = SceneManager.LoadSceneAsync(Sala);
        carregamento.allowSceneActivation = false;
        yield return new WaitUntil(() => carregamento.progress >= 0.9f);
        carregamento.allowSceneActivation = true;
        yield return new WaitUntil(() => carregamento.isDone);


        foreach (Transform t in PortasSaidas.instance.PortasDaCena) //Teleporte Players para a porta de chegada
        {
            if (t != null)
            {
                
                if (t.name == NomePorta.Replace(" (UnityEngine.SceneAsset)", ""))
                {
                    Jogador_Controles.j1.transform.position = t.position;
                    Jogador_Controles.j2.transform.position = t.position;

                    PortasSaidas ps = t.GetComponent<PortasSaidas>();
                    anim_Transicao.SetTrigger("Start");
                    switch (ps.TipoDePorta)
                    {
                        case Tipo.Direita:
                            Jogador_Controles.j1.horizontalForcado = 1;
                            Jogador_Controles.j2.horizontalForcado = 1;

                            yield return new WaitForSeconds(ps.TempoMovimentoForcado);
                            Jogador_Controles.j1.horizontalForcado = 0;
                            Jogador_Controles.j2.horizontalForcado = 0;
                            break;

                        case Tipo.Esquerda:
                            Jogador_Controles.j1.horizontalForcado = -1;
                            Jogador_Controles.j2.horizontalForcado = -1;

                            yield return new WaitForSeconds(ps.TempoMovimentoForcado);
                            Jogador_Controles.j1.horizontalForcado = 0;
                            Jogador_Controles.j2.horizontalForcado = 0;
                            break;

                        case Tipo.SubirD:
                            Jogador_Controles.j1.rb.linearVelocityY = ps.Subida_forca;
                            Jogador_Controles.j1.horizontalForcado = .5f;
                            Jogador_Controles.j2.rb.linearVelocityY = ps.Subida_forca;
                            Jogador_Controles.j2.horizontalForcado = .5f;

                            yield return new WaitForSeconds(ps.Subida_tempo);
                            Jogador_Controles.j1.horizontalForcado = 0;
                            Jogador_Controles.j2.horizontalForcado = 0;
                            yield return new WaitForSeconds(ps.TempoMovimentoForcado - ps.Subida_tempo);                            
                            break;

                        case Tipo.SubirE:
                            Jogador_Controles.j1.rb.linearVelocityY = ps.Subida_forca;
                            Jogador_Controles.j1.horizontalForcado = -.5f;
                            Jogador_Controles.j2.rb.linearVelocityY = ps.Subida_forca;
                            Jogador_Controles.j2.horizontalForcado = -.5f;

                            yield return new WaitForSeconds(ps.Subida_tempo);
                            Jogador_Controles.j1.horizontalForcado = 0;
                            Jogador_Controles.j2.horizontalForcado = 0;
                            yield return new WaitForSeconds(ps.TempoMovimentoForcado - ps.Subida_tempo);
                            break;

                        case Tipo.Cair:
                            yield return new WaitForSeconds(ps.TempoMovimentoForcado);
                            break;

                        case Tipo.Entrar:
                            // por animação do j1 de entrando em porta
                            yield return new WaitForSeconds(ps.TempoMovimentoForcado);
                            break;

                    }



                }
            }
        }
        Jogador_Controles.j1.stagger_Porta = false;
        Jogador_Controles.j2.stagger_Porta = false;

        yield return new WaitForSeconds(CooldownTransicao);

        //desbloqueia as transicoes de cenas
        cdtransicao = false;
    }

}
