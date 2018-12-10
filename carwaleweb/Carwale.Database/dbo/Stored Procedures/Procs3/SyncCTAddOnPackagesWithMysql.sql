IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncCTAddOnPackagesWithMysql]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncCTAddOnPackagesWithMysql]
GO

	-- =============================================
-- Author:		<Author,,Prasad Gawde>
-- Create date: <Create Date,,29/09/2016>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[SyncCTAddOnPackagesWithMysql]
@Id					INT,
@CWDealerId			INT,
@AddOnPackageId		INT,
@StartDate			DATETIME,
@EndDate			DATETIME,
@CreatedOn			DATETIME,
@IsActive           BIT
AS
BEGIN
	  SET NOCOUNT ON;
	  BEGIN TRY
	  INSERT INTO mysql_test...ct_addonpackages(Id,AddOnPackageId, CWDealerId, StartDate, EndDate, IsActive, CreatedOn)
								VALUES(@ID,@AddOnPackageId, @CWDealerId, @StartDate, @EndDate, @IsActive, @CreatedOn)
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncCTAddOnPackagesWithMysql',ERROR_MESSAGE(),'CTAddOnPackages',@Id,GETDATE(),null)
	END CATCH 
END

