using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputReader))]
public class PlayerLook : MonoBehaviour
{
    [Header("Настройки мыши")]
    [SerializeField] private PlayerInputReader _inputReader;
    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform cameraRoot;
    public float mouseSensivity = 0.2f;
    public float xRotation = 0f;
    public float minAngle = -90f;
    public float maxAngle = 90f;

    void Start()
    {
        if (playerBody == null) Debug.LogError("Не подключен объект игрока");
        if (cameraRoot == null) Debug.LogError("Не подключен объект камеры");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
    }

    // Сделать ссылки на инпут ридер, чтобы перехват значений был там, а логика здесь
    void Update()
    {
        // перехват значений мыши по вертикали и горизонтали
        //float _mouseX = Mouse.current.delta.x.ReadValue() * mouseSensivity;
        //float _mouseY = Mouse.current.delta.y.ReadValue() * mouseSensivity;

        // добавляем ограничение на вращение по вертикали
        xRotation -= _inputReader.MouseY;
        xRotation = Mathf.Clamp(xRotation, minAngle, maxAngle);

        // применяем вращение к камере
        cameraRoot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // вращаем игрока по y
        playerBody.Rotate(Vector3.up * _inputReader.MouseX);
    }
}
