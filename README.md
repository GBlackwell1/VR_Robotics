# VR_Robotics

## Documentation for currently used resources
[Rosbridge](http://wiki.ros.org/rosbridge_suite) - Initial work done <br />
[Roslibjs](wiki.ros.org/roslibjs/Tutorials/BasicRosFunctionality) - Initial work done <br />
[Unity Web Request](https://docs.unity3d.com/Manual/UnityWebRequest-HLAPI.html) - Initial work done <br />

# ROS Installation
To use ROS and to have the best experience, it is recommended you use Ubuntu 20.04, as this is the release that supports ROS-noetic.
For details on the installation of ROS-noetic, please refer to ROS' official [documentation](http://wiki.ros.org/Installation/Ubuntu).
## Backend setup
We are using 3 key technologies, Unity Web Requests, which can send send forms to an active server, Rosbridge, which creates a web socket that can recieve ros commands in the form of JSON, and Roslibjs, a way to interface with the previously mentioned websocket server. Using a custom built node server, we can handle and do all these requests ourselves. <br /><br />
To make sure you are setup, ensure that you have node installed. 
To satisfy requirements that the server needs to run using 
```
npm i express  
npm i roslib
```
Running both of these will add some of the packages need for both ros and the server to run. The final package that needs to be added is the rosbridge which is not able to be installed via npm. To install, run
```
sudo apt-get install ros-noetic-rosbridge-server
```
After that, start up both the websocket for rosbridge, the server 
via node, and launch unity to test the connection. Watching the output of the node server should give you confirmation that all three pieces are established correctly and are connected. Echoing the newly created rostopic and restarting Unity will yield output in the terminal that is echoing the topic, if done correctly. 

## VirtualBox
We are using VirtualBox with a Ubuntu 20.04 ISO. This VirtualBox instance handles middleman communication between Unity and Kinova. Download a 20.04 ISO from Ubuntu's website and install and create a VirtualBox instance from the ISO. Once completed, you most forward ports and usb devices through Windows to your new instance. Start by plugging in the Kinova robotics and navigating and creating a filter for the device, see below a picture of correct instructiosn: <br /><br />

Once done you must forward two ports, 9090 this websocket, and 8000 the custom node server, see below the images on how to correctly set up the ports: <br /><br />

Once that is done, unplug the robot and reboot the instance. Wait until the instance is fully booted and plug in the robot, remove the device using "Devices and Settings" in windows, not physically. Once removed through windows, physically unplug the device and plug it in, the USB should now be filtered through to your instance and will have a name "VirtualBox USB" if done correctly.

## Kinova ROS
First, setup a directory in home called ```catkin_ws``` and create a directory named ```src``` within it. 
Next run the commands:
```
cd ~/catkin_ws/src
git clone -b noetic-devel https://github.com/Kinovarobotics/kinova-ros kinova-ros
cd ~/catkin_ws
```
This will clone Kinova's API for the robot. Before you fully create the workspace, you need to install all dependencies that Kinova requires. To do so, run the following command, this will take a while to complete:
```
rosdep install --from-paths src -y --ignore-src
```
After that you can run the command ```catkin_make``` and you will have a fully constructed workspace with Kinova's API. To begin to use these new packages, don't forget to source your workspace in every new terminal by using ```source ~/catkin_ws/devel/setup.bash```.
The next step is to spin up Kinova's stack, you can do this by running the command:
```
roslaunch kinova_bringup kinova_robot.launch kinova_robotType:=m1n6s300 
```
You should see a succesful connection be established with the robot and listing all the rostopics will show all the currently operating topics that Kinova's driver has created.

## Other Information
[Demonstration on how the backend works](https://youtu.be/JW2PU8VDYow)

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
