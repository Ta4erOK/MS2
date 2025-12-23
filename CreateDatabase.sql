-- Скрипт для создания базы данных и таблиц для приложения "Магазин продажи мотоциклов"

-- Создание базы данных
USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'MotorcycleShopDB')
BEGIN
    CREATE DATABASE [MotorcycleShopDB];
END
GO

USE [MotorcycleShopDB];
GO

-- Создание таблицы Motorcycles
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Motorcycles')
BEGIN
    CREATE TABLE [Motorcycles] (
        [Id] int IDENTITY(1,1) PRIMARY KEY,
        [Brand] nvarchar(100) NOT NULL,
        [Model] nvarchar(100) NOT NULL,
        [Year] int NOT NULL,
        [Color] nvarchar(50) NOT NULL,
        [EngineVolume] decimal(10,2) NOT NULL,
        [Mileage] int NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [Description] nvarchar(1000) NULL,
        [ImageUrl] nvarchar(500) NULL,
        [InStock] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL
    );
END
GO

-- Создание таблицы Orders
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Orders')
BEGIN
    CREATE TABLE [Orders] (
        [Id] int IDENTITY(1,1) PRIMARY KEY,
        [OrderNumber] nvarchar(50) NOT NULL,
        [CustomerName] nvarchar(100) NOT NULL,
        [CustomerEmail] nvarchar(100) NOT NULL,
        [CustomerPhone] nvarchar(20) NOT NULL,
        [DeliveryAddress] nvarchar(500) NOT NULL,
        [OrderDate] datetime2 NOT NULL,
        [TotalAmount] decimal(18,2) NOT NULL,
        [Status] nvarchar(50) NOT NULL,
        [Comments] nvarchar(1000) NULL
    );
END
GO

-- Создание таблицы OrderItems
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OrderItems')
BEGIN
    CREATE TABLE [OrderItems] (
        [Id] int IDENTITY(1,1) PRIMARY KEY,
        [OrderId] int NOT NULL,
        [MotorcycleId] int NOT NULL,
        [Quantity] int NOT NULL,
        [UnitPrice] decimal(18,2) NOT NULL
    );
END
GO

-- Создание таблицы Payments
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Payments')
BEGIN
    CREATE TABLE [Payments] (
        [Id] int IDENTITY(1,1) PRIMARY KEY,
        [OrderId] int NOT NULL,
        [PaymentDate] datetime2 NOT NULL,
        [Amount] decimal(18,2) NOT NULL,
        [PaymentMethod] nvarchar(50) NOT NULL,
        [CardLastFour] nvarchar(4) NOT NULL,
        [TransactionId] nvarchar(100) NULL,
        [Status] nvarchar(50) NOT NULL
    );
END
GO

-- Создание таблицы ShoppingCarts
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ShoppingCarts')
BEGIN
    CREATE TABLE [ShoppingCarts] (
        [Id] int IDENTITY(1,1) PRIMARY KEY,
        [SessionId] nvarchar(100) NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        [LastModified] datetime2 NOT NULL,
        [TotalAmount] decimal(18,2) NOT NULL
    );
END
GO

-- Создание таблицы ShoppingCartItems
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ShoppingCartItems')
BEGIN
    CREATE TABLE [ShoppingCartItems] (
        [Id] int IDENTITY(1,1) PRIMARY KEY,
        [ShoppingCartId] int NOT NULL,
        [MotorcycleId] int NOT NULL,
        [Quantity] int NOT NULL,
        [AddedDate] datetime2 NOT NULL
    );
END
GO

-- Создание внешних ключей
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_OrderItems_Orders')
BEGIN
    ALTER TABLE [OrderItems] 
    ADD CONSTRAINT [FK_OrderItems_Orders] 
    FOREIGN KEY ([OrderId]) REFERENCES [Orders]([Id]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_OrderItems_Motorcycles')
BEGIN
    ALTER TABLE [OrderItems] 
    ADD CONSTRAINT [FK_OrderItems_Motorcycles] 
    FOREIGN KEY ([MotorcycleId]) REFERENCES [Motorcycles]([Id]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Payments_Orders')
BEGIN
    ALTER TABLE [Payments] 
    ADD CONSTRAINT [FK_Payments_Orders] 
    FOREIGN KEY ([OrderId]) REFERENCES [Orders]([Id]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ShoppingCartItems_Motorcycles')
BEGIN
    ALTER TABLE [ShoppingCartItems] 
    ADD CONSTRAINT [FK_ShoppingCartItems_Motorcycles] 
    FOREIGN KEY ([MotorcycleId]) REFERENCES [Motorcycles]([Id]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ShoppingCartItems_ShoppingCarts')
BEGIN
    ALTER TABLE [ShoppingCartItems] 
    ADD CONSTRAINT [FK_ShoppingCartItems_ShoppingCarts] 
    FOREIGN KEY ([ShoppingCartId]) REFERENCES [ShoppingCarts]([Id]);
END
GO

-- Заполнение таблицы Motorcycles тестовыми данными
IF NOT EXISTS (SELECT * FROM [Motorcycles])
BEGIN
    INSERT INTO [Motorcycles] ([Brand], [Model], [Year], [Color], [EngineVolume], [Mileage], [Price], [Description], [ImageUrl], [InStock], [CreatedAt])
    VALUES 
    ('Yamaha', 'YZF-R1', 2022, 'Red', 998.00, 1500, 1500000.00, 'Спортивный мотоцикл Yamaha YZF-R1 2022 года', '/Images/yamaha-r1.jpg', 1, GETDATE()),
    ('Honda', 'CBR600RR', 2021, 'White', 599.00, 3200, 1200000.00, 'Средний спортивный мотоцикл Honda CBR600RR', '/Images/honda-cbr600.jpg', 1, GETDATE()),
    ('Kawasaki', 'Ninja ZX-14R', 2020, 'Green', 1441.00, 4500, 1800000.00, 'Туринговый мотоцикл Kawasaki Ninja ZX-14R', '/Images/kawasaki-zx14r.jpg', 0, GETDATE()),
    ('Suzuki', 'GSX-R750', 2023, 'Blue', 750.00, 800, 1350000.00, 'Спортивный мотоцикл Suzuki GSX-R750', '/Images/suzuki-gsxr750.jpg', 1, GETDATE()),
    ('BMW', 'S1000RR', 2022, 'Light Blue', 999.00, 2100, 2000000.00, 'Спортивный мотоцикл BMW S1000RR', '/Images/bmw-s1000rr.jpg', 1, GETDATE());
END
GO

-- Заполнение таблицы Orders тестовыми данными
IF NOT EXISTS (SELECT * FROM [Orders])
BEGIN
    INSERT INTO [Orders] ([OrderNumber], [CustomerName], [CustomerEmail], [CustomerPhone], [DeliveryAddress], [OrderDate], [TotalAmount], [Status], [Comments])
    VALUES 
    ('ORD-20251223-1001', 'Иванов Иван Иванович', 'ivanov@example.com', '+79001234567', 'г. Москва, ул. Примерная, д. 1, кв. 1', GETDATE(), 1500000.00, 'Ожидает оплаты', 'Заказ срочный'),
    ('ORD-20251223-1002', 'Петров Петр Петрович', 'petrov@example.com', '+79007654321', 'г. Санкт-Петербург, ул. Тестовая, д. 2, кв. 2', GETDATE(), 2000000.00, 'Оплачен', 'Забрать с собой');
END
GO

-- Заполнение таблицы OrderItems тестовыми данными
IF NOT EXISTS (SELECT * FROM [OrderItems])
BEGIN
    INSERT INTO [OrderItems] ([OrderId], [MotorcycleId], [Quantity], [UnitPrice])
    VALUES 
    (1, 1, 1, 1500000.00),
    (2, 5, 1, 2000000.00);
END
GO

-- Заполнение таблицы Payments тестовыми данными
IF NOT EXISTS (SELECT * FROM [Payments])
BEGIN
    INSERT INTO [Payments] ([OrderId], [PaymentDate], [Amount], [PaymentMethod], [CardLastFour], [TransactionId], [Status])
    VALUES 
    (2, GETDATE(), 2000000.00, 'Карта', '4567', 'TXN-20251223-001', 'Успешно');
END
GO

-- Заполнение таблицы ShoppingCarts тестовыми данными
IF NOT EXISTS (SELECT * FROM [ShoppingCarts])
BEGIN
    INSERT INTO [ShoppingCarts] ([SessionId], [CreatedDate], [LastModified], [TotalAmount])
    VALUES 
    ('SESSION-001', GETDATE(), GETDATE(), 1500000.00);
END
GO

-- Заполнение таблицы ShoppingCartItems тестовыми данными
IF NOT EXISTS (SELECT * FROM [ShoppingCartItems])
BEGIN
    INSERT INTO [ShoppingCartItems] ([ShoppingCartId], [MotorcycleId], [Quantity], [AddedDate])
    VALUES 
    (1, 1, 1, GETDATE());
END
GO

PRINT 'База данных MotorcycleShopDB и все таблицы успешно созданы и заполнены тестовыми данными';