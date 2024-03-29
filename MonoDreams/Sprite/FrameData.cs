using System.Collections.Generic;
using System.IO;
using System.Linq;
using DefaultEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoDreams.Component;
using Newtonsoft.Json;

namespace MonoDreams.Sprite;

public class FrameData
{
    public Rectangle frame { get; set; }
    public int duration { get; set; }
}

public class AnimationData
{
    public Dictionary<string, FrameData> frames { get; set; }
    
    public static AnimationData LoadAnimationData(string path)
    {
        var jsonContent = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<AnimationData>(jsonContent);
    }

    public void PopulateEntityAnimation(DefaultEcs.Entity entity, Texture2D spriteSheet, Rectangle destination)
    {
        var entityFrames = new List<Rectangle>();
        var durations = new List<int>();

        foreach (var frameData in frames.Values)
        {
            entityFrames.Add(frameData.frame);
            durations.Add(frameData.duration);
        }

        entity.Set(new DrawInfo
        {
            SpriteSheet = spriteSheet,
            Source = entityFrames[0],
            Destination = destination,
            Color = Color.White,
        });

        entity.Set(new Animation
        {
            FrameDuration = (float)durations.Average() / 1000f,
            CurrentFrame = 0,
            TotalFrames = entityFrames.Count
        });
    }
}
