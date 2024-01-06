[Inglês ](#English) | [Portugues](#Portugues)
## English Section

### CreateUser

This method allows the creation of a new user.

#### Endpoint:

`POST /CreateUser`

#### Parameters:

- `user` (request body): An object representing the user information to be created.

#### Responses:

- `201 Created`: The user was created successfully.
- `409 Conflict`: The user already exists.
- `500 Internal Server Error`: Server internal error.

Example request:

```json
{
  "UserName": "exampleUser",
  "Password": "examplePassword",
  "Email": "user@example.com"
}
```

### GetAllUsers

This method returns the list of all registered users.

#### Endpoint:

`GET /getAllUser`

#### Responses:

- `200 OK`: The list of users was successfully retrieved.
- `500 Internal Server Error`: Server internal error.

### Auth

This method authenticates an existing user.

#### Endpoint:

`POST /Auth`

#### Parameters:

- `user` (request body): An object representing the user information to be authenticated.

#### Responses:

- `200 OK`: Authentication was successful.
- `400 Bad Request`: User not found.
- `500 Internal Server Error`: Server internal error.

Example request:

```json
{
  "UserName": "exampleUser",
  "Password": "examplePassword"
}
```

### Seção em Português

### CreateUser

Este método permite a criação de um novo usuário.

#### Endpoint:

`POST /CreateUser`

#### Parâmetros:

- `user` (corpo da solicitação): Um objeto representando as informações do usuário a ser criado.

#### Respostas:

- `201 Created`: O usuário foi criado com sucesso.
- `409 Conflict`: O usuário já existe.
- `500 Internal Server Error`: Erro interno do servidor.

Exemplo de solicitação:

```json
{
  "UserName": "exampleUser",
  "Password": "examplePassword",
  "Email": "user@example.com"
}
```

### GetAllUsers

Este método retorna a lista de todos os usuários cadastrados.

#### Endpoint:

`GET /getAllUser`

#### Respostas:

- `200 OK`: A lista de usuários foi recuperada com sucesso.
- `500 Internal Server Error`: Erro interno do servidor.

### Auth

Este método autentica um usuário existente.

#### Endpoint:

`POST /Auth`

#### Parâmetros:

- `user` (corpo da solicitação): Um objeto representando as informações do usuário a ser autenticado.

#### Respostas:

- `200 OK`: A autenticação foi bem-sucedida.
- `400 Bad Request`: O usuário não foi encontrado.
- `500 Internal Server Error`: Erro interno do servidor.

Exemplo de solicitação:

```json
{
  "UserName": "exampleUser",
  "Password": "examplePassword"
}
```

---

