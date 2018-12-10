IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_FetchInterestedInData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_FetchInterestedInData]
GO

	CREATE PROCEDURE [dbo].[CRM_FetchInterestedInData]

	@InterestedInId			Numeric,
	@LeadId					Numeric OutPut,
	@ProductTypeId			SmallInt,
	@ProductType			VarChar(50) OutPut,
	@ProductStatusId		SmallInt,
	@ProductStatus			VarChar(100) OutPut,
	@ClosingProbability		Int OutPut,
	@UpdatedById			Numeric OutPut,
	@UpdatedByName			VarChar(100) OutPut,

	@ClosingDate			DateTime OutPut,
	@CreatedOn				DateTime OutPut,
	@UpdatedOn				DateTime OutPut
				
 AS
	
BEGIN

	SELECT	
		@LeadId				= CII.LeadId,
		@ProductTypeId		= CII.ProductTypeId,
		@ProductType		= CPT.Name,
		@ProductStatusId	= CII.ProductStatusId,
		@ProductStatus		= CPS.Name,
		@ClosingProbability = CII.ClosingProbability,
		@UpdatedById		= CII.UpdatedBy,
		@UpdatedByName		= OU.UserName,

		@ClosingDate		= CII.ClosingDate,
		@CreatedOn			= CII.CreatedOn,
		@UpdatedOn			= CII.UpdatedOn

	FROM (((CRM_InterestedIn AS CII LEFT JOIN CRM_ProductStatus AS CPS ON CII.ProductStatusId = CPS.Id)
			LEFT JOIN CRM_ProductTypes AS CPT ON CII.ProductTypeId = CPT.Id) 
			LEFT JOIN OprUsers AS OU ON CII.UpdatedBy = OU.Id)

	WHERE CII.Id = @InterestedInId
END







