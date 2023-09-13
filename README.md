# VR_Robotics
 
## Running the project on CEC1081
Always make sure Oculus is launched on the computer to ensure a wired connection. <br />
Upon opening the project, go to Edit -> Project Settings -> XR Plug-in Management -> OpenXR -> Play Mode OpenXR Runtime and within that dropbox make sure that "Oculus" is selected. <br />
Now the project should run correctly and launch on the headset.

## Setting up firebase for remote repositories
### google-services.json
For security reasons, the google-services.json file has been ignored. <br />
When running on a new remote repository, please download the "google-serivces.json" file on the VRAR-Robotics firebase project linked in the Trello. <br />
Once downloaded ensure that it is named <i>google-services.json</i> and then, in Assets, if there is not already a folder within Assets named "StreamingAssets", create that directory and put the google-services.json within it. <br />
### FirebaseCppApp-11_4_0.bundle
A firebase file has been left out of the project due to it's size, this file is named FirebaseCppApp-11_4_0.bundle. <br />
This file can be found on CEC1081 on Desktop/FIREBASE. <br />
Put this file in the following directory: Create-with-VR_2021LTS\HCITECH-Robotics_Project\Assets\Firebase\Plugins\x86_64 <br />
