using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PolygonArtist;
using System.Collections.Generic;

namespace PolygonArtistDemo
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class PolygonArtistDemoGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Artist artist;

        List<Vector2> square;
        List<Vector2> triangle;
        List<Vector2> hexagon;
        
        public PolygonArtistDemoGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            artist = new Artist(GraphicsDevice, GraphicsDevice.Viewport.Bounds);

            square = new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 100),
                new Vector2(100, 100),
                new Vector2(100, 0)
            };

            triangle = new List<Vector2>()
            {
                new Vector2(50, 0),
                new Vector2(0, 100),
                new Vector2(100, 100)
            };

            hexagon = new List<Vector2>()
            {
                new Vector2(25, 0),
                new Vector2(0, 25),
                new Vector2(0, 75),
                new Vector2(25, 100),
                new Vector2(75, 100),
                new Vector2(100, 75),
                new Vector2(100, 25),
                new Vector2(75, 0)
            };

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            artist.DrawPolygon(square, Color.Red, new Vector2(100, 100), 0);
            artist.DrawPolygon(square, Color.Green, new Vector2(100, 100), 5);

            artist.DrawPolygon(hexagon, Color.Purple, new Vector2(350, 125), 0);
            artist.DrawPolygon(hexagon, Color.Orange, new Vector2(350, 125), 7);

            artist.DrawPolygon(triangle, Color.Teal, new Vector2(220, 275), -8);
            artist.DrawPolygon(triangle, Color.Black, new Vector2(220, 275), 0);
            

            base.Draw(gameTime);
        }
    }
}
