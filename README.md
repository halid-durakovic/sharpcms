<img align="left" src="https://avatars0.githubusercontent.com/u/7360948?v=3" />

&nbsp;SharpCms<br /><br />
=============

A simple content management system that lives `outside` of your website with first class REST and Security integration.

##Why was this built?

Professionally I have had many a CMS thrust upon me. Most of them live inside your website, which is good for making 
your CMS portable along with your website but it does bad things like impose requirements which completely take over your 
design. This is why SharpCms was built, because I believe that your CMS should act as a service and not become and inner platform 
that gets in your way.

##Getting Started

To get the tests up and running you will need to complete a few steps. This project uses dotnet core has a sql server dependency. 
Firstly you would need to make sure you have the required software installed. 

###Installing Sql Server

This project uses SQL Server and can be run on Unix or Windows. 

For windows please go [here](https://www.microsoft.com/en-us/sql-server/sql-server-editions-developers).

For ubuntu please go [here](https://docs.microsoft.com/en-us/sql/linux/sql-server-linux-setup-ubuntu).

###Installing Dotnet Core

This project uses dotnet core(v1.1). You will need this to compile the app. 

For windows please go [here](https://www.microsoft.com/net/core#windowsvs2015).

For ubuntu please go [here](https://www.microsoft.com/net/core#linuxredhat).

###Restoring Dependencies

You will need to restore the dependencies before you can do anything. Run the following from the root.

For windows run: `restore`.

For ubuntu: `./restore`

###Building Source

To build the application, there is a handy utility in the root directory.

For windows: `build`

For ubuntu: `./build`

###Authorisation

This application uses IdentityServer(v4). The secrets and default users are generated on the fly using the following commands.

For windows: `auth`

For ubuntu: `./auth`

Please watch the console output carefully as this reveals the identity of the `admin` and `user` for logging into the app.

##Components

SharpCms is broken down into 3 major components. 

 1. Identity Server [http://localhost:5000](http://localhost:5000)
 2. Api Server [http://localhost:5001](http://localhost:5001)
 3. UI Server [http://localhost:5002](http://localhost:5002)

###Identity Server 

You can start the server by running the following commands: 

For windows: `startid`

For ubuntu: `./startid`

###Api server

You can start the server by running the following commands: 

For windows: `startapi`

For ubuntu: `./startapi`

###UI Server 

You can start the server by running the following commands: 

For windows: `startui`

For ubuntu: `./startui`

###Kill All

There is also a handy kill all command: 

For windows: `kill`

For ubuntu: `./kill`

##Security

This project uses (Identity Server)[https://github.com/IdentityServer](v4). A big shout out to these guys. It helped me achieve the 
OAuth2 and OpenIdConnect authorisation flows I needed. 

###REST with Postman

Request Bearer Token using User and Client Credentials.

You need to make a POST request to: http://localhost:5000/connect/token

HEADERS: content-type: application/x-www-form-urlencoded

BODY: 

grant_type=client_credentials

scope=sharpcms-api

client_id=sharpcms-api-client

client_secret=cbeaef20-6d02-4a06-bc1f-7fe821f052ee <-- this will be differnt on your machine

This returns a bearer token. 

Then GET request to resource for example: http://localhost:5001/api/value/secured

HEADERS: 

Content-Type: application/json

Authorization: Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6ImRjN2QzZTY1M2JkOGU5NDBhOTBlYTlkZWEyYWU4OTM5IiwidHlwIjoiSldUIn0.eyJuYmYiOjE0ODUwMTA1NDAsImV4cCI6MTQ4NTAxNDE0MCwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6NTAwMC9yZXNvdXJjZXMiLCJhcGkxIl0sImNsaWVudF9pZCI6ImNsaWVudCIsInNjb3BlIjpbImFwaTEiXX0.jwjKLVKdxRro3cOJdVO9yIaIuEnz81e68HhrTMDccpvzC0_cZju79Vff3b80JiK9ZCYKXzopKeeMknw2_0-qy6pjRYgXagJoNh836Iyg-NsJ8qfwhJBJVC9YaLooMTWRJoziamXn9Nid6MZEYCiGUfD4JHl1Rrkw5JQDo9c9VTnzJ2gDlqnt0jCudfNv4za7r87bEy0Wv0VFXhQAZ8pkYGUStjwN0kL75Q9Xvt4n2kVgG4y011bNqFuA4h13GrAW5VoPNAuy-6RQDwuR8tdzpl1eqrh67Kegyx_5vcoumsND4kiId3_UOKdqdUYOxxS8csfjEfpfHHXoSEYA7ejo1w