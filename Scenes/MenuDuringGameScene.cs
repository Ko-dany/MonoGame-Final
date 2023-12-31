﻿using DKoFinal.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace DKoFinal.Scenes
{
    public class MenuDuringGameScene : GameScene
    {
        SpriteBatch spriteBatch;
        MenuSelection menuSelection;

        public MenuDuringGameScene(Game game, int backgroundWidth, int backgroundHeight) : base(game)
        {
            DkoFinal dkoFinal = (DkoFinal)game;
            spriteBatch = dkoFinal.spriteBatch;

            /* ============= Load image & font content ============= */
            Texture2D mainBackgroundImg = dkoFinal.Content.Load<Texture2D>("MainScene/Blue");
            Texture2D mainTitleImg = dkoFinal.Content.Load<Texture2D>("MainScene/GameTitle");
            SpriteFont regular = dkoFinal.Content.Load<SpriteFont>("Fonts/regular");
            SpriteFont selected = dkoFinal.Content.Load<SpriteFont>("Fonts/selected");

            /* ============= Add background & title image components ============= */
            Background mainBackground = new Background(dkoFinal, spriteBatch, mainBackgroundImg, backgroundWidth, backgroundHeight);
            this.Components.Add(mainBackground);

            Image menuTitle = new Image(dkoFinal, spriteBatch, mainTitleImg, new Vector2(backgroundWidth / 2, backgroundHeight / 3), new Rectangle(0, 0, mainTitleImg.Width, mainTitleImg.Height), Color.White, new Vector2(mainTitleImg.Width / 2, mainTitleImg.Height / 2), 0.0f, 0.3f, SpriteEffects.None, 0.0f);
            this.Components.Add(menuTitle);

            /* ============= Add menu selection component ============= */
            menuSelection = new MenuSelection(dkoFinal, spriteBatch, regular, selected, new Vector2(backgroundWidth / 2, backgroundHeight / 5 * 3), Color.White, Color.Black, new string[] { "RESUME", "HELP", "ABOUT", "BACK TO MAIN" });
            this.Components.Add(menuSelection);
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public int GetSelectedIndex()
        {
            return menuSelection.selectedIndex;
        }

    }
}
