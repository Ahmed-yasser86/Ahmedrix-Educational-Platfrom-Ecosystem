IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'aspnet-OnlineCoursesPlatform')
BEGIN
    CREATE DATABASE [aspnet-OnlineCoursesPlatform] 
    ON (FILENAME = '/var/opt/mssql/backup/aspnet-OnlineCoursesPlatform.mdf'), 
       (FILENAME = '/var/opt/mssql/backup/aspnet-OnlineCoursesPlatform_log.ldf') 
    FOR ATTACH;
END