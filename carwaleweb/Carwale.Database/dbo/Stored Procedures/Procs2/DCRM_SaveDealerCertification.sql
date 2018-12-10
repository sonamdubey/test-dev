IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_SaveDealerCertification]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_SaveDealerCertification]
GO

	-- =============================================
-- Author:     Vinay Kumar Prajapati
-- Create date: 4th Sept 2014
-- Description:	save certification details for dealers
--Modified By : Vinay Kumar Prajapati 2 dec 2014 update logoName
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_SaveDealerCertification]
(
	-- Add the parameters for the stored procedure here
	@CertificationId INT,
	@CertificationName VARCHAR(100) , 
	@LogoName VARCHAR(100),
	@Description VARCHAR(MAX),
	@HostUrl VARCHAR(100),
	@DirectoryPath VARCHAR(100),
	@Advantages VARCHAR(MAX),
	@Criteria VARCHAR(MAX),
	@CoreBenefits VARCHAR(MAX),
	@CheckPoints VARCHAR(MAX),
	@WarrantyServices VARCHAR(MAX),
	@OriginalImgPath VARCHAR(250),
	@ScopeId  INT OUTPUT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;
	
	IF @CertificationId<> -1
		BEGIN
			UPDATE Classified_CertifiedOrg SET
				Description = @Description,
				Advantages = @Advantages,
				Criteria = @Criteria,
				CoreBenefits = @CoreBenefits,
				CheckPoints = @CheckPoints,
				WarrantyServices = @WarrantyServices,
				LogoURL         = @LogoName,
				OriginalImgPath = @OriginalImgPath
			WHERE Id = @CertificationId
			--mysql sync
			begin try
			exec	SyncClassifiedCertifiedOrgWithMysqlUpdate @CertificationId ,
					@LogoName ,
					@Description ,
					null ,
					@Advantages ,
					@Criteria ,
					@CoreBenefits ,
					@CheckPoints ,
					@WarrantyServices ,
					@OriginalImgPath ,
					null ,
					 2
			end try
			BEGIN CATCH
				INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
				VALUES('MysqlSync','DCRM_SaveDealerCertification',ERROR_MESSAGE(),'SyncClassifiedCertifiedOrgWithMysqlUpdate',@CertificationId,GETDATE(),2)
			END CATCH			
				
			--end sync
			--Set ScopeId	
			SET @ScopeId = @CertificationId	
		END
	ELSE
		BEGIN 
			INSERT INTO Classified_CertifiedOrg (CertifiedOrgName,LogoURL,Description,
				IsActive,HostURL,IsReplicated,DirectoryPath,Advantages,Criteria,CoreBenefits,
				CheckPoints,WarrantyServices, OriginalImgPath)
		    VALUES
				(@CertificationName,@LogoName,@Description,1,@HostUrl,0,@DirectoryPath,@Advantages,
				 @Criteria,@CoreBenefits,@CheckPoints,@WarrantyServices, @OriginalImgPath)
				-- Set ScopeId
				SET @ScopeId = SCOPE_IDENTITY()
			begin try
			exec [dbo].[SyncClassifiedCertifiedOrgWithMysql] @ScopeId ,
				@CertificationName ,
				@LogoName ,
				@Description ,
				@HostURL ,
				@DirectoryPath ,
				@Advantages ,
				@Criteria ,
				@CoreBenefits ,
				@CheckPoints ,
				@WarrantyServices ,
				@OriginalImgPath
			end try
			BEGIN CATCH
				INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
				VALUES('MysqlSync','DCRM_SaveDealerCertification',ERROR_MESSAGE(),'SyncClassifiedCertifiedOrgWithMysql',@ScopeId,GETDATE(),null)
			END CATCH	
		END	
END

