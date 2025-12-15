using UnityEngine;

[CreateAssetMenu(fileName = "Player Parameters", menuName = "Player/Player Parameters")]
public class Player_Parameters : ScriptableObject
{

    [Header("PLAYER STATS")]
    public int health = 5;
    public int maxHealth = 5;

    public float HitStagger = 0.5f;
    public enum Personagem
    {
        Curupira,
        Caipora
    }
    public Personagem PersonagemSelecionado;


    [Header("GRAVIDADE")]
    public float fallClamp = -40f;
    public float maxFallSpeed = 120f;
    public float fallGravityMultiplier = 3f;
    public Vector2 groundBoxSize = new Vector2(1f, 1f);
    public Vector2 groundBoxCenter = new Vector2(0f, -1f);
    public LayerMask groundLayer;

    [Header("ANDAR")]
    public float acceleration = 60f;
    public float moveClamp = 8f;
    public float desacelerar = 60f;
    public float apiceHorizontalBonus = 2f;
    public float smallThreshold = 0.01f;

    [Header("PULO")]
    public float jumpHeight = 8f;
    public float jumpApexThreshold = 10f;
    public float coyoteTimeThreshold = 0.1f;
    public float jumpBuffer = 0.1f;
    public float jumpEndEarlyGravityModifier = 20f;

    [Header("BASIC ATTACK")]
    public float basicAttackCooldown = 0.3f;
    public float basicAttackDuration = 0.2f;
    public float basicAttackDamage = 1f;
    public float SelfKnockback_weak = 1f;
    public float SelfKnockback_strong = 8f;
    public float DealtKnockback = 4f;
    public Vector2 basicAttackBoxSize = new Vector2(2.5f, 1.5f);
    public float basicAttackBoxOffsetX = 1.4f;
    public LayerMask damagebleLayer;

    [Header("HEALTH")] // all health related parameters in a game like hollow knight
    public float invincibilityDuration = 1f;
    public float damageKnockback = 8f;
}