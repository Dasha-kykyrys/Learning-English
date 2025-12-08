using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractiveItem : MonoBehaviour
{
    public string itemId;

    [Header("Настройки предмета")]
    public Sprite normalSprite;
    public Sprite outlineSprite;
    public WordDataSO wordData;

    [Header("UI элементы")]
    public Button interactButton;
    public Sprite interactButtonSprite;

    [Header("События")]
    public UnityEvent onInteract;

    private Image buttonImage;
    private SpriteRenderer spriteRenderer;
    public bool canInteract = false;
    private bool isPlayerNear = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buttonImage = interactButton.GetComponent<Image>();

        spriteRenderer.sprite = normalSprite;

        // Отключение кнопки в начале
        interactButton.gameObject.SetActive(false);
        interactButton.onClick.AddListener(Interact);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canInteract)
        {
            isPlayerNear = true;

            // Смена спрайт на обводку
            spriteRenderer.sprite = outlineSprite;

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

            // Смена на обычный спрайт
            spriteRenderer.sprite = normalSprite;

            // Скрытие кнопки
            interactButton.gameObject.SetActive(false);
        }
    }

    public void Interact()
    {
        if (!canInteract || !isPlayerNear) return;

        // Вывод информации о слове
        Debug.Log($"ID: {wordData.wordId}");
        Debug.Log($"Категория: {wordData.category}");
        Debug.Log($"Английское слово: {wordData.englishWord}");
        Debug.Log($"Перевод: {string.Join(", ", wordData.translation)}");
        Debug.Log($"Примеры: {string.Join(", ", wordData.examples)}");
        Debug.Log($"Перевод примеров: {string.Join(", ", wordData.translationExamples)}");
        Debug.Log($"Есть иконка: {wordData.icon != null}");
        Debug.Log($"Есть произношение: {wordData.pronunciation != null}");
        Debug.Log($"Количество озвученных примеров: {wordData.examplesPronunciation?.Length ?? 0}");

        // Вызов события
        onInteract.Invoke();

        // Предмет неинтерактивный
        canInteract = false;

        // Возврат обычного спрайта
        spriteRenderer.sprite = normalSprite;

        // Скрытие кнопки
        interactButton.gameObject.SetActive(false);
    }
}