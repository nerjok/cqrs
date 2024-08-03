docker-compose up -d

to enter to docker image:
docker exec -it <kafka-container-id> /bin/sh

find / -name kafka-topics.sh


create kafka topic 
sudo docker exec -it eeec5e2da6d0 /opt/bitnami/kafka/bin/kafka-topics.sh --create --bootstrap-server localhost:9092 --replicat
ion-factor 1 --partitions 1 --topic my-topic

consume messages:
docker exec -it eeec5e2da6d0 /opt/bitnami/kafka/bin/kafka-console-consumer.sh --bootstrap-server localhost:9092 --topic my-topic --from-beginning

produce:

docker exec -it eeec5e2da6d0 /opt/bitnamikafka/bin/kafka-console-producer.sh --broker-list localhost:9092 --topic my-topic
