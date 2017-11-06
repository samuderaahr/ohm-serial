# OpenHardwareMonitor to Serial

This is a personal fork of OHM that aims to bridge the readings of various OHM sensor with external microcontroller via COM/Serial Port. I use this for controlling external PWM fans, LCD display of temperatures, and RGB lights on my under-desk "desk-bottom" PC.

Sensor access provided by OpenHardwareMonitor Library: https://github.com/openhardwaremonitor/openhardwaremonitor
Cancelled using fancy gauges as it will affect performance, using text labels instead. Probably next time around.

## Repo Organization

The repo is divided into two parts:
- The server (ohm-server folder) which is built with C# .NET 4.0 on Visual Studio 2017 (you can always import to earlier versions by the .sln)
- The client (ohm-client-xxx folders) where xxx denotes the hardware platform on which it's built upon (currently only arduino-avr, but others like arduino-esp and rpi are planned for)
 
Each client type has its own Readme so be sure to check inside. PCB design source are in "pcb" (made with KiCAD) and source codes are in "src". 
