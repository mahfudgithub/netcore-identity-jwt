# API Spec

## Authentication

All API must use this authentication

Request :
- Authorization :
    - Bearer Token : "your secret api key"

## Create Message

Request :
- Method : POST
- Endpoint : `/api/message`
- Header :
    - Content-Type: application/json
    - Accept: application/json
- Body :

```json 
{
    "MSG_CD" : "string, unique",
    "MSG_TEXT" : "string",
}
```

Response :

```json 
{
    "status" : "bool",
    "message" : "string",
    "data" : {
         "MSG_CD" : "string",
		 "MSG_TEXT" : "string",
     }
}
```

## Get Message

Request :
- Method : GET
- Endpoint : `/api/message/{code}`
- Header :
    - Accept: application/json

Response :

```json 
{
    "status" : "bool",
    "message" : "string",
    "data" : {
         "MSG_CD" : "string",
		 "MSG_TEXT" : "string",
     }
}
```

## Update Message

Request :
- Method : PUT
- Endpoint : `/api/message/{code}`
- Header :
    - Content-Type: application/json
    - Accept: application/json
- Body :

```json 
{
    "MSG_TEXT" : "string"
}
```

Response :

```json 
{
    "status" : "bool",
    "message" : "string",
    "data" : {
         "MSG_CD" : "string",
		 "MSG_TEXT" : "string",
     }
}
```

## List Message

Request :
- Method : GET
- Endpoint : `/api/message`
- Header :
    - Accept: application/json
- Query Param :
    - size : number,
    - page : number

Response :

```json 
{
    "status" : "bool",
    "message" : "string",
    "data" : [
        {
             "MSG_CD" : "string",
			 "MSG_TEXT" : "string"
        },
        {
             "MSG_CD" : "string",
			 "MSG_TEXT" : "string"
        },
		{
             "MSG_CD" : "string",
			 "MSG_TEXT" : "string"
        }
    ]
}
```

## Delete Message

Request :
- Method : DELETE
- Endpoint : `/api/message/{code}`
- Header :
    - Accept: application/json

Response :

```json 
{
    "status" : "bool",
    "message" : "string",
    "data" : null
}
```