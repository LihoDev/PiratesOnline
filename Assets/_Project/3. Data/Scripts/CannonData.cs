using UnityEngine;

namespace PiratesOnline.Domain.Data
{
    [CreateAssetMenu(fileName = "New Cannon", menuName = "PiratesOnline/Cannon Data")]
    public class CannonData : ScriptableObject
    {
        [Header("Identity")]
        public string ItemId; // Must match the key in Addressables
        public Sprite Icon;

        [Header("Combat Stats")]
        public float ReloadTime = 2f;
        public float ProjectileSpeed = 10f;
        public int Damage = 10;

        [Header("Prefabs")]
        public string ProjectileAddress;
    }
}