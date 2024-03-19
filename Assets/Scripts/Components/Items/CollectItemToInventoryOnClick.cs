using Components.Interacting;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Components.Items
{
    public class CollectItemToInventoryOnClick: SerializedMonoBehaviour
    {
        [OdinSerialize]
        private float _collectableRadius = 1f;

        [OdinSerialize]
        private InventoryComponent _inventoryComponent;

        [OdinSerialize]
        private Movement _movement;

        private ClickEventTransfer _clickEventTransfer;

        private void Awake()
        {
            _clickEventTransfer = ClickEventTransfer.FindInScene();
            _clickEventTransfer.InteractableObjectInvoked += OnMouseClick;
        }

        private void OnMouseClick(object sender, InteractableObjectEventArgs e)
        {
            if (e.Interaction.Object.Type != InteractableObjectType.Item || e.Interaction.Object is not LootItem item) return;

            _movement.MoveTo(new MoveRequest()
            {
                    TransformTarget = item.transform,
                    OnFinish = (st) =>
                    {
                        if (st != MovementStatus.Finished) return;
                        var result = _inventoryComponent.Inventory.AddItem(item.ItemData, item.Count, true);
                        if (result.ExtraItems == 0) item.Collect();
                    },
                    StoppingDistance = _collectableRadius
            });
        }
    }
}