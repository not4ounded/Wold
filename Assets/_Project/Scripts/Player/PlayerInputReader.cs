using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Обработчик нажатий игрока
/// </summary>
public class PlayerInputReader : MonoBehaviour
{
    public bool JumpPressedThisFrame { get; private set; } // Был ли факт нажатия на прыжок в этом фрейме 
    public bool CrouchIsPressed {  get; private set; } // Удерживается ли шифт
    public Vector2 Moveinput => _moveInput;
    public float MouseX => _mouseX;
    public float MouseY => _mouseY;
    public float mouseSensivity = 0.08f;

    private float _mouseX;
    private float _mouseY;
    private Vector2 _moveInput;

    [Header("Настройки ввода")]
    // Ссылки на значения в .inputactions
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _jumpAction;
    [SerializeField] private InputActionReference _crouchAction;

    // Включение и выключение экшенов (по умолчанию были выключены)
    private void OnEnable()
    {
        _moveAction.action.Enable();
        _jumpAction.action.Enable();
        _crouchAction.action.Enable();
    }
    private void OnDisable()
    {
        _moveAction.action.Disable();
        _jumpAction.action.Disable();
        _crouchAction.action.Disable();
    }

    void Update()
    {
        _moveInput = _moveAction.action.ReadValue<Vector2>();

        // перехват значений мыши по вертикали и горизонтали
        _mouseX = Mouse.current.delta.x.ReadValue() * mouseSensivity;
        _mouseY = Mouse.current.delta.y.ReadValue() * mouseSensivity;

        JumpPressedThisFrame = _jumpAction.action.WasPressedThisFrame();
        CrouchIsPressed = _crouchAction.action.IsPressed();
    }
}
