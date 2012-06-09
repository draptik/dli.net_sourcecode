-- Execute this script in the ASP.NET base services database created for the application with aspnet_regsql.exe
-- This script assumes that the name of this database is 'ComplexCommerce'.
-- If not, modify the 'Use' statement below

USE [ComplexCommerce]
GO

CREATE TABLE [dbo].[Product](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[UnitPrice] [money] NOT NULL,
	[Featured] [bit] NOT NULL DEFAULT 0,
	[DiscountedUnitPrice] [money] NULL,
	CONSTRAINT [PK_FeaturedProduct] PRIMARY KEY CLUSTERED
	(
		[ProductId] ASC
	)
)
GO

CREATE TABLE [dbo].[BasketLine](
	[UserId] [uniqueidentifier] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[UtcUpdated] [datetime] NOT NULL DEFAULT GETUTCDATE(),
	CONSTRAINT [PK_BasketLine] PRIMARY KEY CLUSTERED
	(
		[UserId],
		[ProductId]
	),
	FOREIGN KEY ([ProductId]) REFERENCES [Product]([ProductId]),
	FOREIGN KEY ([UserId]) REFERENCES [aspnet_Users]([UserId])
)
GO

CREATE TABLE [dbo].[ExchangeRate](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CurrencyCode] [nvarchar](3) UNIQUE NOT NULL,
	[Rate] [money] NOT NULL,
	CONSTRAINT [PK_ExchangeRate] PRIMARY KEY CLUSTERED
	(
		[Id]
	)
)
GO

CREATE TABLE [dbo].[AuditEvent](
	[Id] [int] IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	[Time] [datetime] NOT NULL DEFAULT GETUTCDATE(),
	[User] [nvarchar](256) NOT NULL DEFAULT CURRENT_USER
)
GO

CREATE TABLE [dbo].[AuditProductDeleted](
	[Id] [int] IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	[AuditEventId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	FOREIGN KEY ([AuditEventId]) REFERENCES [AuditEvent]([Id])
)
GO

CREATE TABLE [dbo].[AuditProductInserted](
	[Id] [int] IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	[AuditEventId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[UnitPrice] [money] NOT NULL,
	FOREIGN KEY ([AuditEventId]) REFERENCES [AuditEvent]([Id])
)
GO

CREATE TABLE [dbo].[AuditProductUpdated](
	[Id] [int] IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	[AuditEventId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[UnitPrice] [money] NOT NULL,
	FOREIGN KEY ([AuditEventId]) REFERENCES [AuditEvent]([Id])
)
GO