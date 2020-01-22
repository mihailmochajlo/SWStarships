SWStarships
=

**Task**: basing on the information from https://swapi.co calculate count of stops to resupply to cover a given distance for each starship from Star Wars universe.

**Assumption**: no output for starships with incomplete information (name, megalights per hour, consumables time)

Structure of apllication
-
- App folder - .NET Core application (Console template)
- App.Tests folder - .NET Core application (XUnit Test template)

How to run
-
- Requirements: .NET Core 3.1
- Testing (from root directory): `dotnet test`
- Run (from *App* directory): `dotnet run`

Example
-
**Distance**: 1000000  
**Output**:  
Death Star: 0  
EF76 Nebulon-B escort frigate: 0  
Sentinel-class landing craft: 4  
Y-wing: 17  
TIE Advanced x1: 18  
Slave 1: 4  
Executor: 0  
Imperial shuttle: 3  
Millennium Falcon: 2  
X-wing: 13  
A-wing: 11  
Calamari Cruiser: 0  
B-wing: 15  
Rebel transport: 2  
Star Destroyer: 0  
arc-170: 19  
CR90 corvette: 0  
