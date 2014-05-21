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
  
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        enum GameState
        {
            MainMenu,
            Options,
            Playing,
            GameOver,
            Won,
        }

        GameState CurrentGameState = GameState.MainMenu;

        int gameLevel;

        cButton btnPlay;
        cButton btnHelp;
        cButton btnExit;
        cButton btnBack;



        Player player;
        //int playerCentre;

        Boss bossFinal;
       // int bossCentre;
        float bossMoveSpeed;

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        float playerMoveSpeed;

        //boss shooting
        Texture2D projectileTextureBoss;
        List<ProjectileEnemy> projectilesBoss;

        //the rate of fire of teh boss bullet
        TimeSpan fireTimeBoss;
        TimeSpan previousFireTimeBoss;

        //player shooting
        Texture2D projectileTexture;
        List<Projectile> projectiles;

        // The rate of fire of the player laser
        TimeSpan fireTime;
        TimeSpan previousFireTime;

        //enemy shootings
        Texture2D projectileTextureEnemy;
        List<ProjectileEnemy> projectilesEnemy;

        TimeSpan fireTimeEnemy;
        TimeSpan previousFireTimeEnemy;

        // Enemies
        Texture2D enemyTexture;
        List<Enemy> enemies;

        // The rate at which the enemies appear
        TimeSpan enemySpawnTime;
        TimeSpan previousSpawnTime;

        // A random number generator
        Random random;

        //mission brief
        Texture2D missionBriefLvl1;
        Rectangle missionBriefLvl1Rect;
        bool missionBreifLvl1Visible;

        //moonBackground - level1 
        Texture2D mainBackground;

        // Parallaxing Layers - level1
        ParallaxBackground bgLayer1;
        ParallaxBackground bgLayer2;
        ParallaxBackground bgLayer3;

        //Parallaxing Layers - level2
        ParallaxBackground bgLayer1Lvl2;
        ParallaxBackground bgLayer2Lvl2;
        ParallaxBackground bgLayer3Lvl2;

        //explosion
        Texture2D explosionTexture;
        List<Animation> explosions;

        //jet gun sound
        SoundEffect gunSound;

       // SoundEffect hitSound;
        SoundEffect playerHitSound;

        SoundEffect explodeSound;

        SoundEffect takingOffJet;

        // level songs
        Song gameOverMusic;
        bool gameOverMusicStart;

        Song levelOneMusic;
        bool levelOneMusicStart;

        Song levelTwoMusic;
        bool levelTwoMusicStart;

        Song mainMenuMusic;
        bool mainMenuMusicStart;

        Song finalBossMusic;
        bool finalBossMusicStart;

        Song victoryMusic;
        bool victoryMusicStart;

        //particle
        ParticleGenerator rain;

        //interface
        int score;

        SpriteFont font;

        //healthBar
        Texture2D HpTexture;
        Vector2 HpPosition;
        Rectangle HpRectangle;

        Texture2D HpFrameTexture;
        Vector2 HpFramePosition;

        //pause System
        bool paused = false;
        Texture2D pausedTexture;
        Rectangle pausedRectangle;
        cButton btnResume, btnQuit;

        //missionComplete
        bool isMissionComplete = false;
        Texture2D missionCompleteTexture;
        Rectangle missionCompleteRectangle;
        cButton btnProceed;

        //victory-game finish
        Texture2D victoryTexture;
        Rectangle victoryRectangle;
        cButton btnMainMenuVictory;

        //game Over
        bool isGameOver = false;
        Texture2D gameOverTexture;
        Rectangle gameOverRectangle;
        cButton btnRetry, btnMainMenu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        public void reIntOnlyPlayer()
        {                   
            score = 0;
        }

        public void reInt()
        {
            //player
            player = new Player();

            bossFinal = new Boss();
           
            missionBreifLvl1Visible = false;

            //boss
            //bossFinal = new Boss(Content.Load<Texture2D>("bossSample"));
            
           

            //game music
            gameOverMusic = Content.Load<Song>("Sounds/GameOver");
            MediaPlayer.IsRepeating = true;

            mainMenuMusic = Content.Load<Song>("Sounds/mainMenuMusic");
            MediaPlayer.IsRepeating = true;

            finalBossMusic = Content.Load<Song>("Sounds/bossFight");
            MediaPlayer.IsRepeating = true;

            victoryMusic = Content.Load<Song>("Sounds/victory");
            MediaPlayer.IsRepeating = true;

            levelOneMusicStart = false;
            levelTwoMusicStart = false;
            mainMenuMusicStart = false;
            finalBossMusicStart = false;
            victoryMusicStart = false;
            
            //game level
            gameLevel = 1;

            

            //player movement speed(constant)
            playerMoveSpeed = 8.0f;
            bossMoveSpeed = 1.0f;

            //player shooting
            projectiles = new List<Projectile>();
            fireTime = TimeSpan.FromSeconds(.15f);
            

            // boss shooting
            projectilesBoss = new List<ProjectileEnemy>();
            fireTimeBoss = TimeSpan.FromSeconds(.580f);

            //enemy shooting
            projectilesEnemy = new List<ProjectileEnemy>();
            fireTimeEnemy = TimeSpan.FromSeconds(1f);

            // Initialize the enemies list
            enemies = new List<Enemy>();

            // Set the time keepers to zero
            previousSpawnTime = TimeSpan.Zero;

            // Used to determine how fast enemy respawns
            enemySpawnTime = TimeSpan.FromSeconds(1.0f);

            //explosion
            explosions = new List<Animation>();

            





            // Initialize our random number generator
            random = new Random();

            //Initialize the parallaxBackground object
            bgLayer1 = new ParallaxBackground();
            bgLayer2 = new ParallaxBackground();
            bgLayer3 = new ParallaxBackground();

            //level 2 ParallaxBackground object
            bgLayer1Lvl2 = new ParallaxBackground();
            bgLayer2Lvl2 = new ParallaxBackground();
            bgLayer3Lvl2 = new ParallaxBackground();


            font = Content.Load<SpriteFont>("gameFont");
            base.Initialize();
        }


     
        protected override void Initialize()
        {

             reInt();
             reIntOnlyPlayer();

            
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            
                    IsMouseVisible = true;
                    btnPlay = new cButton(Content.Load<Texture2D>("playJet"), graphics.GraphicsDevice);
                    btnPlay.setPosition(new Vector2(350,300));
                    btnHelp = new cButton(Content.Load<Texture2D>("help"), graphics.GraphicsDevice);
                    btnHelp.setPosition(new Vector2(350,350));
                    btnExit = new cButton(Content.Load<Texture2D>("exitJet"), graphics.GraphicsDevice);
                    btnExit.setPosition(new Vector2(350,400));
                    
                    

               
                    btnBack = new cButton(Content.Load<Texture2D>("backOption"), graphics.GraphicsDevice);
                    btnBack.setPosition(new Vector2(490,220));
                    
                    //mission brief
                    missionBriefLvl1 = Content.Load<Texture2D>("missionBriefPng");
                    missionBriefLvl1Rect = new Rectangle(630, -20, missionBriefLvl1.Width, missionBriefLvl1.Height);
                    
               
                    //pause
                    pausedTexture = Content.Load<Texture2D>("Paused");
                    pausedRectangle = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                    btnResume = new cButton(Content.Load<Texture2D>("resume"), graphics.GraphicsDevice);
                    btnResume.setPosition(new Vector2(350, 225));
                    btnQuit = new cButton(Content.Load<Texture2D>("ExitJet"), graphics.GraphicsDevice);
                    btnQuit.setPosition(new Vector2(350, 275));

                    //mission Complete
                    missionCompleteTexture = Content.Load<Texture2D>("missionComplete");
                    missionCompleteRectangle = new Rectangle(0,0, missionCompleteTexture.Width, missionCompleteTexture.Height);
                    btnProceed = new cButton(Content.Load<Texture2D>("proceed"), graphics.GraphicsDevice);
                    btnProceed.setPosition(new Vector2(350, 225));

                    //Game Over
                    gameOverTexture = Content.Load<Texture2D>("gameOverArt");
                    gameOverRectangle = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                    btnMainMenu = new cButton(Content.Load<Texture2D>("mainMenu"), graphics.GraphicsDevice);
                    btnMainMenu.setPosition(new Vector2(300, 225));
                    btnRetry = new cButton(Content.Load<Texture2D>("retry"), graphics.GraphicsDevice);
                    btnRetry.setPosition(new Vector2(450, 225));

                    //victory -game finish
                    victoryTexture = Content.Load<Texture2D>("victoryArt");
                    victoryRectangle = new Rectangle(0,0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                    btnMainMenuVictory = new cButton(Content.Load<Texture2D>("mainMenu2"), graphics.GraphicsDevice);
                    btnMainMenuVictory.setPosition(new Vector2(350, 320));
                    
                    
                   
           

            //load HealthBar
            HpTexture = Content.Load<Texture2D>("HpBar");
            HpPosition = new Vector2(10, 400);
            HpRectangle = new Rectangle(0, 0, HpTexture.Width, HpTexture.Height);

            HpFrameTexture = Content.Load<Texture2D>("HpFrame");
            HpFramePosition = new Vector2(10, 400);


            //level music
            levelOneMusic = Content.Load<Song>("Sounds/level1Sound");
            //PlayMusic(levelOneMusic);
            levelTwoMusic = Content.Load<Song>("Sounds/level2Sound");


            // Load the player resources
            Animation playerAnimation = new Animation();
            Texture2D playerTexture = Content.Load<Texture2D>("planeFlying");
            playerAnimation.Initialize(playerTexture, Vector2.Zero, 120, 40, 8, 30, Color.White, 1f, true);
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y
            + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(playerAnimation, playerPosition);

            //load the boss resources
            Animation bossAnimation = new Animation();
            Texture2D bossTexture = Content.Load<Texture2D>("bossAnimation");
            bossAnimation.Initialize(bossTexture, Vector2.Zero, 77, 60, 8, 60, Color.White, 1f, true);
            Vector2 bossPosition = new Vector2(700, 200);
            bossFinal.Initialize(bossAnimation, bossPosition);
            
            

            //projectiles
            projectileTexture = Content.Load<Texture2D>("orangeLaser");
            projectileTextureEnemy = Content.Load<Texture2D>("redLaser");
            projectileTextureBoss = Content.Load<Texture2D>("fireBall");

            //enemy content
            enemyTexture = Content.Load<Texture2D>("enemySpriteUpdated");

            // Load the parallaxing background

            bgLayer1.Initialize(Content, "mainBgUpdated", GraphicsDevice.Viewport.Width, -1);
            bgLayer2.Initialize(Content, "cloud1", GraphicsDevice.Viewport.Width, -2);
            bgLayer3.Initialize(Content, "cloud2", GraphicsDevice.Viewport.Width, -3);

            mainBackground = Content.Load<Texture2D>("moon");

            //level 2 parallaxing background
            bgLayer1Lvl2.Initialize(Content, "level2/level2MainBgFinal", GraphicsDevice.Viewport.Width, -1);
            bgLayer2Lvl2.Initialize(Content, "cloud1", GraphicsDevice.Viewport.Width, -2);
            bgLayer3Lvl2.Initialize(Content, "cloud2", GraphicsDevice.Viewport.Width, -3);
            

            //explosion
            explosionTexture = Content.Load<Texture2D>("explosion");

            //load the jet gun sound
            gunSound = Content.Load<SoundEffect>("Sounds/gunSound");           

            //explode
            explodeSound = Content.Load<SoundEffect>("Sounds/Explode");

            //menu takingOff SOund
            takingOffJet = Content.Load<SoundEffect>("Sounds/takingOff");

            //player hit sound
            playerHitSound = Content.Load<SoundEffect>("Sounds/ricochet/ricochet1");

            rain = new ParticleGenerator(Content.Load<Texture2D>("rain"), graphics.GraphicsDevice.Viewport.Width, 50);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    
                    break;
                case GameState.Playing:
                    break;
            }

        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            MouseState mouse = Mouse.GetState();
                   
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    if (!mainMenuMusicStart)
                    {
                        MediaPlayer.Play(mainMenuMusic);
                        MediaPlayer.IsRepeating = true;
                        mainMenuMusicStart = true;
                    }                
                    if(btnPlay.isClicked == true)
                    {
                        CurrentGameState = GameState.Playing;                       
                        takingOffJet.Play();
                    }btnPlay.Update(mouse);
                    
                    if (btnHelp.isClicked == true) CurrentGameState = GameState.Options;
                    btnHelp.Update(mouse);
                    if (btnExit.isClicked == true) this.Exit();
                    btnExit.Update(mouse);
                   
                    break;

                case GameState.Options:
                    if(btnBack.isClicked == true)
                    {
                        CurrentGameState = GameState.MainMenu;
                    }                   
                    btnBack.Update(mouse);
                    break;

                case GameState.Playing:
                    if (player.Health <= 0)
                    {
                       // isGameOver = true;
                        CurrentGameState = GameState.GameOver;
                                            
                    }

                    if (gameLevel == 1 && !isMissionComplete)
                    {
                        missionBreifLvl1Visible = true;
                        if (score >= 300)
                        {
                            missionBreifLvl1Visible = false;
                        }
                                                                    
                            if (score >= 500)
                            {
                                isMissionComplete = true;
                                btnProceed.isClicked = false;
                                //gameLevel += 1;
                                
                            }

                        if (!levelOneMusicStart)
                        {
                            MediaPlayer.Play(levelOneMusic);
                            MediaPlayer.IsRepeating = true;
                            levelOneMusicStart = true;
                           // mainMenuMusic.Dispose();
                        }

                        if (!paused)
                        {
                            if (Keyboard.GetState().IsKeyDown(Keys.P))
                            {
                                paused = true;
                                btnResume.isClicked = false;
                            }
                            // Read the current state of the keyboard and store it
                            currentKeyboardState = Keyboard.GetState();

                            //Update the player
                            UpdatePlayer(gameTime);

                            //Update collision
                            UpdateCollision();

                            //Update the projectile
                            UpdateProjectiles();

                            //update enemy projectile
                            UpdateProjectilesEnemy();

                            // Update the explosions
                            UpdateExplosions(gameTime);

                            //update the enemies
                            UpdateEnemies(gameTime);

                            // Update the parallaxing background
                            bgLayer1.Update();
                            bgLayer2.Update();
                            bgLayer3.Update();

                            //rain.Update(gameTime, graphics.GraphicsDevice);
                        }
                        else if (paused)
                        {
                            if (btnResume.isClicked)
                            {
                                paused = false;                                
                            }
                            if (btnQuit.isClicked)
                            {
                                Exit();
                            }
                            btnResume.Update(mouse);
                            btnQuit.Update(mouse);
                        }
                    }                

                
                    //level 2
                    if (gameLevel == 2 && !isMissionComplete)
                    {
                        if (score >= 900)
                        {
                            isMissionComplete = true;
                            btnProceed.isClicked = false;
                            //gameLevel += 1;

                        }
                      
                        if (!levelTwoMusicStart)
                        {
                            MediaPlayer.Play(levelTwoMusic);
                            MediaPlayer.IsRepeating = true;
                            levelTwoMusicStart = true;
                            //levelOneMusic.Dispose();
                        }

                        if (!paused)
                        {
                            if (Keyboard.GetState().IsKeyDown(Keys.P))
                            {
                                paused = true;
                                btnPlay.isClicked = false;
                            }
                            // Read the current state of the keyboard and store it
                            currentKeyboardState = Keyboard.GetState();

                            //Update the player
                            UpdatePlayer(gameTime);

                            //Update collision
                            UpdateCollision();

                            //Update the projectile
                            UpdateProjectiles();

                            //update enemy projectile
                            UpdateProjectilesEnemy();

                            // Update the explosions
                            UpdateExplosions(gameTime);

                            //update the enemies
                            UpdateEnemies(gameTime);

                            // Update the parallaxing background
                            bgLayer1Lvl2.Update();
                            bgLayer2Lvl2.Update();
                            bgLayer3Lvl2.Update();

                            rain.Update(gameTime, graphics.GraphicsDevice);
                        }
                        else if (paused)
                        {
                            if (btnResume.isClicked)
                            {
                                paused = false;
                            }
                            if (btnQuit.isClicked)
                            {
                                Exit();
                            }
                            btnPlay.Update(mouse);
                            btnQuit.Update(mouse);
                        }
                    }

                    //level 3
                    if (gameLevel == 3 && !isMissionComplete)
                    {

                        if (!finalBossMusicStart)
                        {
                            MediaPlayer.Play(finalBossMusic);
                            MediaPlayer.IsRepeating = true;
                            finalBossMusicStart = true;
                        } 

                        if (bossFinal.bossHealth <= 0)
                        {
                            CurrentGameState = GameState.Won;
                        }
                        
                        if (!paused)
                        {
                            if (Keyboard.GetState().IsKeyDown(Keys.P))
                            {
                                paused = true;
                                btnPlay.isClicked = false;
                            }

                            // Read the current state of the keyboard and store it
                            currentKeyboardState = Keyboard.GetState();

                            //Update the player
                            UpdatePlayer(gameTime);

                            //Update collision
                            UpdateCollision();

                            //Update the projectile
                            UpdateProjectiles();

                            //update the boss projectile
                            UpdateProjectileBoss();

                           
                            // Update the explosions
                            UpdateExplosions(gameTime);

                            //update the boss
                            bossFinal.Update(gameTime);

                            //playerCentre = (int)player.Position.Y + (player.PlayerAnimation.FrameHeight / 2);
                            //bossCentre = bossFinal.bossRectangle.Y + (bossFinal.bossTexture.Height / 2);

                            //update the boss movement
                            bossUpdate(gameTime);

                            // Update the parallaxing background
                            bgLayer1Lvl2.Update();
                            bgLayer2Lvl2.Update();
                            bgLayer3Lvl2.Update();
                        }
                        else if (paused)
                        {
                            if (btnResume.isClicked)
                            {
                                paused = false;
                            }
                            if (btnQuit.isClicked)
                            {
                                Exit();
                            }
                            btnPlay.Update(mouse);
                            btnQuit.Update(mouse);
                        }

                    }

                    if (isMissionComplete)
                    {
                        if (btnProceed.isClicked)
                        {
                            isMissionComplete = false;
                            gameLevel += 1;

                        }
                        btnProceed.Update(mouse);

                    }

                    break;

                case GameState.GameOver:

                        if (!gameOverMusicStart)
                        {
                            MediaPlayer.Play(gameOverMusic);
                            MediaPlayer.IsRepeating = true;
                            gameOverMusicStart = true;
                        }       

                         if (btnMainMenu.isClicked)
                        {
                            //isGameOver = false;
                            CurrentGameState = GameState.MainMenu;
                            //gameLevel = 1;
                            btnMainMenu.isClicked = false;                           
                            //player.Health = 100;
                           // score = 0;
                            reInt();
                            reIntOnlyPlayer();

                        }
                        btnMainMenu.Update(mouse);

                        if (btnRetry.isClicked)
                        {                            
                            
                            //gameLevel = 1;
                            
                            CurrentGameState = GameState.Playing;
                            btnRetry.isClicked = false;
                            //isGameOver = false;
                           // player.Health = 100;
                            //score = 0;
                           // HpRectangle
                            reInt();
                            reIntOnlyPlayer();
                         
                        }
                        btnRetry.Update(mouse);
                    break;

                case GameState.Won:

                    if (!victoryMusicStart)
                    {
                        MediaPlayer.Play(victoryMusic);
                        MediaPlayer.IsRepeating = true;
                        victoryMusicStart = true;
                    } 

                    if (btnMainMenuVictory.isClicked)
                    {
                        CurrentGameState = GameState.MainMenu;
                        btnMainMenuVictory.isClicked = false;
                        reInt();
                        reIntOnlyPlayer();
                    }
                    btnMainMenuVictory.Update(mouse);
                    break;
            }

            

            base.Update(gameTime);
        }

        private void bossUpdate(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - previousFireTimeBoss > fireTimeBoss)
            {
                // Reset our current time
                previousFireTimeBoss = gameTime.TotalGameTime;

                // Add the projectile, but add it to the front and center of the player
                AddProjectileBoss(bossFinal.bossPosition + new Vector2(bossFinal.Width / 2, 0));

                //sound here
            }

            if (bossFinal.bossPosition.Y < player.Position.Y)
            {
                bossFinal.bossPosition.Y += bossMoveSpeed;
            }
           // if (playerCentre < bossCentre)
            if (bossFinal.bossPosition.Y > player.Position.Y)
            {
                bossFinal.bossPosition.Y -= bossMoveSpeed;
            }

            /* Make sure that the player does not go out of bounds
            bossFinal.Position.X = MathHelper.Clamp(player.Position.X, 0, GraphicsDevice.Viewport.Width - player.Width);
            player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, GraphicsDevice.Viewport.Height - player.Height);
             * */
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            player.Update(gameTime);

            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                player.Position.X -= playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                player.Position.X += playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
                player.Position.Y -= playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down))
            {
                player.Position.Y += playerMoveSpeed;
            }

            // Make sure that the player does not go out of bounds
            player.Position.X = MathHelper.Clamp(player.Position.X, 0, GraphicsDevice.Viewport.Width - player.Width);
            player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, GraphicsDevice.Viewport.Height - player.Height);

            // Fire only every interval we set as the fireTime
         
            
                if (currentKeyboardState.IsKeyDown(Keys.Space))
                {
                    if (gameTime.TotalGameTime - previousFireTime > fireTime)
                    {
                        // Reset our current time
                        previousFireTime = gameTime.TotalGameTime;

                        // Add the projectile, but add it to the front and center of the player
                        AddProjectile(player.Position + new Vector2(player.Width / 2, 0));
                     
                        gunSound.Play();
                    }
                }
               // reset score if player health goes to zero
              // if (player.Health <= 0)
              // {
                //   player.Health = 100;
                  //  score = 0;
               //}
                              
        }
        private void UpdateProjectileBoss()
        {
            for (int i = projectilesBoss.Count - 1; i >= 0; i--)
            {
                projectilesBoss[i].Update();

                if (projectilesBoss[i].Active == false)
                {
                    projectilesBoss.RemoveAt(i);
                }
            }
        }

        private void UpdateProjectiles()
        {
            // Update the Projectiles
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                projectiles[i].Update();

                if (projectiles[i].Active == false)
                {
                    projectiles.RemoveAt(i);
                }
            }
        }

        private void UpdateProjectilesEnemy()
        {
            // Update the Projectiles
            for (int i = projectilesEnemy.Count - 1; i >= 0; i--)
            {
                projectilesEnemy[i].Update();

                if (projectilesEnemy[i].Active == false)
                {
                    projectilesEnemy.RemoveAt(i);
                }
            }
        }

       
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            

            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Begin();
                    spriteBatch.Draw(Content.Load<Texture2D>("mainMenuArt"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    btnPlay.Draw(spriteBatch);
                    btnHelp.Draw(spriteBatch);
                    btnExit.Draw(spriteBatch);
                    spriteBatch.End();
                    break;

                case GameState.Options:
                    spriteBatch.Begin();
                    spriteBatch.Draw(Content.Load<Texture2D>("mainMenuArt"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    btnPlay.Draw(spriteBatch);
                    btnHelp.Draw(spriteBatch);
                    btnExit.Draw(spriteBatch);
                    spriteBatch.Draw(Content.Load<Texture2D>("OptionFinal"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    btnBack.Draw(spriteBatch);
                    spriteBatch.End();
                    break;

                case GameState.Playing:
                   
                    if (gameLevel == 1)
                    {
                        spriteBatch.Begin();
                        // Draw the moving background
                        bgLayer1.Draw(spriteBatch);
                        spriteBatch.Draw(mainBackground, Vector2.Zero, Color.White);
                        bgLayer2.Draw(spriteBatch);

                        // Draw the Player
                        player.Draw(spriteBatch);



                        // Draw the Enemies
                        for (int i = 0; i < enemies.Count; i++)
                        {
                            enemies[i].Draw(spriteBatch);
                        }

                        // Draw the Projectiles
                        for (int i = 0; i < projectiles.Count; i++)
                        {
                            projectiles[i].Draw(spriteBatch);
                        }

                        // Draw the Enemy Projectiles
                        for (int i = 0; i < projectilesEnemy.Count; i++)
                        {
                            projectilesEnemy[i].Draw(spriteBatch);
                        }

                        // Draw the explosions
                        for (int i = 0; i < explosions.Count; i++)
                        {
                            explosions[i].Draw(spriteBatch);
                        }

                        bgLayer3.Draw(spriteBatch);

                        //rain
                       // rain.Draw(spriteBatch);

                        // Draw the score
                        spriteBatch.DrawString(font, "score: " + score, new
                        Vector2(650, 420), Color.LightGreen);
                        // Draw the player health
                        spriteBatch.DrawString(font, "health: " + player.Health, new
                        Vector2(200, 420), Color.White);

                        spriteBatch.Draw(HpFrameTexture, HpFramePosition, Color.White);
                        spriteBatch.Draw(HpTexture, HpPosition, HpRectangle, Color.White);

                        //mission brief
                        if (missionBreifLvl1Visible)
                        {
                            spriteBatch.Draw(missionBriefLvl1, missionBriefLvl1Rect, Color.White);
                        }

                        //pauseMenu
                        if (paused)
                        {
                            spriteBatch.Draw(pausedTexture, pausedRectangle, Color.White);
                            btnResume.Draw(spriteBatch);
                            btnQuit.Draw(spriteBatch);
                        }
                        
                        //game over

                        spriteBatch.End();
                    }

                    

                    if (gameLevel == 2)
                    {                                                                        
                        spriteBatch.Begin();
                        // Draw the moving background
                        bgLayer1Lvl2.Draw(spriteBatch);
                        //spriteBatch.Draw(mainBackground, Vector2.Zero, Color.White);
                        bgLayer2Lvl2.Draw(spriteBatch);

                        // Draw the Player
                        player.Draw(spriteBatch);



                        // Draw the Enemies
                        for (int i = 0; i < enemies.Count; i++)
                        {
                            enemies[i].Draw(spriteBatch);
                        }

                        // Draw the Projectiles
                        for (int i = 0; i < projectiles.Count; i++)
                        {
                            projectiles[i].Draw(spriteBatch);
                        }

                        // Draw the Enemy Projectiles
                        for (int i = 0; i < projectilesEnemy.Count; i++)
                        {
                            projectilesEnemy[i].Draw(spriteBatch);
                        }

                        // Draw the explosions
                        for (int i = 0; i < explosions.Count; i++)
                        {
                            explosions[i].Draw(spriteBatch);
                        }

                        bgLayer3Lvl2.Draw(spriteBatch);

                        //rain
                        rain.Draw(spriteBatch);

                        // Draw the score
                        spriteBatch.DrawString(font, "score: " + score, new
                        Vector2(650, 420), Color.LightGreen);
                        // Draw the player health
                        spriteBatch.DrawString(font, "health: " + player.Health, new
                        Vector2(200, 420), Color.White);

                        //draw the healthBar
                        spriteBatch.Draw(HpFrameTexture, HpFramePosition, Color.White);
                        spriteBatch.Draw(HpTexture, HpPosition, HpRectangle, Color.White);

                        //pauseMenu
                        if (paused)
                        {
                            spriteBatch.Draw(pausedTexture, pausedRectangle, Color.White);
                            btnResume.Draw(spriteBatch);
                            btnQuit.Draw(spriteBatch);
                        }

                        spriteBatch.End();

                    }

                    if (gameLevel == 3)
                    {
                        spriteBatch.Begin();

                        // Draw the Player
                        player.Draw(spriteBatch);

                        // Draw the Projectiles
                        for (int i = 0; i < projectiles.Count; i++)
                        {
                            projectiles[i].Draw(spriteBatch);
                        }

                        // Draw the Enemy Projectiles
                        for (int i = 0; i < projectilesBoss.Count; i++)
                        {
                            projectilesBoss[i].Draw(spriteBatch);
                        }

                        //draw the boss
                        bossFinal.Draw(spriteBatch);


                        // Draw the score
                        spriteBatch.DrawString(font, "score: " + score, new
                        Vector2(650, 420), Color.LightGreen);
                        // Draw the player health
                        spriteBatch.DrawString(font, "health: " + player.Health, new
                        Vector2(200, 420), Color.White);

                        //draw the healthBar
                        spriteBatch.Draw(HpFrameTexture, HpFramePosition, Color.White);
                        spriteBatch.Draw(HpTexture, HpPosition, HpRectangle, Color.White);

                        spriteBatch.End();
                    }
                    //mission complete
                    if (isMissionComplete)
                    {
                        spriteBatch.Begin();
                        spriteBatch.Draw(missionCompleteTexture, missionCompleteRectangle, Color.White);
                        btnProceed.Draw(spriteBatch);
                        spriteBatch.DrawString(font, "Total body count: " + score / 100, new
                         Vector2(280, 150), Color.DimGray);
                        spriteBatch.End();

                    }
                    break;

                case GameState.GameOver:
                        spriteBatch.Begin();
                        spriteBatch.Draw(gameOverTexture, gameOverRectangle, Color.White);
                        btnRetry.Draw(spriteBatch);
                        btnMainMenu.Draw(spriteBatch);
                            //spriteBatch.DrawString(font, "Total body count: " + score / 100, new
                            //Vector2(280, 150), Color.DimGray);
                        spriteBatch.End();
                    break;

                case GameState.Won:
                    spriteBatch.Begin();
                    spriteBatch.Draw(victoryTexture, victoryRectangle, Color.White);
                    btnMainMenuVictory.Draw(spriteBatch);
                    spriteBatch.DrawString(font, "Total body count: " + score / 100, new
                    Vector2(320, 170), Color.WhiteSmoke);
                    spriteBatch.DrawString(font, "Total score: " + score, new
                    Vector2(320, 240), Color.WhiteSmoke);
                    spriteBatch.End();
                    break;

            }         

            base.Draw(gameTime);
        }

        private void AddEnemy()
        {
            // Create the animation object
            Animation enemyAnimation = new Animation();

            // Initialize the animation with the correct animation information
            enemyAnimation.Initialize(enemyTexture, Vector2.Zero, 110, 45, 8, 30, Color.White, 1f, true);

            // Randomly generate the position of the enemy
            Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + enemyTexture.Width / 2, random.Next(100, GraphicsDevice.Viewport.Height - 100));

            // Create an enemy
            Enemy enemy = new Enemy();

            // Initialize the enemy
            enemy.Initialize(enemyAnimation, position);

            // Add the enemy to the active enemies list
            enemies.Add(enemy);
        }

        private void AddProjectileBoss(Vector2 position)
        {
            ProjectileEnemy projectileBoss = new ProjectileEnemy();
            projectileBoss.Initialize(GraphicsDevice.Viewport, projectileTextureBoss, position);
            projectilesBoss.Add(projectileBoss);
        }

        private void AddProjectile(Vector2 position)
        {
            Projectile projectile = new Projectile();
            projectile.Initialize(GraphicsDevice.Viewport, projectileTexture, position);
            projectiles.Add(projectile);
        }

        private void AddProjectileEnemy (Vector2 position)
        {
            ProjectileEnemy projectileEnemy = new ProjectileEnemy();
            projectileEnemy.Initialize(GraphicsDevice.Viewport, projectileTextureEnemy, position);
            projectilesEnemy.Add(projectileEnemy);
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            // Spawn a new enemy enemy every 1.5 seconds
            if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime)
            {
                previousSpawnTime = gameTime.TotalGameTime;

                // Add an Enemy
                AddEnemy();
            }

            // Update the Enemies
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies[i].Update(gameTime);

                if (enemies[i].Active == false)
                {
                    // If not active and health <= 0
                    if (enemies[i].Health <= 0)
                    {
                        // Add an explosion
                        AddExplosion(enemies[i].Position);
                        explodeSound.Play();
                        //add to player's score
                        score += enemies[i].Value;
                    }
                    enemies.RemoveAt(i);
                }
            }

            for(int e = 0; e < enemies.Count; e++){

                if (gameTime.TotalGameTime - previousFireTimeEnemy > fireTimeEnemy)
                {
                    // Reset our current time
                    previousFireTimeEnemy = gameTime.TotalGameTime;

                    for (int i = 0; i < enemies.Count; i++)
                    {
                        // Add the projectile, but add it to the front and center of the player
                        AddProjectileEnemy(enemies[i].Position - new Vector2(enemies[i].Width / 2, 0));
                    }
                }
            }
        }

        private void UpdateExplosions(GameTime gameTime)
        {
            for (int i = explosions.Count - 1; i >= 0; i--)
            {
                explosions[i].Update(gameTime);
                if (explosions[i].Active == false)
                {
                    explosions.RemoveAt(i);
                }
            }
        }

        private void UpdateCollision()
        {
           
            // Use the Rectangle's built-in intersect function to 
            // determine if two objects are overlapping
            Rectangle rectangle1;
            Rectangle rectangle2;

            /*
            rectangle1 = new Rectangle((int)player.Position.X,
            (int)player.Position.Y,
            player.Width,
            player.Height);*/

            // Do the collision between the player and the enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                rectangle1 = new Rectangle((int)player.Position.X,
               (int)player.Position.Y,
               player.Width,
               player.Height);
    
                rectangle2 = new Rectangle((int)enemies[i].Position.X,
                (int)enemies[i].Position.Y,
                enemies[i].Width,
                enemies[i].Height);

                // Determine if the two objects collided with each
                // other
                if (rectangle1.Intersects(rectangle2))
                {
                    // Subtract the health from the player based on
                    // the enemy damage
                    player.Health -= enemies[i].Damage;
                    HpRectangle.Width -= enemies[i].Damage;

                    // Since the enemy collided with the player
                    // destroy it
                    enemies[i].Health = 0;

                    // If the player health is less than zero we died
                    //if (player.Health <= 0)
                      //  player.Active = false;
                }

            }

            // Projectile vs Enemy Collision
            for (int i = 0; i < projectiles.Count; i++)
            {
                for (int j = 0; j < enemies.Count; j++)
                {
                    // Create the rectangles we need to determine if we collided with each other
                    rectangle1 = new Rectangle((int)projectiles[i].Position.X -
                    projectiles[i].Width / 2, (int)projectiles[i].Position.Y -
                    projectiles[i].Height / 2, projectiles[i].Width, projectiles[i].Height);

                    rectangle2 = new Rectangle((int)enemies[j].Position.X - enemies[j].Width / 2,
                    (int)enemies[j].Position.Y - enemies[j].Height / 2,
                    enemies[j].Width, enemies[j].Height);

                    // Determine if the two objects collided with each other
                    if (rectangle1.Intersects(rectangle2))
                    {
                        
                        enemies[j].Health -= projectiles[i].Damage;
                        projectiles[i].Active = false;

                        //hitSound[hitSoundCount].Play();
                                               
                        //hitSoundCount++;
                                             
                    }
                    
                }
            }
            // Projectile vs player
            for (int i = 0; i < projectilesEnemy.Count; i++)
            {
               
                    // Create the rectangles we need to determine if we collided with each other
                    rectangle1 = new Rectangle((int)projectilesEnemy[i].Position.X -
                    projectilesEnemy[i].Width / 2, (int)projectilesEnemy[i].Position.Y -
                    projectilesEnemy[i].Height / 2, projectilesEnemy[i].Width, projectilesEnemy[i].Height);

                    rectangle2 = new Rectangle((int)player.Position.X,
                      (int)player.Position.Y,
                        player.Width,
                         player.Height);

                    // Determine if the two objects collided with each other
                    if (rectangle1.Intersects(rectangle2))
                    {
                        playerHitSound.Play();
                        player.Health -= projectilesEnemy[i].Damage;
                        projectilesEnemy[i].Active = false;
                        HpRectangle.Width -= projectilesEnemy[i].Damage;
                    }
                
            }

            //boss projectilles vs player
            for (int i = 0; i < projectilesBoss.Count; i++)
            {

                rectangle1 = new Rectangle((int)player.Position.X,
                (int)player.Position.Y,
                player.Width,
                player.Height); 

                    rectangle2 = new Rectangle((int)projectilesBoss[i].Position.X -
                    projectilesBoss[i].Width / 2, (int)projectilesBoss[i].Position.Y -
                    projectilesBoss[i].Height / 2, projectilesBoss[i].Width, projectilesBoss[i].Height);

                    if (rectangle1.Intersects(rectangle2))
                    {
                        // Subtract the health from the player based on
                        // the enemy damage
                        player.Health -= projectilesBoss[i].Damage;
                        HpRectangle.Width -= projectilesBoss[i].Damage;
                        projectilesBoss[i].Active = false;
                    }
            }

            //player projectile vs boss
            for (int i = 0; i < projectiles.Count; i++)
            {
                     rectangle1 = new Rectangle((int)projectiles[i].Position.X -
                     projectiles[i].Width / 2, (int)projectiles[i].Position.Y -
                     projectiles[i].Height / 2, projectiles[i].Width, projectiles[i].Height);

                     rectangle2 = new Rectangle((int)bossFinal.bossPosition.X,
                    (int)bossFinal.bossPosition.Y,
                    bossFinal.Width,
                    bossFinal.Height);

                     if (rectangle1.Intersects(rectangle2))
                     {
                         bossFinal.bossHealth -= projectiles[i].Damage;
                         projectiles[i].Active = false;
                     }
                    
            }

            
        }

        private void PlayMusic(Song song)
        {
            // Due to the way the MediaPlayer plays music,
            // we have to catch the exception. Music will play when the game is not tethered
            try
            {
                // Play the music
                MediaPlayer.Play(song);

                // Loop the currently playing song
                MediaPlayer.IsRepeating = true;
            }
            catch { }
        }

        private void AddExplosion(Vector2 position)
        {
            Animation explosion = new Animation();
            explosion.Initialize(explosionTexture, position, 134, 134, 12, 45, Color.White, 1f, false);
            explosions.Add(explosion);
        }
    }
}
