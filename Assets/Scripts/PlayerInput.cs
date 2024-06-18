using UnityEngine;

public class PlayerInput : IPlayerInput
{
    private const string MouseY = "Mouse Y";
    private const string MouseX = "Mouse X";
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    public float GetVerticalAxis() => Input.GetAxis(Vertical);
    public float GetHorizontalAxis() => Input.GetAxis(Horizontal);
    public float GetMouseX() => Input.GetAxis(MouseX);
    public float GetMouseY() => Input.GetAxis(MouseY);
    public bool GetJumpKeyDown() => Input.GetKeyDown(KeyCode.Space);
}