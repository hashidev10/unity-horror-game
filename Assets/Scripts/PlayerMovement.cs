using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Unity에서는 인스펙터 창에 public 멤버 변수가 나타남
    // public 멤버 변수는 파스칼 케이스가 아닌 카멜 케이스를 사용
    public float turnSpeed = 20f;
    public float moveSpeed = 1.0f;
    // 특성 메서드가 아닌 클래스에 속한 변수를 멤버변수라하고 m_(파스칼 케이스)으로 시작
    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Vector3 m_Movement;
    // Quaternion은 회전을 저장하는 변수
    Quaternion m_Rotation = Quaternion.identity;    


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ok");
        // GetComponent는 제네릭 메서드 이므로 홑화살표 사이에 유형 파라미터를 넣어준다.
        m_Animator = GetComponent<Animator>(); 
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log("ok");

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        // 백터의 정규화를 통해 백터의 방향은 유지하면서 크기를 1로 변경
        m_Movement.Set(horizontal,0f,vertical); 
        m_Movement.Normalize();
        // 수평,수직 값이 0에 가깝지 않으면 참을 반환
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking",isWalking); ;
        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }
        // transform.forward로 시작하여 m_Movement 변수를 목표로 회전
        // 3번째 파라미터는 각도변화, 4번째 파라미터는 크기변화
        // deltaTime(프레임 간 시간)을 곱해 초당 프레임 수가 케릭터의 회전 속도에 영향을 안미치게 한다.
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        // 현재 리지드바디의 위치에서 시작해서 프레임당 이동위치와 이동 벡터를 곱한값을 더함
        m_Rotation = Quaternion.LookRotation(desiredForward);   
    }

    private void OnAnimatorMove()
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position+m_Movement*m_Animator.deltaPosition.magnitude*moveSpeed);   
        m_Rigidbody.MoveRotation(m_Rotation);   
    }

   

}
