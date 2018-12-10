IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_FetchLeadData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_FetchLeadData]
GO

	CREATE PROCEDURE [dbo].[CRM_FetchLeadData]

	@LeadId				Numeric,
	@CustomerId			Numeric OutPut,
	@LeadStageId		SmallInt OutPut,
	@LeadStage			VarChar(50) OutPut,
	@Priority			SmallInt OutPut,
	@OwnerId			Numeric OutPut,
	@Owner				VarChar(100) OutPut,
	@LeadStatusId		Numeric OutPut,

	@IsActive			Bit OutPut,

	@CreatedOn			DateTime OutPut,
	@UpdatedOn			DateTime OutPut
				
 AS
	
BEGIN

	SELECT	
		@CustomerId		= CL.CNS_CustId,
		@LeadStageId	= CL.LeadStageId,
		@LeadStage		= CLS.Name,
		@Priority		= CL.Priority,
		@OwnerId		= CL.Owner,
		@Owner			= Case CL.LeadStageId When 1 Then OU.UserName When 2 Then CAT.Name When 3 Then CAT.Name End,
		@LeadStatusId	= CL.LeadStatusId,	

		@IsActive		= Case CLS.IsClosed When 1 Then 0 When 0 Then 1 End,

		@CreatedOn		= CL.CreatedOn,
		@UpdatedOn		= CL.UpdatedOn

	FROM (((CRM_Leads AS CL LEFT JOIN CRM_LeadStages AS CLS ON CL.LeadStageId = CLS.Id)
			LEFT JOIN OprUsers AS OU ON CL.Owner = OU.Id)
			LEFT JOIN CRM_ADM_Teams AS CAT ON CL.Owner = CAT.Id)
			
	WHERE CL.Id = @LeadId
END