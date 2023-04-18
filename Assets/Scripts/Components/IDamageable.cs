using System;
using UnityEngine.Events;

namespace Scripts.Components
{
    public interface IDamageable
    {
        void TakeDamage(int damage);
    }
}