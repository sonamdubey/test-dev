IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ReceiveUnverifiedLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ReceiveUnverifiedLeads]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 19-01-2015
-- Description:	Load Unverifid lead receive status of the dealer
--Modified By Deepak on 9th Sept 2016 - Addon Package for unverified leads
-- =============================================
CREATE PROCEDURE [dbo].[TC_ReceiveUnverifiedLeads]
	@BranchId BIGINT,
	@SendUnVerifiedLead BIT = 0 OUTPUT,
	@SendSMSUnVerifiedLead BIT = 0 OUTPUT,
	@SendEmailUnVerifiedLead BIT = 0 OUTPUT
AS
BEGIN	
	SET NOCOUNT ON;
	
	IF @BranchId IS NOT NULL
	BEGIN
	   
	   -- IF EXISTS (SELECT Id FROM TC_MappingDealerFeatures WITH(NOLOCK) WHERE BranchId = @BranchId AND TC_DealerFeatureId =  4)
		IF EXISTS (SELECT Id FROM CT_AddOnPackages WITH(NOLOCK) WHERE CWDealerId = @BranchId AND AddOnPackageId =  101 AND IsActive = 1) --Added By Deepak on 9th Sept 2016
			SET @SendUnVerifiedLead = 1
	   
	    IF EXISTS (SELECT Id FROM TC_MappingDealerFeatures WITH(NOLOCK) WHERE BranchId = @BranchId AND TC_DealerFeatureId =  5)
			SET @SendSMSUnVerifiedLead = 1

		IF EXISTS (SELECT Id FROM TC_MappingDealerFeatures WITH(NOLOCK) WHERE BranchId = @BranchId AND TC_DealerFeatureId =  6)
			SET @SendEmailUnVerifiedLead = 1

	END
	 
END
