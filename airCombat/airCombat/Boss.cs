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

    class Boss
    {

        public Animation bossAnimation;

        public Texture2D bossTexture;

        public Vector2 bossPosition;

        public Rectangle bossRectangle;

        public int bossDamage;

        public int bossHealth;

        public bool bossActive;

        // get the width of the boss
        public int Width
        {
            get { return bossAnimation.FrameWidth; }
        }

        // Get the height of the boss
        public int Height
        {
            get { return bossAnimation.FrameHeight; }
        }

        public void Initialize(Animation animation, Vector2 position) //Texture2D texture
        {
            bossPosition = position;

            bossAnimation = animation;

            bossActive = true;

            bossDamage = 10;

            bossHealth = 10;

            
        }

        public void Update(GameTime gameTime)
        {

            bossAnimation.Position = bossPosition;
            bossAnimation.Update(gameTime);

            //bossRectangle = new Rectangle(0, 0, bossTexture.Width, bossTexture.Height);
            if (bossHealth <= 0)
            {
                bossActive = false;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
           bossAnimation.Draw(spriteBatch);
        }



    }
}
