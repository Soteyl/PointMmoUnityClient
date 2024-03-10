using System;
using Business.Weapons;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Components.Entity.Character
{
    public class AnimationOnAttack: SerializedMonoBehaviour
    {
        [OdinSerialize]
        private CharacterComponent _character;

        [OdinSerialize]
        private Animator _attackAnimator;

        private static readonly int Attack = Animator.StringToHash("Attack");

        private void Awake()
        {
            _character.Entity.Attacked += EntityOnAttacked;
        }

        private void EntityOnAttacked(object sender, WeaponAttackedEventArgs e)
        {
            _attackAnimator.SetTrigger(Attack);
        }
    }
}