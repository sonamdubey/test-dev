IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dba].[GetActiveDBConnections]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dba].[GetActiveDBConnections]
GO

	CREATE PROCEDURE dba.GetActiveDBConnections
as
INSERT INTO dba.ActiveDBConnections
SELECT 
    DB_NAME(dbid) as DBName, 
    COUNT(dbid) as NumberOfConnections,
    GETDATE() as UpdatedOn
FROM
    sys.sysprocesses
WHERE 
    dbid > 0
GROUP BY 
    dbid


