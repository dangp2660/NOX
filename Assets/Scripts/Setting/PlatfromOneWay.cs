using System.Collections;
using UnityEngine;

public class PlayerOneWayPlatform : MonoBehaviour
{
    private GameObject currentOneWayPlatform;

    [SerializeField] private BoxCollider2D playerCollider;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWay"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWay"))
        {
            currentOneWayPlatform = null;
        }
    }

    public IEnumerator DisableCollision()
    {
        if (currentOneWayPlatform != null)  // Kiểm tra null trước khi xử lý
        {
            BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

            if (platformCollider != null)  // Kiểm tra thêm xem BoxCollider2D có tồn tại
            {
                platformCollider.enabled = false;
                yield return new WaitForSeconds(1f);
                platformCollider.enabled = true;
            }
        }
    }

}