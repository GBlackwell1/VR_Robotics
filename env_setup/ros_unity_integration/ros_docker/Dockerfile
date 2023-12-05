FROM ros:noetic-ros-base

ENV ROS_WORKSPACE=catkin_ws
ENV DEBIAN_FRONTEND="noninteractive"

# Copy packages for unity into the workspace
COPY ./ros_packages/ $ROS_WORKSPACE/src/

# Update packages, install git, apt-utils and python3.8 AND python2.7
RUN apt-get update && apt-get install -y git && apt-get install -y dialog apt-utils 
RUN apt-get install python3 -y && apt-get install python -y
# Install ros tcp endpoints from the web via github, also install kinova's noetic-development branch for their api
RUN git clone https://github.com/Unity-Technologies/ROS-TCP-Endpoint $ROS_WORKSPACE/src/ros_tcp_endpoint -b v0.7.0
RUN git clone https://github.com/Kinovarobotics/kinova-ros $ROS_WORKSPACE/src/kinova-ros -b noetic-devel
# cd into the directory and install appropriate move-it and tf dependiencies for kinova's api
RUN cd $ROS_WORKSPACE && rosdep install --from-paths src --ignore-src -r -y

COPY ./ros_docker/set-up-workspace /setup.sh
#COPY docker/tutorial /

RUN chmod +x /setup.sh && /setup.sh && rm /setup.sh

WORKDIR $ROS_WORKSPACE

# Source the workspace on sign in
RUN echo ". devel/setup.bash" >> ~/.bashrc

# making sure the file modes are executable
RUN chmod +x src/ros_tcp_endpoint/src/ros_tcp_endpoint/*.py

#ENTRYPOINT ["/tutorial"]

