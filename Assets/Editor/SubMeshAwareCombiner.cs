using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class SubMeshAwareCombiner : EditorWindow
{
    private GameObject parentObject;
    private const int MaxVerticesPerMesh = 65535; // Unityのメッシュ頂点数の上限

    [MenuItem("Tools/SubMesh Aware Combiner")]
    public static void ShowWindow()
    {
        GetWindow<SubMeshAwareCombiner>("SubMesh Aware Combiner");
    }

    private void OnGUI()
    {
        GUILayout.Label("SubMesh Aware Combiner Settings", EditorStyles.boldLabel);
        parentObject = (GameObject)EditorGUILayout.ObjectField("Parent Object", parentObject, typeof(GameObject), true);

        if (GUILayout.Button("Combine Meshes"))
        {
            if (parentObject == null)
            {
                EditorUtility.DisplayDialog("Error", "親オブジェクトを選択してください。", "OK");
            }
            else
            {
                CombineMeshesWithSubMeshes();
            }
        }
    }

    private void CombineMeshesWithSubMeshes()
    {
        if (parentObject == null)
        {
            Debug.LogError("親オブジェクトが設定されていません");
            return;
        }

        // 親オブジェクト以下のすべてのメッシュフィルターを取得
        MeshFilter[] meshFilters = parentObject.GetComponentsInChildren<MeshFilter>(true);
        if (meshFilters.Length == 0)
        {
            Debug.LogWarning("結合対象のメッシュが見つかりませんでした");
            return;
        }

        List<CombineInstance> combineInstances = new List<CombineInstance>();
        List<Material> combinedMaterials = new List<Material>();
        int vertexCount = 0;
        int meshIndex = 0;

        foreach (MeshFilter filter in meshFilters)
        {
            MeshRenderer renderer = filter.GetComponent<MeshRenderer>();
            if (renderer == null || filter.sharedMesh == null)
                continue;

            Mesh mesh = filter.sharedMesh;
            Material[] materials = renderer.sharedMaterials;

            for (int subMeshIndex = 0; subMeshIndex < mesh.subMeshCount; subMeshIndex++)
            {
                if (vertexCount + mesh.vertexCount > MaxVerticesPerMesh)
                {
                    CreateCombinedMesh(combineInstances, combinedMaterials.ToArray(), meshIndex++);
                    combineInstances.Clear();
                    combinedMaterials.Clear();
                    vertexCount = 0;
                }

                CombineInstance instance = new CombineInstance
                {
                    mesh = mesh,
                    subMeshIndex = subMeshIndex,
                    transform = parentObject.transform.worldToLocalMatrix * filter.transform.localToWorldMatrix
                };
                combineInstances.Add(instance);
                combinedMaterials.Add(materials[subMeshIndex]);

                vertexCount += mesh.vertexCount;
            }
        }

        // 最後のバッチを作成
        if (combineInstances.Count > 0)
        {
            CreateCombinedMesh(combineInstances, combinedMaterials.ToArray(), meshIndex);
        }
    }

    private void CreateCombinedMesh(List<CombineInstance> combineInstances, Material[] materials, int index)
    {
        // 新しいメッシュを作成して結合
        Mesh combinedMesh = new Mesh();
        combinedMesh.indexFormat = IndexFormat.UInt32; // 65,535を超える頂点をサポートするために設定
        combinedMesh.CombineMeshes(combineInstances.ToArray(), false, true); // サブメッシュごとに保持

        // 新しいGameObjectを作成して結合メッシュを設定
        GameObject combinedObject = new GameObject($"{parentObject.name}_Combined_{index}");
        combinedObject.transform.SetParent(parentObject.transform, false); // 親オブジェクトのローカル空間に合わせる
        combinedObject.transform.localPosition = Vector3.zero;
        combinedObject.transform.localRotation = Quaternion.identity;
        combinedObject.transform.localScale = Vector3.one;

        MeshFilter filter = combinedObject.AddComponent<MeshFilter>();
        MeshRenderer renderer = combinedObject.AddComponent<MeshRenderer>();

        filter.mesh = combinedMesh;
        renderer.materials = materials; // 結合したマテリアルを設定

        // シーンで新しいオブジェクトを選択して見つけやすくする
        Selection.activeGameObject = combinedObject;
        Debug.Log($"新しいメッシュが作成されました: {combinedObject.name}");
    }
}
