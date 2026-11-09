    using System.Collections;
    using UnityEngine;
    using UnityEngine.Events;

    public class KnockBackFeedback : MonoBehaviour
    {
        private Rigidbody2D rb2D;
        private bool cancelled;
        [SerializeField] private float strength = 16f, delay = 0.15f;

        public void ResetFeedback() => cancelled = false; // call on respawn


        public UnityEvent OnBegin, OnDone;

        void Awake()
        {
            rb2D = GetComponent<Rigidbody2D>();
        }
        public void PlayerFeedback(GameObject sender)
        {
            if(cancelled) return;

    
            StopAllCoroutines();
            OnBegin?.Invoke();
            Vector2 direction = ((Vector2)transform.position - (Vector2)sender.transform.position).normalized;
            if (direction == Vector2.zero) direction = Vector2.right;
            rb2D.AddForce(direction * strength, ForceMode2D.Impulse);
            StartCoroutine(Reset());
        }
        private IEnumerator Reset()
        {
            yield return new WaitForSeconds(delay);
            rb2D.linearVelocity = Vector3.zero;

            if(cancelled) yield break;
            OnDone?.Invoke();
        }

        public void Cancel()
        {
            cancelled = true;
            StopAllCoroutines();
        }
    }
