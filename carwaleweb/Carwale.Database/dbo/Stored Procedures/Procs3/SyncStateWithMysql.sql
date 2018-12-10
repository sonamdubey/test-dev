IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncStateWithMysql]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncStateWithMysql]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[SyncStateWithMysql] --4444,'name'
@StateId INT,
@StateName VARCHAR(30) 
AS
BEGIN
	set nocount on;
	begin try
		INSERT INTO mysql_test...states(Id, Name, IsDeleted ) VALUES(@StateId, @StateName , 0 );
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncStateWithMysql',ERROR_MESSAGE(),'states',@StateId,GETDATE(),null)
	END CATCH
END

