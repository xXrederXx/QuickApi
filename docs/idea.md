# Quick API

## Vision

Quick API should provide an easy and fast to write language. It should then create an api based on the user specs. It should be customizable and easy to use.

## Getting Started

Create a new project:

```bash
qapi new MyApi
```

Run the API:

```bash
qapi run
```

## Project Structure

```text
MyApi
├── MyApi.qaproj
├── entities/
│   ├── User.qa
│   └── Weather.qa
├── endpoints/
│   ├── Users.qa
│   └── Weather.qa
└── data/
```

Projects are organized into dedicated folders:

- `entities/` contains all entity definitions.
- `endpoints/` contains all endpoint and resource definitions.
- `data/` contains application data when using the JSON provider.

Quick API automatically loads all `.qa` files from the `entities` and `endpoints` folders.

## Project File

The project file contains global configuration.

**MyApi.qaproj**

```toml
name = "MyApi"
port = 8080

[database]
provider = "json"

[auth]
enabled = true
```

## Entities

Entities define data models used by endpoints.

Entity definitions are stored inside the `entities/` folder, typically one entity per file.

**entities/Weather.qa**

```text
entity Weather
    date: date
    temp: float
    description: string
```

**entities/User.qa**

```text
entity User
    id: int
    username: string
    email: string
```

## Endpoints

Endpoints define routes and their behavior.

Endpoint definitions are stored inside the `endpoints/` folder.

### Basic Endpoint

```text
GET /weather/today
    returns Weather
```

## Resources

Resources automatically generate CRUD endpoints.

Resources are also defined inside the `endpoints/` folder.

```text
resource User
```

Equivalent to:

```text
GET    /users
GET    /users/{id}
POST   /users
PUT    /users/{id}
DELETE /users/{id}
```

## Authentication

Authentication can be enabled per endpoint.

```text
GET /profile
    auth
    returns User
```

Role-based access:

```text
GET /admin/users
    role admin
    returns User[]
```

## Validation

Validation rules can be attached to entity properties.

```text
entity User
    username: string required
    email: string required
    age: int min(18)
```
