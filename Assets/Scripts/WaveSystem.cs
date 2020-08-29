using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace enBask
{
    public class WaveSystem : MonoBehaviour
    {
        public WaveDatabase Database;
        public CharacterSpawner Spawner;

        private List<WaveTracker> waves = new List<WaveTracker>();


        private void Awake()
        {
            this.waves = this.Database.Items.Select(w => new WaveTracker
            {
                Wave        = w,
                Accumulator = (w.SpawnAtStart ? w.Delay :  0f) - w.StartDelay,
            }).ToList();
        }

        private void Update()
        {
            foreach (WaveTracker tracker in this.waves.ToArray())
            {
                tracker.Accumulator += Time.deltaTime;
                if (tracker.Accumulator >= tracker.Wave.Delay)
                {
                    tracker.Accumulator -= tracker.Wave.Delay;
                    this.RunWave(tracker);
                }
            }
        }

        private void RunWave(WaveTracker wave)
        {
            if (!wave.Wave.Repeat)
            {
                this.waves.Remove(wave);
            }

            if (wave.SpawnCount >= wave.Wave.MaxWaveCount && wave.Wave.MaxWaveCount != 0)
            {
                this.waves.Remove(wave);
                return;
            }

            wave.SpawnCount++;
            Debug.Log("Spawning: " + wave.Wave.name);
            this.Spawner.SpawnCharacters(wave.Wave);
        }
    }

    class WaveTracker
    {
        public Wave Wave;
        public float Accumulator;
        public int SpawnCount;
    }
}