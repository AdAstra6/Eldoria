using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D newCursorTexture;
    public static Texture2D defaultCursorTexture;
    public static Texture2D typingCursor;
    public Vector2 hotSpot = Vector2.zero;

    // Call this method to load the default cursor from the specified path
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void LoadDefaultCursor()
    {
        // The path is relative to the Resources folder, so move your cursor image to Assets/Resources/Sprites/Cursors/DefaultCursor.png
        defaultCursorTexture = Resources.Load<Texture2D>(Path.Combine("Sprites","Cursors", "DefaultCursor"));
        typingCursor = Resources.Load<Texture2D>(Path.Combine("Sprites", "Cursors", "TypingCursor"));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(newCursorTexture, hotSpot, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(defaultCursorTexture, hotSpot, CursorMode.Auto); // Reset to default
    }
    public void OnDisable()
    {
        Cursor.SetCursor(defaultCursorTexture, hotSpot, CursorMode.Auto); 
    }


    public static void SetDefaultCursor()
    {
        if (defaultCursorTexture == null)
        {
            LoadDefaultCursor();
        }
        Cursor.SetCursor(defaultCursorTexture, Vector2.zero, CursorMode.Auto); // Reset to default
    }
    public static void SetTypingCursor(PointerEventData eventData) {
        Cursor.SetCursor(typingCursor, Vector2.zero, CursorMode.Auto); // Set to typing cursor
    }
}
