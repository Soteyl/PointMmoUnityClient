using I2.Loc;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Components.Items
{
    public class CollectableItemNameDisplay: SerializedMonoBehaviour
    {
        [OdinSerialize]
        private CollectableItem _collectableItem;

        [OdinSerialize]
        private Camera _camera;
        
        [OdinSerialize]
        private Localize _localize;
        
        [OdinSerialize]
        private Canvas _canvas;
        
        private void Awake()
        {
            _camera ??= Camera.main;
            _localize.SetTerm("InventoryItems/" + _collectableItem.ItemData.Id);
        }
        
        private void Update()
        {
            var rotation = _camera.transform.rotation;
            _canvas.transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
        }
    }
}