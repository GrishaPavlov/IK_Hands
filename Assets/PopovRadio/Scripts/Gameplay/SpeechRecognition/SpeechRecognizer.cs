using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;
using Vosk;

namespace PopovRadio.Scripts.Gameplay.SpeechRecognition
{
    // TODO: Отрефакторить с использованием нативных вызовов
    /// <summary>
    /// Управление распознаванием речи
    /// </summary>
    public class SpeechRecognizer : MonoBehaviour
    {
        #region Constant Fields

        private const string JavaUnityClassName = "com.unity3d.player.UnityPlayer";
        private const string JavaRecognitionClassName = "org.vosk.Recognition";

        #endregion

        #region Settings

        [Tooltip("Путь до модели для распознавания")] [SerializeField]
        private string modelPath;

        [Tooltip("Частота записи микрофона")] [SerializeField]
        private int microphoneFrequency;

        #endregion

        #region Fields

        private Model _model;
        private VoskRecognizer _recognizer;
        private AndroidJavaObject _javaRecognitionObject;

        private bool _microphoneConnected;
        private bool _recordingStarted;

#if UNITY_EDITOR
        private int _lastSample;
        private AudioClip _recordedClip;
#endif

        #endregion

        #region Properties

        private bool IsPlatformAndroid => Application.platform == RuntimePlatform.Android;

        #endregion

        #region Events

        /// <summary>
        /// Результат частичного распознавания
        /// </summary>
        public UnityEvent<PartialResult> OnReceivePartialResult;

        /// <summary>
        /// Результат итогового распознавания предложения
        /// </summary>
        public UnityEvent<FinalResult> OnReceiveFinalResult;

        #endregion

        #region LifeCycle

        private void Awake()
        {
            TryInitMicrophone();
            TryInitVosk();
            // TODO: Сделать, чтобы метод вызывался позже, например, только когда увидим Попова
            // StartRecognition();
            StartCoroutine(DelayedStart());
        }

        private IEnumerator DelayedStart()
        {
            yield return new WaitForSeconds(3);
            StartRecognition();
        }

#if UNITY_EDITOR
        private void FixedUpdate()
        {
            if (!_recordingStarted) return;

            Recognize();
        }
#endif

        #endregion

        #region Public Methods

        /// <summary>
        /// Запускает запись аудио с микрофона и распознавание речи
        /// </summary>
        public void StartRecognition()
        {
            if (!_microphoneConnected) return;

#if UNITY_EDITOR
            _recognizer = new VoskRecognizer(_model, microphoneFrequency);

            _recordedClip = Microphone.Start(null, true, 2, microphoneFrequency);
#endif

            if (IsPlatformAndroid)
            {
                _javaRecognitionObject.Call("recognizeMicrophone");
            }

            _recordingStarted = true;
        }

        /// <summary>
        /// Останавливает запись аудио с микрофона и распознавание речи
        /// </summary>
        public void StopRecognition()
        {
            if (!_recordingStarted) return;
#if UNITY_EDITOR
            Microphone.End(Microphone.devices[0]);
#endif

            if (IsPlatformAndroid)
            {
                _javaRecognitionObject.Call("pause", true);
            }

            _recordingStarted = false;
        }

        #endregion

        #region Private Methods

        private static void RequestMicrophonePermission()
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {
                Permission.RequestUserPermission(Permission.Microphone);
            }
        }

        private void TryInitVosk()
        {
#if UNITY_EDITOR
            Vosk.Vosk.SetLogLevel(0);

            var path = $"{Application.dataPath}/{modelPath}";
            if (!Directory.Exists(path))
            {
                Debug.LogError("Модель для распознавания речи не найдена.");
                return;
            }

            _model = new Model(path);
#endif
            if (!IsPlatformAndroid) return;

            var unityClass = new AndroidJavaClass(JavaUnityClassName);
            var unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
            var unityContext = unityActivity.Call<AndroidJavaObject>("getApplicationContext");

            _javaRecognitionObject = new AndroidJavaObject(JavaRecognitionClassName);
            _javaRecognitionObject.Call("setupVosk", unityContext, "model-ru-ru");
        }

        private void TryInitMicrophone()
        {
#if UNITY_EDITOR
            if (Microphone.devices.Length <= 0)
            {
                Debug.LogError("Микрофон не найден");
                return;
            }

            Debug.LogWarning($"Найден микрофон: {Microphone.devices[0]}");
#endif
            if (IsPlatformAndroid) RequestMicrophonePermission();

            _microphoneConnected = true;
        }

        private void OnPartialResult(string hypothesis)
        {
            var partialResult = JsonUtility.FromJson<PartialResult>(hypothesis);
            OnReceivePartialResult.Invoke(partialResult);
        }

        private void OnResult(string hypothesis)
        {
            var finalResult = JsonUtility.FromJson<FinalResult>(hypothesis);
            if (finalResult.text.Length <= 0) return;

            OnReceiveFinalResult.Invoke(finalResult);
        }

#if UNITY_EDITOR
        private static byte[] FloatArr2ByteArr(IReadOnlyList<float> samples)
        {
            var numArray1 = new short[samples.Count];
            var numArray2 = new byte[samples.Count * 2];
            for (var index = 0; index < samples.Count; ++index)
            {
                numArray1[index] = (short) (samples[index] * (double) short.MaxValue);
                BitConverter.GetBytes(numArray1[index]).CopyTo(numArray2, index * 2);
            }

            return numArray2;
        }

        private void Recognize()
        {
            var pos = Microphone.GetPosition(null);
            var diff = pos - _lastSample;
            if (diff > 0)
            {
                var samples = new float[diff * _recordedClip.channels];
                _recordedClip.GetData(samples, _lastSample);
                var byteArr = FloatArr2ByteArr(samples);

                if (_recognizer.AcceptWaveform(byteArr, byteArr.Length))
                {
                    OnResult(_recognizer.Result());
                }
                else
                {
                    OnPartialResult(_recognizer.PartialResult());
                }
            }

            _lastSample = pos;
        }
#endif

        #endregion
    }
}