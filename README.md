# Player-Guided Procedural Dungeon Generation

A Unity dissertation project exploring how **interactive genetic algorithms** can give players direct control over procedurally generated game content.

[![Unity CI/CD](https://github.com/braddarzs/Comp303-Dissertation/actions/workflows/main.yml/badge.svg)](https://github.com/braddarzs/Comp303-Dissertation/actions/workflows/main.yml)

[Watch the demo](ArtifactVideo.mp4) · [Read the development log](Devlog.md)

## Overview

The project generates playable dungeon rooms using a genetic algorithm. Players evaluate five candidate rooms, rating their preferred combinations of room size, enemies and loot. Those ratings become the fitness values used to evolve the next generation.

After three generations, the selected DNA is converted into a playable room containing enemies, loot and an exit. A random-generation mode is also included as a comparison against the player-guided approach.

## Engineering highlights

- Interactive genetic algorithm with player-defined fitness scores
- Roulette-wheel selection
- Crossover and mutation across room size, enemy count and loot count
- Runtime Tilemap generation and NavMesh rebuilding
- Top-down movement, combat, enemies and collectible loot
- JSON data logging for room choices, deaths and session duration
- Automated EditMode and PlayMode testing

**Tech:** Unity · C# · Unity Test Framework · GitHub Actions · GameCI

## CI/CD with GitHub Actions

The repository includes a CI/CD pipeline that runs automated EditMode and PlayMode tests.

The project-specific tests verify player movement and confirm that a selected candidate produces a playable Tilemap room.

## Controls

| Input | Action |
| --- | --- |
| `WASD` / Arrow keys | Move |
| Left click | Attack |
| Mouse | Navigate and rate room candidates |
