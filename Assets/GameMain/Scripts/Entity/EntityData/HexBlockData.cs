using System;
using UnityEngine;

namespace StarForce
{
    [Serializable]
    public class HexBlockData : EntityData
    {
        bool m_isOdd;
        Vector2 m_pos;
        BlockType m_blockType;

        public HexBlockData(int entityId, int typeId, Vector2 pos, BlockType blockType)
            : base(entityId, typeId)
        {
            m_blockType = blockType;
            m_pos = pos;
            m_isOdd = pos.x % 2 == 1;
        }
        
        /// <summary>
        /// x为奇数还是偶数
        /// </summary>
        public bool IsOdd
        {
            get
            {
                return m_isOdd;
            }
        }

        /// <summary>
        /// 位置索引
        /// </summary>
        public Vector2 Pos
        {
            get
            {
                return m_pos;
            }
        }

        /// <summary>
        /// 地块基础类型
        /// </summary>
        public BlockType BlockType
        {
            get
            {
                return m_blockType;
            }
        }
    }
}
