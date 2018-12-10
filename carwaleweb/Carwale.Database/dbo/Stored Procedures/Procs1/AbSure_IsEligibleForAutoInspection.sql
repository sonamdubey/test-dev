IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_IsEligibleForAutoInspection]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_IsEligibleForAutoInspection]
GO

	
-- =============================================
-- Author: Vinay Kumar Prajapati 
-- Purpose  : To get auto inspection eligibility status when adding new stock. (Except Quikr,absure.in,CampBtl,DealerClassified dealers ) 
-- DelerId For Quikr is 11392 on live and Staging
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_IsEligibleForAutoInspection]--for dealers autobiz.
(
	-- Add the parameters for the stored procedure here
	@BranchId     BIGINT,
	@ModelId      BIGINT,
	@IsEligibleForAutoInspection BIT OUTPUT    -- 1 for Eligible  and 0 for not eligible 
)	
AS  

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SET @IsEligibleForAutoInspection = 0
	
	SELECT ID FROM Dealers AS D WITH(NOLOCK)  
	WHERE D.ID = @BranchId AND ISNULL(D.AutoInspection, 1) = 1	AND (ISNULL(D.IsWarranty,0) = 1 OR ISNULL(D.IsInspection,0) = 1)
	
	--Check Dealer is Eligible for Warranty  or Inspection
	IF @@ROWCOUNT > 0
		BEGIN					

			SELECT AE.ID
			FROM AbSure_EligibleModels AE WITH(NOLOCK) 					
			WHERE AE.ModelId=@ModelId AND AE.IsActive = 1 AND (ISNULL(AE.IsEligibleWarranty,0) = 1 OR ISNULL(AE.IsEligibleCertification,0) = 1)

			IF @@ROWCOUNT > 0
				SET @IsEligibleForAutoInspection = 1						
          END 

END
