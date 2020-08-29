using System;
using UnityEngine;

namespace enBask
{
    public class WaveDisplayer : MonoBehaviour
    {
        public Wave Wave;
        public Color Color = UnityEngine.Color.yellow;

        private void OnDrawGizmos()
        {
            if (!this.Wave) return;

            Gizmos.matrix = this.transform.localToWorldMatrix;
            Gizmos.color = this.Color;
            Gizmos.DrawWireCube(this.Wave.Position, this.Wave.Size);
        }
    }
}