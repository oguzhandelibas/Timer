using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Timer
{
    public class CountDownTimer : MonoBehaviour
    {
    #region Fields
        private List<IUpdateablePerSecond> _updateables;
        private List<ITimerListener> _listeners;
        private Coroutine _countDownRoutine;
        private readonly YieldInstruction _oneSecond = new WaitForSeconds(1f);
#endregion

    #region Variables
        private int _secondsLeft;
        private int _duration;
        private bool _paused;
#endregion

    #region Public Methods
        public void Initialize()
        {
            _updateables = new List<IUpdateablePerSecond>();
            _listeners = new List<ITimerListener>();
            _duration = 20;
            _secondsLeft = _duration;
        }

        public void Clear()
        {
            _updateables.Clear();
            _listeners.Clear();
            _countDownRoutine = null;
        }

        public void AddUpdateable(IUpdateablePerSecond updateable)
        {
            _updateables.Add(updateable);
        }

        public void AddListener(ITimerListener listener)
        {
            _listeners.Add(listener);
        }

        public void ResetTimer()
        {
            _secondsLeft = _duration;
        }

        public void StartTimer()
        {
            _countDownRoutine = StartCoroutine(CountDownRoutine());
            StartAll();
        }

        public void PauseTimer()
        {
            _paused = true;
        }

        public void UnPauseTimer()
        {
            _paused = false;
        }

        public void StopTimer()
        {
            if (_countDownRoutine != null)
            {
                StopCoroutine(_countDownRoutine);
                _countDownRoutine = null;
            }

            ResetTimer();
        }
#endregion

    #region Local Methods
        private IEnumerator CountDownRoutine()
        {
            while (true)
            {
                while (_paused)
                {
                    yield return null;
                }

                yield return _oneSecond;

                _secondsLeft--;
                UpdateAll(_secondsLeft);

                if (_secondsLeft > 0)
                {
                    continue;
                }

                StopAll();
                Debug.Log("Time Out!");
                // DO SOMETHING !!

                yield break;
            }
        }

        private void StartAll()
        {
            foreach (var listener in _listeners)
            {
                listener.OnTimerStart(_duration);
            }
        }

        private void UpdateAll(int secondsLeft)
        {
            foreach (var updateable in _updateables)
            {
                updateable.OnUpdate(secondsLeft);
            }
        }

        private void StopAll()
        {
            foreach (var listener in _listeners)
            {
                listener.OnTimerStop();
            }
        }
#endregion
    }
}
