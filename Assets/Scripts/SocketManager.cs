using UnityEngine;
using System.Collections.Generic;
using System.Linq; // 引入Linq以便于查询

public class SocketManager : MonoBehaviour
{
    // 定义一个更智能的Socket类，包含位置和侧边信息
    [System.Serializable]
    public class Socket
    {
        public Transform socketTransform;
        public SocketSide side;
        // 我们可以存储当前附加的部件，以了解插槽是否被占用
        [System.NonSerialized]
        public GameObject attachedPart = null;
    }

    [Header("Socket Configuration")]
    public List<Socket> sockets = new List<Socket>();

    /// <summary>
    /// 尝试将一个部件的预制件附加到匹配的空闲插槽上。
    /// </summary>
    /// <param name="partPrefab">要附加的部件预制件，其上必须有PartIdentifier组件。</param>
    public bool AttachPart(GameObject partPrefab)
    {
        // 1. 获取部件信息
        PartIdentifier partInfo = partPrefab.GetComponent<PartIdentifier>();
        if (partInfo == null)
        {
            Debug.LogError($"部件预制件 {partPrefab.name} 上缺少 PartIdentifier 组件!", partPrefab);
            return false;
        }

        // 2. 查找一个匹配且可用的插槽
        // 使用Linq查找：插槽的侧边必须匹配部件要求的侧边(或者任一方为Any)，并且该插槽当前未被占用
        Socket targetSocket = sockets.FirstOrDefault(socket =>
            (socket.side == partInfo.requiredSide || partInfo.requiredSide == SocketSide.Any || socket.side == SocketSide.Any)
            && socket.attachedPart == null);

        // 3. 如果找到了合适的插槽
        if (targetSocket != null)
        {
            // 实例化新部件并附加到插槽上
            GameObject newPart = Instantiate(partPrefab, targetSocket.socketTransform.position, targetSocket.socketTransform.rotation);
            newPart.transform.SetParent(targetSocket.socketTransform);
            newPart.transform.localPosition = Vector3.zero;
            newPart.transform.localRotation = Quaternion.identity;

            // 记录已附加的部件
            targetSocket.attachedPart = newPart;
            Debug.Log($"成功将 {partPrefab.name} 附加到 {targetSocket.side} 侧的插槽上。");
            return true;
        }
        else
        {
            Debug.LogWarning($"没有找到适合 {partPrefab.name} (要求: {partInfo.requiredSide}) 的空闲插槽。");
            return false;
        }
    }

    /// <summary>
    /// 从指定的插槽上分离并销毁部件。
    /// </summary>
    /// <param name="socketSide">要清空的插槽侧边。</param>
    public void DetachPart(SocketSide socketSide)
    {
        // 查找所有匹配侧边的插槽
        var targetSockets = sockets.Where(s => s.side == socketSide);

        foreach (var socket in targetSockets)
        {
            if (socket.attachedPart != null)
            {
                Debug.Log($"从 {socket.side} 侧的插槽上分离 {socket.attachedPart.name}。");
                Destroy(socket.attachedPart);
                socket.attachedPart = null; // 标记插槽为空闲
            }
        }
    }
}