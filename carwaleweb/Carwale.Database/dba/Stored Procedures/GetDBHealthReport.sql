IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dba].[GetDBHealthReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dba].[GetDBHealthReport]
GO

	CREATE PROCEDURE [dba].[GetDBHealthReport]
AS
BEGIN
    exec [dba].[BlockedQueries]
    
    exec [dba].[GetActiveDBConnections]
    
    exec [dba].[GetBlockingStmt]
    
    exec [dba].[Getdbfilestats]
    
    exec [dba].[GetQueryLogs]
    
    exec [dba].[getTopCPUIntensivestmt]
    
    exec [dba].[GetTopExecutedstmt]
    
    exec [dba].[GetTopIOIntensivestmt]
    
END