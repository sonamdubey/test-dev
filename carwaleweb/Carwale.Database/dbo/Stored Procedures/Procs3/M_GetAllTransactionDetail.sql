IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_GetAllTransactionDetail]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_GetAllTransactionDetail]
GO

	
-- =============================================
-- Author	:	Sachin Bharti
-- Description	:	Get all done transaction detail complete , incomplete 
--					and part payment transaction detail
-- Modifier	:	Sachin Bharti(16th Jan 2015)
-- Modificatione	:	Move part payments completed transactions to 
--						all complete transactions
--					:	Also get all product name for that dealer 
--						those transaction created already
-- execute [dbo].[M_GetAllTransactionDetail]  3377
-- Modifier : Ajay Singh(30 dec 2015) get transcations order by createdon desc in the case of incomplete transcation
-- Author	: Amit Yadav (17-02-2016)
-- Purpose	: To get the transactions with rejected payments.
-- Modified By:Komal Manjare(21-JUne-2016)
-- Desc: get deliverystart date for the contract 
-- =============================================
CREATE PROCEDURE [dbo].[M_GetAllTransactionDetail] 

		@DealerId	INT 

	AS
BEGIN
	
	SET NOCOUNT ON;

	--Get all completed transaction for part payment type
	SELECT 
		DISTINCT DPT.TransactionId,
		DPT.TotalClosingAmount,
		DPT.ProductAmount AS FinalAmount,
		(
			SELECT 
				ISNULL(SUM(DPD1.PaymentReceived),0) 
			FROM 
				DCRM_PaymentDetails DPD1(NOLOCK) 
			WHERE 
				DPD1.TransactionId = DPT.TransactionId AND
				DPD1.PaymentType IN(1,2,3) AND
				ISNULL(DPD1.IsApproved,1) = 1 --either approved or not yet approved
		) AS FinalCollectionAmount,
		DPT.CreatedOn
	FROM 
		DCRM_PaymentTransaction DPT(NOLOCK) 
		INNER	JOIN DCRM_SalesDealer DSD(NOLOCK) ON  DSD.DealerId = @DealerId AND DSD.TransactionId = DPT.TransactionId
		INNER	JOIN DCRM_PaymentDetails DPD(NOLOCK) ON DPD.TransactionId = DPT.TransactionId AND DPD.PaymentType IN(1,2,3) --part payment type
	WHERE
		ISNULL(DPT.IsActive,1) = 1  
	GROUP BY  
		DPT.TransactionId,DPT.ProductAmount, DPT.TotalClosingAmount,DPT.CreatedOn
	HAVING	
			(	
				(DPT.ProductAmount - (SELECT ISNULL(SUM(DPD.Amount),0)	FROM DCRM_PaymentDetails DPD(NOLOCK) 
																WHERE DPD.TransactionId = DPT.TransactionId AND DPD.PaymentType IN(1,2,3) AND ISNULL(DPD.IsApproved,1) = 1) < 6 ) --part payment type
			)

	ORDER BY DPT.CreatedOn DESC

	--Get all incompleted transactions ie transaction is created but dont have 
	--any payment details in DCRM_PayemntDetails table or having less payment
	SELECT 
		DISTINCT DPT.TransactionId,
		DPT.TotalClosingAmount,
		DPT.DiscountAmount,
		DPT.ProductAmount AS FinalAmount,
		ISNULL(DPD.PaymentType,1) AS PaymentType,
		(
			SELECT 
				ISNULL(SUM(DPD1.Amount),0) 
			FROM 
				DCRM_PaymentDetails DPD1(NOLOCK) 
			WHERE 
				DPD1.TransactionId = DPT.TransactionId AND 
				DPD1.PaymentType IN (1,2) AND --for full and installment payments
				ISNULL(DPD1.IsApproved,1) = 1 --either approved or not approved yet
		) AS CollectedAmount,
		(
			SELECT 
				ISNULL((DPT.ProductAmount - ISNULL(SUM(DPD2.Amount),0)) ,0) 
			FROM 
				DCRM_PaymentDetails DPD2(NOLOCK) 
			WHERE 
				DPD2.TransactionId = DPT.TransactionId AND 
				DPD2.PaymentType IN (1,2) AND --for full and installment payments
				ISNULL(DPD2.IsApproved,1) = 1 --either approved or not approved yet
		) AS OutStandingAmount,
		DPT.CreatedOn
	FROM 
		DCRM_PaymentTransaction DPT(NOLOCK)
		INNER JOIN DCRM_SalesDealer DSD(NOLOCK) ON DSD.TransactionId = DPT.TransactionId AND DSD.DealerId = @DealerId
		INNER JOIN DCRM_PaymentDetails DPD(NOLOCK) ON DPD.TransactionId = DPT.TransactionId AND DPD.PaymentType IN (1,2)
	WHERE
		ISNULL(DPT.IsActive,1) = 1
	GROUP BY
		DPT.ProductAmount , DPT.TransactionId,DPT.TotalClosingAmount,DPT.TotalClosingAmount,DPT.DiscountAmount,DPT.CreatedOn,DPD.PaymentType
	HAVING 
		((DPT.ProductAmount - (SELECT ISNULL(SUM(DPD.Amount),0) FROM DCRM_PaymentDetails DPD(NOLOCK) 
																WHERE	DPD.TransactionId = DPT.TransactionId AND
																		DPD.PaymentType IN (1,2)  --for full and installment payments only
																		AND ISNULL(DPD.IsApproved,1) = 1--either approved or not approved yet
																		)
		) > 5)
	ORDER BY
    DPT.CreatedOn DESC ---added by Ajay Singh


	--get all part payment transactions with outstanding amount more than 5
	SELECT 
		DISTINCT DPT.TransactionId,
		DPT.TotalClosingAmount,
		ISNULL(DPT.DiscountAmount,0) AS DiscountAmount,
		DPT.ProductAmount AS FinalAmount,
		(
			SELECT 
				ISNULL(SUM(DPD1.Amount),0) 
			FROM 
				DCRM_PaymentDetails DPD1(NOLOCK) 
			WHERE 
				DPD1.TransactionId = DPT.TransactionId AND 
				DPD1.PaymentType IN (3) AND
				(ISNULL(DPD1.IsApproved,1) = 1 ) --either approved or not approved yet
		) AS CollectedAmount,
		(
			SELECT 
				ISNULL((DPT.ProductAmount - ISNULL(SUM(DPD2.Amount),0)) ,0) 
			FROM 
				DCRM_PaymentDetails DPD2(NOLOCK) 
			WHERE 
				DPD2.TransactionId = DPT.TransactionId AND 
				DPD2.PaymentType IN (3) AND
				(ISNULL(DPD2.IsApproved,1) = 1)  --either approved or not approved yet
		) AS OutStandingAmount,
		'click' AS IsCompleted,
		DPT.CreatedOn
	FROM 
		DCRM_PaymentTransaction DPT(NOLOCK) 
		INNER	JOIN DCRM_SalesDealer DSD(NOLOCK) ON  DSD.DealerId = @DealerId AND DSD.TransactionId = DPT.TransactionId
		INNER	JOIN DCRM_PaymentDetails DPD(NOLOCK) ON DPD.TransactionId = DPT.TransactionId AND DPD.PaymentType = 3--For part payments
	WHERE
		ISNULL(DPT.IsActive,1) = 1
	GROUP BY 
		DPT.TransactionId,DPT.TotalClosingAmount , DPT.ProductAmount,DPT.CreatedOn,ISNULL(DPT.DiscountAmount,0)
	HAVING 
		(DPT.ProductAmount - (SELECT ISNULL(SUM(DPD.Amount),0) FROM DCRM_PaymentDetails DPD(NOLOCK) WHERE DPD.TransactionId = DPT.TransactionId AND DPD.PaymentType IN (3) AND ISNULL(DPD.IsApproved,1) = 1) > 1) --either approved or not approved yet && --Added by Amit Yadav 

	ORDER BY 
		DPT.CreatedOn DESC

	--Get all products name for which tranactions are created for that dealer only
	-- Modified By:Komal Manjare(21-06-2016) get deliverystartdate for the product pitched
	SELECT 
		DISTINCT PK.Name AS Product,DSD.TransactionId, DSD.Id AS SalesId,
		CASE WHEN PK.Id in(70,59) THEN CONVERT(VARCHAR,TCC.StartDate,106) ELSE '' END AS DeliveryStartDate ,TCC.ContractId		
	FROM 
		DCRM_SalesDealer DSD(NOLOCK)
		INNER JOIN 	Packages PK(NOLOCK) ON PK.Id = DSD.PitchingProduct 
		LEFT JOIN RVN_DealerPackageFeatures RDP(NOLOCK) ON RDP.ProductSalesDealerId=DSD.Id
		LEFT JOIN TC_ContractCampaignMapping TCC(NOLOCK) ON TCC.ContractId=RDP.DealerPackageFeatureID
		
	WHERE 
		 DSD.DealerId = @DealerId AND DSD.TransactionId IS NOT NULL AND DSD.TransactionId > - 1	
END
-------------------------------------------------------------------------------------------------------
