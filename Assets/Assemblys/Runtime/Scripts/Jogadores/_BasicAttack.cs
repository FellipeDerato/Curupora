using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

public class _BasicAttack : MonoBehaviour
{
    public BoolGameEvent BasicAttackEvent;
    public Vec2GameEvent SelfKnockbackEvent; //Direction and Force
    public FloatGameEvent AttackDamageEvent;
    public Player_Parameters _parameters;


    public bool offCooldown = true;
    bool Stuned;
    public float direction;
    public float knockbackDirection;
    public bool DirectionRight;
    bool LastXDirectionLeft;
    public Vector2 inputDirection;

    public Collider2D[] hits;

    public List<Transform> DamageableTargets;

    Vector2 boxOrigin;
    Vector2 boxSize;

    public void PlayerStunned(bool state)
    {
        Stuned = state;
    }

    IEnumerator AttackCoroutine()
    {
        BasicAttackEvent.TriggerEvent(true);
        float timerDuration = _parameters.basicAttackDuration;
        AttackRotation(inputDirection);
        offCooldown = false;
        
        while (timerDuration > 0)
        {
            Check_CollisionOverlapBox();
            timerDuration -= Time.deltaTime;
            yield return null;
        }

        
        BasicAttackEvent.TriggerEvent(false);
        float timerCooldown = _parameters.basicAttackCooldown - _parameters.basicAttackDuration;

        while (timerCooldown > 0)
        {
            FixRotation(inputDirection.x);
            timerCooldown -= Time.deltaTime;
            yield return null;
        }
        
        
        offCooldown = true;
        DamageableTargets.Clear();
        AttackRotation(inputDirection);
    }

    public void AttackKey(bool state)
    {
        if (state && offCooldown && !Stuned)
        {
            StartCoroutine(AttackCoroutine());            
        }
    }

    public void InputDirection(Vector2 input)
    {
        inputDirection = input;        
        AttackRotation(inputDirection);
    }

    void FixRotation(float last)
    {
        if (last != 0)
        {
            DirectionRight = last > 0;
        }
    }

    public void AttackRotation(Vector2 input)
    {
        if (!offCooldown) { return; }

        switch (input.y)
        {
            case > 0f:
                direction = -90; // Up
                break;
            case < 0f:
                direction = 90; // Down
                break;
            default:
                direction = 0; // Straight
                break;
        }
        if (input.x != 0)
        {
            DirectionRight = input.x > 0;
        }

        transform.localRotation = Quaternion.Euler(0, 0, direction);        
    }

    public void AttackBox()
    {
        float boxSizeX = _parameters.basicAttackBoxSize.x;
        float offset = -(boxSizeX * 0.5f) + _parameters.basicAttackBoxOffsetX;

        switch (direction)
        {
            case -90: //Up
                boxOrigin = (Vector2)transform.position + new Vector2(0, -offset + .45f);
                boxSize = new Vector2(_parameters.basicAttackBoxSize.y, _parameters.basicAttackBoxSize.x);
                knockbackDirection = 3;
                break;

            case 90: //Down
                boxOrigin = (Vector2)transform.position + new Vector2(0, offset - .65f);
                boxSize = new Vector2(_parameters.basicAttackBoxSize.y, _parameters.basicAttackBoxSize.x);
                knockbackDirection = 4;
                break;
                
            default:
                if (DirectionRight) //Right
                {
                    boxOrigin = (Vector2)transform.position + new Vector2(-offset, 0);
                    knockbackDirection = 2;
                }
                else //Left
                {
                    boxOrigin = (Vector2)transform.position + new Vector2(offset, 0);
                    knockbackDirection = 1;
                }
                boxSize = _parameters.basicAttackBoxSize;
                break;
        }
    }

    public bool Check_CollisionOverlapBox()
    {
        if (offCooldown) { return false; }
        AttackBox();
        hits = Physics2D.OverlapBoxAll(boxOrigin, boxSize, transform.eulerAngles.z);

        if (hits.Length > 0)
        {
            HitTarget();
        }
        else if (DamageableTargets.Count > 0)
        {
            DamageableTargets.Clear();
        }

        return hits.Length > 0;
    }

    public void HitTarget()
    {
        foreach (var col in hits)
        {
            if (col == null) continue;
            if (col.gameObject.layer.Equals(8) && !DamageableTargets.Contains(col.transform))
            {
                DamageableTargets.Add(col.transform);
                AttackDamageEvent?.TriggerEvent(_parameters.basicAttackDamage);
                SelfKnockback("damageable");
            }
            else if (col.gameObject.layer.Equals(3))
            {
                SelfKnockback("player");
            }
            else if (col.gameObject.layer.Equals(6) && hits.Contains(col))
            {
                SelfKnockback("ground");
            }
        }
    }

    

    public void SelfKnockback(string thing)
    {
        if (knockbackDirection == 4 && thing == "damageable")
        {
            SelfKnockbackEvent.TriggerEvent(new Vector2(knockbackDirection, _parameters.SelfKnockback_strong));
        }
        else if (knockbackDirection == 4 && thing == "player")
        {
            SelfKnockbackEvent.TriggerEvent(new Vector2(knockbackDirection, _parameters.SelfKnockback_strong));
        }
        else if (knockbackDirection < 4 && thing != "player" && thing != "damageable")
        {
            SelfKnockbackEvent.TriggerEvent(new Vector2(knockbackDirection, _parameters.SelfKnockback_weak));
        } 
    }
   
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxOrigin, boxSize);
    }
    

}
