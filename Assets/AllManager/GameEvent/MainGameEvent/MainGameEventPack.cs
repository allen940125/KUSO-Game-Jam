using Gamemanager;
using System;
using UniRx;

namespace Gamemanager
{
    public class MainGameEventPack : GameEventPack
    {
        // ======================
        // Common Event Streams
        // ======================
        public IObservable<GameInitializedEvent> OnGameInitializedEvent => getSubject<GameInitializedEvent>();
        public IObservable<CursorToggledEvent> OnCursorToggledEvent => getSubject<CursorToggledEvent>();

        // ======================
        // Inventory Event Streams
        // ======================

        public IObservable<PlayerBagRefreshedEvent> OnPlayerBagRefreshedEvent => getSubject<PlayerBagRefreshedEvent>();
        public IObservable<InventoryItemClickedEvent> OnInventoryItemClickedEvent => getSubject<InventoryItemClickedEvent>();
        public IObservable<ItemAddedToBagEvent> OnItemAddedToBagEvent => getSubject<ItemAddedToBagEvent>();

        // ======================
        // Store Event Streams
        // ======================

        public IObservable<StoreItemsRefreshedEvent> OnStoreItemsRefreshedEvent => getSubject<StoreItemsRefreshedEvent>();
        public IObservable<StoreItemClickedEvent> OnStoreItemClickedEvent => getSubject<StoreItemClickedEvent>();
        public IObservable<PurchaseItemClickedEvent> OnPurchaseItemClickedEvent => getSubject<PurchaseItemClickedEvent>();

        // ======================
        // Scene Event Streams
        // ======================

        public IObservable<SceneTransitionStartedEvent> OnSceneTransitionStartedEvent => getSubject<SceneTransitionStartedEvent>();
        public IObservable<SceneLoadedEvent> OnSceneLoadedEvent => getSubject<SceneLoadedEvent>();

        // ======================
        // Input Event Streams
        // ======================

        public IObservable<EscapeKeyPressedEvent> OnEscapeKeyPressedEvent => getSubject<EscapeKeyPressedEvent>();
        public IObservable<OpenBackpackKeyPressedEvent> OnOpenBackpackKeyPressedEvent => getSubject<OpenBackpackKeyPressedEvent>();
    }
}
