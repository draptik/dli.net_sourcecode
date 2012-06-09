-- This script assumes that the name of the sample database is 'ComplexCommerce'.
-- If not, modify the 'Use' statement below

USE ComplexCommerce
GO

INSERT INTO Product ([Name] ,[UnitPrice], [Featured])
VALUES ('Criollo Chocolate', 34.9500, 1)

INSERT INTO Product ([Name] ,[UnitPrice], [Featured], [DiscountedUnitPrice])
VALUES ('Arborio Rice', 22.7500, 1, 19.5)

INSERT INTO Product ([Name] ,[UnitPrice], [Featured])
VALUES ('White Asparagus', 39.8000, 1)

INSERT INTO Product ([Name] ,[UnitPrice], [DiscountedUnitPrice])
VALUES ('Maldon Sea Salt', 19.5000, 15.85)

INSERT INTO Product ([Name] ,[UnitPrice], [Featured])
VALUES ('Gruyère', 48.5000, 1)

INSERT INTO Product ([Name] ,[UnitPrice], [Featured])
VALUES ('Anchovies', 18.7500, 1)

GO

INSERT INTO ExchangeRate ([CurrencyCode], [Rate])
VALUES ('DKK', 1)

INSERT INTO ExchangeRate ([CurrencyCode], [Rate])
VALUES ('USD', 5.16)

INSERT INTO ExchangeRate ([CurrencyCode], [Rate])
VALUES ('EUR', 7.44)

GO