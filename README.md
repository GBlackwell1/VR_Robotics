# VR_Robotics

# ROS Installation
To use ROS and to have the best experience, it is recommended you use Ubuntu 20.04, as this is the release that supports ROS-noetic.
For details on the installation of ROS-noetic, please refer to ROS' official [documentation](http://wiki.ros.org/Installation/Ubuntu).
## Environment Setup
For ease of use we're using docker to make it easier to setup your environment. Open the env_setup folder in the project and navigate to <b>ros_unity_integration</b>. Here you will find everything that has to do with ROS and Unity packages. <br/><br/> Once ROS-noetic has been installed, you should have docker on your system, run ``` docker ``` and see if you get output. If you recieve an error, view ROS-noetic's documentation to ensure docker is installed. Once you have output run ``` docker pull ros:noetic-robot && docker pull ros:noetic-ros-base ```. This will make sure you have the correct ROS image for our docker container. 
<br/><br/>
Once pulled you can now run the command ``` docker build -t noetic -f ros_docker/Dockerfile . ``` to build your container, (don't worry this step might take some time, ensure that you have a reliable internet connection). Once built, run ``` docker run -it --rm -p 10000:10000 noetic /bin/bash ``` to enter into your container, you should now be put into a new shell, go ahead and run ``` source devel/setup.bash ``` to source your new environment then you're good, congrats!
## Container Configuration
Unfortunately there's still a couple steps left to complete, once in your container, you should have python2.7 and python3.8 installed, run both ``` python --version ``` and ``` python3 --version ``` to confirm both are installed. <br /><br />
There will probably be a couple packages missing for both ROS packages, since these are reliant on python and not ROS, additional steps are required. The ros_tcp_endpoint package expects a python version 2.X and kinova-ros expects 3.X, so now lets install some python modules using python's pip installer. To get this installer, run ``` apt install curl ```, this will help to download pip from online. <br /><br />
Run this command with curl: ``` curl -sSL https://bootstrap.pypa.io/get-pip.py -o get-pip.py ``` to get python3's package manger "pip3", then with python3, run ``` python3 get-pip.py ``` to install the manager. For python2, similarly run ``` curl -sSL https://bootstrap.pypa.io/pip/2.7/get-pip.py -o get-pip.py  ``` to get it's manager then run ``` python get-pip.py ``` to install its manager. <br /><br /> To get the final remaining dependencies, run ``` python2.7 -m pip install PyYAML ``` and ``` python2.7 -m pip install rospkg ```. These commands will install the missing dependencies that ros_tcp_endpoint expects. For the final bit, now ROS noetic expects to call python and recienve python3.X but instead it recieves python2.X, we must link python3 to python by running the command: ``` sudo ln -sf /usr/bin/python3 /usr/local/bin/python ```. Since at this point we are accessing the arm via usb connection, run the command ``` sudo cp kinova_driver/udev/10-kinova-arm.rules /etc/udev/rules.d/ ``` to have the ability to connect to the robot via usb. <br /><br /> Congrats! You've now setup your container fully and are ready to run code! (I will make a shell script soon to make the process a little easier).

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
