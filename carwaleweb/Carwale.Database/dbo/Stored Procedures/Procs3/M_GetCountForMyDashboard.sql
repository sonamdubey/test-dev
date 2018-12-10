IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_GetCountForMyDashboard]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_GetCountForMyDashboard]
GO

	 
  
 
-- =============================================
-- Author:		Amit Yadav
-- Create date: 21th Dec 2015
-- Description:	Get the count for MyDashboard(API).
-- Modified By : Sunil Yadav On 25th march 2016
-- Description : Select UCD dealer also for executive , TC_DealerTypeId = 1
-- EXEC M_GetCountForMyDashboard 3
-- Modified By:Komal Manajre (28-April-2016)
-- Desc : filter count for dealer on basis of businessUnitId
-- Modified By:Komal Manjare on(20-May-2016)
-- get details for active transactions 
-- Modified By:Komal Manjare on 25 July 2016 ,Isdeleted condition for dealers insteadt of status check
-- =============================================
CREATE PROCEDURE [dbo].[M_GetCountForMyDashboard] 


		@UserId INT = NULL,
		@BusinessUnitId INT =NULL

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    
	DECLARE @Dealers TABLE(DealerId INT,DealerName VARCHAR(100))
	INSERT INTO @Dealers(DealerId,DealerName)
	SELECT D.ID,D.Organization FROM Dealers AS D WITH(NOLOCK)
	INNER JOIN DCRM_ADM_UserDealers AS DAUD WITH(NOLOCK) ON DAUD.UserId = @UserId AND DAUD.RoleId=3 AND DAUD.DealerId=D.ID
	WHERE 
	D.TC_DealerTypeId IN (@BusinessUnitId,3) AND 
	--D.Status = 0  
	D.IsDealerDeleted=0  -- Modified By:Komal Manjare on 25 July 2016 ,Isdeleted condition for dealers insteadt of status check
 --SELECT * FROM @Dealers
    
--YET TO BE CONVERTED PRODUCT
	SELECT COUNT(DISTINCT DSD.Id) AS YetToBeConverted
	FROM
	DCRM_SalesDealer AS DSD WITH(NOLOCK)
	INNER JOIN @Dealers AS D  ON D.DealerId = DSD.DealerId 
	WHERE
	DSD.LeadStatus=1 --Open product
	AND DSD.ClosingProbability < 90 --Less than closing probability

--CONVERTED PRODUCT
	SELECT COUNT(DISTINCT DSD.Id) AS Converted
	FROM
	DCRM_SalesDealer AS DSD WITH(NOLOCK)
	INNER JOIN @Dealers AS D  ON D.DealerId = DSD.DealerId 
	LEFT JOIN DCRM_PaymentDetails AS DPD WITH(NOLOCK) ON DPD.SalesDealerID = DSD.Id
	WHERE
	 DSD.LeadStatus=2 --Converted product
	 AND DSD.TransactionId IS NULL

--TRANSACTION

		--GET COUNT FOR COMPLETED TRANSACTION
	SELECT COUNT( DISTINCT DPT.TransactionId) AS CompletedTransaction
	FROM 
	DCRM_PaymentTransaction AS DPT WITH(NOLOCK)
	INNER JOIN DCRM_PaymentDetails DPD WITH(NOLOCK) ON DPD.TransactionId = DPT.TransactionId AND DPD.PaymentType IN(1,2,3) AND DPT.IsActive=1 --Added By:Komal Manjare(20-05-2016) get active transaction 
	INNER JOIN DCRM_SalesDealer AS DSD WITH(NOLOCK) ON DSD.TransactionId = DPT.TransactionId
	INNER JOIN @Dealers AS D  ON D.DealerId = DSD.DealerId 
	WHERE 
	(DPT.ProductAmount - (SELECT SUM(DPD.Amount)	FROM DCRM_PaymentDetails DPD WITH(NOLOCK) 
													WHERE DPD.TransactionId = DPT.TransactionId 
													AND DPD.PaymentType IN(1,2,3)			--For all types of payment(Part, Full, Installment)
													AND ISNULL(DPD.IsApproved,1) = 1) < 6 ) --Where payment is done. 
		--GET COUNT FOR INCOMPLETE TRANSACTION 
	SELECT COUNT(DISTINCT DPT.TransactionId) AS IncompleteTransaction
	FROM
	DCRM_PaymentTransaction AS DPT WITH(NOLOCK)
	INNER JOIN DCRM_PaymentDetails DPD WITH(NOLOCK) ON DPD.TransactionId = DPT.TransactionId AND DPD.PaymentType IN(1,2) AND DPT.IsActive=1 --Added By:Komal Manjare(20-05-2016) get active transaction 
	INNER JOIN DCRM_SalesDealer AS DSD WITH(NOLOCK) ON DSD.TransactionId = DPT.TransactionId
	INNER JOIN @Dealers AS D  ON D.DealerId = DSD.DealerId 
	WHERE 
	((DPT.ProductAmount - (SELECT ISNULL(SUM(DPD.Amount),0) FROM DCRM_PaymentDetails DPD WITH(NOLOCK) 
															WHERE	DPD.TransactionId = DPT.TransactionId 
															AND DPD.PaymentType IN (1,2)  --for full and installment payments only
															AND	ISNULL(DPD.IsApproved,1) = 1)--either approved or not approved yet
															) > 5)

		--GET COUNT FOR PART PAYMENT
	SELECT COUNT(DISTINCT DPT.TransactionId) AS PartPayment
	FROM
	DCRM_PaymentTransaction AS DPT WITH(NOLOCK)
	INNER JOIN DCRM_PaymentDetails DPD WITH(NOLOCK) ON DPD.TransactionId = DPT.TransactionId AND DPD.PaymentType IN(3) AND DPT.IsActive=1 --Added By:Komal Manjare(20-05-2016) get active transaction 
	INNER JOIN DCRM_SalesDealer AS DSD WITH(NOLOCK) ON DSD.TransactionId = DPT.TransactionId
	INNER JOIN @Dealers AS D  ON D.DealerId = DSD.DealerId 
	WHERE 
	(DPT.ProductAmount - (SELECT SUM(DPD.Amount) FROM DCRM_PaymentDetails DPD WITH(NOLOCK)
												 WHERE DPD.TransactionId = DPT.TransactionId 
												 AND DPD.PaymentType IN (3) 
												 --AND ISNULL(DPD.IsApproved,1) = 1--either approved or not approved yet
												 ) > 1)
--INVOICE
		--GENERATE INVOICES
	SELECT COUNT(DISTINCT DPT.TransactionId) AS GenerateInvoices
	FROM DCRM_PaymentTransaction DPT WITH(NOLOCK)
		INNER JOIN DCRM_PaymentDetails DPD WITH(NOLOCK) ON DPD.TransactionId = DPT.TransactionId AND ISNULL(DPT.ProductAmount,0) > 1  AND (( DPD.Mode = 8 AND DPD.ChequeDDPdcDate <=GETDATE()) OR DPD.Mode <> 8) AND DPT.IsActive=1 --Added By:Komal Manjare(20-05-2016) get active transaction 
		INNER JOIN DCRM_SalesDealer DSD WITH(NOLOCK) ON DSD.TransactionId = DPT.TransactionId AND ISNULL(DSD.CampaignType,3) IN (3,5) AND DSD.PitchingProduct <> 46 --Exclude free listing products.
		INNER JOIN @Dealers AS D  ON D.DealerId = DSD.DealerId 
		LEFT JOIN M_GeneratedInvoice MG WITH(NOLOCK) ON DPT.TransactionId = MG.TransactionId
	WHERE
	MG.TransactionId IS NULL 
	OR
	(((	SELECT ISNULL(SUM(DPD.Amount),0) FROM DCRM_PaymentDetails DPD(NOLOCK) 
						WHERE DPD.TransactionId = DPT.TransactionId  AND ( ( DPD.Mode = 8 AND DPD.ChequeDDPdcDate <=GETDATE()) OR DPD.Mode <> 8) AND ISNULL(DPD.IsApproved,1) =1)
					- 
					(	SELECT ISNULL(SUM(ISNULL(MG1.InvoiceAmount,0)),0) FROM M_GeneratedInvoice MG1 (NOLOCK)
						WHERE MG1.TransactionId = DPT.TransactionId AND ISNULL(MG1.Status,2) IN (1,2,5))--Taking approved and pending cases.
				) > 1)

		--APPROVED INVOICES
	SELECT COUNT(DISTINCT MGI.ID) AS ApprovedInvoice
	FROM
	M_GeneratedInvoice AS MGI WITH(NOLOCK) 
	INNER JOIN DCRM_PaymentTransaction AS DPT WITH(NOLOCK) ON DPT.TransactionId = MGI.TransactionId  AND DPT.IsActive=1 --Added By:Komal Manjare(20-05-2016) get active transaction 
	INNER JOIN DCRM_SalesDealer AS DSD WITH(NOLOCK) ON DSD.TransactionId = DPT.TransactionId
	INNER JOIN @Dealers AS D  ON D.DealerId = DSD.DealerId 
	WHERE 
	MGI.Status=2

	--CHEQUE/DD/PDC OR CASH
	SELECT
	COUNT(DISTINCT DPT.ID) AS PendingCheques
	FROM
	DCRM_PaymentDetails DPT WITH(NOLOCK)
	INNER JOIN DCRM_PaymentTransaction DP (NOLOCK) ON DP.TransactionId=DPT.TransactionId AND DP.IsActive=1 --Added By:Komal Manjare(20-05-2016) get active transaction and join 	
	INNER JOIN DCRM_SalesDealer DSD WITH(NOLOCK) ON DPT.TransactionId = DSD.TransactionId
	AND
	((
	DPT.Mode IN (2,3,8) --For Cheque,DD and PDC case only.
	AND ( ISNULL(DPT.BankName,'') = '' OR ISNULL(DPT.BranchName,'') = '')
	)
	OR
	(DPT.Mode = 1 --For Cash Only.
	AND ( ISNULL(DPT.DepSlipHostUrl,'') = '' OR ISNULL(DPT.DepSlipOriginalImgPath,'') = '')
	AND DPT.IsApproved IS NULL
	AND DPT.Amount > 0
	))
	INNER JOIN @Dealers AS D  ON D.DealerId = DSD.DealerId 

	--DEALERS NOT VISITED
	SELECT COUNT(DISTINCT DSM.DealerId) AS DealersNotVisited
	FROM DCRM_SalesMeeting AS DSM WITH(NOLOCK)
	INNER JOIN @Dealers AS D  ON D.DealerId = DSM.DealerId
	--WHERE
	--(CONVERT(DATE,DSM.MeetingDate)) NOT BETWEEN CONVERT(DATE,GETDATE()-7) AND CONVERT(DATE,GETDATE())
	--AND (CONVERT(DATE,DSM.MeetingDate) <  CONVERT(DATE,GETDATE()))
	GROUP BY DSM.DealerId
	HAVING
	MAX(CONVERT(DATE,DSM.MeetingDate)) NOT BETWEEN CONVERT(DATE,GETDATE()-7) AND CONVERT(DATE,GETDATE())
	AND MAX(CONVERT(DATE,DSM.MeetingDate)) <  CONVERT(DATE,GETDATE())
	
END

