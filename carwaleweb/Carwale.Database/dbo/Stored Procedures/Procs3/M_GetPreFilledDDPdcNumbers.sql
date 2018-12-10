IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_GetPreFilledDDPdcNumbers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_GetPreFilledDDPdcNumbers]
GO

	-- =============================================
-- Author	:	Ajay Singh(23th Dec 2015)
-- Description	:	Get Details of a cash required to prefilled data
-- =============================================
CREATE PROCEDURE [dbo].[M_GetPreFilledDDPdcNumbers]
	@PaymentDetailsId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN
		SELECT BankName,BranchName,DepositedDate,DepositedBy 
		FROM DCRM_PaymentDetails (NOLOCK)
		WHERE ID = @PaymentDetailsId
	END

END



