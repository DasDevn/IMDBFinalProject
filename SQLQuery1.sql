RESTORE FILELISTONLY
FROM DISK = 'C:\TestDatabases\IMDB_Project.bak'


RESTORE DATABASE IMDB from DISK = 'C:\TestDatabases\IMDB_Project.bak'
WITH MOVE 'IMDB_Project' TO 'C:\Users\dark_\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\IMDB_Project.mdf',
MOVE 'IMDB_Project_Log' TO 'C:\Users\dark_\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\IMDB_Project_Log.ldf',
RECOVERY, REPLACE, STATS = 10;