// using System;
// using Gamemanager;
// using Unity.Cinemachine;
// using UnityEngine;
// using UnityEngine.InputSystem;
//
// namespace GenshinImpactMovementSystem
// {
//     public class CameraCursor : MonoBehaviour
//     {
//         [SerializeField] private InputActionReference cameraToggleInputAction;
//         [SerializeField] private bool startHidden;
//
//         [SerializeField] private CinemachineInputAxisController cinemachineInputAxisController;
//         [SerializeField] private bool disableCameraLookOnCursorVisible;
//         [SerializeField] private bool disableCameraZoomOnCursorVisible;
//
//         [Tooltip("If you're using Cinemachine 2.8.4 or under, untick this option.\nIf unticked, both Look and Zoom will be disabled.")]
//         [SerializeField] private bool fixedCinemachineVersion;
//
//         private IDisposable toggleCursor;
//         
//         private void Awake()
//         {
//             cameraToggleInputAction.action.started += OnCameraCursorToggled;
//
//             if (startHidden)
//             {
//                 ToggleCursor();
//             }
//             
//             toggleCursor = GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnCursorToggledEvent, OnCursorToggledEvent);
//         }
//
//         private void OnEnable()
//         {
//             cameraToggleInputAction.asset.Enable();
//             Debug.Log("cameraToggleInputAction的Asset開啟");
//         }
//
//         private void OnDisable()
//         {
//             cameraToggleInputAction.asset.Disable();
//             Debug.Log("cameraToggleInputAction的Asset關閉");
//             toggleCursor.Dispose();
//         }
//
//         #region 事件註冊
//
//         private void OnCursorToggledEvent(CursorToggledEvent cmd)
//         {
//             ToggleCursor(cmd.ShowCursor ?? false);
//         }
//
//         #endregion
//         
//         private void OnCameraCursorToggled(InputAction.CallbackContext context)
//         {
//             ToggleCursor();
//         }
//
//         private void ToggleCursor(bool? showCursor = null)
//         {
//             // 如果 showCursor 為 null，則執行切換邏輯
//             if (showCursor == null)
//             {
//                 Cursor.visible = !Cursor.visible;
//             }
//             else
//             {
//                 Cursor.visible = showCursor.Value;
//             }
//
//             if (!Cursor.visible)
//             {
//                 Cursor.lockState = CursorLockMode.Locked;
//
//                 if (!fixedCinemachineVersion)
//                 {
//                     cinemachineInputAxisController.enabled = true;
//                 }
//             }
//             else
//             {
//                 Cursor.lockState = CursorLockMode.None;
//
//                 if (!fixedCinemachineVersion)
//                 {
//                     cinemachineInputAxisController.enabled = false;
//                 }
//             }
//         }
//
//     }
// }