IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RVN_CanUpdatePackage]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RVN_CanUpdatePackage]
GO

	-- =============================================
-- Author	:	Sachin Bharti(13th Oct 2013)
-- Description	:	Check whether user can activate or not that camapaign
-- =============================================
CREATE PROCEDURE	[dbo].[RVN_CanUpdatePackage]
	
	@CampaignId		INT,
	@DealerId		INT,
	@PackageId		INT,
	@PackageStartDate	DATETIME = NULL,
	@PackageEndDate		DATETIME = NULL,
	@MakeId			INT = NULL,
	@ModelId		INT = NULL,
	@CanUpdate		SMALLINT OUTPUT

AS
BEGIN
	
	SET NOCOUNT ON;
	SET @CanUpdate = -1
	DECLARE	@RowCount	INT = 0
	DECLARE	@IsExist	INT = 0
	DECLARE	@IsActive	INT = 0

	--Check package start or end date does not match with previous dates
	SELECT 
		RVN.DealerPackageFeatureID 
	FROM 
		RVN_DealerPackageFeatures RVN(NOLOCK)
	WHERE	
		RVN.DealerId = @DealerId 
		AND RVN.PackageId = @PackageId
		AND
		( 
			( @PackageStartDate IS NOT NULL AND @PackageEndDate IS NOT NULL) AND
			(
				( @PackageStartDate BETWEEN RVN.PackageStartDate AND RVN.PackageEndDate  ) OR
				( @PackageEndDate   BETWEEN RVN.PackageStartDate AND RVN.PackageEndDate  ) OR
				( @PackageStartDate < RVN.PackageStartDate ) OR
				( @PackageEndDate < RVN.PackageEndDate)
			)
		)
		AND ( @MakeId IS NULL OR RVN.MakeId = @MakeId	)
		AND ( @ModelId IS NULL OR RVN.ModelId = @ModelId )
		AND RVN.PackageStatus <> 4 --Excluding delivered campaigns
		AND RVN.DealerPackageFeatureID <> @CampaignId
	SET @RowCount = @@ROWCOUNT
	
	IF @RowCount > 0 
		SET @IsExist = 1

	--Check if campaign is same as already active campaign
	SELECT 
		RVN.DealerPackageFeatureID 
	FROM 
		RVN_DealerPackageFeatures RVN(NOLOCK)
	WHERE	
		RVN.DealerId	=	@DealerId 
		AND RVN.PackageId	=	@PackageId
		AND (@MakeId IS NULL OR RVN.MakeId = @MakeId)
		AND (@ModelId IS NULL OR RVN.ModelId = @ModelId)
		AND RVN.PackageStatus = 2
		AND RVN.DealerPackageFeatureID <> @CampaignId
	SET @RowCount = @@ROWCOUNT
	
	IF @RowCount > 0 
		SET @IsActive = 1

	--If both condition false then package can be activated
	IF @IsExist = 0 AND @IsActive = 0
	BEGIN
		SET @CanUpdate = 1
	END
	--Else package can not be activated
	ELSE
	BEGIN
		SET @CanUpdate = 0
	END	

	PRINT @CanUpdate
END
