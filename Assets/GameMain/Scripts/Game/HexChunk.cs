using UnityEngine;
using System.Collections.Generic;
using SimplexNoise;

namespace StarForce
{
    public enum BlockType
    {
        None = 0,
        Sea = 1,
        Water = 2,
        Beach = 3,
        Glass = 4,
        Glass2 = 5,
        Hill1 = 6,
        Hill2 = 7,
        Hill3 = 8,
        Hill4 = 9,
        Hill5 = 10,
        Hill6 = 11,
        Hill7 = 12,
        Hill8 = 13,
        Mountain1 = 14,
        Mountain2 = 15,
        Mountain3 = 16,
        Mountain4 = 17,
    }

    public class HexChunk : MonoBehaviour
    {
        public static List<HexChunk> chunks = new List<HexChunk>();
        public static int width = 100;
        public static float HexHeight = 5;

        
        public float baseHeight = 10;
        public float waterHeight = 20;
        public float deltaHeight = 2;
        public int seed;
        public float frequency = 0.025f;
        public float amplitude = 1;
        public Transform waterPlaneTrans;

        Transform transCache;
        float sideLength;
        float dLength;
        float halfHeight;

        BlockType[,] map;

        Vector3 offset0;
        Vector3 offset1;
        Vector3 offset2;
        
        void Start ()
        {
            //初始化时将自己加入chunks列表
            chunks.Add(this);
            transCache = this.transform;
            halfHeight = HexHeight / 2;
            sideLength = (float)(HexHeight / System.Math.Sqrt(3));
            dLength = (float)(1.5 * sideLength);
            
            if (waterPlaneTrans != null)
            {
                waterPlaneTrans.localPosition = new Vector3(width * 2.5f, -0.1f, width * 2.5f);
                waterPlaneTrans.localScale = new Vector3(width / 2, 1, width / 2);
            }
            
            //初始化地图
            InitMap();
        }

        void InitMap()
        {
            //初始化随机种子
            Random.InitState(seed);
            offset0 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);
            offset1 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);
            offset2 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);

            //初始化Map
            map = new BlockType[width, width];

            //遍历map，生成其中每个Block的信息
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    map[x, y] = GenerateBlockType(new Vector3(x, y, 0) + transform.position);
                }
            }

            //根据生成的信息，Build出Chunk的网格
            BuildChunk();
        }

        int GenerateHeight(Vector3 wPos)
        {
            //让随机种子，振幅，频率，应用于我们的噪音采样结果
            float x0 = (wPos.x + offset0.x) * frequency;
            float y0 = (wPos.y + offset0.y) * frequency;

            float x1 = (wPos.x + offset1.x) * frequency * 2;
            float y1 = (wPos.y + offset1.y) * frequency * 2;

            float x2 = (wPos.x + offset2.x) * frequency / 4;
            float y2 = (wPos.y + offset2.y) * frequency / 4;

            float noise0 = Noise.Generate(x0, y0) * amplitude;
            float noise1 = Noise.Generate(x1, y1) * amplitude / 2;
            float noise2 = Noise.Generate(x2, y2) * amplitude / 4;

            //在采样结果上，叠加上baseHeight，限制随机生成的高度下限
            return Mathf.FloorToInt(noise0 + noise1 + noise2 + baseHeight);
        }

        BlockType GenerateBlockType(Vector3 wPos)
        {
            //获取当前位置方块随机生成的高度值
            float genHeight = GenerateHeight(wPos);

            if (genHeight < waterHeight) 
            {
                if (genHeight < waterHeight - deltaHeight)
                {
                    return BlockType.Sea;
                }
                else
                {
                    return BlockType.Water;
                }
                    
            }

            int lvl = (int)(genHeight - waterHeight / deltaHeight) + (int)BlockType.Beach;
            lvl = Mathf.Min(lvl, (int)BlockType.Mountain4);
            return (BlockType)lvl;
        }   

        public void BuildChunk()
        {
            //遍历chunk, 生成其中的每一个Block
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    BuildBlock(x, y);
                }
            }
        }

        void BuildBlock(int x, int y)
        {
            if (map[x, y] <= BlockType.Water) return;

            BlockType typeid = map[x, y];

            bool isOddNum = x % 2 == 1;
            float extY = isOddNum ?  halfHeight : 0;
            GameEntry.Entity.ShowHexBlock(new HexBlockData(GameEntry.Entity.GenerateSerialId(), 80000 + (int)typeid, new Vector2(x, y), typeid)
            {
                Position = new Vector3(x * dLength, 0, y * HexHeight + extY),
            });
        }
    }
}

