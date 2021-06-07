using System;
using DefaultEcs;
using DefaultEcs.System;
using MonoDreams.Component;
using MonoDreams.Message;

namespace MonoDreams.System
{
    public sealed class HitSystem : ISystem<float>
    {
        private readonly World _world;

        public HitSystem(World world)
        {
            _world = world;
        }

        public bool IsEnabled { get; set; } = true;

        public void Update(float state)
        {
            Span<HitDelay> hits = _world.Get<HitDelay>();
            for (int i = 0; i < hits.Length; ++i)
            {
                ref HitDelay hit = ref hits[i];
                if ((hit.Current -= state) <= 0f)
                {
                    hit.Current = hit.Delay;
                    _world.Publish(new PlayerHitMessage());
                }
            }
        }

        public void Dispose() { }
    }
}