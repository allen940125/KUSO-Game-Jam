using Game.SceneManagement;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Input
{
    [CreateAssetMenu(fileName = "InputConfig", menuName = "Input/InputConfig")]
    public class InputConfig : ScriptableObject
    {
        [Serializable]
        public class SceneInput
        {
            public SceneType sceneType; // 場景名稱
            public List<InputType> startInputActions; // 該場景的初始輸入動作
            public bool uiOnly;
        }

        public List<SceneInput> sceneInputs; // 所有場景的輸入配置

        /// <summary>
        /// 根據當前場景名稱獲取對應的輸入配置
        /// </summary>
        /// <param name="sceneName">場景名稱</param>
        /// <returns>該場景的輸入動作列表</returns>
        public SceneInput GetInputActionsForScene(string sceneName)
        {
            foreach (var sceneInput in sceneInputs)
            {
                if (sceneInput.sceneType.ToString() == sceneName)
                {
                    return sceneInput;
                }
            }
            return null; // 如果未找到對應的場景配置，返回空
        }
    }

}