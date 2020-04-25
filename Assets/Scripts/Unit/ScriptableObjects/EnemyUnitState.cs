using UnityEngine;
using System.Collections;

namespace SG.Unit
{
    [CreateAssetMenu(fileName = "EnemyUnitState", menuName = "States/EnemyUnitState")]
    public class EnemyUnitState : ScriptableObject
    {
        [Header("Movables")]
        public bool _facingRight = false;

        [Header("Health/Damage")]
        public float _health;
        public float _damage;

    }
}
