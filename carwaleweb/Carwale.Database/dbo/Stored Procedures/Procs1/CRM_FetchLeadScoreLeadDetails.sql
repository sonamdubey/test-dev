IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_FetchLeadScoreLeadDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_FetchLeadScoreLeadDetails]
GO

	
CREATE PROCEDURE [dbo].[CRM_FetchLeadScoreLeadDetails]

	@LeadId				NUMERIC,
	@CustomerId			NUMERIC OUTPUT,
	@IsActive			BIT OUTPUT
				
 AS
	
BEGIN
	
	IF @LeadId > 0
		BEGIN									
			 SELECT @CustomerId = CL.CNS_CustId, @IsActive = (CASE CL.LeadStageId WHEN 3 THEN 0 ELSE 1 END)
			 FROM CRM_Leads CL WITH (NOLOCK)
			 WHERE CL.ID = @LeadId
		END
END















