using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Datamanager
{
    /// <summary>
    /// 提供一個用於透過 Addressable 系統非同步載入資源的輔助方法。
    /// </summary>
    public class AddressableSearcher
    {
        /// <summary>
        /// 非同步地從 Addressables 系統中載入指定地址的資源。
        /// </summary>
        /// <typeparam name="T">
        /// 指定要載入資源的類型，例如 <see cref="TextAsset"/>、<see cref="GameObject"/> 等。
        /// </typeparam>
        /// <param name="assetAddress">
        /// 資源的地址。這個地址是在 Unity Addressable Asset Groups 中所設定的「Address」值，
        /// 並非實體檔案的路徑。
        /// </param>
        /// <returns>
        /// 一個 <see cref="Task{T}"/>，當任務完成時會返回指定類型的資源。
        /// </returns>
        public static async Task<T> GetAddressableAssetAsync<T>(string assetAddress)
        {
            // 透過 Addressables 開始非同步載入資源，取得對應的 Task。
            var resultTask = Addressables.LoadAssetAsync<T>(assetAddress).Task;
            // 等待資源載入完成，同時不阻塞主執行緒。
            T result = await resultTask;
            // 返回載入後的資源。
            return result;
        }
    }
}


