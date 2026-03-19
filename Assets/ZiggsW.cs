using UnityEngine;
using System.Collections;

public class ZiggsW : MonoBehaviour
{
    public GameObject bombPrefab;         // ManualExplode 스크립트가 붙은 프리팹
    public float throwDistance = 5f;
    public float explosionDelay = 1.5f;
    public float pushForce = 10f;
    public float explosionRadius = 5f;
    public float explosionForce = 300f;
    public float upwardsModifier = 1f;

    private GameObject currentBomb;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ThrowBomb();
        }
    }

    void ThrowBomb()
    {
        Vector3 throwPosition = transform.position + transform.forward * throwDistance;
        currentBomb = Instantiate(bombPrefab, throwPosition, Quaternion.identity);
        StartCoroutine(ExplodeAfterDelay(currentBomb));
    }

    IEnumerator ExplodeAfterDelay(GameObject bomb)
    {
        yield return new WaitForSeconds(explosionDelay);

        Vector3 explosionPos = bomb.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);

        foreach (var col in colliders)
        {
            Rigidbody rb = col.attachedRigidbody;
            if (rb == null) continue;

            Vector3 toTarget = rb.position - explosionPos;
            float distance = toTarget.magnitude;
            Vector3 dir = toTarget.normalized;
            float attenuation = 1f - Mathf.Clamp01(distance / explosionRadius);
            dir += Vector3.up * upwardsModifier;
            dir = dir.normalized;
            Vector3 impulse = dir * explosionForce * attenuation;

            rb.AddForce(impulse, ForceMode.Impulse);
        }

        // 플레이어 자신을 폭탄 방향으로 밀어냄
        Rigidbody playerRb = GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            Vector3 directionToPlayer = (transform.position - explosionPos).normalized;
            playerRb.velocity = directionToPlayer * pushForce;
        }

        Destroy(bomb);
    }

    void OnDrawGizmosSelected()
    {
        if (currentBomb != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(currentBomb.transform.position, explosionRadius);
        }
    }
}
