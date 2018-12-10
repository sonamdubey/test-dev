IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_UpdateTC_AvailableRSAPackages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_UpdateTC_AvailableRSAPackages]
GO

	
-- =============================================
-- Author	:	Sachin Bharti (22th Sep 2014)
-- Description	:	Add RSA package quantity to TC_AvailableRSAPackages table
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_UpdateTC_AvailableRSAPackages]
	
	@DealerId	INT ,
	@PackageId	INT	,
	@Quantity	INT	,
	@UserId		INT

AS
BEGIN
	
	SET NOCOUNT ON;
	
	DECLARE	@TC_AvailableRSAPackageId	INT 
	DECLARE	@TC_RSAPackageQunatity		INT		
	
	--Check if package is already added for given DealerId 
	SELECT @TC_AvailableRSAPackageId = Id , @TC_RSAPackageQunatity = AvailableQuantity FROM TC_AvailableRSAPackages WHERE BranchId = @DealerId AND PackageId = @PackageId
	IF @@ROWCOUNT <> 0
	BEGIN
		SET @Quantity = @TC_RSAPackageQunatity + @Quantity

		--Then add qunatity to existing package quantity
		UPDATE	TC_AvailableRSAPackages 
				SET	AvailableQuantity = @Quantity ,
					UserId	=	@UserId ,
					UpdatedOn = GETDATE()
				WHERE
					BranchId	= @DealerId AND
					PackageId	= @PackageId 
				
	END
	--If no package exist then add a new entry
	ELSE
	BEGIN
		INSERT INTO TC_AvailableRSAPackages (
												BranchId	,
												PackageId	,
												AvailableQuantity	,
												UserId	,
												EntryDate	,
												UpdatedOn	
											)
									VALUES	(
												@DealerId	,
												@PackageId	,
												@Quantity	,
												@UserId		,
												GETDATE()	,
												GETDATE()
											)
	END
END

