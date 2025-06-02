# StoreSample

## Running with Docker Compose

You can find the relevant `docker-compose` files in the `build` directory.

### Start all services

To start the application, databases (PostgreSQL, Redis, RabbitMQ), and the observability suite (OpenTelemetry Collector, OpenObserve), run the following command in the `build` directory:

```bash
docker-compose -f docker-compose.yml -f docker-compose.db.yml -f docker-compose.ob.yml up -d