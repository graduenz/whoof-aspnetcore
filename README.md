whoof-aspnetcore
---

## Description

Implementation of [Whoof API](https://gui.rdnz.dev/_/labs/whoof-api) using ASP.NET Core.

## Swagger

https://whoof-api.cr.rdnz.dev/swagger/index.html

## Features

This project makes use of all the following things:

### Concepts and design practices
- Clean architecture design
- CQRS
- PaaS
- Integration tests (+ AAA pattern)
- CI/CD

### Tools
- Auth0
- Swagger
- PostgreSQL
- Loki
- Grafana
- CapRover
- GitHub Actions
- SonarCloud

### Libraries
- Entity Framework Core
- Swashbuckle
- MediatR
- FluentValidation
- AutoMapper
- xUnit

### Cloud services
- **AWS RDS:** managed PostgreSQL instance
- **AWS EC2:** to run Docker and CapRover
- **Grafana Cloud:** free tier includes Grafana, Loki, Prometheus and more with good limits

## Milestones

This is a simple todo list only to remember what needs to be done.

- [x] Create all endpoints
- [x] PostgreSQL with Entity Framework Core
- [x] Swagger
- [x] Modular refactoring
- [x] Adopt more scalable design practices
- [x] Auth0
- [ ] Implement integration tests
- [ ] Adapt [ASP.NET Core Integration Tests](https://gui.rdnz.dev/_/.net-engineering/asp.net-core-integration-tests) page to project changes
- [x] SonarCloud
- [x] CI with GitHub Actions
- [x] ~~CD with GitHub Actions~~ CD with CapRover's webhook
- [x] Logging
- [x] Launch