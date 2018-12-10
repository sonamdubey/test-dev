IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncCarFuelTypesWithMysql]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncCarFuelTypesWithMysql]
GO

	-- =============================================
-- Author:		Pratik Vasa
-- Create date: 23/9/16
-- Description:	added to sync Carwale sqlserver with Mysql
-- =============================================
CREATE PROCEDURE  [dbo].[SyncCarFuelTypesWithMysql]
	-- Add the parameters for the stored procedure here
	@CarFuelTypeId tinyint ,
	@Descr varchar(20) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
   INSERT INTO mysql_test...carfueltypes(CarFuelTypeId  ,
	Descr )  
	VALUES (@CarFuelTypeId  ,
	@Descr );
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncCarFuelTypesWithMysql',ERROR_MESSAGE(),'CarFuelTypes',@CarFuelTypeId,GETDATE(),null)
	END CATCH
END

