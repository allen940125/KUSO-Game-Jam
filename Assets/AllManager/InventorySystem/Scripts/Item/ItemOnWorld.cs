using Datamanager;
using Game.Audio;
using Gamemanager;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public ItemDataBaseTemplete itemData;//物品的Data資訊
    public int itemID;
    public int quantity;

    [Header("物品撿取音效")]
    [SerializeField] AudioData audioData;

    /// <summary>
    /// 初始化LootItme的數值
    /// </summary>
    /// <param name="itemData">物品資訊</param>
    /// <param name="quantity">掉落數量</param>
    public void Initialize(ItemDataBaseTemplete itemData, int quantity)
    {
        this.itemData = itemData;
        this.quantity = quantity;

        GetComponent<SpriteRenderer>().sprite = itemData.ItemIconPath;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(itemData == null)
        {
            itemData = GameContainer.Get<DataManager>().GetDataByID<ItemDataBaseTemplete>(itemID);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.MainGameEvent.Send(new ItemAddedToBagEvent() { ItemID = itemID, Quantity = quantity });  
            AudioManager.Instance.PlaySFX(audioData);
            Destroy(gameObject);
        }
    }
}
