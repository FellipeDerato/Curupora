using UnityEngine;

public class Jogador_Controles : MonoBehaviour
{
    [Header("INPUT")]
    public string inputHorizontal = "Horizontal";
    public string inputJump = "Jump";
    public string inputVertical = "Vertical";

    [Header("COLLISAO")]
    public Vector2 characterBoundsCenter = Vector2.zero;
    public Vector2 characterBoundsExtent = new Vector2(0.75f, 0.8f);
    public LayerMask groundLayer;
    public float detectionRayLength = 0.1f;

    [Header("GRAVIDADE")]
    public float fallClamp = -40f;
    public float maxFallSpeed = 120f;
    public float fallGravityMultiplier = 3f;

    [Header("ANDAR")]
    public float acceleration = 90f;
    public float moveClamp = 13f;
    public float desacelerar = 60f;
    public float apiceHorizontalBonus = 2f;
    private const float smallThreshold = 0.01f;

    public float horizontalInput;
    public float horizontalForcado;
    private float Direcao;

    [Header("PULO")]
    public float jumpHeight = 8f; 
    public float jumpApexThreshold = 10f;
    public float coyoteTimeThreshold = 0.1f;
    public float jumpBuffer = 0.1f;
    public float jumpEndEarlyGravityModifier = 20f;

    [Header("Stagger")]
    public bool stagger_Porta = false;

    // Privados
    public Rigidbody2D rb;
    private CapsuleCollider2D col;
    private Animator animator;

    public Vector2 velocity;
    private float pontoApice;

    private float lastGroundedTime;
    private float lastJumpPressedTime;

    
    
    public bool isGrounded;
    public bool jumpCut;

    
    public static Jogador_Controles j1;
    public static Jogador_Controles j2;
    public Camera_Principal camera_;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        animator = GetComponentInChildren<Animator>();        
    }
    private void Start()
    {
        if (name == "Jogador 1") { j1 = this; } else { j2 = this; } //define as public Static
    }

    private void Update()
    {
        Inputs();

        isGrounded = ChecarNoChao();

        if (isGrounded)
        {
            lastGroundedTime = Time.time;
            jumpCut = false;
        }
    }

    private void FixedUpdate()
    {
        velocity = rb.linearVelocity;

        CalcularAltoPulo();
        MovimentoHorizontal();
        AplicarGravidade();
        PodePular();
        Animacao();

        Debug.Log("j1 vlcty: " + velocity);

        rb.linearVelocity = velocity;
    }

    #region Movimentacao



    /// <summary>
    /// Todos os Inputs de cena do Jogador
    /// </summary>
    private void Inputs()
    {
        if (stagger_Porta) { horizontalInput = 0; return; }
        horizontalInput = Input.GetAxisRaw(inputHorizontal);
        // Checa se estiver muito longe e bloqueia o movimento
        if (camera_.distanciaX >= camera_.distanciaMaximaEntrePlayersHorizontal * 2.4f)
        {
            if (camera_.j1 == GetComponent<Jogador_Controles>())
            {
                if (transform.position.x > camera_.j2.transform.position.x)
                {
                    horizontalInput = Mathf.Clamp(horizontalInput, -1, 0); // pode ir só pra esquerda
                }
                else
                {
                    horizontalInput = Mathf.Clamp(horizontalInput, 0, 1); // pode ir só pra direita
                }
            }
            else // é o j2
            {
                if (transform.position.x > camera_.j1.transform.position.x)
                {
                    horizontalInput = Mathf.Clamp(horizontalInput, -1, 0); // pode ir só pra esquerda
                }
                else
                {
                    horizontalInput = Mathf.Clamp(horizontalInput, 0, 1); // pode ir só pra direita
                }
            }
        }

        if (Input.GetButtonDown(inputJump))
        {
            lastJumpPressedTime = Time.time;
        }
            
        if (Input.GetButtonUp(inputJump))
        {
            jumpCut = true;
        }
    }

    /// <summary>
    /// Checa se o jogador encostou no chão.
    /// </summary>
    /// <returns>Se for sim ou não no update</returns>
    private bool ChecarNoChao()
    {
        // Posição central dos pés
        Vector2 origin = (Vector2)transform.position + characterBoundsCenter + Vector2.down * (characterBoundsExtent.y + detectionRayLength * 0.5f);

        // Tamanho horizontal normal, altura mínima
        Vector2 footCheckSize = new Vector2(characterBoundsExtent.x * 1.2f, 0.05f);

        RaycastHit2D hit = Physics2D.BoxCast(origin, footCheckSize, 0f, Vector2.down, 0f, groundLayer);
        return hit.collider != null;
    }
    /*
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Vector2 origin = (Vector2)transform.position + characterBoundsCenter + Vector2.down * (characterBoundsExtent.y + detectionRayLength * 0.5f);
        Vector2 footCheckSize = new Vector2(characterBoundsExtent.x * 1.2f, 0.05f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(origin, footCheckSize);
    }
    */



    private void CalcularAltoPulo()
    {
        if (!isGrounded)
            pontoApice = Mathf.InverseLerp(jumpApexThreshold, 0, Mathf.Abs(rb.linearVelocity.y));
        else
            pontoApice = 0;
    }


    /// <summary>
    /// Altera a movimentacao horizontal dos players
    /// </summary>
    private void MovimentoHorizontal()
    {
        if (horizontalForcado != 0) { Direcao = horizontalForcado; } else { Direcao = horizontalInput; }
        if (Mathf.Abs(Direcao) > smallThreshold)
        {
            velocity.x += Direcao * acceleration * Time.fixedDeltaTime;
            float apiceHorizontalBonusSpeed = apiceHorizontalBonus * pontoApice;
            velocity.x += Direcao * apiceHorizontalBonusSpeed * Time.fixedDeltaTime;
        }
        else 
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, desacelerar * Time.fixedDeltaTime);
        }

        velocity.x = Mathf.Clamp(velocity.x, -moveClamp, moveClamp);
    }

    private void AplicarGravidade()
    {
        float gravityScale;

        if (velocity.y > 0) // Subindo
        {
            gravityScale = (jumpCut) ? jumpEndEarlyGravityModifier : 1f;
        }
        else // Caindo
        {
            gravityScale = fallGravityMultiplier;
        }

        velocity.y += Physics2D.gravity.y * gravityScale * Time.fixedDeltaTime;
        velocity.y = Mathf.Clamp(velocity.y, fallClamp, maxFallSpeed);
    }

    public void PodePular()
    {
        if (Time.time - lastGroundedTime <= coyoteTimeThreshold && Time.time - lastJumpPressedTime <= jumpBuffer && !stagger_Porta) { Pulo(); }
    }

    public void Pulo()
    {
        float jumpSpeed = Mathf.Sqrt(2f * Mathf.Abs(Physics2D.gravity.y) * jumpHeight);
        velocity.y = jumpSpeed;

        jumpCut = false;
        lastJumpPressedTime = -999f;
        lastGroundedTime = -999f;
        Debug.Log(jumpSpeed);
    }
   
    #endregion

    #region Animacoes

    private void Animacao()
    {
        if (Mathf.Abs(Direcao) > 0.01f)
        {
            animator.Play("andando");
            // Rotaciona para esquerda ou direita usando EulerAngles
            if (Direcao > 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0); // olhando pra esquerda
            }
            else if (Direcao < 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);   // olhando pra direita
            }
        }
        else
        {
            animator.Play("parado");
        }
    }

    #endregion

    #region Colisoes
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 7)
        {
            jumpCut = false;
            stagger_Porta = true;
            string NomeSala = other.name;
            string Sala = NomeSala.Replace("PortaPara>>>", "");
            PortasSaidas ps = other.GetComponent<PortasSaidas>();
            string NomePorta = "PortaPara>>>" + ps.SaindoDaSala;

            ControleDeCenas.Instance.ProximaSala(Sala, NomePorta);            
        }
    }

    #endregion


}
