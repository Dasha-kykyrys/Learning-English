using UnityEngine;

[CreateAssetMenu(fileName = "GrammarData", menuName = "Dictionary/GrammarData")]
public class GrammarDataSO : ScriptableObject
{
    public string grammarId;
    public string grammarRule;
    public string description;
    public string[] examples;
    public string[] translationExamples;
    public AudioClip[] examplesPronunciation;
}
