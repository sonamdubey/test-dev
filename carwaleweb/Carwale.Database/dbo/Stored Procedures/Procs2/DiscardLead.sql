IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DiscardLead]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DiscardLead]
GO

	
-- =============================================
-- Author:		Sanjay Soni
-- Create date: 28/06/2016
-- Description:	Discard Lead Data
-- =============================================
CREATE PROCEDURE [dbo].[DiscardLead] @LeadId NUMERIC(20,0), @UpdatedBy Int
AS
BEGIN
	INSERT INTO DiscardLeads VALUES(@LeadId, @UpdatedBy , GETDATE());
END

