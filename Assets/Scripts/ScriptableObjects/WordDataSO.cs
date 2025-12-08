using UnityEngine;

[CreateAssetMenu(fileName = "WordData", menuName = "Dictionary/WordData")]
public class WordDataSO : ScriptableObject
{
    public string wordId;
    public string category;
    public Sprite icon;
    public string englishWord;
    public AudioClip pronunciation;
    public string[] translation;
    public string[] examples;
    public string[] translationExamples;
    public AudioClip[] examplesPronunciation;
}