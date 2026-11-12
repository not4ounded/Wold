using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputReader))]
public class PlayerMotor : MonoBehaviour
{
    [Header("Настройки игрока")]
    [SerializeField] private PlayerInputReader _inputReader;
    [SerializeField] private Transform _meshTransform;
    [SerializeField] private float normalSpeed = 10f;
    [SerializeField] private float crouchSpeed = 5;
    [SerializeField] private float gravity = -19.6f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float playerStandHeight = 2f;
    [SerializeField] private float playerCrouchHeight = 1f;

    [Header("Настройки камеры")]
    [SerializeField] private Transform _cameraPivot;
    [SerializeField] private float cameraStandHeight = 1.5f;
    [SerializeField] private float cameraCrouchHeight = 0.8f;
    [SerializeField] private float cameraSmoothSpeed = 10f;

    private CharacterController _controller;
    private float verticalVelocity;
    private InputAction moveAction;
    private InputAction jumpAction;
    private float currentSpeed = 10f;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        if (_controller != null) SetControllerHeight(playerStandHeight);
        if (_inputReader == null) Debug.LogError("InputReader отсутствует на объекте игрока!");
        if (_meshTransform == null) Debug.LogError("meshTransform отсутствует на объекте игрока!");
    }

    // Update is called once per frame
    void Update()
    {
        bool IsGrounded = _controller.isGrounded;

        // Небольшая прижимная сила, чтобы игрок не отлетал от земли
        if (!_inputReader.JumpPressedThisFrame && IsGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; 
        }

        // прыжок и ускорение свободного падения
        if (IsGrounded && _inputReader.JumpPressedThisFrame)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // шифт и проверка потолка
        if (_inputReader.CrouchIsPressed)
        {
            SetControllerHeight(playerCrouchHeight);
            UpdateCameraHeight(cameraCrouchHeight);
            currentSpeed = crouchSpeed;
        }
        else
        {
            if (CanPlayerStandAfterCrouch(playerStandHeight))
            {
                SetControllerHeight(playerStandHeight);
                UpdateCameraHeight(cameraStandHeight);
                currentSpeed = normalSpeed;
            }
        }

        // гравитация
        verticalVelocity += gravity * Time.deltaTime;

        // Вертикальный вектор движения
        Vector3 horizontalMove = transform.right * _inputReader.Moveinput.x + transform.forward * _inputReader.Moveinput.y;

        // Собираем итоговый вектор
        Vector3 finalMove = horizontalMove * currentSpeed + Vector3.up * verticalVelocity;

        // Передаем контроллеру
        _controller.Move(finalMove * Time.deltaTime);
    }

    /// <summary>
    /// М/д для смещения игрока и его центра для шифта
    /// </summary>
    /// <param name="newHeight">новая высота контроллера</param>
    private void SetControllerHeight(float newHeight)
    {
        _controller.height = newHeight;
        _controller.center = new Vector3(0, newHeight / 2f, 0);

        if (_meshTransform != null)
        {
            float scaleY = newHeight / playerStandHeight;
            _meshTransform.localScale = new Vector3(1f, scaleY, 1f);

            _meshTransform.localPosition = new Vector3(0, newHeight / 2f, 0);
        }
    }

    /// <summary>
    /// М/д для смещения камеры для шифта
    /// </summary>
    /// <param name="newHeight">новая высота камеры</param>
    private void UpdateCameraHeight(float newHeight)
    {
        if (_cameraPivot == null) return;

        float targetHeight = newHeight;

        // позиция высоты камеры по y измерению
        Vector3 targetPos = new Vector3(
            _cameraPivot.localPosition.x, 
            targetHeight, 
            _cameraPivot.localPosition.z
        );

        // плавный интерполированный переход от а до б
        _cameraPivot.localPosition = Vector3.Lerp(
            _cameraPivot.localPosition,
            targetPos,
            cameraSmoothSpeed * Time.deltaTime
        );
    }

    /// <summary>
    /// М/д проверки может ли игрок встать после шифта
    /// </summary>
    /// <param name="standHeight">полный рост игрока</param>
    /// <returns>Если коллайдер пересекается сверху, возвращается false и наоборот</returns>
    public bool CanPlayerStandAfterCrouch(float standHeight)
    {
        // радиус двух полусфер контроллера
        float radius = _controller.radius;

        // низ объекта 
        Vector3 bottom = transform.position + Vector3.up * radius;

        // Верх
        Vector3 top = transform.position + Vector3.up * (standHeight - radius);

        // Слои, которые игнорируются
        int layerMask = ~LayerMask.GetMask("Ground", "Player", "Enemy", "PlayerMesh");

        // Проверка
        return !Physics.CheckCapsule(bottom, top, radius, layerMask, QueryTriggerInteraction.Ignore);
    }
}
