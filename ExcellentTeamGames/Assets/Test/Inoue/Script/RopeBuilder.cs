// ===============================================
// RopeBuilder.cs
// 中間チェイン生成＋曲線表現スクリプト
// （始点と終点を連結する複数の中間チェインを自動生成し、LineRendererで曲線状に描画）
// ===============================================

using UnityEngine;
using System.Collections.Generic;

// RopeBuilderクラス：ロープ（チェイン）を生成して描画するクラス
public class RopeBuilder : MonoBehaviour
{
    //鮮度カラー
    public float freshness;
    //鮮度減少率
    public float freshnessLost;

    // 始点となるRigidbody
    public Rigidbody startPoint;

    // 終点となるRigidbody
    public Rigidbody endPoint;

    // 作成する中間チェインの数
    public int chainCount = 10;

    // チェイン同士の間隔
    public float chainSpacing = 0.5f;

    // チェイン用に使うPrefab（Sphereなど）
    public GameObject chainPrefab;

    // 生成されたチェインのRigidbodyリスト
    private List<Rigidbody> chainLinks = new List<Rigidbody>();

    // 曲線描画用LineRenderer
    private LineRenderer lineRenderer;
    private LineRenderer outlineRenderer;

    [SerializeField]
    private float lineSize = 1.1f;

    [SerializeField]
    private Gradient m_color = new Gradient();
    public Color m_OutLineColor = Color.white;

    public GameManager manager;

    // 起動時にチェイン生成＆LineRendererセットアップ
    void Start()
    {
        freshness = 1.0f;
        // 中間チェインを生成
        CreateChain();

        // LineRendererをセットアップ
        SetupLineRenderer();
    }

    // 毎フレーム、ロープの曲線を更新
    void Update()
    {
        UpdateLineRenderer();
    }

    // 中間チェインを生成して連結する処理
    void CreateChain()
    {
        // 直前のRigidbody（最初はstartPoint）
        Rigidbody prevBody = startPoint;

        // 指定数だけ中間チェインを生成
        for (int i = 0; i < chainCount; i++)
        {
            // 始点から終点まで等間隔で配置
            GameObject chain = Instantiate(
                chainPrefab,
                Vector3.Lerp(startPoint.position, endPoint.position, (i + 1f) / (chainCount + 1f)),
                Quaternion.identity,
                transform
            );

            // Rigidbodyが無ければ追加
            Rigidbody rb = chain.GetComponent<Rigidbody>();
            if (rb == null) rb = chain.AddComponent<Rigidbody>();

            // SpringJointで前のボディと接続
            SpringJoint joint = chain.AddComponent<SpringJoint>();
            joint.connectedBody = prevBody;

            // 張力（バネの強さ）を設定
            joint.spring = 100f;

            // 減衰（バネの揺れ抑制）を設定
            joint.damper = 5f;

            // 自動アンカー設定を無効化
            joint.autoConfigureConnectedAnchor = false;

            // 接続点（自身側）を原点に
            joint.anchor = Vector3.zero;

            // 接続点（相手側）を原点に
            joint.connectedAnchor = Vector3.zero;

            // 作成したチェインをリストに追加
            chainLinks.Add(rb);

            // 次のチェインの接続先にする
            prevBody = rb;
        }

        // 最後にendPointと最後のチェインを接続
        SpringJoint endJoint = endPoint.gameObject.AddComponent<SpringJoint>();
        endJoint.connectedBody = prevBody;
        endJoint.spring = 100f;
        endJoint.damper = 5f;
        endJoint.autoConfigureConnectedAnchor = false;
        endJoint.anchor = Vector3.zero;
        endJoint.connectedAnchor = Vector3.zero;
    }

    //// LineRendererを設定する処理
    //void SetupLineRenderer()
    //{
    //    // 自身にLineRendererを追加
    //    lineRenderer = gameObject.AddComponent<LineRenderer>();

    //    // 線の頂点数（始点＋中間＋終点）
    //    lineRenderer.positionCount = chainCount + 2;

    //    // 線の始点側の幅
    //    lineRenderer.startWidth = lineSize;

    //    // 線の終点側の幅
    //    lineRenderer.endWidth = lineSize;

    //    // 簡易マテリアル（Sprite用）を設定
    //    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

    //    // 線の両端を滑らかに丸める
    //    lineRenderer.numCapVertices = 5;
    //}

    void SetupLineRenderer()
    {
        // アウトライン用オブジェクトを作成
        GameObject outlineObj = new GameObject("LineOutline");
        outlineObj.transform.SetParent(this.transform, false);
        outlineRenderer = outlineObj.AddComponent<LineRenderer>();

        // 本体用 LineRenderer を追加
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.sortingOrder = 10;
        outlineRenderer.sortingOrder = 9;

        // --- 共通設定 ---
        SetupRendererCommon(lineRenderer, lineSize, Color.white);
        SetupRendererCommon(outlineRenderer, lineSize + 0.05f, m_OutLineColor);
    }

    void SetupRendererCommon(LineRenderer lr, float width, Color color)
    {
        lr.positionCount = chainCount + 2;
        lr.startWidth = width;
        lr.endWidth = width;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.material.color = color;
        lr.numCapVertices = 5;
    }

    // LineRendererを毎フレーム更新（各チェイン位置を反映）
    void UpdateLineRenderer()
    {
        /*
        freshness -= freshnessLost;
        if (freshness < 0.0f)
            freshness = 0.0f;
        */

        float STL = (1.0f / manager.MaxTime) * manager.elapsedTime;
        if (STL > 1.0f) STL = 1.0f;

        Color freshnessSet = m_color.Evaluate(STL);


        lineRenderer.startColor = freshnessSet;
        lineRenderer.endColor = freshnessSet;

        // LineRendererが存在しなければ何もしない
        if (lineRenderer == null) return;

        // 始点位置をセット
        lineRenderer.SetPosition(0, startPoint.position);
        outlineRenderer.SetPosition(0, startPoint.position);

        // 中間チェインの位置を順番にセット
        for (int i = 0; i < chainLinks.Count; i++)
        {
            lineRenderer.SetPosition(i + 1, chainLinks[i].position);
            outlineRenderer.SetPosition(i + 1, chainLinks[i].position);
        }

        // 終点位置をセット
        lineRenderer.SetPosition(chainLinks.Count + 1, endPoint.position);
        outlineRenderer.SetPosition(chainLinks.Count + 1, endPoint.position);
    }
}
