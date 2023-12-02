﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKoFinal.Renderers
{
    internal class GameScene : DrawableGameComponent
    {
        public List<GameComponent> Components { get; set; }

        public GameScene(Game game) : base(game)
        {
            Components = new List<GameComponent>();
            Hide();
        }

        public virtual void Hide()
        {
            Visible = false;
            Enabled = false;
        }

        public virtual void Display()
        {
            Visible = true;
            Enabled = true;
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (DrawableGameComponent component in Components)
            {
                if (component.Visible)
                {
                    component.Draw(gameTime);
                }
            }
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GameComponent component in Components)
            {
                if (component.Enabled)
                {
                    component.Update(gameTime);
                }
            }
            base.Update(gameTime);
        }
    }
}