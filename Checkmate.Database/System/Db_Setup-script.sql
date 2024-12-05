-- NEW admin account
USE [master]
GO
CREATE LOGIN [adminminion] WITH PASSWORD=N'sbQq-FE!Cqgy.8tD.4NrGfNQ', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO
ALTER SERVER ROLE [sysadmin] ADD MEMBER [adminminion]
GO

-- DISABLE SA account
USE [master]
GO
DENY CONNECT SQL TO [sa]
GO
ALTER LOGIN [sa] DISABLE
GO

-- CREATE database
CREATE DATABASE checkmate;
GO

USE checkmate;
GO