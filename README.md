# MobileRobot_Pololu3Pi
 By using the application, we can connect to a mobile robot, control it, and provide feedback on the state of the robot.

## General
  
The application allows you to connect to the robot via the socket, it enables the exchange of information between the robot and the computer. Robot control consists in sending text frames in the appropriate format, which the robot decodes and changes into control instructions - in return, the robot sends return frames that must be decoded and thus we receive information about the robot's state and about the light sensors installed in the robot.
