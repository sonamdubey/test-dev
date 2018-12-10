IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dba].[Getdbfilestats]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dba].[Getdbfilestats]
GO

	CREATE procedure [dba].[Getdbfilestats]
as
insert into dba.dbfilestats
SELECT name AS [File Name] , physical_name AS [Physical Name], size/128 AS [Total Size in MB],
size/128.0 - CAST(FILEPROPERTY(name, 'SpaceUsed') AS int)/128.0 AS [Available Space In MB],
GETDATE() as UpdatedOn
FROM sys.database_files;