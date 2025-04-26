using Game.SceneManagement;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Audio
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Audio/AudioConfig")]
    public class AudioConfig : ScriptableObject
    {
        [Serializable]
        public class SceneAudio
        {
            public SceneType sceneType; // 場景名稱
            public List<AudioData> startBGMData;
            public List<AudioData> startSFXData; // 該場景的初始輸入動作
        }

        public List<SceneAudio> sceneAudios; // 所有場景的輸入配置

        /// <summary>
        /// 根據當前場景名稱獲取對應的輸入配置
        /// </summary>
        /// <param name="sceneName">場景名稱</param>
        /// <returns>該場景的輸入動作列表</returns>
        public SceneAudio GetAudioDataForScene(string sceneName)
        {
            foreach (var sceneAudio in sceneAudios)
            {
                if (sceneAudio.sceneType.ToString() == sceneName)
                {
                    return sceneAudio;
                }
            }
            return null; // 如果未找到對應的場景配置，返回空
        }
    }

}
