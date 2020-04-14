using UnityEngine;
using System.Collections;

namespace SG.Unit
{
    [CreateAssetMenu(fileName = "PlayerUnitState", menuName = "States/PlayerUnitState")]

    public class PlayerUnitState : ScriptableObject
    {
        [Header("Movables")]
        public float _walkingSpeed = 9f;
        public bool _facingRight = false;

        [Header("Health/Damage")]
        public float _health;
        public float _damage;
    }
}
