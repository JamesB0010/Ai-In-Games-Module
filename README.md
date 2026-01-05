  <h3 align="center">Artificial Intelligence for Games</h3>


  James Bland - 22142846

  Game project included in folder called mouseback trials

  game project included in folder called proj

  <p align="center">
    A game created to explore different ways to implement AI
    <br />
    <a href="https://github.com/othneildrew/Best-README-Template">View Demo</a>
  </p>
</div>

![Ai in games image](https://github.com/user-attachments/assets/1bc7fce2-b737-4512-b9c8-bf7240ef299e)


<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project
As mentioned earlier this is the repository for the game created in the Artificial Intelligence for games module on the Computer Games Technology course at Birmingham City University.
<p align="right">(<a href="#readme-top">back to top</a>)</p>


## Game Design

### Player Character
the player character will be able to run around, shoot and dive. 
the player will have health and can take damage and die

## Ai designs

Fodder - this fodder enemy will chase the player and attack them with a melee attack

Turret - the turret enemy will attempt to get into a medium distance from the player, once at a medium distance it will plant its self and shoot at the player. when the player goes out of range the enemy will get up and move closer to the player. if the player gets too close the turret will retreat

Spawner - the spawner will stay at medium range and spawn new fodder

Heavy - the heavy moves towards the player to attack them, if the player is far away it will use its long range attack if the player is close the heavy will deal big damage so the player wants to keep them at a medium range

Phantom - the phantom will find and enemy to possess. when it possesses an enemy the enemy is buffed. when the enemy dies the phantom will be released and will find a new enemy to possess.


AI round manager - the ai round manager will take in information such as player health and will spawn enemies. the goal of the round manager is to spawn enemies in a way that is challenging and keeps the player in the flow state without beeing too challenging


The player should survive fighting against the enemies. Every so often the player will have to complete a defence mission. if the player fails the defence mission their health will be capped. The player will get the option to escape every so often.


## Ai Techniques
the project will explore different types of ai such as finite state machines and behaviour trees

additionally intelligent ai and search ai

<p align="right">(<a href="#readme-top">back to top</a>)</p>


# Weekly diaries

## Week 1
Added third person character controller from unity assets store
Refactored and cleaned third person controller

Added logic needed to shoot guns and bullets. Muzzle flash and bullet impact particle systems added.

Version 1 of player controller done


## Week 2
Added enemy model

Made it so enemies can take damage and die

Enemies travel straight at player



## Week 3
Week three reflection

Upgraded fsm implementation so that it can support different types of scriptable object references

Following feedback made it so the state connection conditions can be evaluated by the agent.
Custom inspector editor for fsm

Added project to github

#Week 5
Worked on pathfinding algorithms. Used strategy pattern so each pathfinding algorithm is an object held in the asset browser and can be swapped in and out at runtime

#Week 6
Followed first 9 tutorials on Goal oriented action planning

Followed the Kiwi Coder tutorial series on implementing a behaviour tree system and editor



### Built With

The project was built in unity! 😇

[![Unity][Unity-shield]][Unity-url]
<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- GETTING STARTED -->
## Getting Started

Download code from here! 

Download built game from itch.io (game not finished yet, mind our dust!)

### Prerequisites

If downloading the project to extend or work on you will need
*unity version x
*A code editor, rider or visual studio etc...

### Installation

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- USAGE EXAMPLES -->
## Usage

This bit will contain gameplay screenshots and gifs 

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- ROADMAP -->
## Roadmap
See https://github.com/orgs/Birmingham-City-Uni/projects/54/views/1 for the project roadmap

See the [open issues](https://github.com/othneildrew/Best-README-Template/issues) for a full list of proposed features (and known issues).

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE.txt` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- CONTACT -->
## Contact

James - [Linkedin]([https://twitter.com/your_username](https://www.linkedin.com/in/james-richard-bland/))

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- ACKNOWLEDGMENTS -->
## Acknowledgments

Use this space to list resources you find helpful and would like to give credit to. I've included a few of my favorites to kick things off!




<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[Unity-shield]: https://img.shields.io/badge/unity-%23000000.svg?style=for-the-badge&logo=unity&logoColor=white
[Unity-url]: https://unity.com/
