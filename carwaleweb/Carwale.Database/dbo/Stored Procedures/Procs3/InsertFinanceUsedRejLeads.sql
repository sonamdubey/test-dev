IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertFinanceUsedRejLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertFinanceUsedRejLeads]
GO

	


--THIS PROCEDURE INSERTS THE VALUES FOR THE Cities

CREATE PROCEDURE [dbo].[InsertFinanceUsedRejLeads]
	@LoanId			NUMERIC,
	@Reason			VARCHAR(500),
	@LeadDate		DATETIME
AS
	
BEGIN
	INSERT INTO UsedCarLoanRejected(LoanId, Reason, LeadDate, RejectDate)			
	VALUES(@LoanId, @Reason, @LeadDate, GETDATE())	
END





