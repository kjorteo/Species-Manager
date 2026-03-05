using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XRL;
using XRL.Core;
using XRL.Wish;
using XRL.World;
using XRL.World.Effects;
using XRL.UI;

namespace XRL.World.Parts
{
    [PlayerMutator]
    [HasWishCommand]
    [Serializable]

    public class Kjorteo_SpeciesManager : IPlayerPart, IPlayerMutator
    {
        // Declaring and initializing the player's technical and display species
        public string Species
        {
            get => ParentObject?.GetTagOrStringProperty("Species");
            set => ParentObject?.SetStringProperty("Species", value);
        }

        public string DisplaySpecies
        {
            get => ParentObject?.GetStringProperty("Kjorteo_DisplaySpecies", Species);
            set => ParentObject?.SetStringProperty("Kjorteo_DisplaySpecies", value);
        }
        // the selection has too many SPECES and VAULES
        // (dumb joke reference, please ignore me memeing in the comments :v)

        public void mutate(GameObject player)
        {
            var part = player?.RequirePart<Kjorteo_SpeciesManager>();
            part.DisplaySpecies = part.Species;
        }

        // The event handler functions are a little advanced for us to explain with full confidence what exactly they do... this is one of those things that came as a result of help and guidance from the good folks over in the Qudcord.  All thanks and praise to them for helping us get this mod off the ground. <3
        public override bool WantEvent(int ID, int Cascade)
            => base.WantEvent(ID, Cascade)
            || ID == GetApparentSpeciesEvent.ID
            || ID == AfterPlayerBodyChangeEvent.ID
            ;

        public override bool HandleEvent(GetApparentSpeciesEvent E)
        {
            if (E.Priority == 0 && !ParentObject.HasEffect<Disguised>()) // This limitation is to fix issues with running Species Manager while disguised.
            {   E.ApparentSpecies = DisplaySpecies;   }
            return base.HandleEvent(E);
        }

        // Declaring arrays to be used in the main menu
        static List<char> hotkeysMain = new List<char> {'a', 'b', 'c', 'd', 'e', 'f'};
        static List<string> optionsMain = new List<string>
        {
            "Change techncial species by list",
            "Change technical species by free entry",
            "Change display species",
            "Read explanation",
            "(Optional) Support the authors",
            "Exit menu"
        };

        // Declaring miscellaneous variables to be used in submeus
        public static List<string> DistinctSpecies = null;
        public static List<char> HotkeysTSpecies = null;
        public static string newTechnicalSpecies = null;
        public static string newDisplaySpecies = null;

        // Call main menu when player wishes for "species"
        [WishCommand(Command = "species")]
        public static int Kjorteo_SpeciesManager_Handler()
        {
            int selectionMain = -1; // Variable for the chosen main menu option
            string DisguiseWarning1 = null; // Attaches to the end of the listed Display Species line if the player is currently disguised
            string DisguiseWarning2 = null; // Attaches to the window asking the player for their new Display Species if they're currently disguised

            // Let's do something sneaky here!  I'm not sure if it's even possilble to have conditional text, such as text that only appears specifically if it senses the player is disguised, in something like the pre-options text portion of a Popup.PickOption() function.  So, instead, we're going to get the condition checking and prep work out of the way now.  DisguiseWarning1 and 2 start out as completely null/blank/empty strings, but if the player is disguised, the warning text gets written to them.  This will come into play later.
            if (The.Player.HasEffect<Disguised>())
            {
                DisguiseWarning1 = " {{R|(Disguised)}}\n\n{{r|NOTE: You are currently disguised. Your Display Species will only show as the species you are currently disguised as. To see your true Display Species, please remove your disguise.}}";
                DisguiseWarning2 = "\n\n{{r|NOTE: You are currently disguised. Changing your Display Species now will change your {{R|undisguised}} Display Species only. Your Display Species will continue to show the species you are disguised as so long as you remain disguised, but the change you enter here will take effect as soon as you remove your disguise.}}";
            }

            while (true) // This loop will keep going unless/until the player manually breaks it.
            {
                selectionMain = Popup.PickOption
                (
                    "Species Manager", // Window title
                    // { All of this is for the value representing the opening text before the options are presented
                        "{{W|Your current technical species is:}}\n"
                        + The.Player.GetSpecies() + "\n"
                        + "\n"
                        + "{{W|Your current display species is:}}\n"
                        + The.Player.GetApparentSpecies() + DisguiseWarning1 + "\n" // The payoff to our earlier sneakiness!  We don't need conditional checks anymore: The contents of DisguiseWarning1 itself are already either nothing at all or a perfectly formatted warning depending on (what else?) whether the player is disguised.  So, at this point, we just do a blind print of whatever's in that variable; it will produce the desired end result in both cases.
                        + "\n",
                    // }
                    Options: optionsMain.ToArray(),
                    Hotkeys: hotkeysMain.ToArray(),
                    AllowEscape: true // QoL; it's obnoxious if the player can't do this.
                );

                // The actual choices the player can choose from correspond to cases 0 through 5.  If selectionMain is equal to 5 after the player has interacted with the menu, that means that the player chose the "Exit menu" option.  Meanwhile, selectionMain becomes -1 if the player escapes out of the main menu without making a selection at all.  Therefore, we can assume a value of -1 or 5 both mean that the player is trying to close the menu and return to the game.  Let's help them with that!  In other words, this is where we put the manual loop break.
                if(selectionMain == -1 || selectionMain == 5) break;

                // If selectionMain is 0-4, meanwhile, those values correspond with the other selections.  This is where we process those.
                switch(selectionMain)
                {
                    case 0: // Happens when the player has chosen to change technical species by list
                        DistinctSpecies = GameObjectFactory.Factory
                            ?.BlueprintList                             // Goes to the list of all objets in GameObjectBlueprints
                            ?.Where(bp => bp.HasTag("Species"))         // Only selects the ones where the "Species" field isn't blank
                            ?.Select(bp => bp.GetTag("Species"))        // Converts the entire object data structure into a string for just the "Species" value
                            ?.Distinct()                                // Filters out duplicates
                            ?.ToList();
                        if (!DistinctSpecies.IsNullOrEmpty())
                        {
                            DistinctSpecies.Sort(); // Alphabetize the list real quick
                            int selectionTSpecies = Popup.PickOption
                            (
                                "Species Manager", // Window title
                                "{{W|Here are the existing technical species held by all objects this world excluding the player. Select one to change your technical species to match.}}\n\n", // Pre-selection text
                                Options: DistinctSpecies.ToArray() // The list of selectable options
                            );
                            The.Player.SetStringProperty("Species", DistinctSpecies[selectionTSpecies]);
                        }
                        else
                        {   Popup.Show("There are no species in the game??");   }
                        break;

                    case 1: // Happens when the player has chosen to change technical species by free entry
                        newTechnicalSpecies = Popup.AskString("{{W|What is your new technical species? (You may leave this field blank to cancel.)}}\n\n");
                        if(newTechnicalSpecies != null && newTechnicalSpecies != "")
                        {   The.Player.SetStringProperty("Species", newTechnicalSpecies);   }
                        newTechnicalSpecies = ""; // Clean up the variable after we're done so this check still works next time
                        break;

                    case 2: // Happens when the player has chosen to change display species
                        if (The.Player.TryGetPart(out Kjorteo_SpeciesManager DisplaySpeciesOverride))
                        {
                            newDisplaySpecies = Popup.AskString("{{W|What is your new display species? (You may leave this field blank to cancel.)}}" + DisguiseWarning2 + "\n\n"); // Just like in the main menu, we can just do a blind print of DisguiseWarning2; the proper conditional checks were already handled earlier so this will either print nothing or the exact warning we were looking for depending on whether the warning is needed.
                            if(newDisplaySpecies != null && newDisplaySpecies != "")
                            {   DisplaySpeciesOverride.DisplaySpecies = newDisplaySpecies;   }
                        }
                        newDisplaySpecies = ""; // Clean up the variable after we're done so this check still works next time
                        break;

                    case 3: // Happens when the player has chosen to read the explanation.
                        Popup.Show("Your display species is what you will see in-game in cases such as NPC dialogue. By itself, it does not serve any mechanical or gameplay-affecting purpose; this value is for display purposes only.\n\nYour technical species is the opposite: It should never be visible to the player, but it silently affects gameplay mechanics. The main example of this in the standard unmodded Caves of Qud experience is weapons with the \"morphogenetic\" multiplier, but other mods may use the player's species to determine other mechanics as well.\n\nThe Species Manager mod separates your technical and display species so that you may be choose to have NPCs refer to your species with whatever fanciful name you desire while not disturbing gameplay mechanics that rely on your technical species.\n\nFor example, one might wish to play a lizardfolk character of their own invention while matching a preexisting in-game species for mechanical purposes. To manage this, the display species could be \"lizardfolk\" while the technical species could be the closest reptilian species on the list—perhaps something like \"iguana\" or \"salamander\".\n\nIf you prefer not to belong to any preexisting species even for mechanical purposes, the option to chose your own technical species by free entry exists as well.");
                        break;

                    case 4: // Happens when the player has chosen to support the authors.  (Thank you. uwu)
                        Popup.Show("Hello! We are Celine Kalante Love and Friends, AKA the Woodling System. (\"Kjorteo\" is a general all-purpose account name to refer to the whole system while being unique enough to make it easier to sign up for things.) We're a disabled neurodiverse furry plural system who gets hyperfixated on things and enjoys making content for whatever we're currently into. Supporting ourselves is tricky since ADHD tends to make it hard to finish projects and thus have anything we could sell. The fact that so many of our works are mods for whatever we're currently playing (like this one!) doesn't help, either; obviously we're not going to start paywalling things like this. Therefore, the best way we've thought of to attempt to monetize our efforts is a Ko-Fi subscription that offers sneak-peek access to WIP updates and blog entries talking about what we're currently working on.\n\n{{Y|https://ko-fi.com/kjorteo/}}\n\nWe also take hourly-rate commissions for coding, writing, 3D model texturing, pixel art, or whatever else you'd like us to do that's within our skill set. You may email inquiries to {{Y|husky@kjorteo.net}} or reach out to us in the DMs or comments of various online platforms.\n\nConsider all of this a sort of tip jar: Nothing is ever required or expected of you. (In fact, it wasn't until version 1.1 that this mod even included support info at all.) Any support you choose to provide is greatly appreciated, though, and helps us continue to make things like this for you. Thank you so much!");
                        break;
                }
            }
            return 0;
        }
    }

    // This node is called when a save game is loaded; this is what makes the overall mod compatible with already-in-progress campaigns.
    [HasCallAfterGameLoadedAttribute]
    public class Kjorteo_SpeciesManager_LoadGameHandler
    {
        [CallAfterGameLoadedAttribute]
        public static void Kjorteo_Species_LoadGameCallback()
        {
            // Called whenever loading a saved game
            GameObject player = XRLCore.Core?.Game?.Player?.Body;
            if (player != null) // Only set this the FIRST time a saved game is loaded, not EVERY time
            {
                var part = player?.RequirePart<Kjorteo_SpeciesManager>();
                if (part.DisplaySpecies == null)
                {   part.DisplaySpecies = part.Species;   }
            }
        }
    }
}
