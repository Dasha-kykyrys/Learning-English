using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Quest/DialogueData")]
public class DialogueDataSO : ScriptableObject
{
    public string dialogueId;
    public DialogueLine[] lines;
}

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    public string text;
}
