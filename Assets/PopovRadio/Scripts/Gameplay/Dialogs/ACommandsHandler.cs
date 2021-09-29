using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FuzzySharp;
using PopovRadio.Scripts.Gameplay.SpeechRecognition;
using UnityEngine;

namespace PopovRadio.Scripts.Gameplay.Dialogs
{
    /// <summary>
    /// Обработчик голосовых команд
    /// </summary>
    public abstract class ACommandsHandler<T> : MonoBehaviour where T : Command
    {
        #region Constant Fields

        private const string RussianLangPattern = "[^ а-яА-Я0-9]";

        #endregion

        #region Settings

        [SerializeField] private SpeechRecognizer speechRecognizer;

        [Tooltip("Минимальный уровень совпадения команды с распознанным текстом")] [Range(0, 100)] [SerializeField]
        private int minCommandAccuracy = 70;

        [Tooltip("Список команд для распознавания")] [SerializeField]
        private List<T> commands = new List<T>();

        #endregion

        #region Properties

        private IEnumerable<string> CommandsText => commands.Select(command => command.Text);

        #endregion

        #region LifeCycle

        private void OnEnable()
        {
            speechRecognizer.OnReceiveFinalResult.AddListener(OnReceiveFinalCommandResult);
        }

        private void OnDisable()
        {
            speechRecognizer.OnReceiveFinalResult.RemoveListener(OnReceiveFinalCommandResult);
        }

        #endregion

        #region Private Methods

        private void OnReceiveFinalCommandResult(FinalResult finalResult)
        {
            CheckCommand(finalResult.text);
        }

        private void CheckCommand(string command)
        {
            var matchedElement =
                Process.ExtractOne(command, CommandsText, RussianFuzzyPreprocessor, null, minCommandAccuracy);
            if (matchedElement == null) return;

            Debug.Log($"Распознанная команда: {matchedElement.Value}");
            HandleCommand(commands[matchedElement.Index]);
        }

        protected virtual void HandleCommand(T command)
        {
            command.AppEvent?.Invoke();
        }

        private static string RussianFuzzyPreprocessor(string input)
        {
            input = Regex.Replace(input, RussianLangPattern, " ");
            input = input.ToLower();

            return input.Trim();
        }

        #endregion
    }
}