IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Certification_Update]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Certification_Update]
GO

	
-- =============================================
-- Author:		Umesh Ojha
-- Create date: 24/4/2013
-- Description:	Update Certification certification for dealers
-- =============================================
Create PROCEDURE [dbo].[Certification_Update] 
	-- Add the parameters for the stored procedure here
	@CertificationId INT,
	@Description VARCHAR(MAX),	
	@Advantages VARCHAR(MAX),
	@Criteria VARCHAR(MAX),
	@CoreBenefits VARCHAR(MAX),
	@CheckPoints VARCHAR(MAX),
	@WarrantyServices VARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	UPDATE Classified_CertifiedOrg SET
		Description = @Description,
		Advantages = @Advantages,
		Criteria = @Criteria,
		CoreBenefits = @CoreBenefits,
		CheckPoints = @CheckPoints,
		WarrantyServices = @WarrantyServices
	WHERE Id = @CertificationId	
	
END


