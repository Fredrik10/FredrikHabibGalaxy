using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FredrikHabibGalaxy
{
    class GameElements
    {
        static Texture2D menuSprite;
        static Vector2 menuPos;
        static Player player;
        static List<Enemy> enemies;
        static List<GoldCoin> goldCoins;
        static Texture2D goldCoinSprite;
        static PrintText printText;

        public enum State { Menu, Run, Highscore, Quit, HighScore };
        public static State currentState;

        public static void Initialize()
        {
            //kod
            goldCoins = new List<GoldCoin>();
        }

        public static void LoadContent(ContentManager content, GameWindow window)
        {
            //kod
            menuSprite = content.Load<Texture2D>("menu");
            menuPos.X = window.ClientBounds.Width / 2 - menuSprite.Width / 2;
            menuPos.Y = window.ClientBounds.Height / 2 - menuSprite.Height / 2;

            player = new Player(content.Load<Texture2D>("ship"), 380, 400, 2.5f, 4.5f, content.Load<Texture2D>("bullet"));

            enemies = new List<Enemy>();
            Random random = new Random();
            Texture2D tmpSprite = content.Load<Texture2D>("mine");

            for (int i = 0; i < 5; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 2);
                Mine temp = new Mine(tmpSprite, rndX, rndY);
                enemies.Add(temp);
            }
            tmpSprite = content.Load<Texture2D>("tripod");
            for (int i = 0; i < 5; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 2);
                Tripod temp = new Tripod(tmpSprite, rndX, rndY);
                enemies.Add(temp);
            }
            goldCoinSprite = content.Load<Texture2D>("coin");
            printText = new PrintText(content.Load<SpriteBatch>("MyFont"));
        }
        public static State MenuUpdate()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.S))
                return State.Run;
            if (keyboardState.IsKeyDown(Keys.H))
                return State.Highscore;
            if (keyboardState.IsKeyDown(Keys.A))
                return State.Menu;

        }
        public static void MenuDraw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(menuSprite, menuPos, Color.White);
        }
        public static State RunUpdate(ContentManager content, GameWindow window, GameTime gameTime)
        {
            player.Update(window, gameTime);


            foreach (Enemy e in enemies.ToList())
            {
                foreach (Bullet b in player.Bullets)
                {
                    if (e.CheckCollision(b))
                    {
                        e.IsAlive = false;
                        player.Points++;
                    }
                }

                if (e.IsAlive)
                {
                    if (e.CheckCollision(player))
                        player.IsAlive = false;
                    e.Update(window);
                }
                else
                    enemies.Remove(e);
            }


            Random random = new Random();
            int newCoin = random.Next(1, 200);
            if (newCoin == 1)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - goldCoinSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height - goldCoinSprite.Height);

                goldCoins.Add(new GoldCoin(goldCoinSprite, rndX, rndY, gameTime));
            }


            foreach (GoldCoin gc in goldCoins.ToList())
            {
                if (gc.IsAlive)
                {
                    gc.Update(gameTime);

                    if (gc.CheckCollision(player))
                    {
                        goldCoins.Remove(gc);
                        player.Points++;
                    }
                }
                else
                    goldCoins.Remove(gc);
            }
            if (!player.IsAlive)
                return State.Menu;
        }
    }
    public static void RunDraw(SpriteBatch spriteBatch)
    {
        player.Draw(spriteBatch);
        foreach (Enemy e in enemies.ToList())
            e.Draw(spriteBatch);
        foreach (GoldCoin gc in goldCoins)
            gc.Draw(spriteBatch);
        printText.Print("points" + player.Points, spriteBatch, 0, 0);
    }
    public static State HighScoreUpdate()
    {
        KeyboardState keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.Escape)) ;
        return State.Menu;

        return State.Higscore;
    }

    public static void HighScoreDraw()
    {
    }



}

