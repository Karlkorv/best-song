# Spotify Song Tournament

## How to use:
Register an app at https://developer.spotify.com/dashboard <br>
Clone the repository <br>
cd to src folder and run <br>
```bash
dotnet user-secrets init
dotnet user-secrets set "Spotify:ClientId" "YOUR-CLIENT-ID" 
dotnet user-secrets set "Spotify:ClientSecret" "YOUR-CLIENT-SECRET"
``` 
If done correctly the secrets.json file should look like this
```json
{
  "Spotify": {
    "ClientId": "YOUR-CLIENT-ID",
    "ClientSecret": "YOUR-CLIENT-SECRET"
  }
}
```
then run `dotnet run` and navigate to https://localhost:7196/

## Resources
Spotify color palette (hex):
#1db954 Dark green
#1ed760 Light green
#ffffff Pure white
#191414 black
