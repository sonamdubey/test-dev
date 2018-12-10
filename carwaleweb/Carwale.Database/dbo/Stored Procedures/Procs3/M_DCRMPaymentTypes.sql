IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_DCRMPaymentTypes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_DCRMPaymentTypes]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(9th Dec 2014)
-- Description	:	Get all payment types for package 
--					payment details
-- Modified By : KARTIK Rathod on 17 Nov 2015, fetch PAN and TAN number from Dealers table
-- =============================================
CREATE PROCEDURE [dbo].[M_DCRMPaymentTypes]
	
	@DealerId INT 

AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT 
		PT.PaymentTypeId AS VALUE,
		PT.Name AS TEXT
	FROM 
		DCRM_PaymentType PT(NOLOCK)
		
	--Get PAN Number if exist for the dealer
	IF (SELECT PanNumber FROM Dealers WITH(NOLOCK) WHERE Id=@DealerId) IS NULL			--if pan is null in Dealers Table then fetch PAn from DCRM_PaymentDetails
		BEGIN
			SELECT 
				TOP 1 DPD.PANNumber,DPD.TANNumber
			FROM 
				DCRM_PaymentDetails DPD(NOLOCK) 
				INNER JOIN DCRM_SalesDealer DSD(NOLOCK) ON DSD.TransactionId = DPD.TransactionId 
			WHERE 
				DSD.DealerId = @DealerId AND DPD.PANNumber IS NOT NULL AND DPD.PANNumber <> '' 
		END
	ELSE
		BEGIN
			SELECT PanNumber AS PANNumber,TanNumber AS TANNumber FROM Dealers WITH(NOLOCK) WHERE Id=@DealerId
		END
END
 --=================================================


 
