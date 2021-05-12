# MobileRobot_Pololu3Pi
 By using the application, we can connect to a mobile robot, control it, and provide feedback on the state of the robot.

## General
  
The application allows you to connect to the robot via the socket, it enables the exchange of information between the robot and the computer. Robot control consists in sending text frames in the appropriate format, which the robot decodes and changes into control instructions - in return, the robot sends return frames that must be decoded and thus we receive information about the robot's state and about the light sensors installed in the robot.

The application uses Background Worker, thanks to which the exchange of information between the robot and the computer occurs in separate threads.

## Using

Using the application is simple, just enter the IP of the robot with which we want to connect, connect to it and you can control it. The sliders control the left and right motor, respectively, and the LED1 and LED2 buttons turn on the corresponding lights in the robot.

In the application, we can control the robot manually by moving the sliders, but also using a pad connected to the computer, or simply using the keyboard - just select the appropriate control mode.

As part of the robot's response, we receive information about its status and the status of light sensors.

## Sample application operation
![111111](https://user-images.githubusercontent.com/59517489/117930123-e6961e80-b2fd-11eb-80a0-5ac4c0acde1e.jpg)

## Summary
The application works properly, however, it is one of the initial applications created by me in C# - in the future I plan to organize the application code more, add appropriate comments, divide the code into appropriate sections and classes to increase its clarity.

The application can also be developed by adding a robot trajectory generator that would show the approximate path that the robot could follow.

The project status stays open and ready for the development of additional functions.
