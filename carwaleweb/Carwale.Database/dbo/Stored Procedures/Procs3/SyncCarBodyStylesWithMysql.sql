IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncCarBodyStylesWithMysql]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncCarBodyStylesWithMysql]
GO

	-- =============================================
-- Author:		Pratik Vasa
-- Create date: 23/9/16
-- Description:	added to sync Carwale sqlserver with Mysql
-- =============================================
CREATE PROCEDURE  [dbo].[SyncCarBodyStylesWithMysql]
	-- Add the parameters for the stored procedure here
	@ID numeric(18, 0) ,
	@Name varchar(50) ,
	@ImageUrl varchar(50) ,
	@IsReplicated bit ,
	@HostURL varchar(100) ,
	@IsBodyStyleActive bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
   INSERT INTO mysql_test...carbodystyles(ID ,
	Name ,
	ImageUrl ,
	IsReplicated ,
	HostURL ,
	IsBodyStyleActive )  
	VALUES (@ID ,
	@Name ,
	@ImageUrl ,
	@IsReplicated ,
	@HostURL ,
	@IsBodyStyleActive);
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncCarBodyStylesWithMysql',ERROR_MESSAGE(),'CarBodyStyles',@ID,GETDATE(),null)
	END CATCH
END

