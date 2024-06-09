using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    private Rigidbody playerRigidbody;
    public float speed = 8f;
    public int health = 100;
    public Text healthText;
    public Vector3 healthTextOffset = new Vector3(0, 2, 0);
    public GameObject playerBulletPrefab; // 플레이어 전용 총알 프리팹 추가
    public float bulletSpeed = 10f; // 총알 발사 속도 추가

    void Start() {
        playerRigidbody = GetComponent<Rigidbody>();

        if (healthText == null) {
            Debug.LogError("Health Text is not assigned!");
        } else {
            UpdateHealthUI();
        }
    }

    void Update() {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        float xSpeed = xInput * speed;
        float zSpeed = zInput * speed;

        Vector3 newVelocity = new Vector3(xSpeed, 0f, zSpeed);
        playerRigidbody.velocity = newVelocity;

        if (healthText != null) {
            healthText.transform.position = transform.position + healthTextOffset;
            healthText.transform.rotation = Quaternion.identity;
        }

        if (Input.GetButtonDown("Fire1")) { // Fire1은 기본적으로 마우스 왼쪽 버튼에 매핑되어 있음
            Fire();
        }
    }

    void Fire() {
        if (playerBulletPrefab != null) {
            Vector3 bulletDirection = playerRigidbody.velocity.normalized; // 이동 방향
            if (bulletDirection != Vector3.zero) {
                GameObject bullet = Instantiate(playerBulletPrefab, transform.position + bulletDirection, Quaternion.identity);
                bullet.transform.forward = bulletDirection; // 총알의 방향 설정
                Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
                if (bulletRigidbody != null) {
                    bulletRigidbody.velocity = bulletDirection * bulletSpeed;
                }
            }
        }
    }

    public void TakeDamage(int damage) {
        health -= damage;
        UpdateHealthUI();
        if (health <= 0) {
            Die();
        }
    }

    void UpdateHealthUI() {
        if (healthText != null) {
            healthText.text = "Health: " + health;
        }
    }

    public void Die() {
        gameObject.SetActive(false);
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null) {
            gameManager.EndGame();
        } else {
            Debug.LogError("GameManager not found!");
        }
    }
}
