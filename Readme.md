# Codehesion Hackathon 1

## requirements
- node
- C compiler
- docker

1. You can create a new folder with your name, and create your project inside there.
1. The project should have a Dockerfile that can run your project
1. Your project should execute your code in a while loop that terminates after 5 seconds and prints out your response in the format: {total number of parses};{time taken in seconds};
    -   for example `6969;5.00023;`
1. once your output is correct you can verify that your code works correctly by using the node program tools.
1. to simplify this there is a makefile to handle this.
    - you can just run `make` and this will benchmark all the projects with a dockerfile and collect the data
    - you can pass in the parameter `DIRECTORY` eg `make DIRECTORY=Zandre`
    - for interpreted languages you might get some better performance with Unconfined mode eg: `make UNCONFINED=1`