# API Description

This API building with C# languange , framework .net core, and package from nuget package manager . And Database SQL Server

## Feature
- Register
	- User can be register with `API Spec`
	
	Request :
	- Method : POST
	- Endpoint : `/api/account/register`
	- Header :
		- Content-Type: application/json
	- Body :
	```json 
	{
		"FirstName" : "string",
		"LastName" : "string, null",
		"UserName" : "string,unique",
		"Email" : "string, EmailAddress",
		"Password" : "string",
		"ConfirmPassword": "string"
	}
	```
	
	Response : 
	```json 
	{
		"status" : "bool",
		"message" : "string",
		"data" : {
			 "FirstName" : "string",
			 "LastName" : "string, null",
			 "UserName" : "string,unique",
			 "Email" : "string, EmailAddress"
		 }
	}
	```
- Login (Sign In)
	

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