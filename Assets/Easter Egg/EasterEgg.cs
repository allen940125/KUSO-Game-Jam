using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    
    public Texture2D crosshairShoot;  
    public BoxCollider2D shootRange; 
    private Vector2 mousePosition;

    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        HandleCrosshair();
    }

    void HandleCrosshair()
    {
        Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition, LayerMask.GetMask("EasterEgg"));
        bool isMouseOverEasterEgg = hitCollider != null && hitCollider.TryGetComponent<EasterEgg>(out _); 
        if(isMouseOverEasterEgg)
        {
            Cursor.SetCursor(crosshairShoot,  new Vector2(32,32), CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    
}
