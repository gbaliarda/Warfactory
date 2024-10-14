using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour, IPotion
{

    public PotionStats PotionStats => potionStats;

    public IBuffable Owner => owner;

    public float DurationLeft => durationLeft;

    public float CooldownLeft => cooldownLeft;

    [SerializeField] protected Item _potionItem;
    [SerializeField] protected PotionStats potionStats;
    [SerializeField] protected IBuffable owner;
    protected float durationLeft;
    protected float cooldownLeft;
    private bool _buffActive = false;
    private BuffCommand _buffCommand;

    public void Buff()
    {
        if (InventoryManager.Instance.GetAmountOfItem(_potionItem) == 0) return;
        if (cooldownLeft > 0f) return;
        EventQueueManager.Instance.AddEvent(_buffCommand);
        _buffActive = true;
        SetDuration(potionStats.PotionDuration);
        SetCooldown(potionStats.PotionCooldown);

        InventoryManager.Instance.ConsumeItem(_potionItem, 1);
    }

    public virtual void SetCooldown(float cooldown) => cooldownLeft = cooldown;
    public virtual void SetDuration(float duration) => durationLeft = duration;

    void Start()
    {
        cooldownLeft = 0f;
        _buffActive = false;
        owner = GetComponentInParent<IBuffable>();
        _buffCommand = new BuffCommand(owner, this);
    }

    void Update()
    {
        durationLeft -= Time.deltaTime;
        cooldownLeft -= Time.deltaTime;
        if (_buffActive && durationLeft <= 0)
        {
            EventQueueManager.Instance.AddUndoEvent(_buffCommand);
            _buffActive = false;
        }
    }
}
