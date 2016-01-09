# EndlessReach
Source code for the last Unity version of Endless Reach, dated November 2014.

// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

This repository contains the source code for the final version of Endless Reach, built by Soverance Studios. The last version update was version 2.4.1, released in November of 2014. This application was built in Unity 4 and programmed in C#, and as it will never again be updated, the project is now entirely open source. No assets are included, this is only the source code.

Endless Reach is a hardcore shoot ‘em up that puts you in the shoes of a lone space drone pilot, laying claim to the uncharted galaxy known as the Reach.

Send your starfighter drone out into the unknown, and use it to battle your way through the alien hordes and make the regions safe for colonization. The future of humanity depends on you!

Endless Reach was originally prototyped for mobile devices, and released for Android and Windows Phone devices. Oculus Rift support was added shortly afterward, and the VR supported version was released to Oculus Share. We also ran promotional campaigns for Endless Reach on both Steam Greenlight and the Square Enix Collective, though both were ultimately unsuccessful. The Greenlight page has since been removed, but the remaining informative links persist:

Main Website:  http://soverance.com/endlessreach/

Google Play:  https://play.google.com/store/apps/details?id=com.Soverance.EndlessReach&hl=en

Windows Phone:  https://www.microsoft.com/en-us/store/apps/endless-reach/9nblggh0k1tz

Oculus Share:  https://share.oculus.com/app/endless-reach

Square Enix Collective:  http://collective.square-enix.com/projects/54/endless-reach

Reddit:  https://www.reddit.com/r/EndlessReach/

Endless Reach contains some basic shoot 'em up logic with the addition of a few new things, namely the Overload and Boost features. Online leaderboards are also incorporated, and they are powered a public Google Spreadsheet. There's saving and loading, as well as a "world hub" called the Nautilus that essentially serves as a fancy level select menu.

[KEY FEATURES]

10 action-packed levels set in beautiful spacescapes
Amazing electronic soundtrack by Shiny Baubles
High powered particle effects and visual stimuli
Power-up chaining and limit break mode
5 enemy types, 3 turret types, and a different boss in every stage!
Missile targeting via head tracking!

[PLATFORMS]

note that all platforms other than VR are no longer in development. VR ONLY!

Complete Version History is found on soverance.com.

* Windows PC - VR [Oculus Rift]
* Android
* Windows Phone
 

[INSTALLATION INSTRUCTIONS]

Download from Official Website: http://endlessreach.soverance.com

[CONTROLS]

Endless Reach supports the Xbox 360 gamepad ONLY

* Head Tracking - Lock On Target [look at target to lock]
* Left Analog - Drone movement
* Left Stick Down (L3) - Unused
* Right Analog - Unused
* Right Stick Down (R3) - Unused
* Directional pad - Menu movement
* A - Fire
* X - Purge [when prompted]
* B - Missile [when locked on target]
* Y - Unused
* Left Trigger - Boost [hold to continue]
* Right Trigger - Unused
* Left Shoulder - Toggle FPS counter
* Right Shoulder - Unused
* Start - Pause / Unpause
* Back - Reset HMD Orientation

[GAMEPLAY FEATURES]

[FIRE MODES]

Collect BLUE orbs to advance fire mode to the next level.

* Standard [0.4 second fire rate, single shot, blue effect]
* Plus [0.2 second fire rate, single shot, green effect]
* Multi [0.1 second fire rate, triple shot loose, yellow effect]
* Cannon [0.06 second fire rate, double shot strong, red effect]
* Limit Break [20 second duration, 0.03 second fire rate, single shot strong, purple effect + shield]

[LIMIT BREAK]

* When entering a Limit Break, a timer will appear on the HUD. This timer indicates the remaining duration before the ship begins to overload.
* You can reset the timer, thereby extending the duration of a Limit Break, by simply collecting another blue power orb.
* The drone takes no damage during limit break.
* When the Limit timer reaches zero, the drone will begin to Overload.

[OVERLOADING]

* The drone becomes unable to move while overloading.
* Overloading is a sort of quick time event, in which the player will be prompted to press button “X” in order to safely purge the overloading energy.
* A successful purge will send a large shockwave to destroy all enemies within a large radius, and deal damage to bosses as 25% of their maximum HP.
* Failure to purge at the appropriate time will destroy the drone.

[BOOST]

Hold the Left Trigger to increase movement speed. Initial boost shockwave will destroy all enemies in a small radius. The shockwave does no damage to bosses. Boost power is limited to a meter on the HUD. When the meter is depleted, the boost feature will no longer be available. Collect orange orbs to refill the boost meter.

[LOCK-ON TARGETING]

Using the Oculus Rift HMD, certain objects can be targeted simply by looking directly at them. When locked on, a targeting reticle will appear around the object, and the drone will notify the pilot of it's locked on status via a sound effect. The following objects are currently targetable:

* Turrets
* Certain portions of Bosses

[HOMING MISSILES]

Once locked on to a target, press "B" to fire a homing missile of charged energy. The drone can charge an infinite number of missiles, but they can only be fired while locked on to a target.

[The NAUTILUS]

The Nautilus is the pilot's vessel, used to explore the Reach, and it serves as the starfighter drone's dock. Since sectors of the Reach are currently too dangerous to send humans to, the Alliance has chosen to deploy these starfighter drones to eradicate hostile alien life in the area, making the Reach safe for colonization.

[Ethereal]

Ethereal is the A.I. companion on board the Nautilus. He is your psychic link to your starfighter drone, Aurgus, and will be your sole source of information concerning mission objectives and status. Listen to what he has to say, as the voice of Ethereal provides valuable insight into playing the game!

[Galaxy Map]

Activate the Galaxy Map by pressing "A" when in range of the pedestal. A black hole will appear to let you know you are in range. Use the Galaxy Map to select the system you wish to explore. Choosing a system will notify your intent to Ethereal, who will open a portal in the staging area. Entering the portal will link your mind to his, and together you will launch the Aurgus to the chosen planetary system.

* You can hit "B" at any time to close the Galaxy Map
* 
[ONLINE LEADERBOARDS]

View the Leaderboards at the official website

Leaderboards are separated by each level, and high scores are level-independent. Pilots will be prompted at the status screen to enter a name if their score beats one of the level's Universal High Scores.

* Only ten (10) Universal High Scores are allowed per level.
* Names can only be 15 characters long.

[LEVEL DETAILS]

Level Name (Music Track) [Boss Name]

0 - Main Menu (Song For You, Milkman)
1 - Lumoria Nebula (Thunder Muscle) [Rex'Aern]
2 - Celestial Plains (Lightstabber) [Celestial Archon]
3 - The Spiral Downs (House-Step) [The Watcher]
4 - Coral Dreamscape (Power Moves!!! v2)
5 - The Drudgeon Edge (This One’s For You)
6 - Cirrus Straight (Aphrodisia)
7 - Visceral Downfall (Hocus Pocus)
8 - Centurion Dust Fields (It’s a Mega Dub Style)
9 - The Shinkansen Spiral (They Come)
10 - Omega Terminal (Fatima’s Funk)

LEVEL LAYOUT AND ENEMY LIST

1: (Ra'Aern)/ Turret_01 - Boss 01 - Aern - (Easy Boss)
2: (Ra'Aern)/ Turret_01 - Boss 02 - Celestial Archon - (Easy Boss+1)
3: (Disarray Probe)/Turret_02 -Boss 03 - The Watcher - (Easy Boss+1)
4: (Disarray Probe)/Turret_02 -Boss 04 - Harbinger of Chaos - (Intermediate Boss)
5: (Centipede Minion)/Turret_03 -Boss 05 - Wyvern - (Intermediate Boss)
6: (Centipede Minion)/Turret_03 -Boss06 - Dragon Worm - (Hard Boss)
7: (All minions + turrets) -Boss07- The Hammer - (Very Tough)
8: (All minions + turrets) -Boss08- Karasue - (Very Tough +1)
9: (All minions + turrets) -Boss09- Guardian of The Reach - (Incredibly Tough)
10:THE GAUNTLET - CONCURRENT BOSS FIGHTS ( 02,04,06,07,08,09) 10 - Omega - (Impossible to Gauge)



