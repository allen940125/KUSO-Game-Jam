using Game.Input;
using Game.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSo", menuName = "Data/GameSystem/GameSo")]
public class GameSo : ScriptableObject
{
    [field: Header("各場景UI配置")]
    [field: SerializeField] public UIConfig uiConfig { get; set; }
    
    [field: Header("各場景input配置")]
    [field: SerializeField] public InputConfig inputConfig { get; set; }
}