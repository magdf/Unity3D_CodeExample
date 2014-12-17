using System.Collections.Generic;

public class WarriorBehaviour_Normal : BaseUnitBehaviour
{
    private readonly List<Checkpoint> _traversedCheckpoints = new List<Checkpoint>();

    void Start()
    {
        
    }

    private void OnCheckpointEnter(Checkpoint checkPoint)
    {
        Actions.AIAction.TryChangeTarget(this, _mediator.RichAI, checkPoint, _traversedCheckpoints);
    }

    private void OnFinalCheckpointEnter(Checkpoint checkPoint)
    {
        if (!enabled)
            return;
        EventAggregator.PublishT(GameEvent.OnPlayerDamage, this, new Damage(DamageType.Close, _mediator.Stats.AttackPower));//send damage to player
        EventAggregator.PublishT(GameEvent.OnRemoveUnitFromGame, this, _mediator.Stats.SpawnRate);
        Destroy(gameObject); //delete unit because he came to the fort.
    }
}
