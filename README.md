#Tiny Blades

Tiny Blades is mini combat game with cute (but deadly) warriors.

![Untitled video - Made with Clipchamp (2)](https://github.com/BilGameDev/TinyBlades/assets/107997032/acc99753-88a4-4823-90d8-1b0b1ae74550)

The game uses Mirror networking and features a server-authoritative player controller along with proper audio, health and animations syncronizations.
This game is not based on a template and was developed from the ground up by me. I made use of some assets for visuals as I am not the greatest artist. All assets present are available for free on the Asset Store.

![Untitled video - Made with Clipchamdp (2)](https://github.com/BilGameDev/TinyBlades/assets/107997032/56d4f4db-85c3-4858-b731-3b6b1f6d2546)

This project also contains a NetworkManager which uses Epic Games Online Services to run. It can be found in Prefabs -> Network -> NetworkEOS.
It is based off of EpicOnlineTransport available here: https://github.com/FakeByte/EpicOnlineTransport

I have opted for a Raycast based collision detection as opposed to a Physics based system as it can be quite heavy and unreliable in a networked enviroment.

This project will grow as I understand more about Networking. I plan to introduce multiple Epic Games Services to this project.
