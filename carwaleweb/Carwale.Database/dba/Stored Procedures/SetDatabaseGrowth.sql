IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dba].[SetDatabaseGrowth]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dba].[SetDatabaseGrowth]
GO

	
-- =============================================  
-- Author:  Manish  
-- Create date: 06-March-2013 
-- Details: SP will insert the logs of all the table in CarWale_com database into table TablesRecordCountLog
-- This SP is executing through Batch Job.
-- =============================================  

CREATE PROCEDURE  [dba].[SetDatabaseGrowth]
AS
		BEGIN
			CREATE TABLE #tempsize
			(TableName VARCHAR(256),
			Rows int,
			Reserved VARCHAR(256),
			Data VARCHAR(256),
			IndexSize VARCHAR(256),
			UnUsed VARCHAR(256)
			)

			INSERT INTO #tempsize
			EXEC sp_msforeachtable 'exec sp_spaceused ''?'''

			INSERT INTO dba.DatabaseGrowth
			SELECT *,GETDATE() FROM #tempsize

			DROP TABLE #tempsize

      END
