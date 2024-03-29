using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoDreams.Component;
using MonoDreams.Renderer;
using MonoDreams.State;

namespace MonoDreams.System;

public sealed class DrawSystem : AEntitySetSystem<GameState>
{
    private readonly SpriteBatch _batch;
    private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
    private readonly Camera _camera;

    public DrawSystem(
        ResolutionIndependentRenderer resolutionIndependentRenderer,
        Camera camera,
        SpriteBatch batch,
        World world
        ) : base(world.GetEntities().With<DrawInfo>().AsSet())
    {
        _resolutionIndependentRenderer = resolutionIndependentRenderer;
        _camera = camera;
        _batch = batch;
    }

    protected override void PreUpdate(GameState state)
    {
        _resolutionIndependentRenderer.BeginDraw();
        _batch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp,
            DepthStencilState.None,
            RasterizerState.CullNone, 
            null,
            _camera.GetViewTransformationMatrix());
    }

    protected override void Update(GameState state, in Entity entity)
    {
        ref var drawInfo = ref entity.Get<DrawInfo>();

        if (entity.Has<Animation>())
        {
            ref var animation = ref entity.Get<Animation>();
        
            var totalDuration = animation.FrameDuration * animation.TotalFrames;
        
            animation.CurrentFrame = (int)((state.TotalTime % totalDuration) / animation.FrameDuration);
        
            var frameWidth = drawInfo.SpriteSheet.Width / animation.TotalFrames;
            drawInfo.Source = new Rectangle(frameWidth * animation.CurrentFrame, 0, frameWidth, drawInfo.SpriteSheet.Height);
        }

        _batch.Draw(drawInfo.SpriteSheet, drawInfo.Destination, drawInfo.Source, drawInfo.Color);
    }

    protected override void PostUpdate(GameState state) => _batch.End();
}