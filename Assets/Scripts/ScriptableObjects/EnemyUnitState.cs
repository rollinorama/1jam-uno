using UnityEngine;
using System.Collections;

namespace SG.Unit
{
    [CreateAssetMenu(fileName = "EnemyUnitState", menuName = "States/EnemyUnitState")]
    public class EnemyUnitState : ScriptableObject
    {
        [Header("Movables")]
        public float _walkingSpeed = 9f;
        public float _walkingSpeedChase = 9f;
        public bool _facingRight = false;
        public float _idleTime;

        [Header("Health/Damage")]
        public float _health;
        public float _damage;

    }
}
