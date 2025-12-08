using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractiveNPC : MonoBehaviour
{
    public string npcId; 

    [Header("UI элементы")]
    public Button interactButton;
    public Sprite interactButtonSprite;

    [Header("События")]
    public UnityEvent onInteract;

    private Image buttonImage;
    public bool canInteract = false;
    private bool isPlayerNear = false;

    void Start()
    {
        buttonImage = interactButton.GetComponent<Image>();

        // Отключение кнопки в начале
        interactButton.gameObject.SetActive(false);
        interactButton.onClick.AddListener(Interact);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canInteract)
        {
            isPlayerNear = true;

            // Активирование кнопки
            buttonImage.sprite = interactButtonSprite;
            interactButton.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;

            // Скрытие кнопки
            interactButton.gameObject.SetActive(false);
        }
    }

    public void Interact()
    {
        if (!canInteract || !isPlayerNear) return;

        // Вызов события
        onInteract.Invoke();

        // Предмет неинтерактивный
        canInteract = false;

        // Скрытие кнопки
        interactButton.gameObject.SetActive(false);
    }
}