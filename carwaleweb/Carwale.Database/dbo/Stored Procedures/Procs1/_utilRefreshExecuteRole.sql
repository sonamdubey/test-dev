IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[_utilRefreshExecuteRole]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[_utilRefreshExecuteRole]
GO

	
CREATE PROCEDURE _utilRefreshExecuteRole AS 
BEGIN

	-- Create cursor for looping through all procedures and functions. Will exclude exec permissions on this proc to users though...
	DECLARE #curProcedures SCROLL CURSOR FOR 
		SELECT name, xtype 
		  FROM sysobjects
		 WHERE status >= 0 and 
		       xtype in ('p', 'fn' ) AND
		       name <> '_utilRefreshExecuteRole'
		ORDER BY xtype, name

	DECLARE @procName sysname, 
					@cmd varchar(1000) , 
					@table varchar(100), 
					@type varchar(2), 
					@priv varchar(30)
	OPEN #curProcedures 

	PRINT Cast ( GetDate() as varchar(25) )  + '  Providing access through SQL role, u_ExecuteProcs'
	FETCH NEXT FROM #curProcedures INTO @table, @type

	-- Loop through list of tables to grant execute access for all Stored Procedure and Functions.
	WHILE (@@fetch_status <> -1) 
		BEGIN 
		SET @cmd = '  GRANT EXECUTE ON [' + @table + '] TO u_ExecuteProcs'
		PRINT @cmd
		EXECUTE (@cmd) 
		FETCH NEXT FROM #curProcedures INTO @table, @type
		END

	-- Get rid of the cursor
	CLOSE #curProcedures 
	DEALLOCATE #curProcedures
END

