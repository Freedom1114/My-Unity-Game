using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {
    public float speed = 10f; // 총알 속도
    public int damage = 10; // 총알 데미지
    public GameObject hitEffectPrefab; // 이펙트 프리팹 추가
    public float effectDuration = 1f; // 이펙트 지속 시간

    private Rigidbody bulletRigidbody;

    void Start() {
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletRigidbody.velocity = transform.forward * speed;
        Destroy(gameObject, 3f); // 3초 후에 총알 파괴
    }

    void OnTriggerEnter(Collider other) {
        // BulletSpawner 오브젝트에 맞을 경우 처리
        if (other.CompareTag("BulletSpawner")) {
            BulletSpawner spawner = other.GetComponent<BulletSpawner>();
            if (spawner != null) {
                spawner.TakeDamage(damage);
            }

            // 이펙트 생성
            if (hitEffectPrefab != null) {
                GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
                Destroy(effect, effectDuration); // 일정 시간 후 이펙트 오브젝트 파괴
            }

            Destroy(gameObject); // 총알 파괴
        }
    }

    void OnCollisionEnter(Collision collision) {
        // Bullet과 충돌할 때 통과
        if (collision.gameObject.CompareTag("Bullet")) {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }
}
