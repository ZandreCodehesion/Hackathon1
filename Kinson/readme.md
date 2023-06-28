docker build -t node-1 .
docker run node-1
docker start -a -i `docker ps -q -l`
