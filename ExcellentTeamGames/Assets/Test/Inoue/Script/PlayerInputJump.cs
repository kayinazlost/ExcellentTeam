using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInputJump : MonoBehaviour
{
    public List<string> m_Message;
    public TMPro.TextMeshProUGUI m_MessageText;
    public float m_TextTime = 0;
    [Header("物理")]
    public Rigidbody m_Rigidbody;
    [Header("接地フラグ")]
    public bool m_EarthFlag;
    [Header("ジャンプパワー[現在値,最小値,最大値]")]
    public Vector3 m_JumpPower = new Vector3(0,100.0f,1200.0f);
    [Header("ジャンプ倍率/加速減退率")]
    public float m_AngularAndPowerAgnification = 10;
    [Header("プレイヤーサイド")]
    public bool m_PlayerSide;
    [Header("ジャンプパワーゲージ")]
    public Image m_arrowImage;
    [SerializeField]
    private Gradient m_chargeGra;
    [Header("オーディオソース")]
    public AudioSource m_AudioSource;
    [Header("ジャンプ不可時間")]
    public float m_JUmpOKChargePoint = 0.25f;
    [Header("チャージクールタイム")]
    public float m_ChargeCoolTime = 0.0f;
    [Header("チャージクール最大タイム")]
    public float m_ChargeCoolMaxTime = 2.0f;
    [SerializeField]
    private GameObject m_effectObj;
    void Start()
    {
        m_ChargeCoolTime = 0.0f;
    }

    void Update()
    {
        if (m_TextTime <= 0.0f)
        {
            m_MessageText.text = "";
        }
        else
        { 
            m_TextTime -= Time.deltaTime;
            if (m_TextTime <= 0.0f)
                m_TextTime = 0.0f;
        }

        m_ChargeCoolTime += 1.0f * Time.deltaTime;
        if (m_ChargeCoolTime >= m_ChargeCoolMaxTime)
            m_ChargeCoolTime = m_ChargeCoolMaxTime;

        if (GameManager.Instance.IsRun)
        {
            PlayerSeidInput();
        }
        /*
        if (m_EarthFlag)
        {
        }
        else
        {
            m_JumpPower.x = m_JumpPower.y;
        }
        */

        if (m_arrowImage)
        {
            if (ChargePoint() > m_JUmpOKChargePoint)
            {
                m_arrowImage.color = m_chargeGra.Evaluate(ChargePoint());
            }
            else
                m_arrowImage.color = Color.clear;
        }
    }
    public float ChargePoint()
    {
        float Powers = m_JumpPower.x - m_JumpPower.y;
        if (Powers < 0)
            Powers = 0;
        return (1.0f / (m_JumpPower.z - m_JumpPower.y)) * Powers;
    }
    public void PlayerSeidInput()
    {
        if (m_PlayerSide)
        {
            //チャージ
            if (Input.GetKey(KeyCode.RightArrow))
                ChargeJumpPowers();
            //ジャンプ
            if (Input.GetKeyUp(KeyCode.RightArrow))
                JumpUp();
        }
        else
        {
            //チャージ
            if (Input.GetKey(KeyCode.A))
                ChargeJumpPowers();
            //ジャンプ
            if (Input.GetKeyUp(KeyCode.A))
                JumpUp();
        }
    }
    public void ChargeJumpPowers()
    {
        if (m_ChargeCoolTime >=  m_ChargeCoolMaxTime)
        {
            //チャージ
            m_JumpPower.x += m_AngularAndPowerAgnification * Time.deltaTime;
            if (m_JumpPower.x > m_JumpPower.z)
                m_JumpPower.x = m_JumpPower.z;

            Vector2 jumpDir =
                    (Vector2)(this.transform.up * m_JumpPower.x) +
                    (Vector2)(this.transform.right * Mathf.Max(
                        10.0f,
                        (m_AngularAndPowerAgnification / 10) - (m_JumpPower.x - m_JumpPower.z)));

            // Z回転角を求める（上が0度、右が-90度になるよう調整）
            float angle = Mathf.Atan2(jumpDir.y, jumpDir.x) * Mathf.Rad2Deg;
            float zRotation = angle - 90f; // 「上」を基準にするため90度引く

            m_arrowImage.rectTransform.rotation = Quaternion.Euler(0, 0, zRotation);
        }
    }
    public void JumpUp()
    {
        if (ChargePoint() > m_JUmpOKChargePoint)
        {
            RaycastHit hit;
            Vector3 origin = transform.position;

            if (Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity))
            {
                // エフェクト
                var pos = hit.point;
                //pos.y = 0.05f;
                GameObject obj = Instantiate(m_effectObj, pos, Quaternion.identity);
                Destroy(obj, 3f); // 3秒後に自動で削除される
            }

            //ジャンプ
            m_Rigidbody.AddForce(
                (this.transform.up * m_JumpPower.x) +
                (this.transform.right * Mathf.Max(
                    10.0f,
                    (m_AngularAndPowerAgnification / 10) - (m_JumpPower.x - m_JumpPower.z))));
            m_JumpPower.x = m_JumpPower.y;
            m_EarthFlag = false;
            m_ChargeCoolTime = 0.0f;
            m_MessageText.text = m_Message[Random.Range(0, m_Message.Count)];
            m_TextTime = 2.0f;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        m_AudioSource.Stop();
        m_AudioSource.Play();
        //SoundManager.Instance.PlaySe("プレイヤー着地");
    }
    /// <summary>
    /// 接地中
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision)
    {
        if (!m_EarthFlag && collision.transform.tag == "Map")
        {
            m_EarthFlag = true;
        }
    }
    /// <summary>
    /// 接地解除
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision)
    {
        if(collision.transform.tag == "Map")
            m_EarthFlag = false;
    }
}
