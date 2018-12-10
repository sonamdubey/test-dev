IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncCWCTDealerMappingWithMysql]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncCWCTDealerMappingWithMysql]
GO

	-- =============================================
-- Author:		<Author,,Prasad Gawde>
-- Create date: <Create Date,,29/09/2016>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[SyncCWCTDealerMappingWithMysql] --1,1,1,'10-10-2016',1,'11-10-2016','11-12-2016','12121',1,1,1
-- Add the parameters for the stored procedure here
	@CwCtId smallint,
	@DEALERID INT,
	@CTDealerID INT = NULL,
	@CurrentDate Datetime,	
	@PackageId INT = NULL,
	@PkgStartDate DATETIME = NULL,
	@PkgEndDate DATETIME = NULL,
	@MaskingNo VARCHAR(20) = NULL,
	@HasSellerLeadPkg BIT = NULL,
	@HasBannerAd BIT = NULL,
	@IsMigrated BIT  = NULL,
	@InsertType int =1,
	@UpdatedBy	INT = NULL
AS
BEGIN
		SET NOCOUNT ON;
		BEGIN TRY
		if @InsertType=1
	  INSERT INTO mysql_test...cwctdealermapping(Id,CWDealerID,CTDealerID,CreatedOn,PackageId,PackageStartDate,PackageEndDate,MaskingNumber,
																	HasSellerLeadPackage,HasBannerAd,IsMigrated)
									 VALUES (@CwCtId,@DEALERID,@CTDealerID,@CurrentDate,@PackageId,@PkgStartDate,@PkgEndDate,@MaskingNo,@HasSellerLeadPkg,@HasBannerAd,@IsMigrated)
		
		else if @InsertType=2

		INSERT INTO mysql_test...cwctdealermapping
			(Id,CWDealerID, CTDealerID, UpdatedOn, UpdatedBy)
		VALUES
			(@CwCtId, @DEALERID, @CTDealerID, @CurrentDate, @UpdatedBy)
		end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncCWCTDealerMappingWithMysql',ERROR_MESSAGE(),'CWCTDealerMapping',@DEALERID,GETDATE(),@InsertType)
	END CATCH 
END


