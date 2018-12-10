IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateAutoAssignDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateAutoAssignDealer]
GO

	-- =============================================
-- Author:		Vinayak
-- Create date: 11/11/16
-- Update Auto Assign dealers to pqdealeradleads
-- =============================================
CREATE PROCEDURE [dbo].[UpdateAutoAssignDealer] 
	-- Add the parameters for the stored procedure here
	 @PqDealerLeadId int
	,@AutoAssignDealerId int
	
AS
BEGIN
	SET NOCOUNT ON;
	update PQDealerAdLeads set AssignedDealerID=@AutoAssignDealerId where id=@PqDealerLeadId
END