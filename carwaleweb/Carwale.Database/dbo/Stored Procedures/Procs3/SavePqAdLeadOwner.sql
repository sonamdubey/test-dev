IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SavePqAdLeadOwner]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SavePqAdLeadOwner]
GO

	-- =============================================
-- Author:		Vinayak
-- Create date: 01/03/2015
-- Description:	To save assignee of PQ Leads
-- =============================================

CREATE PROCEDURE [dbo].[SavePqAdLeadOwner] 
	-- Add the parameters for the stored procedure here
	 @LeadId INT
	,@OwnerId INT
AS
BEGIN
	SET NOCOUNT ON;
    -- Insert statements for procedure here
			INSERT INTO PqAdLeadOwner (LeadId,OwnerId) values (@LeadId,@OwnerId)
END