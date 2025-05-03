    using UnityEngine;

    public class LookAtPlayer : MonoBehaviour
    {
        [SerializeField] private Transform player; // Tham chiếu đến Player

        private float initialScaleX;

        private void Start()
        {
            // Lưu lại hướng ban đầu để lật đúng
            initialScaleX = transform.localScale.x;
        }

        private void Update()
        {
        if (player == null)
        {
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if (found != null)
                player = found.transform;
            else
                return; // Không tìm thấy player, bỏ qua
        }
        float direction = player.position.x - transform.position.x;

            if (Mathf.Abs(direction) > 0.01f)
            {
                // Nếu Player ở bên phải -> scale.x dương, bên trái -> âm
                float newScaleX = direction > 0 ? Mathf.Abs(initialScaleX) : -Mathf.Abs(initialScaleX);
                transform.localScale = new Vector3(newScaleX, transform.localScale.y, transform.localScale.z);
            }
        }
    }
