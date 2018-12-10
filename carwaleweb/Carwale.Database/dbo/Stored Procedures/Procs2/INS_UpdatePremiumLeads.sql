IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[INS_UpdatePremiumLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[INS_UpdatePremiumLeads]
GO

	-- =============================================
-- Author:		Akansha
-- Create date: 14.08.2013
-- Description:	Set ClientId, PushStatus based on CustomerId
-- =============================================
CREATE PROCEDURE [dbo].[INS_UpdatePremiumLeads] @ClientId INT
	,@Id INT
	,@Status VARCHAR(15)
AS
BEGIN
	IF EXISTS (
			SELECT id
			FROM INS_PremiumLeads
			WHERE Id = @Id
			)
	BEGIN
		UPDATE INS_PremiumLeads
		SET ClientId = @ClientId
			,PushStatus = @Status
		WHERE Id = @Id 
	END
END
