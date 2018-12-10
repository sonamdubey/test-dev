IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DeleteArea]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DeleteArea]
GO

	-- =============================================
-- Author:		Pratik Vasa
-- Create date: 17/10/2016
-- Description:	delete an area
-- =============================================
CREATE PROCEDURE [dbo].[DeleteArea]
	-- Add the parameters for the stored procedure here
	@AreaId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	 UPDATE Areas SET IsDeleted = 1 WHERE ID = @AreaId
	 
	 begin try 
		exec SyncAreasWithMysqlUpdate @AreaId, NULL, NULL, NULL, NULL, 2
	 end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','DeleteArea',ERROR_MESSAGE(),'SyncAreasWithMysqlUpdate',@AreaId,GETDATE(),2)
	END CATCH
END

