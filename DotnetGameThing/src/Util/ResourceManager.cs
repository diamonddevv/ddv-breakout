using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
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

            // Sound
            Raylib.UnloadSound(BOUNCE);
            Raylib.UnloadSound(DIE);
            Raylib.UnloadSound(CLEARED);
            Raylib.UnloadSound(LOSE);
            Raylib.UnloadSound(SELECT_MENU);
        }
        

    }
}
