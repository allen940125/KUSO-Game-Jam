namespace Gamemanager
{
    // ======================
    // Common Event Streams
    // ======================
    
    public class GameInitializedEvent : GameEventMessageBase
    {
        public int SavePointValue;
    }
    
    public class CursorToggledEvent : GameEventMessageBase
    {
        public bool? ShowCursor ;
    }
    
    // ======================
    // Inventory Event Streams
    // ======================
    
    public class PlayerBagRefreshedEvent : GameEventMessageBase
    {
        public ItemControllerType ItemControllerType;
    }

    public class InventoryItemClickedEvent : GameEventMessageBase
    {
        public InventoryItemRuntimeData StoredInventoryItemRuntimeData;
    }
    
    public class ItemAddedToBagEvent : GameEventMessageBase
    {
        public int ItemID;
        public int Quantity;
    }
    
    // ======================
    // Store Event Streams
    // ======================

    public class StoreItemsRefreshedEvent : GameEventMessageBase
    {
        public ItemControllerType ItemControllerType;
    }
    
    public class StoreItemClickedEvent : GameEventMessageBase
    {
        public StoreItemRuntimeData StoreItemData;
    }

    public class PurchaseItemClickedEvent : GameEventMessageBase
    {
        
    }

    // ======================
    // Scene Event Streams
    // ======================
    
    public class SceneTransitionStartedEvent : GameEventMessageBase
    {

    }

    public class SceneLoadedEvent : GameEventMessageBase
    {
        
    }
    
    // ======================
    // Input Event Streams
    // ======================

    public class EscapeKeyPressedEvent : GameEventMessageBase
    {
        
    }

    public class OpenBackpackKeyPressedEvent : GameEventMessageBase
    {
        
    }
}
