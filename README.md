| | |
| :---        |          ---: |
| **DO NOT download this project via the ZIP file option on GitHub (i.e. largefiles are not included -- fbx, png, etc. files will be missing). Clone the project using git.** | ![DONT DOWNLOAD ZIP](../assets/dont-download-zip.png?raw=true) |

# AI Planner: Samples
Welcome to the samples repository for the [AI Planner](https://docs.unity3d.com/Packages/com.unity.ai.planner@latest/). The following samples are included in this repository:
* VacuumRobot - Control a robot optimizing its path through an ever-dirty world.
* Match3 - Use planning to solve goal-based, tile-matching puzzles.
* EscapeRoom - Escape a room with locked doors, a key, and pressure switches by coordinating three agents with a single planner.

## What is the AI Planner?
The AI Planner includes authoring tools and a system for automated decision-making. 

Automated planners are useful for:
* Directing agent behavior either in a cooperative, neutral, or adversarial capacity
* Auto-generating storylines or as an online story manager
* Validating game design mechanics
* Assisting in creating tutorials
* Automated testing

Start by defining a domain definition of traits/enumerations. Then, create action definitions for what actions are possible in the domain. Once the planning problem is defined, the planner system will iteratively build a plan that converges to an optimal solution. Execute these plans by adding a decision controller to your agent.

## Installation Guide
1. Clone this repo (downloading a .zip file will not include largefiles -- e.g. fbx, png)
2. Open any of the sample projects in Unity version 2019.3

## Documentation
Documentation for the AI Planner is available through the [package documentation](https://docs.unity3d.com/Packages/com.unity.ai.planner@latest/).

For further discussion, [please visit the forum](https://forum.unity.com/forums/ai-navigation-previews.122/).
