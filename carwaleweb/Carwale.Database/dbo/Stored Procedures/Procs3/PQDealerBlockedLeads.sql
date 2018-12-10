IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PQDealerBlockedLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PQDealerBlockedLeads]
GO

	-- =============================================
-- Author:		Vinayak
-- Create date: 16/11/15
-- Insert blocked leads into table
-- =============================================
CREATE PROCEDURE [dbo].[PQDealerBlockedLeads] 
	-- Add the parameters for the stored procedure here
	 @PqDealerLeadId int
	,@ReasonId tinyint
	
AS
BEGIN
	SET NOCOUNT ON;
		INSERT INTO PQDealerFailedLeads (PqDealerLeadId,Reason) values (@PqDealerLeadId,@ReasonId)
END
