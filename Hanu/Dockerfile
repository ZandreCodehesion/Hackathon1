FROM ubuntu:22.04 AS build

RUN apt-get update -qq \
    && apt-get install -y clang \
    && apt-get install -y libcurl4-gnutls-dev


WORKDIR /opt/app
COPY *.cpp .
COPY *.hpp .

RUN clang++ -Ofast -std=c++17 main.cpp -o main -lcurl

FROM ubuntu:22.04
RUN apt-get update -qq \
    && apt-get install -y libcurl4-gnutls-dev

COPY --from=build /opt/app/main /usr/local/bin

ENTRYPOINT [ "main" ]
