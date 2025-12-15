using System.Collections;
using UnityEngine;

public class _Health : MonoBehaviour
{
    #region Smart Inspector Attributes
    [Header("Smart Inspector Attributes")]

    public string CustomName;
    public Texture2D CustomIcon;
    public Colors CustomCollor;
    public enum Colors
    {
        white, red, green, blue, black, gray, yellow, cyan, magenta, clear
    }
    #endregion

    public Player_Parameters _parameters;
    public BoolGameEvent OnDeath;

    public bool staggered;

    public void DamageTaken(int damage)
    {
        if (staggered) { return; }
        StartCoroutine(GettingHit(damage));
    }

    public IEnumerator GettingHit(int damage)
    {
        staggered = true;
        _parameters.health -= damage;

        float timerStagger = _parameters.HitStagger;
        while (timerStagger > 0)
        {


            timerStagger -= Time.deltaTime;
            yield return null;
        }

        if (_parameters.health < 0)
        {
            _parameters.health = 0;
            Death();
        }

        staggered = false;
    }

    public void Stagger()
    {

    }

    public void Death()
    {
        OnDeath.TriggerEvent(true);
    }
}
