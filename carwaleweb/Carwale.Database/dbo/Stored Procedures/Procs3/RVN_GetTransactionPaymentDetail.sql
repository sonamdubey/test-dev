IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RVN_GetTransactionPaymentDetail]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RVN_GetTransactionPaymentDetail]
GO

	-- =============================================
-- Author	:	Sachin Bharti(17th Dec 2014)
-- Description	:	Get transaction payment details for particular transaction
-- =============================================
CREATE PROCEDURE [dbo].[RVN_GetTransactionPaymentDetail]-- 598
	
	@TransactionId	INT

AS
BEGIN
	
	SET NOCOUNT ON;
	
	--Check if TransactionId is exist or not
	SELECT DSD.Id FROM DCRM_SalesDealer DSD(NOLOCK) WHERE DSD.TransactionId = @TransactionId
	IF @@ROWCOUNT > 0 
	BEGIN

		--Get transaction details for that transactionId
		SELECT 
			DISTINCT D.Id AS DealerId,
			D.Organization AS DealerName,
			C.Name AS City,
			D.TC_DealerTypeId AS DealerType	,
			ISNULL(DPT.TotalClosingAmount,0)	AS ClosingAmount,
			ISNULL(DPT.DiscountAmount,0)AS	DiscountAmount,
			ISNULL(DPT.ProductAmount,0) AS 	ProductAmount
			
		FROM 
			DCRM_PaymentTransaction DPT(NOLOCK)
			INNER JOIN DCRM_SalesDealer DSD(NOLOCK) ON DPT.TransactionId = DSD.TransactionId AND DPT.TransactionId = @TransactionId
			INNER JOIN Dealers	D(NOLOCK)	ON D.ID = DSD.DealerId AND DSD.TransactionId = @TransactionId
			INNER JOIN Cities	C(NOLOCK)	ON D.CityId = C.ID
			INNER JOIN Packages PK(NOLOCK)	ON PK.Id = DSD.PitchingProduct
			INNER JOIN InquiryPointCategory IPC(NOLOCK) ON IPC.Id = PK.InqPtCategoryId
			INNER JOIN TC_DealerType DT(NOLOCK)		ON D.TC_DealerTypeId = DT.TC_DealerTypeId 
		WHERE 
			DPT.TransactionId = @TransactionId
		
		--Get product names for that transaction and in case of RSA
		--packages get qantity also

		SELECT 
			PK.Name AS PackageName,
			ISNULL(DSD.Quantity,0) AS Quantity,
			CASE WHEN (IPC.Id = 33 OR IPC.Id = 37) THEN 'visible' ELSE 'hide' END AS IsRSA,--for RSA and Warranty based products
			CASE WHEN (IPC.Id = 33 OR IPC.Id = 37) THEN 'isRSA' ELSE '' END AS IsRSAProduct

		FROM 
			DCRM_SalesDealer DSD(NOLOCK)
			INNER JOIN Packages PK(NOLOCK) ON PK.Id = DSD.PitchingProduct
			INNER JOIN InquiryPointCategory IPC(NOLOCK) ON IPC.Id = PK.InqPtCategoryId 
			INNER JOIN DCRM_PaymentTransaction DPT(NOLOCK) ON DPT.TransactionId = DSD.TransactionId
		WHERE
			DSD.TransactionId = @TransactionId
		ORDER BY 
			PK.Name 
	END
END
