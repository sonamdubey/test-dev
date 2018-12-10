IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncStateWithMysqlUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncStateWithMysqlUpdate]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SyncStateWithMysqlUpdate] 
	-- Add the parameters for the stored procedure here
	@StateId INT,
@StateName VARCHAR(30) ,
@UpdateType int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
    if @UpdateType = 1
	begin
	UPDATE mysql_test...states SET 
				Name=@StateName
				WHERE Id=@StateId
	end
	else if @UpdateType =2 
	begin
		UPDATE mysql_test...states SET IsDeleted=1 WHERE Id=@StateId
	end
	END TRY
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncStateWithMysqlUpdate',ERROR_MESSAGE(),'states',@StateId,GETDATE(),@UpdateType)
	END CATCH
END

