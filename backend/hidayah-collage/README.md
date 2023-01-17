# API Description

This API building with C# languange , framework .net core, and package from nuget package manager . And Database SQL Server

## Feature
- Register
	- User can be register this application with `API Spec`
	
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
	- This function you can be login with your username or email address

	Request :
	- Method : POST
	- Endpoint : `/api/account/login`
	- Header :
		- Content-Type: application/json
	- Body :
	
	```json 
	{
		"Email" : "string, EmailAddress",
		"Password" : "string"
	}
	```
	
	Response : 
	
	```json 
	{
		"status" : "bool",
		"message" : "string",
		"data" : {
			 "refreshToken" : "string",
			 "firstName" : "string",
			 "lastName" : "string",
			 "username" : "string",
			 "token" : "string",
			 "expireDate" : "Date"
		 }
	}
	```
	
- Confirm Email
	- After user register, system will be send to your email for confirmation. no need api spec
	
- Forget Password
	- This function use, when you forget your password, system will be send email to your email address
	
	Request :
	- Method : POST
	- Endpoint : `/api/account/forgotpassword`
	- Header :
		- Content-Type: application/json
	- Body :
	
	```json 
	{
		"Email" : "string, EmailAddress"
	}
	```
	
	Response : 
	
	```json 
	{
		"status" : "bool",
		"message" : "string",
		"data" : {
			 "status" : "bool",
			 "body" : "string"
		 }
	}
	```
	
- Reset Password (Set Password after you forget email)
	
	Request :
	- Method : POST
	- Endpoint : `/api/account/resetpassword`
	- Form Param :
		- email : string
		- token : string
		- password : string 
		- newpassword : string
		
	Response :
	
	```json 
	{
		"status" : "bool",
		"message" : "string",
		"data" : {
			 "email" : "string",
		 }
	}
	```
	
- Refresh Token
	- This function for generate new token when you call api with authentication
	
	Request :
	- Method : POST
	- Endpoint : `/api/account/refresh`
	- Header :
		- Content-Type: application/json
	- Body :
		
	```json 
	{
		"RefreshToken" : "string"
	}
	```
		
	Response :
	
	```json 
	{
		"status" : "bool",
		"message" : "string",
		"data" : {
			 "refreshToken" : "string",
			 "firstName" : "string",
			 "lastName" : "string",
			 "username" : "string",
			 "token" : "string",
			 "expireDate" : "Date"
		 }
	}
	```
	
- Logout

	Request :
	- Method : DELETE
	- Endpoint : `/api/account/logout`
	- Authorization
		- Bearer Token : "your secret api key"
		
	Response :
	
	```json 
	{
		"status" : "bool",
		"message" : "string",
		"data" : null
	}
	```
	
	
	

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
    "MSG_TEXT" : "string"
}
```

Response :

```json 
{
    "status" : "bool",
    "message" : "string",
    "data" : {
         "msG_CD" : "string",
	 "msG_TEXT" : "string"
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
	 "MSG_TEXT" : "string"
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
	 "MSG_TEXT" : "string"
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
