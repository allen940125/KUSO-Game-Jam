using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    
    public Texture2D crosshairShoot;  
    public BoxCollider2D shootRange; 
    private Vector2 mousePosition;
    private bool isMouseOverEasterEgg;

    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        HandleCrosshair();
        HandleShooting();
    }

    void HandleCrosshair()
    {
        Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition, LayerMask.GetMask("EasterEgg"));
        isMouseOverEasterEgg = hitCollider != null && hitCollider.TryGetComponent<EasterEgg>(out _); 
        if(isMouseOverEasterEgg)
        {
            Cursor.SetCursor(crosshairShoot,  new Vector2(32,32), CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
    void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0) && isMouseOverEasterEgg)
        {
            #if UNITY_EDITOR
            // 在編輯器中停止播放模式
            UnityEditor.EditorApplication.isPlaying = false;
#else
    // 在執行檔中退出應用程式
    Application.Quit();
#endif
        }
    }

    
}
