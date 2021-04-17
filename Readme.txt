# Producer

Producer is an application created for demo purposes for pubsub pattern implementations. 

## Prerequisites

RabbitMq,
Jeager,
ELK
In project you will find DockerCompose.yml if you go to the project path and run the $Dockercompose up command those three 
component will be installed directly to your computer.

## Installed Components
MassTransit.RabbitMQ, for enterprise service bus;
RabbitMQ.Client, for queue mechanism;
Serilog.AspNetCore, for logging mechanism;
Swashbuckle.AspNetCore, for swagger implementation;
Jaeger.Core, for jeager;
Nest, for elesticsearch implementation;
xunit, for test project;

## Usage

Producer program is the source of the application it is publishing messages. 
Producer creates ModelInfra:ProductMessage exchanges and subsribers subcribe this exchance. 
Consumer1 and consumer2 is subscribers of the Producers both of them listening same 
producer and both of them have error handling and retry mechanisms. Consumer1 push the consumed data to the 
eleastic and consumer1s controller show the data.
You can fallow all steps through below links;
Rabbit Mq: http://localhost:15672/
ELK: http://localhost:5601
Jeager: http://localhost:16686

## License
MIT