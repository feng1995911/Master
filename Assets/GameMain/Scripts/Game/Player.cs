using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        CharacterController cc;
        public float speed = 20;
        public float viewRange = 30;
        public HexChunk chunkPrefab;
        Transform transformCache;

        private void Start()
        {
            cc = GetComponent<CharacterController>();
            transformCache = transform;
        }

        void Update ()
        {
            UpdateInput();
            UpdateWorld();
        }

        void UpdateInput()
        {
            if (cc == null)
                return;

            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");

            var x = Input.GetAxis("Mouse X");
            var y = Input.GetAxis("Mouse Y");

            transformCache.rotation *= Quaternion.Euler(0f, x, 0f);
            transformCache.rotation *= Quaternion.Euler(-y, 0f, 0f);

            if (Input.GetButton("Jump"))
            {
                // cc.Move((transformCache.right * h + transformCache.forward * v + transformCache.up) * speed * Time.deltaTime);
            }
            else
            {
                // cc.SimpleMove(transformCache.right * h + transformCache.forward * v * speed);
                // cc.SimpleMove(new Vector3(1,1,1));
                transform.Translate(new Vector3(h, 0, v));
            }
        }

        void UpdateWorld()
        {
            for (float x = transform.position.x - viewRange; x < transform.position.x + viewRange; x += HexChunk.width)
            {
                for (float z = transform.position.z - viewRange; z < transform.position.z + viewRange; z += HexChunk.width)
                {
                    Vector3 pos = new Vector3(x, 0, z);
                    pos.x = Mathf.Floor(pos.x / (float)HexChunk.width) * HexChunk.width;
                    pos.z = Mathf.Floor(pos.z / (float)HexChunk.width) * HexChunk.width;

                    // HexChunk chunk = HexChunk.GetChunk(pos);
                    // if (chunk != null) continue;

                    // chunk = (HexChunk)Instantiate(chunkPrefab, pos, Quaternion.identity);
                }
            }
        }
    }
}