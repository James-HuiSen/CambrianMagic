using UnityEngine;
using System.Collections.Generic;

public class SocketManager : MonoBehaviour
{
    // 用于在 Inspector 面板中引用的所有插槽
    public List<Transform> sockets = new List<Transform>();

    // 用于跟踪每个插槽当前附加的部件
    private Dictionary<Transform, GameObject> attachedParts = new Dictionary<Transform, GameObject>();

    /// <summary>
    /// 将一个部件的预制件附加到指定的插槽上。
    /// </summary>
    /// <param name="partPrefab">要附加的部件预制件。</param>
    /// <param name="socketIndex">目标插槽在 sockets 列表中的索引。</param>
    public void AttachPart(GameObject partPrefab, int socketIndex)
    {
        // 1. 检查索引是否有效
        if (socketIndex < 0 || socketIndex >= sockets.Count)
        {
            Debug.LogError($"无效的插槽索引: {socketIndex}");
            return;
        }

        Transform socket = sockets[socketIndex];

        // 2. (可选) 如果插槽已被占用，先移除旧部件
        if (attachedParts.ContainsKey(socket))
        {
            DetachPart(socketIndex);
        }

        // 3. 实例化新部件并附加到插槽上
        GameObject newPart = Instantiate(partPrefab, socket.position, socket.rotation);
        newPart.transform.SetParent(socket);

        // 4. (重要) 重置局部变换以确保其与插槽对齐
        newPart.transform.localPosition = Vector3.zero;
        newPart.transform.localRotation = Quaternion.identity;

        // 5. 记录已附加的部件
        attachedParts[socket] = newPart;
    }

    /// <summary>
    /// 从指定的插槽上分离并销毁部件。
    /// </summary>
    /// <param name="socketIndex">目标插槽的索引。</param>
    public void DetachPart(int socketIndex)
    {
        if (socketIndex < 0 || socketIndex >= sockets.Count)
        {
            Debug.LogError($"无效的插槽索引: {socketIndex}");
            return;
        }

        Transform socket = sockets[socketIndex];

        if (attachedParts.TryGetValue(socket, out GameObject partToDetach))
        {
            Destroy(partToDetach);
            attachedParts.Remove(socket);
        }
    }
}