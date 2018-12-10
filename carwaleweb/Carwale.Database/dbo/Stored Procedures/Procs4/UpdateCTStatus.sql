IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateCTStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateCTStatus]
GO

	-- =============================================
-- Author:		Pawan
-- Create date: 28-07-2016
-- Description: To update IsSentToCarTrade in respective tables
-- =============================================
CREATE PROCEDURE [dbo].[UpdateCTStatus]
@LeadType INT,
@Id INT
AS
BEGIN	
	SET NOCOUNT ON;

	IF @LeadType = 1
	BEGIN
		UPDATE UsedCarPurchaseInquiries 
		SET IsSentToCarTrade = 1
		WHERE Id = @Id;
	END
	
	IF @LeadType = 2
	BEGIN
		UPDATE ClassifiedLeads 
		SET IsSentToSource = 1
		WHERE Id = @Id;
	END	
	
	IF @LeadType = 3
	BEGIN
		UPDATE ClassifiedAskTheSeller 
		SET IsSentToCarTrade = 1
		WHERE Id = @Id;
	END
END

