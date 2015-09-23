# nuclear-watchdog

Small program that demonstrates a heartbeat architectural pattern for SWEN-755.

## Technology

The application is written in C#/.NET with no additional frameworks in use. The
primary platform feature in use for this application is the Windows Communication
Foundation (WCF).

WCF allows communication between processes through a contracted interface that is
defined by the server (the heartbeat receiver in this case) and consumed by the
client (the heartbeat sender).

## Running

Open the solution file in Visual Studio (as administrator) and run both projects in
the solution. Note that the heartbeat receiver must be started first, otherwise the
"Nuclear Watchdog" will not be able to register itself as a client.

In order to simulate the effectiveness of the heartbeat attribute, the watchdog
program will crash if the "Reactor" reaches a critical temperature, but only 50% of
the time.

The heartbeat receiver will consider a client dead if it does not receive three
consecutive heartbeats from the connected client.
