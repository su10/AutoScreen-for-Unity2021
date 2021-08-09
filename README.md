# AutoScreen for Device Simulator

AutoScreen provides a smart solution for Safe Area in Unity.

## Description

Unity's Device Simulator has a lot of information about Safe Area, but there is no feature to use them to support Safe Areas or to preview them comfortably in the Unity Editor.

By using components such as `SafeArea` provided by AutoScreen, you can support and quickly check the Safe Area with Device Simulator regardless of whether it's playing or not.

This is done by transforming the RectTransform, but the changes are not saved in the file, so it makes no difference in the version control system.

## Installation

### Requirement

* Unity: 2019.4 or higher
* Device Simulator: 2.2.4-preview or higher

### Install

1. Open the Package Manager
1. Press \[＋▼\] button and click `Add package from git URL...`
1. Enter the following:
  * https://github.com/su10/AutoScreen-for-Device-Simulator.git?path=Assets/Jagapippi/AutoScreen

or add a following line to `dependencies` field of your Packages/manifest.json.

* `"com.jagapippi.auto-screen": "https://github.com/su10/AutoScreen-for-Device-Simulator.git?path=Assets/Jagapippi/AutoScreen"`
