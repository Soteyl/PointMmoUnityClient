using System;
using System.Threading;
using System.Threading.Tasks;
using Business.Entities;

namespace Business.Weapons
{
    public interface IWeapon
    {
        float Distance { get; }
        
        Task AttackAsync(Entity owner, Entity target, CancellationToken cancellationToken = default);

        event EventHandler<WeaponAttackedEventArgs> Attacked;
    }
}