using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float speed = 8f;
    private Rigidbody bulletRigidbody;
    public int damage = 10;
    public GameObject hitEffectPrefab; // 이펙트 프리팹 추가
    public float effectDuration = 1f; // 이펙트 지속 시간 추가

    void Start() {
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletRigidbody.velocity = transform.forward * speed;
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            PlayerController playerController = other.GetComponent<PlayerController>();

            if (playerController != null) {
                playerController.TakeDamage(damage);
                
                // 이펙트 생성
                if (hitEffectPrefab != null) {
                    GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
                    Destroy(effect, effectDuration); // 일정 시간 후 이펙트 오브젝트 파괴
                }
            }

            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision) {
        // PlayerBullet과 충돌할 때 통과
        if (collision.gameObject.CompareTag("PlayerBullet")) {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }
}