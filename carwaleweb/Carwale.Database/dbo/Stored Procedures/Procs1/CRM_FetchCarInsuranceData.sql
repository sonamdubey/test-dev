IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_FetchCarInsuranceData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_FetchCarInsuranceData]
GO

	CREATE PROCEDURE [dbo].[CRM_FetchCarInsuranceData]

	@CarBasicDataId			Numeric,
	@CarInsuranceId			Numeric OutPut,
	@LeadId					Numeric OutPut,
	@InsAgencyId			Numeric OutPut,
	@InsAgencyName			VarChar(200) OutPut,
	@InsCoverLetterNumber	VarChar(50) OutPut,
	@InsComments			VarChar(1000) OutPut,
	@UpdatedById			Numeric OutPut,
	@UpdatedByName			VarChar(100) OutPut,
	
	@IsInsCoverLetterIssued	Bit OutPut,
	@IsInsPaymentCollected	Bit OutPut,
	@IsInsPaymentRealised	Bit OutPut,

	@InsStartDate			DateTime OutPut,
	@CreatedOn				DateTime OutPut,
	@UpdatedOn				DateTime OutPut
				
 AS
	
BEGIN

	SELECT	
		@CarInsuranceId			= CID.Id,
		@LeadId					= CBD.LeadId,
		@InsAgencyId			= CID.InsAgencyId,
		@InsAgencyName			= NIA.Name, 
		@InsCoverLetterNumber	= CID.InsCoverLetterNumber,
		@InsComments			= CID.InsComments,
		@UpdatedById			= CID.UpdatedBy,
		@UpdatedByName			= OU.UserName,
		
		@IsInsCoverLetterIssued	= CID.IsInsCoverLetterIssued,
		@IsInsPaymentCollected	= CID.IsInsPaymentCollected,
		@IsInsPaymentRealised	= CID.IsInsPaymentRealised,

		@InsStartDate			= CID.InsStartDate,
		@CreatedOn				= CID.CreatedOn,
		@UpdatedOn				= CID.UpdatedOn

	FROM (((CRM_CarInsuranceData AS CID LEFT JOIN CRM_CarBasicData AS CBD ON CID.CarBasicDataId = CBD.Id)
			LEFT JOIN OprUsers AS OU ON CID.UpdatedBy = OU.Id)
			LEFT JOIN NCS_InsuranceAgency AS NIA ON CID.InsAgencyId = NIA.Id)

	WHERE CID.CarBasicDataId = @CarBasicDataId
END







