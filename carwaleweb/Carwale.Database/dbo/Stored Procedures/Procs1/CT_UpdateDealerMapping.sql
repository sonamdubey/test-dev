IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CT_UpdateDealerMapping]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CT_UpdateDealerMapping]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 27 Oct 2016
-- Description:	Update and Validate CarWale CarTrade dealer mapping 
-- =============================================
CREATE PROCEDURE [dbo].[CT_UpdateDealerMapping]
	-- Add the parameters for the stored procedure here
	@CTDealerId	INT,
	@CWDealerId	INT,
	@IsDelete	BIT = 0,
	@UpdatedBy	INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @CwCtId smallint,
			@DEALERID INT = null,
			@CurrentDate Datetime= null,	
			@PackageId INT = NULL,
			@PkgStartDate DATETIME = NULL,
			@PkgEndDate DATETIME = NULL,
			@MaskingNo VARCHAR(20) = NULL,
			@HasSellerLeadPkg BIT = NULL,
			@HasBannerAd BIT = NULL,
			@IsMigrated BIT  = NULL,
			@InsertType int =2,
			@UpdateType int
	IF EXISTS (SELECT ID FROM Dealers WITH(NOLOCK) WHERE ID = @CWDealerId)
	BEGIN
		IF(@IsDelete = 0)
			BEGIN
				--Check if any of the dealer id is migrated or not else proceed
				SELECT CWDealerID, CTDealerID, IsMigrated, MigrationRequestDate, MigrationSuccessDate
				FROM CWCTDealerMapping WITH (NOLOCK)
				WHERE ((CWDealerID = @CWDealerId OR CTDealerID = @CTDealerId) AND IsMigrated IS NOT NULL) OR CWDealerID = @CWDealerId
				IF @@ROWCOUNT = 0
					BEGIN
						IF EXISTS (SELECT CWDealerID FROM CWCTDealerMapping WITH(NOLOCK) WHERE CTDealerID = @CTDealerId )
							BEGIN
								--update mapping of ctdealerid with the new cwdealerid else insert
								INSERT INTO CWCTDealerMappingLog (CWCTDealerMappingId,CWDealerID,CTDealerID,PackageId,PackageStartDate,PackageEndDate,MaskingNumber,HasSellerLeadPackage,HasBannerAd,IsMigrated,MigrationRequestDate,MigrationSuccessDate,UpdatedBy)
								SELECT Id,CWDealerID,CTDealerID,PackageId,PackageStartDate,PackageEndDate,MaskingNumber,HasSellerLeadPackage,HasBannerAd,IsMigrated,MigrationRequestDate,MigrationSuccessDate,UpdatedBy
								FROM CWCTDealerMapping WITH(NOLOCK)
								WHERE CTDealerID = @CTDealerId
								UPDATE CWCTDealerMapping SET CWDealerID = @CWDealerId, UpdatedOn = GETDATE(), UpdatedBy = @UpdatedBy WHERE CTDealerID = @CTDealerId
								set @UpdateType =3
								set @CurrentDate=getdate()
								exec [dbo].[SyncCWCTDealerMappingWithMysqlUpdate] @CWDealerID,
									@CTDealerID ,
									@CurrentDate,	
									@PackageId,
									@PkgStartDate,
									@PkgEndDate,
									@MaskingNo,
									@HasSellerLeadPkg ,
									@HasBannerAd,
									@IsMigrated,
									@UpdateType,
									@UpdatedBy  
							END
						ELSE
							BEGIN
								--if no mapping present for ctdealerid then insert mapping for ctdealerid and cwdealerid
									INSERT INTO CWCTDealerMapping
									(CWDealerID, CTDealerID, UpdatedOn, UpdatedBy)
									VALUES
									(@CWDealerId, @CTDealerId, GETDATE(), @UpdatedBy)
				
									set @CwCtId=SCOPE_IDENTITY()

									set @InsertType  =2
									set @CurrentDate = getdate()
									set @DEALERID = @CWDealerId
									exec [dbo].[SyncCWCTDealerMappingWithMysql] @CwCtId ,
											@DEALERID ,
											@CTDealerID ,
											@CurrentDate ,	
											@PackageId ,
											@PkgStartDate ,
											@PkgEndDate,
											@MaskingNo ,
											@HasSellerLeadPkg ,
											@HasBannerAd,
											@IsMigrated ,
											@InsertType,
											@UpdatedBy
							END
						SELECT CWDealerID,CTDealerID,IsMigrated, MigrationRequestDate, MigrationSuccessDate FROM CWCTDealerMapping WITH(NOLOCK) WHERE CTDealerID = @CTDealerId 
					END
			END
		ELSE
			BEGIN
				INSERT INTO CWCTDealerMappingLog (CWCTDealerMappingId,CWDealerID,CTDealerID,PackageId,PackageStartDate,PackageEndDate,MaskingNumber,HasSellerLeadPackage,HasBannerAd,IsMigrated,MigrationRequestDate,MigrationSuccessDate,UpdatedBy)
				SELECT Id,CWDealerID,CTDealerID,PackageId,PackageStartDate,PackageEndDate,MaskingNumber,HasSellerLeadPackage,HasBannerAd,IsMigrated,MigrationRequestDate,MigrationSuccessDate,@UpdatedBy
				FROM CWCTDealerMapping WITH(NOLOCK)
				WHERE CWDealerID = @CWDealerId AND CTDealerID = @CTDealerId
				DELETE FROM CWCTDealerMapping 
				WHERE CTDealerID = @CTDealerId AND CWDealerID = @CWDealerId
				set @UpdateType =4
				exec [dbo].[SyncCWCTDealerMappingWithMysqlUpdate] @CWDealerID,
					@CTDealerID ,
					@CurrentDate,	
					@PackageId,
					@PkgStartDate,
					@PkgEndDate,
					@MaskingNo,
					@HasSellerLeadPkg ,
					@HasBannerAd,
					@IsMigrated,
					@UpdateType,
					@UpdatedBy  
			END
	END
END