IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Ins_Requests_Sp]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Ins_Requests_Sp]
GO

	-- =============================================
-- Author:		Satish Sharma
-- Create date: 18-July-2008
-- Description:	SP to Insert/Update Ins_Requests Data
-- =============================================
CREATE PROCEDURE [dbo].[Ins_Requests_Sp] 
	-- Add the parameters for the stored procedure here
	@InsPoolId			NUMERIC,
	@InsuredValue		NUMERIC,
	@PremiumAmount		NUMERIC,
	@PaymentModeId		INT,
	@RequestDateTime	DATETIME,
	@Status				Numeric OUTPUT --Id of the last inserted insurance request
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO Ins_Requests(InsPoolId, InsuredValue, PremiumAmount, PaymentModeId, RequestDateTime)
	VALUES(@InsPoolId, @InsuredValue, @PremiumAmount, @PaymentModeId, @RequestDateTime)

	Set @Status = Scope_Identity()
END