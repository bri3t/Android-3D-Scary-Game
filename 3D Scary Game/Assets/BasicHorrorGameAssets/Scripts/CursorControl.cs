using UnityEngine;

public class CursorControl : MonoBehaviour
{
    private void Start()
    {
        // Make the cursor visible and unlock it
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
