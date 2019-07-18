# ForgeAPI_SoundsRegister
Automatically creates sounds.json, SoundLocations.java and SoundEvents.java for all your sound-assets. Kills the routine
Before the launch you should move all your sound-assets to %ProjectDirectory%\src\main\resources\\%modid%\sounds\
Two modes:
1. Launch with cmd wth two attributes.
    The first one is directory of your minecraft mod project (there where the src folder contains)
    The second one is the main package of your project generally it's looks like "%domain%.%author%.%modid%", for the sentence "com.patrulteam.patrulmod". If this package would not exist it will be created automatically.
2. Launch as simple console application. In this mode console will tell you by itself all you need to input.
