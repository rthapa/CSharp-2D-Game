using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace airCombat
{
    class ParticleGenerator
    {
        Texture2D texture;
        float spawnWidth; // the area in which raindrops will spawn
        float density; // the higher density the more rain drops will come

        List<RainDrops> raindrops = new List<RainDrops>();

        float timer;

        Random rand1, rand2;

        public ParticleGenerator(Texture2D newTexture,
            float newSpawnWidth, float newDensity)
        {
            texture = newTexture;
            spawnWidth = newSpawnWidth;
            density = newDensity;

            rand1 = new Random();
            rand2 = new Random();
        }

        public void createParticle()
        {
            //double anything = rand1.Next();

            //texture, position , velocity
            raindrops.Add(new RainDrops(texture, new Vector2(
                50 + (float)rand1.NextDouble() * spawnWidth, 0),
                new Vector2(-3, rand2.Next(5, 8))));
        }

        public void Update(GameTime gameTime, GraphicsDevice graphics)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            while (timer > 0)
            {
                timer -= 1f / density;
                createParticle();
            }

            for (int i = 0; i < raindrops.Count; i++)
            {
                raindrops[i].Update();
                if (raindrops[i].Position.Y > graphics.Viewport.Height)
                {
                    raindrops.RemoveAt(i);
                    i--;
                }

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (RainDrops raindrop in raindrops)
                raindrop.Draw(spriteBatch);
        }
    }
}
