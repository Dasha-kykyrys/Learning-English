using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("Все квесты игры")]
    public QuestDataSO[] allQuests;

    [Header("Настройки")]
    public string startQuestId;
    private bool autoStartNextQuests = true;

    [Header("Текущее состояние")]
    private QuestDataSO currentQuest;
    private int currentStepIndex = 0;
    private Dictionary<string, bool> completedQuests = new Dictionary<string, bool>();
    private Dictionary<string, QuestProgress> questProgress = new Dictionary<string, QuestProgress>();

    [Header("События")]
    public Action<QuestDataSO> OnQuestStarted;
    public Action<QuestDataSO> OnQuestCompleted;
    public Action<QuestDataSO, int> OnQuestStepStarted;
    public Action<QuestDataSO, int> OnQuestStepCompleted;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadQuestProgress();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Автоматически запускаем начальный квест
        if (!string.IsNullOrEmpty(startQuestId))
        {
            StartQuest(startQuestId);
        }
    }

        // Запуск квеста
    public void StartQuest(string questId)
    {
        Debug.Log($"Попытка запуска квеста: {questId}");

        if (completedQuests.ContainsKey(questId))
        {
            Debug.Log($"Квест {questId} уже завершён");
            return;
        }

        currentQuest = GetQuestData(questId);
        if (currentQuest == null)
        {
            Debug.LogError($"Квест {questId} не найден!");
            return;
        }

        // Проверяем зависимости
        if (!CheckDependencies(currentQuest))
        {
            Debug.Log($"Не выполнены зависимости для квеста {questId}");
            return;
        }

        currentStepIndex = 0;

        // Инициализируем прогресс
        if (!questProgress.ContainsKey(questId))
        {
            questProgress[questId] = new QuestProgress();
        }

        OnQuestStarted?.Invoke(currentQuest);
        StartCurrentStep();
    }

    // Запуск текущего шага
    void StartCurrentStep()
    {
        if (currentQuest == null || currentStepIndex >= currentQuest.steps.Length)
            return;

        var step = currentQuest.steps[currentStepIndex];
        OnQuestStepStarted?.Invoke(currentQuest, currentStepIndex);

        // В зависимости от типа шага выполняем разные действия
        switch (step.type)
        {
            case StepType.TalkToNPC:
                // Активируем NPC для диалога
                ActivateNPC(step.targetId);
                break;

            case StepType.InteractWithItem:
                // Активируем предмет
                ActivateItem(step.targetId);
                break;
        }
    }

    // Завершение шага
    public void CompleteStep(string stepId)
    {
        if (currentQuest == null)
            return;

        // Проверяем, что это действительно текущий шаг
        if (currentStepIndex < currentQuest.steps.Length &&
            currentQuest.steps[currentStepIndex].stepId == stepId)
        {
            OnQuestStepCompleted?.Invoke(currentQuest, currentStepIndex);
            questProgress[currentQuest.questId].completedSteps.Add(stepId);

            currentStepIndex++;

            // Проверяем, завершён ли квест
            if (currentStepIndex >= currentQuest.steps.Length)
            {
                CompleteQuest(currentQuest.questId);
            }
            else
            {
                StartCurrentStep();
            }
        }
    }

    // Завершение квеста
    void CompleteQuest(string questId)
    {
        Debug.Log($"Квест завершён: {questId}");

        completedQuests[questId] = true;
        SaveQuestProgress();

        // Выдаём награду
        GiveReward(currentQuest);

        OnQuestCompleted?.Invoke(currentQuest);

        // Сохраняем ссылку на завершённый квест
        var completedQuest = currentQuest;

        currentQuest = null;
        currentStepIndex = 0;

        // Автоматически запускаем следующий квест
        if (autoStartNextQuests && completedQuest != null)
        {
            TryStartNextQuest(completedQuest);
        }
    }

    void TryStartNextQuest(QuestDataSO completedQuest)
    {
        // 1. Проверяем поле nextQuestId в завершённом квесте
        if (!string.IsNullOrEmpty(completedQuest.nextQuestId))
        {
            StartQuest(completedQuest.nextQuestId);
            return;
        }

        // 2. Ищем квест, у которого этот квест в зависимостях
        foreach (var quest in allQuests)
        {
            if (quest.requiredQuests != null &&
                quest.requiredQuests.Contains(completedQuest.questId))
            {
                StartQuest(quest.questId);
                return;
            }
        }
    }

    // Проверка зависимостей
    bool CheckDependencies(QuestDataSO quest)
    {
        //foreach (var requiredQuestId in quest.requiredQuests)
        //{
        //    if (!completedQuests.ContainsKey(requiredQuestId))
        //        return false;
        //}
        return true;
    }

    // Вспомогательные методы
    QuestDataSO GetQuestData(string questId)
    {
        foreach (var quest in allQuests)
        {
            if (quest.questId == questId)
                return quest;
        }
        return null;
    }

    void ActivateNPC(string npcId)
    {
        // Находим NPC в сцене и делаем его интерактивным
        var npc = GameObject.Find(npcId);
        if (npc != null)
        {
            var interactiveNPC = npc.GetComponent<InteractiveNPC>();
            if (interactiveNPC != null)
            {
                interactiveNPC.canInteract = true;
            }
        }
    }

    void ActivateItem(string itemId)
    {
        // Находим предмет в сцене и делаем его интерактивным
        var item = GameObject.Find(itemId);
        if (item != null)
        {
            var interactiveItem = item.GetComponent<InteractiveItem>();
            if (interactiveItem != null)
            {
                interactiveItem.canInteract = true;
            }
        }
    }

    void GiveReward(QuestDataSO quest)
    {
        
    }

    // Сохранение прогресса
    void SaveQuestProgress()
    {
        
    }

    void LoadQuestProgress()
    {
        // Загрузка сохранённого прогресса
    }

    // Методы для проверки состояния
    public bool IsQuestCompleted(string questId)
    {
        return completedQuests.ContainsKey(questId);
    }

    public bool IsQuestActive(string questId)
    {
        return currentQuest != null && currentQuest.questId == questId;
    }

    public QuestDataSO GetCurrentQuest()
    {
        return currentQuest;
    }

    public int GetCurrentStepIndex()
    {
        return currentStepIndex;
    }
}

// Класс для хранения прогресса по квесту
[System.Serializable]
public class QuestProgress
{
    public List<string> completedSteps = new List<string>();
    public Dictionary<string, object> customData = new Dictionary<string, object>();
}