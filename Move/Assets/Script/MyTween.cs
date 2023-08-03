using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDT.Tweening
{
    public enum Ease
    {
        Line,
        QuadEaseIn,
        QuadEaseOut,
        QuadEaseInOut
    }
    public class MyTweenCore
    {
        internal Coroutine coroutine;
        internal Ease ease = Ease.Line;
        internal Action<Vector3> setter;
        internal Vector3 begin;
        internal Vector3 end;
        internal float time;
        internal bool pingpong;

        public MyTweenCore(Action<Vector3> setter, Vector3 begin, Vector3 end, float time, bool pingpong)
        {
            this.setter = setter;
            this.begin = begin;
            this.end = end;
            this.time = time;
            this.pingpong = pingpong;
        }

        public MyTweenCore SetEase(Ease ease)
        {
            this.ease = ease;
            return this;
        }
    }
    public class MyTween : MonoBehaviour
    {
        private static bool applicationIsQuitting = false;
        private static MyTween _instance = default;
        public static MyTween Instance
        {
            get
            {
                if (applicationIsQuitting)
                {
                    return null;
                }
                if (_instance == null)
                {
                    GameObject go = new GameObject($"{typeof(MyTween)}(MonoSingleton)");
                    _instance = go.AddComponent<MyTween>();
                    DontDestroyOnLoad(_instance);
                }
                return _instance;
            }
        }

        protected MyTween()
        {
        }

        public static void Kill(MyTweenCore myTweenCore)
        {
            Instance.KillTween(myTweenCore);
        }
        public static MyTweenCore DoMove(GameObject gameObject, Vector3 begin, Vector3 end, float time, bool pingpong)
        {
            return Instance.move(gameObject,  begin,  end,  time, pingpong);
        }

        private void KillTween(MyTweenCore  myTweenCore)
        {
            StopCoroutine(myTweenCore.coroutine);
        }

        private MyTweenCore move(GameObject gameObject, Vector3 begin, Vector3 end, float time, bool pingpong)
        {
            var _tween = new MyTweenCore((p) => { gameObject.transform.position = p; }, begin, end, time, pingpong);
            _tween.coroutine = StartCoroutine(MoveTweenCoroutine(_tween));
            return _tween;
        }

        IEnumerator MoveTweenCoroutine(MyTweenCore myTweenCore)
        {
            float timer = 0f;
            float t = 0;
            while (t <= 1f)
            {
                myTweenCore.setter?.Invoke(Vector3.Lerp(myTweenCore.begin, myTweenCore.end, t));
                if (!myTweenCore.pingpong && t == 1f) break;
                yield return null;
                timer += Time.deltaTime / myTweenCore.time;
                t = myTweenCore.pingpong ? Mathf.PingPong(timer, 1) : Mathf.Clamp01(timer);
                switch (myTweenCore.ease)
                {
                    case Ease.Line:
                        break;
                    case Ease.QuadEaseIn:
                        t = Quad.EaseIn(t);
                        break;
                    case Ease.QuadEaseOut:
                        t = Quad.EaseOut(t);
                        break;
                    case Ease.QuadEaseInOut:
                        t = Quad.EaseInOut(t);
                        break;
                }
            }
        }

        public class Quad
        {
            public static float EaseIn(float time)
            {
                return time * time;
            }
            public static float EaseOut(float time)
            {
                return -time * (time - 2f);
            }
            public static float EaseInOut(float time)
            {
                if ((time *= 2f) < 1f)
                {
                    return 0.5f * time * time;
                }
                return -0.5f * (--time * (time - 2) - 1);
            }
        }
    }

    public static class MyTweenExtensions
    {
        public static MyTweenCore DoMove(this GameObject gameObject, Vector3 begin, Vector3 end, float time, bool pingpong)
        {
            return MyTween.DoMove(gameObject, begin, end, time, pingpong);
        }
    }
}
