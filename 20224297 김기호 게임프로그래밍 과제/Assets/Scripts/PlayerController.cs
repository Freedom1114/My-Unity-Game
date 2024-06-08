using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    private Rigidbody playerRigidbody; // 이동에 사용할 리지드바디 컴포넌트
    public float speed = 8f; // 이동 속력
    public int maxHP = 100; // 최대 HP
    private int currentHP; // 현재 HP

    public Image hpBar; // HP 바 UI 이미지
    public Transform hpBarBackground; // HP 바 배경 Transform

    void Start() {
        // 게임 오브젝트에서 Rigidbody 컴포넌트를 찾아 playerRigidbody에 할당
        playerRigidbody = GetComponent<Rigidbody>();
        // 현재 HP를 최대 HP로 초기화
        currentHP = maxHP;
        // HP 바를 초기화
        UpdateHPBar();
    }

    void Update() {
        // 수평과 수직 축 입력 값을 감지하여 저장
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        // 실제 이동 속도를 입력 값과 이동 속력을 통해 결정
        float xSpeed = xInput * speed;
        float zSpeed = xInput * speed;

        // Vector3 속도를 (xSpeed, 0, zSpeed)으로 생성
        Vector3 newVelocity = new Vector3(xSpeed, 0f, zSpeed);
        // 리지드바디의 속도에 newVelocity를 할당
        playerRigidbody.velocity = newVelocity;
    }

    // LateUpdate는 모든 Update 메서드가 실행된 후에 호출됨
    void LateUpdate() {
        // HP 바를 업데이트
        UpdateHPBarPosition();
    }

    // 플레이어가 데미지를 입었을 때 호출할 메서드
    public void TakeDamage(int damage) {
        // 현재 HP를 감소시킴
        currentHP -= damage;

        // HP 바를 업데이트
        UpdateHPBar();

        // 현재 HP가 0 이하가 되면 Die 메서드 호출
        if (currentHP <= 0) {
            Die();
        }
    }

    void UpdateHPBar() {
        // HP 비율을 계산하여 HP 바의 fillAmount를 설정
        hpBar.fillAmount = (float)currentHP / maxHP;
    }

    void UpdateHPBarPosition() {
        // HP 바의 위치를 플레이어의 위치로 설정
        Vector3 hpBarPosition = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        hpBarBackground.position = hpBarPosition;
    }

    public void Die() {
        // 자신의 게임 오브젝트를 비활성화
        gameObject.SetActive(false);

        // 씬에 존재하는 GameManager 타입의 오브젝트를 찾아서 가져오기
        GameManager gameManager = FindObjectOfType<GameManager>();
        // 가져온 GameManager 오브젝트의 EndGame() 메서드 실행
        gameManager.EndGame();
    }
}