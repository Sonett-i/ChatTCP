ChatTCP Readme.txt

Author: Sam C
Date: May 15, 2024
Version: 2.0.0

==Notes==
Source code is located in folders named \src\
Binary executables are located in folders named \bin\

==Requirements==

MySQL 8.0
.net 6.0


==Database Setup==

Server requires user: 
username: ChatTCP
password: none

For security, only permit connections from localhost.

In MySQL Workbench:
1. Database User Setup
	1.1 Create Database user with username: ChatTCP, with no password. Limit connections to localhost
	2. Make sure it has access to schema privileges SELECT, INSERT, UPDATE, DELETE.
2. Database Setup
	2.1 Run the MySQL\Create.sql file, and ensure no errors.




==Running Server==

run \server\bin\ChatTCP_Console.exe

Common errors include:
	Invalid Database Configuration: Read Supplied Exception Errors
		If database user fails to authenticate, error should read something like: unable to authenticate (using password: true/false) etc. Consult MySQL Workbench->Server->Users and Privileges and ensure user has required permissions.


==Running Client==
run \client\bin\ChatTCP_Client.exe