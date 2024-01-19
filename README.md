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

## WSL - Access to USB ports
By default, ports are not accessible between WSL2 and Windows. A third party software is needed to forward ports and bind them to WSL to allow access. Install the .msi installer on windows by navigating to this link [here](https://github.com/dorssel/usbipd-win/releases). Once installed, you need to install the appropriate tools on WSL, run both of these commands: 
```
sudo apt install linux-tools-generic hwdata
sudo update-alternatives --install /usr/local/bin/usbip usbip /usr/lib/linux-tools/*-generic/usbip 20
```
To install the appropriate software for WSL.<br />
### ONCE INSTALLED, RESTART YOUR DEVICE OR ELSE THE FUNCTIONALITY WILL NOT WORK
Next, in powershell, list the usb devices by running ```usbipd list```, identify the exact device you want to bind and note its bus-id, next run the command ```usbipd bind -b <your_bus-id_here>```. <br />
This will then allow you to attach your device to WSL by running the command:
```
usbipd attach --wsl -b <your_bus-id_here>
```
Finally, open WSL and run ```lsusb``` to list all usb devices available to WSL. You should now see your usb device that you have attached to WSL. <br />
<b>NOTE:</b> keep in mind that binding and attaching a usb device to WSL means that windows no longer has access to that device. To give windows access back to that usb, you need to unbind from WSL.

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
