# AltenTest

Post-Covid scenario:
People are now free to travel everywhere but because of the pandemic, a lot of hotels went
bankrupt. Some former famous travel places are left with only one hotel.
You’ve been given the responsibility to develop a booking API for the very last hotel in Cancun.
The requirements are:

- API will be maintained by the hotel’s IT department.
- As it’s the very last hotel, the quality of service must be 99.99 to 100% => no downtime
- For the purpose of the test, we assume the hotel has only one room available
- To give a chance to everyone to book the room, the stay can’t be longer than 3 days and
can’t be reserved more than 30 days in advance.
- All reservations start at least the next day of booking,
- To simplify the use case, a “DAY’ in the hotel room starts from 00:00 to 23:59:59.
- Every end-user can check the room availability, place a reservation, cancel it or modify it.
- To simplify the API is insecure.

Instructions :

- Pas de limite de temps (très bien fait il faut au moins 3 à 4 soirées)
- Le minimum requis est un README et du code.
- Tous les shortcuts pour gagner du temps sont autorisés dans la mesure où c’est
documenté. Tout shortcut non expliqué doit etre consideré comme une erreur. On
pourrait accepter un rendu avec 3 lignes de code si elles ont du sens et que tout le
raisonnement et les problèmatiques à prendre en compte sont decrites.

## ABOUT THE PROJECT

This project is running on a docker composer containing:

- 1 server for a sql server database
- 2 servers running the .Net 6 API
- 1 server running nginx reverse proxy to serve as a load balancer

To meet the requirement of 99.99% of availability I exemplified the creation of the project using the load balancer between the APIs. As I understand that the DB server is the weak point of this structure, in a real life scenario I would suggest 2 db servers running in cluster.

Also, on a real life scenario, this environment should be maintained via an orchestrator such as Kubernetes or Docker Swarm.

As for the API, the data is validated using FluentValidation. The API is documented using Swagger.

As a shortcut, when handeling the dates I did not treat different timezones.

## DOCKER IMAGES USED

Pulling the docker images before executing the application is optional, but results in a faster first build.

``` command
docker pull nginx:alpine
docker pull mcr.microsoft.com/mssql/server:2019-latest
docker pull mcr.microsoft.com/dotnet/aspnet:6.0
```

## RUNNING THE APPLICATION

From a terminal at the repository root folder, run following command to start the application.

``` command
docker-compose up
```

The application should run on [http:/localhost:8000/](http:/localhost:8000/). The Swagger documentation is on [http:/localhost:8000/swagger](http:/localhost:8000/swagger).

To end the application you can press `Ctrl+C` on the terminal where the application is running, or from another terminal in the same folder, run the following command.

``` command
docker-compose down
```

## TESTING THE APPLICATION

Beside the Swagger UI, on the repository root folder I exported a Thunder Client collection with all methods to facilitate the method execution.
