using System;
using System.Reflection;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Datamanager;
using UniRx;
using Cysharp.Threading.Tasks;

namespace Datamanager
{
    public class DataManager
    {
        public DataGroup DataGroup = new DataGroup();
        public RealTimePlayerData realTimePlayerData = new RealTimePlayerData();
        public async Task InitDataMananger()
        {
            try
            {
                Debug.Log("DataManager::InitDataMananger");

                Debug.Log("Start loading CSV...");
                var CSVString = await AddressableSearcher.GetAddressableAssetAsync<TextAsset>("CSV/ShamanKingCSV");

                if (CSVString == null)
                {
                    Debug.LogError("❌ CSV 加載失敗: CSVString 為 null!");
                    return;
                }
                Debug.Log($"✅ CSV Loaded! Data: {(CSVString.text.Length > 100 ? CSVString.text.Substring(0, 100) + "..." : CSVString.text)}");

                var stringData = await CSVClassGenerator.GenClassArrayByCSV<DatasPath>(CSVString);
                if (stringData == null)
                {
                    Debug.LogError("❌ CSV 解析失敗: stringData 為 null!");
                    return;
                }
                Debug.Log($"✅ CSV 解析成功! 數量: {stringData.Length}");

                PropertyInfo[] propertyInfo = typeof(DataGroup).GetProperties();
                if (propertyInfo == null || propertyInfo.Length == 0)
                {
                    Debug.LogError("❌ DataGroup 沒有任何屬性!");
                    return;
                }
                Debug.Log($"✅ DataGroup 屬性數量: {propertyInfo.Length}");

                for (int i = 0; i < propertyInfo.Length; i++)
                {
                    Debug.Log($"🔍 讀取第 {i} 個 DataGroup 屬性: {propertyInfo[i].Name}");

                    object dataObj = propertyInfo[i].GetValue(DataGroup);
                    if (dataObj == null)
                    {
                        Debug.LogWarning($"⚠️ DataGroup 屬性 {propertyInfo[i].Name} 為 null，跳過");
                        continue;
                    }

                    if (dataObj is IDataBase database)
                    {
                        Type type = database.ThisDataType;
                        Debug.Log($"✅ 取得 DataType: {type}");

                        if (stringData.Length <= i)
                        {
                            Debug.LogError($"❌ stringData 數量不足! 無法讀取索引 {i}");
                            continue;
                        }

                        Debug.Log($"📄 準備讀取 CSV: {stringData[i].Path}");
                        if (string.IsNullOrEmpty(stringData[i].Path))
                        {
                            Debug.LogError($"❌ 路徑為 null 或空: stringData[{i}].Path");
                            continue;
                        }

                        var CSVstring = await AddressableSearcher.GetAddressableAssetAsync<TextAsset>(stringData[i].Path);
                        if (CSVstring == null)
                        {
                            Debug.LogError($"❌ CSV 加載失敗: {stringData[i].Path}");
                            continue;
                        }
                        Debug.Log($"✅ CSV 讀取成功: {stringData[i].Path}");
                        Debug.Log(type + "跟" + CSVstring);
                        var List = await CSVClassGenerator.GenClassArrayByCSV(type, CSVstring);
                        if (List == null)
                        {
                            Debug.LogError($"❌ CSV 解析失敗: {stringData[i].Path}");
                            continue;
                        }
                        Debug.Log($"✅ CSV 解析成功! 總數: {List.Length}");

                        database.DataArray = List;
                        Debug.Log($"✅ DataArray 設定成功: {database.DataArray.Length} 筆資料");
                    }
                    else
                    {
                        Debug.LogWarning($"⚠️ DataGroup 屬性 {propertyInfo[i].Name} 不是 IDataBase，跳過");
                    }
                }

                Debug.Log("Start Delay 100");
                await UniTask.Delay(100);
                Debug.Log("FinishInit");

                // 確保查詢 ItemDataBaseTemplete 時不會發生錯誤
                var itemData = GetDataByID<ItemDataBaseTemplete>(101);
                if (itemData != null)
                {
                    Debug.Log($"DataManager 物品查詢結果: {itemData.Name}");
                    Debug.Log($"DataManager 物品查詢結果: {itemData.ItemIconPath}");
                }
                else
                {
                    Debug.LogError("❌ DataManager 物品查詢失敗，可能 ID 101 不存在!");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"❌ InitDataMananger 發生錯誤: {ex}");
            }
        }
        
        public T GetDataByID<T>(int id) where T : class
        {
            var database = DataGroup.TryGetDataBase<T>();
            foreach (var item in database.DataArray)
            {
                if (item is not IWithIdData data)
                {
                    throw new UnityException("Not a IWithIdData");
                }
                if (data.Id == id)
                {
                    return data as T;
                }
            }
            return null;
        }
        public T GetDataByName<T>(string name) where T : class
        {
            var database = DataGroup.TryGetDataBase<T>();
            foreach (var item in database.DataArray)
            {
                if (item is not IWithNameData data)
                {
                    throw new UnityException("Not a IWithNameData");
                }
                if (data.Name == name)
                {
                    return data as T;
                }
            }
            return null;
        }
    }
}

