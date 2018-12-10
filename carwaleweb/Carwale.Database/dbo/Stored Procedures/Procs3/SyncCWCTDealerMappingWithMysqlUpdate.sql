IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncCWCTDealerMappingWithMysqlUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncCWCTDealerMappingWithMysqlUpdate]
GO

	-- =============================================
-- Author:		<Author,,Prasad Gawde>
-- Create date: <Create Date,,29/09/2016>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[SyncCWCTDealerMappingWithMysqlUpdate] --1,1,'10-10-2016',1,'11-11-2016','12-12-2016','1212',1,1,1,2
-- Add the parameters for the stored procedure here
	@CWDealerID INT,
	@CTDealerID INT = NULL,
	@CurrentDate Datetime,	
	@PackageId INT = NULL,
	@PkgStartDate DATETIME = NULL,
	@PkgEndDate DATETIME = NULL,
	@MaskingNo VARCHAR(20) = NULL,
	@HasSellerLeadPkg BIT = NULL,
	@HasBannerAd BIT = NULL,
	@IsMigrated BIT  = NULL,
	@UpdateType INT,
	@UpdatedBy int = null
AS
BEGIN	
	SET NOCOUNT ON;
	BEGIN TRY
	if @UpdateType=1
		UPDATE mysql_test...cwctdealermapping 
		SET CWDealerID = @CWDealerID,PackageId = ISNULL(@PackageId,PackageId),PackageStartDate = ISNULL(@PkgStartDate,PackageStartDate) , 
			PackageEndDate = ISNULL(@PkgEndDate,PackageEndDate) , MaskingNumber =  ISNULL(@MaskingNo,MaskingNumber),
			UpdatedOn = @CurrentDate , HasSellerLeadPackage = ISNULL(@HasSellerLeadPkg,HasSellerLeadPackage),HasBannerAd = ISNULL(@HasBannerAd,HasBannerAd)
		WHERE CTDealerID = @CTDealerID
	else if @UpdateType=2						
		UPDATE mysql_test...cwctdealermapping SET IsMigrated = @IsMigrated, MigrationRequestDate = @CurrentDate WHERE CWDealerID = @CWDealerID
	else if @UpdateType=3
		UPDATE mysql_test...cwctdealermapping SET CWDealerID = @CWDealerId, UpdatedOn = GETDATE(), UpdatedBy = @UpdatedBy WHERE CTDealerID = @CTDealerID
	else if @UpdateType=4
		DELETE FROM mysql_test...cwctdealermapping WHERE CTDealerID = @CTDealerId AND CWDealerID = @CWDealerId
		end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncCWCTDealerMappingWithMysqlUpdate',ERROR_MESSAGE(),'CWCTDealerMapping',@CWDealerID,GETDATE(),@UpdateType)
	END CATCH 
END

