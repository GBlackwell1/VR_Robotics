# VR_Robotics

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
We are using VirtualBox with a Ubuntu 20.04 ISO. This VirtualBox instance handles middleman communication between Unity and Kinova. Download a 20.04 ISO from Ubuntu's website and install and create a VirtualBox instance from the ISO. Once completed, you most forward ports and usb devices through Windows to your new instance. Start by plugging in the Kinova robotics and navigating and creating a filter for the device, see below a picture of correct instructions: <br />[TODO: GIF]<br />

Once done you must forward create a bridged network adapter, do so here but make sure your network adapter reflects the one on your system: <br />[TODO: GIF]<br />

Once that is done, unplug the robot and reboot the instance. Wait until the instance is fully booted and plug in the robot, remove the device using "Devices and Settings" in windows, not physically. Once removed through windows, physically unplug the device and plug it in, the USB should now be filtered through to your instance and will have a name "VirtualBox USB" if done correctly.

## Kinova ROS
First, setup a directory in home called ```catkin_ws``` and create a directory named ```src``` within it. 
Next run the commands:
```
cd ~/catkin_ws/src
git clone -b noetic-devel https://github.com/GBlackwell1/kinova-ros-unity
cd ~/catkin_ws
```
This will clone our version of Kinova's API for the robot. Before you fully create the workspace, you need to install all dependencies that Kinova requires. To do so, run the following command, this will take a while to complete:
```
rosdep install --from-paths src -y --ignore-src
```
After that you can run the command ```catkin_make``` and you will have a fully constructed workspace with Kinova's API. To begin to use these new packages, don't forget to source your workspace in every new terminal by using ```source ~/catkin_ws/devel/setup.bash```.
The next step is to spin up Kinova's stack, you can do this by running the command:
```
roslaunch kinova_bringup kinova_robot.launch kinova_robotType:=m1n6s300 
```
You should see a succesful connection be established with the robot and listing all the rostopics will show all the currently operating topics that Kinova's driver has created.

## Connecting Unity and ROS
Once the ROS master service is running and you have launched Kinova's stack, it's time to launch our project's specific items. The first of which is to start the rosbridge instance by running:
```
roslaunch rosbridge_server rosbridge_websocket.launch
```
This command launches the websocket that stays in direct communication with the ROS instance. <br/><br/>

Next go ahead and start our node server by navigating into WebRobotics and making sure everything is installed by running `npm i`, next start the server by running:
```
node robo-server.js
```
You should see output that it has succesfully connected to the previously ran websocket.<br/><br/>

Finally you should be able to start our custom ROS node. Go to your catkin_ws and run:
```
rosrun kinova_unity unity_comm.py
```
This will startup our node that facilitates connection between the websocket and the robot. For more information on this repository see this link [here](https://github.com/GBlackwell1/kinova-ros-unity).

## Other Information
[Demonstration on how the backend works](https://youtu.be/JW2PU8VDYow) <- OUTDATED 

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
