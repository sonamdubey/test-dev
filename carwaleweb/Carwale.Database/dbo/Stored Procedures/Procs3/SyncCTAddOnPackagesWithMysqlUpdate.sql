IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncCTAddOnPackagesWithMysqlUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncCTAddOnPackagesWithMysqlUpdate]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[SyncCTAddOnPackagesWithMysqlUpdate] --1212,1,'11-11-2016','12-12-2016','10-10-2016',1,1,3
@CWDealerId			INT,
@AddOnPackageId		INT,
@StartDate			DATETIME,
@EndDate			DATETIME,
@UpdatedOn			DATETIME,
@IsActive           BIT,
@Id					INT,
@UpdateType			INT
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
if @UpdateType=1
	UPDATE mysql_test...ct_addonpackages
			SET    AddOnPackageId=ISNULL(@AddOnPackageId,AddOnPackageId),
					StartDate=ISNULL(@StartDate,StartDate),
					EndDate=ISNULL(@EndDate,EndDate),
					IsActive=ISNULL(@IsActive,IsActive),
					UpdatedOn=@UpdatedOn
			WHERE	 CWDealerId=@CWDealerId AND AddOnPackageId=@AddOnPackageId
		  	  
else if @UpdateType=2
	UPDATE mysql_test...ct_addonpackages SET IsActive = @IsActive WHERE CWDealerId = @CWDealerId AND AddOnPackageId = @AddOnPackageId
	
else if @UpdateType=3
	UPDATE mysql_test...ct_addonpackages 
			SET StartDate = @StartDate, EndDate = @EndDate,UpdatedOn = @UpdatedOn, IsActive = @IsActive 
			WHERE Id = @Id
									
else if @UpdateType=4			
	UPDATE mysql_test...ct_addonpackages SET EndDate = @EndDate, UpdatedOn = @UpdatedOn, IsActive = @IsActive 
			WHERE Id = @Id
			end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncCTAddOnPackagesWithMysqlUpdate',ERROR_MESSAGE(),'CTAddOnPackages',@Id,GETDATE(),@UpdateType)
	END CATCH 
END

