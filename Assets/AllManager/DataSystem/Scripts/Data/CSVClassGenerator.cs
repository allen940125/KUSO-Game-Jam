using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Datamanager
{
    /// <summary>
    /// CSVClassGenerator 負責解析 CSV 格式的文本檔，並根據 CSV 中的資料生成對應類型的物件陣列。
    /// </summary>
    public class CSVClassGenerator
    {
        /// <summary>
        /// 解析 CSV 資料並生成泛型物件陣列。CSV 第一行預設為標題行，因此從第二行開始解析。
        /// </summary>
        /// <typeparam name="T">要生成的資料物件類型，必須具有無參構造函數。</typeparam>
        /// <param name="textAsset">包含 CSV 文字內容的 TextAsset。</param>
        /// <returns>返回由 CSV 內容生成的 T 類型物件陣列。</returns>
        public static async Task<T[]> GenClassArrayByCSV<T>(TextAsset textAsset) where T : new()
        {
            // 使用換行符號分割 CSV 檔案為多行
            string[] data = textAsset.text.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
            
            // 將每行的內容再以逗號分割成字串陣列
            string[][] tempdata = new string[data.Length][];
            for (int i = 0; i < data.Length; i++)
            {
                string[] datastring = data[i].Split(new string[] { "," }, System.StringSplitOptions.None);
                tempdata[i] = datastring;
            }
            
            // 跳過第一行（標題行），生成資料陣列
            var resultArray = new T[tempdata.Length - 1];
            for (int i = 1; i < tempdata.Length; i++)
            {
                var result = new T();
                // 利用反射與非同步方法為每個物件設定屬性值
                await SetClassData<T>(result, tempdata[i]);
                resultArray[i - 1] = result;
            }
            return resultArray;
        }

        /// <summary>
        /// 解析 CSV 資料並生成指定型別（使用 Type）的物件陣列。與泛型版本功能相同，
        /// 只是透過反射建立物件實例。
        /// </summary>
        /// <param name="type">要生成的資料物件的 Type。</param>
        /// <param name="textAsset">包含 CSV 文字內容的 TextAsset。</param>
        /// <returns>返回由 CSV 內容生成的物件陣列。</returns>
        public static async Task<object[]> GenClassArrayByCSV(Type type, TextAsset textAsset)
        {
            Debug.Log(type.Name);
            // 分割 CSV 文字內容為多行
            string[] data = textAsset.text.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
            string[][] tempdata = new string[data.Length][];
            for (int i = 0; i < data.Length; i++)
            {
                string[] datastring = data[i].Split(new string[] { "," }, System.StringSplitOptions.None);
                tempdata[i] = datastring;
            }
            var resultArray = new object[tempdata.Length - 1];
            // 從第二行開始建立資料物件
            for (int i = 1; i < tempdata.Length; i++)
            {
                // 使用反射建立物件實例
                var result = Activator.CreateInstance(type);
                await SetClassData(type, result, tempdata[i]);
                resultArray[i - 1] = result;
            }
            return resultArray;
        }

        /// <summary>
        /// 解析 CSV 資料並返回一個二維字串陣列，每個內部陣列代表一行資料。
        /// </summary>
        /// <param name="textAsset">包含 CSV 文字內容的 TextAsset。</param>
        /// <returns>二維字串陣列，表示 CSV 中的所有資料。</returns>
        public static string[][] GenStringArrayFromCsvData(TextAsset textAsset)
        {
            string[] data = textAsset.text.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
            string[][] tempdata = new string[data.Length][];
            for (int i = 0; i < data.Length; i++)
            {
                string[] datastring = data[i].Split(new string[] { "," }, System.StringSplitOptions.None);
                tempdata[i] = datastring;
            }
            return tempdata;
        }

        /// <summary>
        /// 解析 CSV 資料並返回一個字串列表陣列，每行資料轉換為 List&lt;string&gt;。
        /// </summary>
        /// <param name="textAsset">包含 CSV 文字內容的 TextAsset。</param>
        /// <returns>字串列表陣列，每個 List&lt;string&gt; 代表 CSV 中的一行資料。</returns>
        public static List<string>[] GenStringListFromCsvData(TextAsset textAsset)
        {
            string[] data = textAsset.text.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
            List<string>[] tempdata = new List<string>[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                // 將每行資料以逗號分割後轉換成 List<string>
                List<string> datastring = data[i].Split(new string[] { "," }, System.StringSplitOptions.None).ToList<string>();
                tempdata[i] = datastring;
            }
            return tempdata;
        }

        /// <summary>
        /// 根據 CSV 的一行資料，利用反射將資料設置到泛型物件的屬性上。
        /// 支援多種基本型別和透過 Addressable 載入的資源類型。
        /// </summary>
        /// <typeparam name="T">要設置資料的物件類型。</typeparam>
        /// <param name="DataBeSet">要設定資料的物件實例。</param>
        /// <param name="dataText">CSV 中一行以逗號分割後的字串陣列。</param>
        public static async Task SetClassData<T>(T DataBeSet, string[] dataText)
        {
            // 取得 T 的所有公共屬性
            PropertyInfo[] propertyInfo = typeof(T).GetProperties();
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                // 根據屬性型別進行轉換與設定
                if (propertyInfo[i].PropertyType == typeof(string))
                {
                    propertyInfo[i].SetValue(DataBeSet, dataText[i].ToString());
                }
                else if (propertyInfo[i].PropertyType == typeof(int))
                {
                    propertyInfo[i].SetValue(DataBeSet, int.Parse(dataText[i]));
                }
                else if (propertyInfo[i].PropertyType == typeof(float))
                {
                    propertyInfo[i].SetValue(DataBeSet, float.Parse(dataText[i]));
                }
                else if (propertyInfo[i].PropertyType == typeof(GameObject))
                {
                    // 非同步載入 GameObject 資源
                    var gameobjectPrefab = await AddressableSearcher.GetAddressableAssetAsync<GameObject>(dataText[i]);
                    propertyInfo[i].SetValue(DataBeSet, gameobjectPrefab);
                }
                else if (propertyInfo[i].PropertyType == typeof(bool))
                {
                    // 判斷字串是否等於 "TRUE" 或 "FALSE"
                    if (dataText[i].ToString() == "TRUE")
                    {
                        propertyInfo[i].SetValue(DataBeSet, true);
                    }
                    else if (dataText[i].ToString() == "FALSE")
                    {
                        propertyInfo[i].SetValue(DataBeSet, false);
                    }
                }
                else if (propertyInfo[i].PropertyType == typeof(Sprite))
                {
                    // 非同步載入 Sprite 資源
                    var sprite = await AddressableSearcher.GetAddressableAssetAsync<Sprite>(dataText[i]);
                    propertyInfo[i].SetValue(DataBeSet, sprite);
                }
                else if (propertyInfo[i].PropertyType == typeof(AudioClip))
                {
                    // 非同步載入 AudioClip 資源
                    var audio = await AddressableSearcher.GetAddressableAssetAsync<AudioClip>(dataText[i]);
                    propertyInfo[i].SetValue(DataBeSet, audio);
                }
                else if (propertyInfo[i].PropertyType == typeof(string[][]))
                {
                    // 當屬性為二維字串陣列時，先載入 TextAsset，再解析成二維陣列
                    var data = await AddressableSearcher.GetAddressableAssetAsync<TextAsset>(dataText[i]);
                    var parseData = GenStringArrayFromCsvData(data);
                    propertyInfo[i].SetValue(DataBeSet, parseData);
                }
                else if (propertyInfo[i].PropertyType == typeof(List<object>))
                {
                    // 若屬性為 List<object>，則解析成物件列表
                    var typeData = dataText[i];
                    var data = genAListByStringAndType(dataText[i]);
                    listParser(propertyInfo[i], data, typeData, DataBeSet);
                }
            }
        }

        /// <summary>
        /// 非泛型版本：根據 CSV 的一行資料，利用反射將資料設置到指定物件的屬性上。
        /// </summary>
        /// <param name="type">物件的型別。</param>
        /// <param name="DataBeSet">要設定資料的物件實例。</param>
        /// <param name="dataText">CSV 中一行以逗號分割後的字串陣列。</param>
        public static async Task SetClassData(Type type, object DataBeSet, string[] dataText)
        {
            Debug.Log(type.Name);
            PropertyInfo[] propertyInfo = type.GetProperties();
            Debug.Log(dataText.ToString());
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                Debug.Log(propertyInfo[i].Name);
                Debug.Log(dataText[i] + "的Type是" + propertyInfo[i].PropertyType);
                if (propertyInfo[i].PropertyType == typeof(string))
                {
                    propertyInfo[i].SetValue(DataBeSet, dataText[i].ToString());
                }
                else if (propertyInfo[i].PropertyType == typeof(int))
                {
                    propertyInfo[i].SetValue(DataBeSet, int.Parse(dataText[i]));
                }
                else if (propertyInfo[i].PropertyType == typeof(float))
                {
                    propertyInfo[i].SetValue(DataBeSet, float.Parse(dataText[i]));
                }
                else if (propertyInfo[i].PropertyType == typeof(GameObject))
                {
                    var gameobjectPrefab = await AddressableSearcher.GetAddressableAssetAsync<GameObject>(dataText[i]);
                    propertyInfo[i].SetValue(DataBeSet, gameobjectPrefab);
                }
                else if (propertyInfo[i].PropertyType == typeof(bool))
                {
                    if (dataText[i].ToString() == "TRUE")
                    {
                        propertyInfo[i].SetValue(DataBeSet, true);
                    }
                    else if (dataText[i].ToString() == "FALSE")
                    {
                        propertyInfo[i].SetValue(DataBeSet, false);
                    }
                }
                else if (propertyInfo[i].PropertyType.IsEnum)
                {
                    // 根據 Description Attribute 或枚舉名稱解析
                    object enumValue = ParseEnumWithDescription(
                        propertyInfo[i].PropertyType, 
                        dataText[i]
                    );
                    propertyInfo[i].SetValue(DataBeSet, enumValue);
                }
                else if (propertyInfo[i].PropertyType == typeof(Sprite))
                {
                    var sprite = await AddressableSearcher.GetAddressableAssetAsync<Sprite>(dataText[i]);
                    propertyInfo[i].SetValue(DataBeSet, sprite);
                }
                else if (propertyInfo[i].PropertyType == typeof(AudioClip))
                {
                    var audio = await AddressableSearcher.GetAddressableAssetAsync<AudioClip>(dataText[i]);
                    propertyInfo[i].SetValue(DataBeSet, audio);
                }
                else if (propertyInfo[i].PropertyType == typeof(string[][]))
                {
                    var data = await AddressableSearcher.GetAddressableAssetAsync<TextAsset>(dataText[i]);
                    var parseData = GenStringArrayFromCsvData(data);
                    propertyInfo[i].SetValue(DataBeSet, parseData);
                }
                else if (propertyInfo[i].PropertyType == typeof(List<object>))
                {
                    // 處理 List<object> 型別的資料，先去除括號，分割成各部分
                    var content = dataText[i].Trim('(', ')');
                    var dataArray = content.Split(';');
                    var typeData = dataArray[0];
                    var data = genAListByStringAndType(dataText[i]);
                    listParser(propertyInfo[i], data, typeData, DataBeSet);
                }
            }
        }

        /// <summary>
        /// 解析 List<object> 資料並根據指定類型轉換，然後設定到屬性上。
        /// 目前僅對 bool 類型做處理。
        /// </summary>
        /// <param name="property">欲設定的屬性資訊。</param>
        /// <param name="data">解析後的物件列表。</param>
        /// <param name="type">資料中指定的類型字串。</param>
        /// <param name="DataBeSet">要設定資料的物件實例。</param>
        static void listParser(PropertyInfo property, List<object> data, string type, object DataBeSet)
        {
            switch (type)
            {
                case "bool":
                    // 將物件列表轉換為 bool 類型的列表，並設定給屬性
                    List<bool> boolList = data.Select(ToBool).ToList();
                    property.SetValue(DataBeSet, data);
                    return;
            }
        }

        /// <summary>
        /// 將物件轉換為 bool 型別。如果無法轉換則拋出例外。
        /// </summary>
        /// <param name="item">欲轉換的物件。</param>
        /// <returns>轉換後的 bool 值。</returns>
        static bool ToBool(object item)
        {
            if (item is bool)
                return (bool)item;
            throw new ArgumentException($"無法將物件轉換為bool：{item}");
        }
        
        // 枚舉解析工具方法
        private static object ParseEnumWithDescription(Type enumType, string value)
        {
            foreach (var field in enumType.GetFields())
            {
                var descAttr = field.GetCustomAttribute<DescriptionAttribute>();
                if (descAttr != null && descAttr.Description == value)
                    return Enum.Parse(enumType, field.Name);
            }
            return Enum.Parse(enumType, value); // 回退到枚舉名稱
        }

        /// <summary>
        /// 根據資料字串解析出 List&lt;object&gt;。目前僅支援 bool 類型的處理。
        /// 資料字串格式例如 "(bool;TRUE;FALSE;...)"。
        /// </summary>
        /// <param name="data">包含型別和資料的字串。</param>
        /// <returns>解析後的 List&lt;object&gt;，根據型別轉換後的值。</returns>
        private static List<object> genAListByStringAndType(string data)
        {
            // 去除前後括號，並以分號分割
            var content = data.Trim('(', ')');
            var dataArray = content.Split(';');
            var typeData = dataArray[0];
            var result = new List<object>();
            switch (typeData)
            {
                case "bool":
                    // 從第二個元素開始，解析每一個字串成 bool 值
                    for (int i = 1; i < dataArray.Length; i++)
                    {
                        if (dataArray[i] == "TRUE")
                        {
                            result.Add(true);
                        }
                        else
                        {
                            result.Add(false);
                        }
                    }
                    break;
            }
            return result;
        }
    }
}
