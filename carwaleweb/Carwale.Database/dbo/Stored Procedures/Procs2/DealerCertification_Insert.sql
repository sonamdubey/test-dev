IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DealerCertification_Insert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DealerCertification_Insert]
GO

	-- =============================================
-- Author:		Umesh Ojha
-- Create date: 8/4/2013
-- Description:	Insert New certification for dealers
-- Modified By :Umesh Ojha on 24 apr 2013 for insert additional data in table

-- =============================================
CREATE PROCEDURE [dbo].[DealerCertification_Insert] 
	-- Add the parameters for the stored procedure here
	@CertificationName VARCHAR(100) , 
	@LogoName VARCHAR(100),
	@Description VARCHAR(MAX),
	@HostUrl VARCHAR(100),
	@DirectoryPath VARCHAR(100),
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
	
	INSERT INTO Classified_CertifiedOrg 
		(CertifiedOrgName,LogoURL,Description,IsActive,HostURL,IsReplicated,DirectoryPath,
		Advantages,Criteria,CoreBenefits,CheckPoints,WarrantyServices)
	VALUES
		(@CertificationName,@LogoName,@Description,1,@HostUrl,0,@DirectoryPath,
		@Advantages,@Criteria,@CoreBenefits,@CheckPoints,@WarrantyServices)

	SELECT SCOPE_IDENTITY()	
	
END

