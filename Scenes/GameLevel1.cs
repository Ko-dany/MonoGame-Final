﻿using DKoFinal.GameManager;
using DKoFinal.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DKoFinal.Scenes
{
    public class GameLevel1 : GameScene
    {
        SpriteBatch spriteBatch;

        ObstacleCollision obstacleCollision;
        CheckpointCollision checkpointCollision;
        TerrainCollision terrainCollision;

        bool gameOver;
        bool gameClear;

        public GameLevel1(Game game, int backgroundWidth, int backgroundHeight) : base(game)
        {
            DkoFinal dkoFinal = (DkoFinal)game;
            spriteBatch = dkoFinal.spriteBatch;

            /* ============= Load image & font content ============= */
            Texture2D mainBackgroundImg = dkoFinal.Content.Load<Texture2D>("Level1/Yellow");
            Texture2D obstacleImage = dkoFinal.Content.Load<Texture2D>("Level1/SpikeHead");
            Texture2D horizontalTexture = dkoFinal.Content.Load<Texture2D>("Level1/Spikes");
            Texture2D verticalTexture = dkoFinal.Content.Load<Texture2D>("Level1/Spikes_Vertical");

            gameOver = false;
            gameClear = false;

            /*============ Add background component ============*/
            Background mainBackground = new Background(dkoFinal, spriteBatch, mainBackgroundImg, backgroundWidth,  backgroundHeight);
            this.Components.Add(mainBackground);

            /*============ Add player character component ============*/
            PlayerCharacter player = new PlayerCharacter(dkoFinal, spriteBatch, backgroundWidth, backgroundHeight);
            this.Components.Add(player);

            /*============ Generate random obstacle components & add obstacle collision manager ============*/
            Random random = new Random();
            const int stages = 5;
            const int obstacleCount = 3;

            List<Obstacle> obstacles = new List<Obstacle>();
            List<Rectangle> obstacleBounds = new List<Rectangle>();
            for (int k = 1; k <= stages; k++)
            {
                for (int i = 0; i < obstacleCount; i++)
                {
                    Rectangle newObstacleBounds = new Rectangle(random.Next(backgroundWidth * k, backgroundWidth * (k + 1)), random.Next(0, backgroundHeight -obstacleImage.Height), obstacleImage.Width,obstacleImage.Height);

                    while (ObstacleOverlaps(newObstacleBounds, obstacleBounds))
                    {
                        newObstacleBounds.X = random.Next(backgroundWidth * k, backgroundWidth * (k + 1));
                        newObstacleBounds.Y = random.Next(0, backgroundHeight - obstacleImage.Height);
                    }

                    Vector2 randomPosition = new Vector2(newObstacleBounds.X, newObstacleBounds.Y);
                    Vector2 randomSpeed = new Vector2(random.Next(3, 5), 0);

                    Obstacle obstacle = new Obstacle(dkoFinal, spriteBatch, obstacleImage, 1.8f, randomPosition, randomSpeed, 4, backgroundWidth, backgroundHeight);
                    obstacles.Add(obstacle);
                    this.Components.Add(obstacle);

                    obstacleBounds.Add(newObstacleBounds);
                }
            }
            obstacleCollision = new ObstacleCollision(dkoFinal, player, obstacles);
            this.Components.Add(obstacleCollision);

            /*============ Add terrain component & terrain collision manager ============*/
            Terrain terrain = new Terrain(dkoFinal, spriteBatch, horizontalTexture, new Vector2(0, -horizontalTexture.Height), new Vector2(0, backgroundHeight + horizontalTexture.Height), new Vector2(horizontalTexture.Width, -horizontalTexture.Height), new Vector2(horizontalTexture.Width, backgroundHeight + horizontalTexture.Height), verticalTexture, new Vector2(-verticalTexture.Width,0), new Vector2(backgroundWidth * (stages + 1) + backgroundWidth / 2, 0));
            this.Components.Add(terrain);
            terrainCollision = new TerrainCollision(dkoFinal, player, terrain);
            this.Components.Add(terrainCollision);

            /*============ Add checkpoint component & checkpoint collision manager ============*/
            CheckpointAnimation checkpoint = new CheckpointAnimation(dkoFinal, spriteBatch, new Vector2(backgroundWidth * (stages+1) + backgroundWidth / 3, backgroundHeight / 2));
            this.Components.Add(checkpoint);
            checkpointCollision = new CheckpointCollision(dkoFinal, player, checkpoint);
            this.Components.Add(checkpointCollision);
        }

        public override void Update(GameTime gameTime)
        {
            if(terrainCollision.DetectCollision() || obstacleCollision.DetectCollision()) { gameOver = true; }
            if (checkpointCollision.DetectCollision()) { gameClear = true; }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /*============ Check if newly generated object collides with existing ones ============*/
        bool ObstacleOverlaps(Rectangle newObstacle, List<Rectangle> existingObstacles)
        {
            foreach (var obstacle in existingObstacles)
            {
                if (obstacle.Intersects(newObstacle))
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckGameOver()
        {
            return gameOver;
        }

        public bool CheckGameClear()
        {
            return gameClear;
        }


    }
}
