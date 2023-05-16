using BreakoutGame;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Resource
{
    internal class ResourceManager
    {
        // Texture
        public static Texture2D LOGO;
        public static Texture2D DDV_ICON;
        public static Texture2D SPECIAL_BLOCK_OVERLAY;
        public static Texture2D ATLAS_BOOLEAN;
        public static Texture2D ATLAS_INT;
        public static Texture2D ATLAS_DECOR;
        public static Texture2D MENU_GRADIENT_MAP;
        public static Texture2D PAUSE_OVERLAY;

        // Sound
        public static Sound BOUNCE;
        public static Sound DIE;
        public static Sound CLEARED;
        public static Sound LOSE;
        public static Sound SELECT_MENU;


        public static void LoadResources()
        {
            // Texture
            LOGO =                  Raylib.LoadTexture("assets/logo.png");
            DDV_ICON =              Raylib.LoadTexture("assets/ddv.png");
            SPECIAL_BLOCK_OVERLAY = Raylib.LoadTexture("assets/special_block_overlay.png");
            ATLAS_BOOLEAN =         Raylib.LoadTexture("assets/boolean.png");
            ATLAS_INT =             Raylib.LoadTexture("assets/int.png");
            ATLAS_DECOR =           Raylib.LoadTexture("assets/decoration_atlas.png");
            MENU_GRADIENT_MAP =     Raylib.LoadTexture("assets/menu_gradient_map.png");
            PAUSE_OVERLAY =         Raylib.LoadTexture("assets/pause_overlay.png");

            // Sound
            BOUNCE =                Raylib.LoadSound("assets/bounce.wav");
            DIE =                   Raylib.LoadSound("assets/die.wav");
            CLEARED =               Raylib.LoadSound("assets/cleared.wav");
            LOSE =                  Raylib.LoadSound("assets/lose.wav");
            SELECT_MENU =           Raylib.LoadSound("assets/select.wav");
        }

        public static void UnloadResources()
        {
            // Texture
            Raylib.UnloadTexture(DDV_ICON);
            Raylib.UnloadTexture(SPECIAL_BLOCK_OVERLAY);
            Raylib.UnloadTexture(ATLAS_BOOLEAN);
            Raylib.UnloadTexture(ATLAS_INT);
            Raylib.UnloadTexture(ATLAS_DECOR);
            Raylib.UnloadTexture(MENU_GRADIENT_MAP);
            Raylib.UnloadTexture(PAUSE_OVERLAY);

            // Sound
            Raylib.UnloadSound(BOUNCE);
            Raylib.UnloadSound(DIE);
            Raylib.UnloadSound(CLEARED);
            Raylib.UnloadSound(LOSE);
            Raylib.UnloadSound(SELECT_MENU);
        }



        public static void DrawFromToScaled(Texture2D texture, (int x, int y) a, (int x, int y) b, Color tint)
        {
            Raylib.DrawTexturePro(
                texture, new Rectangle(0, 0, texture.width, -texture.height),
                new Rectangle(a.x, a.y, b.x, b.y),
                new Vector2(a.x, a.y),
                0f, tint
            );
        }

        public static void DrawSpreadTexture(Texture2D background, int targetWidth, int targetHeight, Color tint)
        {
            DrawFromToScaled(background, (0, 0), (targetWidth, targetHeight), tint);
        }
    }
}
