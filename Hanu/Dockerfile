FROM arm64v8/ubuntu:latest AS build

RUN apt-get update -qq \
    && apt-get install -y g++ \
    && apt-get install -y libcurl4-gnutls-dev

WORKDIR /opt/app
COPY *.cpp .
COPY *.hpp .

RUN g++ main.cpp -std=c++20 -o main -lcurl

FROM arm64v8/ubuntu:latest

RUN apt-get update -qq \
    && apt-get install -y libcurl4-gnutls-dev \
    && apt-get install -y g++


COPY --from=build /opt/app/main /usr/local/bin

ENTRYPOINT [ "main" ]
