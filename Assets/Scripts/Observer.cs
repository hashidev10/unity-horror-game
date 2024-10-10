using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    // 플레이어 캐릭터의 Transform 확인
    public Transform player;
    public GameEnding gameEnding;
    bool m_IsPlayerInRange;

    /*
     * 플레이어가 적 감지 범위에 들어갔는지 확인
     */
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform == player)
        {
            m_IsPlayerInRange = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = false;
        }
    }

    // 특정 지점에서 시작하는 선을 레이, 레이를 따라 콜라이더를 확인하는 것을 레이캐스트라고 함
    private void Update()
    {
        if(m_IsPlayerInRange) {
            Vector3 direction = player.position - transform.position + Vector3.up;
            Ray ray = new Ray(transform.position, direction);

            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.collider.transform == player)
                {
                    gameEnding.CaughtPlayer();
                }
            }
        }
    }
}
