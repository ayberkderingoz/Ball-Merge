using UnityEngine;

namespace DefaultNamespace
{
    public class GameTimer : MonoBehaviour
    {
        //singleton
        private static GameTimer _instance;
        public static GameTimer Instance => _instance;

        private float _time;

        public float Time => _time;
        
        private float maxTime = 60f;
        
        public float MaxTime => maxTime;
        
        private bool _isTimeOver = false;

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }

        void Start()
        {
            _time = 0;
            GameManager.Instance.OnRestart += OnRestart;
        }

        
        private void OnRestart()
        {
            _time = 0;
        }
        

        void Update()
        {
            if(_isTimeOver) return;
            
            _time += UnityEngine.Time.deltaTime;
            if (_time >= maxTime)
            {
                _isTimeOver = true;
            }
        }


    }
}