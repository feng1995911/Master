using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class HexBlock : Entity
    {
        [SerializeField]
        private HexBlockData m_HexBlockDataa = null;

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);
            gameObject.SetLayerRecursively(Constant.Layer.TargetableObjectLayerId);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnShow(object userData)
#else
        protected internal override void OnShow(object userData)
#endif
        {
            base.OnShow(userData);

            m_HexBlockDataa = userData as HexBlockData;
            if (m_HexBlockDataa == null)
            {
                Log.Error("Targetable object data is invalid.");
                return;
            }
        }
    }
}
