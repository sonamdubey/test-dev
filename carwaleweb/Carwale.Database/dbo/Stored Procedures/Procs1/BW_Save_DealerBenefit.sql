IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_Save_DealerBenefit]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_Save_DealerBenefit]
GO

	
-- =============================================
-- Author		:	Sumit Kate
-- Create date	:	10 Mar 2016
-- Description	:	Saves the Dealer Benefits. If @BenefitId is passed then It will update the existing record.
-- Parameters	
--	@DealerId	:	Dealer Id
--	@CityId		:	City Id
--	@CatId		:	BW_PQ_DealerBenefit_Category Id column value
--	@BenefitText:	Benefit Text
--  @UserId		:	User Id
--  @IsActive	:	Is Active
--	@BenefitId	:	Benefit Id	
CREATE PROC BW_Save_DealerBenefit
(
	@DealerId INT,
	@CatId SMALLINT,
	@BenefitText VARCHAR(200),
	@UserId INT,
	@IsActive BIT = 1,
	@CityId INT,
	@BenefitId INT = NULL)
AS
BEGIN
	IF @BenefitId IS NULL
		BEGIN
			INSERT INTO BW_PQ_DealerBenefit (
			DealerId
			,CityId
			,CatId
			,BenefitText
			,IsActive
			,LastUpdatedBy
			)
			VALUES (@DealerId
			,@CityId
			,@CatId
			,@BenefitText
			,@IsActive
			,@UserId)		
		END
	ELSE
		BEGIN
			UPDATE BW_PQ_DealerBenefit
			SET BenefitText = @BenefitText, CatId = @CatId, LastUpdated = GETDATE(), LastUpdatedBy = @UserId
			WHERE Id = @BenefitId
		END
END
