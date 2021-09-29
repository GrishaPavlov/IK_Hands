using System;

namespace PopovRadio.Scripts.Gameplay.SpeechRecognition
{
    /// <summary>
    /// Результат частичного распознавания
    /// </summary>
    [Serializable]
    public class PartialResult
    {
        /// <summary>
        /// Распознанный текст
        /// </summary>
        public string partial;
    }

    /// <summary>
    /// Информация о распознанном слове
    /// </summary>
    [Serializable]
    public class RecognizedWord
    {
        public float conf;
        public float end;
        public float start;
        public string word;
    }

    /// <summary>
    /// Итоговый результат распознавания
    /// </summary>
    [Serializable]
    public class FinalResult
    {
        /// <summary>
        /// Результат по распознанным словам
        /// </summary>
        public RecognizedWord[] result;

        /// <summary>
        /// Распознанный текст
        /// </summary>
        public string text;
    }
}