IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_DeleteTransaction]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_DeleteTransaction]
GO

	-- =============================================
-- Author:		Amit Yadav
-- Create date: 25th Nov,2015
-- Description:	To get Dealer Lead Detail.
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_DeleteTransaction]

	@TransactionId INT
AS
BEGIN

	--SET NOCOUNT ON;

    UPDATE DCRM_PaymentTransaction SET
	 IsActive = 0
	WHERE TransactionId=@TransactionId
	 
END




