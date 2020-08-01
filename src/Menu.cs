using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using NativeUI;

namespace Delta
{
    public static class Menu
    {
        private static MenuPool menuPool;
        private static UIMenu mainMenu;

        // Made by London Studios
        static Menu()
        {
            menuPool = new MenuPool()
            {
                MouseEdgeEnabled = false,
                ControlDisablingEnabled = false
            };
            mainMenu = new UIMenu("Absperrband", "Absperrungen verwalten")
            {
                MouseControlsEnabled = false,
                ControlDisablingEnabled = false
            };
            menuPool.Add(mainMenu);
            TapeMenu(mainMenu);
            menuPool.RefreshIndex();
        }

        internal static async void Toggle()
        {
            if (menuPool.IsAnyMenuOpen())
            {
                menuPool.CloseAllMenus();
            }
            else
            {
                mainMenu.Visible = true;
                while (menuPool.IsAnyMenuOpen())
                {
                    menuPool.ProcessMenus();
                    await BaseScript.Delay(0);   
                }
            }
        }

        private static void TapeMenu(UIMenu menu)
        {
            var newTape = menuPool.AddSubMenu(menu, "Neues Absperrband aufspannen");
            newTape.MouseControlsEnabled = false;
            newTape.ControlDisablingEnabled = false;
            var length = new UIMenuSliderProgressItem("Absperrbandlänge", 35, 10);
            newTape.AddItem(length);

            var policeTape = new UIMenuItem(Main.tape1Name, $"Platziere {Main.tape1Name} ");
            newTape.AddItem(policeTape);

            var innerCordonTape = new UIMenuItem(Main.tape2Name, $"Platziere {Main.tape2Name} ");
            newTape.AddItem(innerCordonTape);

            var fireTape = new UIMenuItem(Main.tape3Name, $"Platziere {Main.tape3Name} ");
            newTape.AddItem(fireTape);

            var manageTapes = menuPool.AddSubMenu(menu, "Absperrungen verwalten");

            var moveTape = new UIMenuItem("Platziere Absperrband", "Platziere das Absperrband");
            manageTapes.AddItem(moveTape);

            var deleteTape = new UIMenuItem("Absperrband entfernen", "Absperrband entfernen");
            manageTapes.AddItem(deleteTape);

            manageTapes.MouseControlsEnabled = false;

            manageTapes.OnItemSelect += (sender, item, index) =>
            {
                if (item == moveTape)
                {
                    Main.MoveTape();
                }
                else if (item == deleteTape)
                {
                    Main.DeleteTape();
                }
                Toggle();
            };

            newTape.OnItemSelect += (sender, item, index) =>
            {
                var rnd = new Random();
                if (item == policeTape)
                {
                    Main.startPosition = GetEntityCoords(PlayerPedId(), true);
                    Main.CalculateTape(rnd.Next(1, 20000), GetHashKey("p_clothtarp_s"), length.Value);
                }
                else if (item == fireTape)
                {
                    Main.startPosition = GetEntityCoords(PlayerPedId(), true);
                    Main.CalculateTape(rnd.Next(1, 20000), GetHashKey("prop_fire_tape"), length.Value);
                }
                else if (item == innerCordonTape)
                {
                    Main.startPosition = GetEntityCoords(PlayerPedId(), true);
                    Main.CalculateTape(rnd.Next(1, 20000), GetHashKey("prop_cordon_tape"), length.Value);
                }
                Toggle();
            };
        }
    }
}
