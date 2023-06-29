# Base image
FROM gcc:latest

# Set the working directory inside the container
WORKDIR /app

# Copy the C++ source code to the container
COPY . /app

# Compile the C++ program
RUN g++ -o program main.cpp

# Set the entry point
ENTRYPOINT ["./program"]
