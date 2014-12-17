using UnityEngine;
using System.Collections;

public class DamageDecrease_Bonus : BaseBonus
{
    void Start()
    {
        //Invoke("Test", 8);
    }

    public override void RunEffect()
    {
        base.RunEffect();
        PlayerStats.Instance.DefenseFactor = 0.5f;
    }

    protected override void DeleteEffect()
    {
        base.DeleteEffect();
        PlayerStats.Instance.DefenseFactor = 1f;
    }
}
