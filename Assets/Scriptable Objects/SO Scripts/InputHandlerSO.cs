using UnityEngine;

[CreateAssetMenu(fileName = "InputHandlerSO", menuName = "Game/ScriptableObjects/Player/Input Handler")]
public class InputHandlerSO :ScriptableObject
{
    public KeyCode shootKey = KeyCode.Space;

    private Vector2 inputAxis;
    private bool shootPressed;

    public void onUpdateInternal()
    {
        inputAxis.x = Input.GetAxisRaw("Horizontal");
        inputAxis.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(shootKey))
        {
            shootPressed = true;
        }
        else
        {
            shootPressed = false;
        }
    }

    public Vector2 GetInputVector()
    {
        return inputAxis;
    }

    public bool GetShootPressed()
    {
        return shootPressed;
    }
}
