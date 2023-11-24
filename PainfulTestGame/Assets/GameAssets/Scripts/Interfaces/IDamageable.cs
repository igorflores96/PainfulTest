using UnityEngine.Events;

public interface IDamageable
{
    UnityEvent OnShipDestroyed { get; set; }

    void TakeDamage(float damageValue);
}
