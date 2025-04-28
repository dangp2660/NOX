using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public GameObject boss; // Tham chiếu đến boss
    public GameObject player; // Tham chiếu đến người chơi
    private bool bossFightStarted = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra nếu người chơi vào trigger
        if (collision.CompareTag("Player"))
        {
            StartBossFight();
        }
    }

    // Khởi động trận chiến với boss
    private void StartBossFight()
    {
        bossFightStarted = true;

        // Khởi động trận chiến boss
        boss.GetComponent<BossBase>().StartBossFight();

        // Thêm hiệu ứng âm thanh, thay đổi ánh sáng, hoặc những thứ khác ở đây nếu cần
        Debug.Log("Boss fight has started!");
    }
}
