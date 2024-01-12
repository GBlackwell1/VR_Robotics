# VR_Robotics

## Documentation for currently used resources
[Rosbridge](http://wiki.ros.org/rosbridge_suite) - In progress <br />
[Roslibjs](wiki.ros.org/roslibjs/Tutorials/BasicRosFunctionality) - In progress <br />
[Unity Web Request](https://docs.unity3d.com/Manual/UnityWebRequest-HLAPI.html) - Initial work done <br />

# ROS Installation
To use ROS and to have the best experience, it is recommended you use Ubuntu 20.04, as this is the release that supports ROS-noetic.
For details on the installation of ROS-noetic, please refer to ROS' official [documentation](http://wiki.ros.org/Installation/Ubuntu).
## Backend setup
We are using 3 key technologies, Unity Web Requests, which can send send forms to an active server, Rosbridge, which creates a web socket that can recieve ros commands in the form of JSON, and Roslibjs, a way to 
interface with the previously mentioned websocket server. Using a custom built node server, we can handle and do all these requests ourselves. To make sure you are setup, ensure that you have node installed, to satisfy requirements
that the server needs to run, install ```express roslib``` using ```npm i``` and make sure the ros packages for both ```rosbridge roslibjs``` are installed via ros. After that, start up both the websocket for rosbridge, the server 
via node, and launch unity to test the connection. Watching the output of the node server should give you confirmation that all three pieces are established correctly and are connected.

## Other Information

## Running the project on CEC1081
Always make sure Oculus is launched on the computer to ensure a wired connection. <br />
Upon opening the project, go to Edit -> Project Settings -> XR Plug-in Management -> OpenXR -> Play Mode OpenXR Runtime and within that dropbox make sure that "Oculus" is selected. <br />
Now the project should run correctly and launch on the headset.

## Setting up firebase for remote repositories
### google-services.json
For security reasons, the google-services.json file has been ignored. <br />
When running on a new remote repository, please download the "google-services.json" file on the VRAR-Robotics firebase project linked in the Trello. <br />
Once downloaded ensure that it is named <i>google-services.json</i> and then, in Assets, if there is not already a folder within Assets named "StreamingAssets", create that directory and put the google-services.json within it. <br />
### FirebaseCppApp-11_4_0.bundle
A firebase file has been left out of the project due to it's size, this file is named FirebaseCppApp-11_4_0.bundle. <br />
This file can be found on CEC1081 on Desktop/FIREBASE. <br />
Put this file in the following directory: Create-with-VR_2021LTS\HCITECH-Robotics_Project\Assets\Firebase\Plugins\x86_64 <br />
