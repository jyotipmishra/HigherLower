# HigherLower
API based game to guess a higher or lower card value. 

# Pre-requisites to run the application
  >Docker must be installed to easily run the APIs.
  >Clone the project into local directory.
  >Using powershell, go to the directory path where the docker and docker-compose file available.
  >Run docker-compose.yml file by using docker-compose up command.
  >Once the service is up, browse to http://localhost:5005/index.html to check if the service is up and running.
  >Then follow the next section to play the game. 

# Steps to Play
  1> First you need to create a game using **POST /api/games** by giving a name and number of cards(optional, default value is 52).
  2> From the response, use the gameid, and facevalue to play the game using **POST /api/games/play**. 
  3> Once you know the result if you win or loose, then you can move to the player by using **PUT /api/games/nextplayer** endpoint by passing the gameid.
  3> Continue until all cards get over.
  4> To check all available games, the endpoint **GET /api/games** can be used.
  
  
# Available end points to play around
![image](https://user-images.githubusercontent.com/16212487/116118347-cef24f80-a6da-11eb-9d38-15c289377291.png)



