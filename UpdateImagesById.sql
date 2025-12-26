-- Скрипт для обновления таблицы Motorcycles с правильными путями к изображениям
-- Используйте этот скрипт для обновления базы данных после запуска приложения и создания таблиц

-- Обновляем изображения мотоциклов с правильными путями
UPDATE [Motorcycles] SET [ImageUrl] = '/Images/yamaha-r1.png' WHERE [Id] = 1;
UPDATE [Motorcycles] SET [ImageUrl] = '/Images/honda-cbr600.png' WHERE [Id] = 2;
UPDATE [Motorcycles] SET [ImageUrl] = '/Images/kawasaki-zx14r.png' WHERE [Id] = 3;
UPDATE [Motorcycles] SET [ImageUrl] = '/Images/suzuki-gsxr750.png' WHERE [Id] = 4;
UPDATE [Motorcycles] SET [ImageUrl] = '/Images/bmw-s1000rr.png' WHERE [Id] = 5;
UPDATE [Motorcycles] SET [ImageUrl] = '/Images/ducati-panigale.png' WHERE [Id] = 6;
UPDATE [Motorcycles] SET [ImageUrl] = '/Images/harley-street750.png' WHERE [Id] = 7;
UPDATE [Motorcycles] SET [ImageUrl] = '/Images/ktm-superduke.png' WHERE [Id] = 8;
UPDATE [Motorcycles] SET [ImageUrl] = '/Images/aprilia-rsv4.png' WHERE [Id] = 9;
UPDATE [Motorcycles] SET [ImageUrl] = '/Images/triumph-street.png' WHERE [Id] = 10;
UPDATE [Motorcycles] SET [ImageUrl] = '/Images/suzuki-vstrom.png' WHERE [Id] = 11;
UPDATE [Motorcycles] SET [ImageUrl] = '/Images/yamaha-mt07.png' WHERE [Id] = 12;
UPDATE [Motorcycles] SET [ImageUrl] = '/Images/honda-goldwing.png' WHERE [Id] = 13;
UPDATE [Motorcycles] SET [ImageUrl] = '/Images/kawasaki-z900.png' WHERE [Id] = 14;
UPDATE [Motorcycles] SET [ImageUrl] = '/Images/bmw-r1250gs.jpg' WHERE [Id] = 15;