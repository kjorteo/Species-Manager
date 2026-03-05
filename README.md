SPECIES MANAGER
A Caves of Qud mod by Celine Kalante Love & Frirends (the Woodling System) AKA Kjorteo
Version 1.1.0

=====
ABOUT
=====
Caves of Qud is the most openly transhumanist game we've ever played.  The two vanilla genotypes have different specialties: Mutated Humans with their mutations, True Kin with their cybernetics.  However, these are just how each genotype answers the same basic question, which is how one can become better than the limits of a standard human form.

It's a little irksome that NPCs keep calling us "human," though, no matter how much we're either not or at least not anymore.  It feels almost like the species equivalent of misgendering, you know?  We'd expect the Putus Templar to be that closed-minded about how we've developed, perhaps, but not dromads.

This mod started as a simple idea: give the player a wish to change species.  We then ran into gameplay and mechanics-related reasons why the system is set up the way it is.  See, in the base game, weapons with the "morphogenetic" item mod react to striking a target by attempting to stun everyone in the entire area who shares that target's species.  Morphogenetic weapons are thus great for clearing out crowds of swarming monsters such as snapjaws, goatfolk, arachnids, and so on, but the game balance is that taking them against the Putus Templar (or any of several other technically-human factions) is a bad idea, lest you stun yourself.  Plus, even if morphogenetic weapons are the only mechanical effect that uses the player's species we can think of offhand in the base game, you never know what modders will come up with....

To balance vanity with... well, balance, we thus split "technical species" and "display species" into two separate parameters.  Now characters have a species that's the visible one in NPC dialogue (but that doesn't have any mechanical effects) and the species that affects morphogenetic weapons, whatever modders decide to do with the player's species, etc. (but isn't displayed in text anywhere.)  We then lumped everything into one big hopefully-convenient one-stop hub menu: simply wish for "species" and adjust the options to your heart's content!

You even have two ways to change your technical species: You can enter whatever you want in a free-form text entry box or you can pull up a list of the preexisting in-game species and select yours via multiple choice.  This eliminates the guesswork when you want your character to belong to some particular species but you can't remember whether they're spider (singular) or spiders (plural) or arachnids or what.

(Your display species does not have a similar multiple-choice preexisting list for what hopefully should be obvious reasons, though you of course are free to enter whatever you'd like in the free-form text entry box.  This is how people will refer to your species in dialogue, now completely unattached to any mechanical concerns or considerations, so have fun with it!)

If you're looking for an example of an OC species (currently just CYF expansion tiles, but perhaps playable genotypes and more could be added someday..?) that greatly benefit from having Species Manager to give them their proper intended display species, might we recommend [url=https://steamcommunity.com/sharedfiles/filedetails/?id=3668028848]The Ibekki[/url]?

=====
MOSTLY-UNRELATED BONUS
=====
While we're at it, this mod also even sort of halfway-fixes the capitalization issue with species dialogue!  This requires some explanation, but bear with us....

You might have noticed that dromad traders (and any mod that uses your species name to start with a sentence) all fail to capitalize your species at the start of a sentence.  There are actually two reasons for this.  One is that default dialogue incorrectly references =player.apparentSpecies= instead of =player.ApparentSpecies= (notice the capital A.)  The former is the actual value as-is, and the latter is supposed to be that but with the first letter capitalized.  This mod includes a conversations.xml file that correctly puts the =player.ApparentSpecies= references where they're supposed to be.

However, this by itself does absolutely nothing, because the other issue is a bug in Caves of Qud itself that fails to see the difference and properly capitalize it even when someone does use the correct tag.  Therefore, this mod by itself will NOT magically fix the capitalization issue; it will change the dialogue to make the correct dialogue calls, but the game itself will still ignore those calls.  However, this mod at least... lays the groundwork, we suppose?  Two things need to happen for capitalized species to work and one is completely out of our hands, but this mod fixes the other just in case/in advance.  If there's a Caves of Qud update and/or some ambitious modder out there and the tag behavior is fixed, you will be ready.

=====
CREDITS
=====
This mod never would have happened without the amazing and generous support from the good people of the Qudcord modding channel.  Seriously, if you've never modded before and don't know anything about anything, those folks will take you from newbie to... someone who's capable of releasing something like this, at least.

Tam's custom icon in the preview thumbnail is from Unique Makeovers by Balthichou.

=====
VERSION HISTORY
=====
1.1.0: Fixed the bug where display species weren't saving properly; you shouldn't have to set your display species again every time you load your saved game anymore. Fixed bugs and overall behavior with how Species Manager interacts with the player swapping bodies (wishes, Domination, etc.) and with being disguised. The "Explanation" option is now "Read Explanation" (same thing, clearer wording) and its writing has been very slightly revised. Cleaned up the source code in the Kjorteo_SpeciesManager.cs file down to the comments and commented out parts; it should now be a lot easier to read along and see what it's doing and how for aspiring modders who want to learn by reverse-engineering it. Finally, added an optional "support the authors" option in the main menu with a link to our Ko-Fi. uwu
1.0.0: Initial release
