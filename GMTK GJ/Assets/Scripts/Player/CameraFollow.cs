using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform m_MinLevelBoarder;
    [SerializeField] private Transform m_MaxLevelBoarder;

    private CinemachineVirtualCamera m_VCam;
    private Transform m_Target;

    // Start is called before the first frame update
    void Start()
    {
        m_VCam = GetComponent<CinemachineVirtualCamera>();
        m_Target = m_VCam.Follow;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Target.position.x >= m_MaxLevelBoarder.position.x)
            m_VCam.Follow = m_MaxLevelBoarder;
        else if (m_Target.position.x <= m_MinLevelBoarder.position.x)
            m_VCam.Follow = m_MinLevelBoarder;
        else m_VCam.Follow = m_Target;
    }
}
