# nuclear-watchdog

Small system that demonstrates a several architectural patterns for SWEN-755.
The first pattern is a heartbeat pattern that fulfills the architectural tactic
of fault detection. The second pattern is an active/passive redundancy pattern
that fulfills the architectural tactic of fault recovery.

The system is split into three components: the Monitor, the Reactor, and the
Watchdog(s). The Monitor is responsible for keeping track of heartbeats as well
as which watchdog is active and which are standbys. The Reactor is responsible
for "collecting" (in reality it is simply a simulation generating random
numbers) raw temperatures and handing that temperature out to anyone that asks.

## Technology

The application is written in C#/.NET with no additional frameworks in use. The
primary platform feature in use for this application is the Windows Communication
Foundation (WCF).

WCF allows communication between processes through a contracted interface that is
defined by the server (the heartbeat receiver in this case) and consumed by the
client (the heartbeat sender).

## Running

Open the solution file in Visual Studio (as administrator) and build the entire
solution. Then, run each executable outside of Visual Studio once built. It is
critical to do this because Visual Studio will pause the entire system once it
has detected one of the processes has crashed (which is what we expect). Note
that the heartbeat receiver must be started first, followed by the reactor, and
finally the Watchdog. Otherwise, the "Watchdog" will not be able to register
itself as a client to the other processes.

In order to simulate the effectiveness of the fault detection and recovery, the
watchdog will crash if a) the watchdog is the "active" system and b) the "Reactor"
reaches a critical temperature, but only 50% of the time.

The heartbeat receiver will consider a client dead if it does not receive three
consecutive heartbeats from the connected client.

The first connected watchdog instance will be considered the active system. Any
subsequent number of instances will become the standbys (and will be activated in
the order that each instance connected, respectively).