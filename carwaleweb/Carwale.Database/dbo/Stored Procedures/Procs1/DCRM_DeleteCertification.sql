IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_DeleteCertification]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_DeleteCertification]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(26th Aug 2014)
-- Description	:	Delete certification for dealers
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_DeleteCertification]
	
	@CertificationId	INT,
	@DeletedBy			INT,
	@Result				SMALLINT OUTPUT
AS
BEGIN
	
	SET NOCOUNT ON;
	SET @Result = 0

	SELECT ID FROM Classified_CertifiedOrg WHERE ID = @CertificationId
	IF @@ROWCOUNT <> 0
	BEGIN
		INSERT INTO Classified_CertifiedOrgDeleteLog (CertifiedOrgName,LogoURL,Description,IsActive,HostURL,IsReplicated,DirectoryPath,Advantages,
														Criteria,CoreBenefits,CheckPoints,WarrantyServices,DeletedBy)  
			SELECT CertifiedOrgName,LogoURL,Description,IsActive,HostURL,IsReplicated,DirectoryPath,Advantages,
					Criteria,CoreBenefits,CheckPoints,WarrantyServices,@DeletedBy
			FROM Classified_CertifiedOrg WHERE Id = @CertificationId
			
		DELETE FROM Classified_CertifiedOrg WHERE Id = @CertificationId
		IF @@ROWCOUNT <> 0 
			SET @Result = 1
	END
    
END

