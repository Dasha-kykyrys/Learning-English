using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "QuestData", menuName = "Quest/QuestData")]
public class QuestDataSO : ScriptableObject
{
    [Header("Основная информация")]
    public string questId;
    public string questName;
    public string questDescription;
    public int rewardCoins;

    [Header("Следующий квест")]
    public string nextQuestId;

    [Header("Зависимости")]
    public string requiredQuests;

    [Header("Шаги квеста")]
    public QuestStep[] steps;
}

[System.Serializable]
public class QuestStep
{
    public string stepId;
    public string description;
    public StepType type;
    public string targetId; // ID NPC, предмета
}

public enum StepType
{
    TalkToNPC,      // Поговорить с NPC
    InteractWithItem // Взаимодействовать с предметом
}