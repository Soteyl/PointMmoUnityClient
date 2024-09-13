using Business.Weapons;
using Components;
using Components.Entity.Character;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class CharacterAnimation : SerializedMonoBehaviour
{
    [OdinSerialize]
    private Animator _animator;
    
    [OdinSerialize]
    private Movement _movement;
    
    [OdinSerialize]
    private CharacterComponent _character;

    private void Start()
    {
        _movement.MoveStatusChanged += OnMoveStatusChanged;
        _character.Entity.Attacked += OnAttacked;
    }

    private void OnAttacked(object sender, WeaponAttackedEventArgs e)
    {
        _animator.SetTrigger("IsFighting");
    }

    private void OnMoveStatusChanged(object sender, MoveStatusChangedEventArgs e)
    {
        _animator.SetBool("IsRunning", e.IsMoving);
    }
}
