IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_GetPendingPayment]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_GetPendingPayment]
GO

	-- =============================================
-- Author:		Amit Yadav
-- Create date: 3rd Nov 2015
-- Description:	To get the pending payment details for dealer. 
--EXEC M_GetPendingPayment 4271
-- Modified By : Sunil M. Yadav On 1st July 2016 , Get product delivery start date comma separated for same transation.
-- =============================================
CREATE PROCEDURE [dbo].[M_GetPendingPayment] 

	@DealerId	INT = NULL,
	@UserId INT = NULL
AS
BEGIN
	
	SET NOCOUNT ON;
	--Get the transactionid, pending amount fro that transaction and package start date. 
	WITH CTE AS 
	(
    SELECT DISTINCT 
		DPT.TransactionId,

		(SELECT OU.UserName FROM DCRM_ADM_MappedUsers AS DAM1(NOLOCK) LEFT JOIN OprUsers OU(NOLOCK) ON OU.ID= DAM1.OprUserId WHERE DAM1.NodeRec=DAMU.NodeRec.GetAncestor(1)) AS L2Name,

		(SELECT OU.Id FROM DCRM_ADM_MappedUsers AS DAM1(NOLOCK) LEFT JOIN OprUsers OU(NOLOCK) ON OU.ID= DAM1.OprUserId WHERE DAM1.NodeRec=DAMU.NodeRec.GetAncestor(1)) AS L2Id,
		(ISNULL(DPT.ProductAmount,0) - ISNULL((SELECT SUM(ISNULL(DPD.Amount,0)) 
		FROM DCRM_PaymentDetails DPD(NOLOCK) WHERE DPD.TransactionId = DPT.TransactionId AND ISNULL(DPD.IsApproved,1)=1 ),0))  AS CollectionPendingProductAmount,
		--(DPT.FinalAmount - (SELECT SUM(DPD.Amount) FROM DCRM_PaymentDetails DPD(NOLOCK) WHERE DPD.TransactionId = DPT.TransactionId AND DPD.PaymentType IN(1,2,3) AND ISNULL(DPD.IsApproved,1) = 1))  AS CollectionPendingProductAmount, --either approved or not yet approved))
	
		SUBSTRING((SELECT ',' + CONVERT(VARCHAR(11),RDPF.PackageStartDate,106)				-- Sunil M. Yadav On 1st July 2016 , Get product delivery start date comma separated for same transation.
					FROM RVN_DealerPackageFeatures AS RDPF WITH(NOLOCK)					
					   WHERE RDPF.TransactionId=DPT.TransactionId ORDER BY RDPF.ProductSalesDealerId FOR XML PATH('')),2,200) 
				      AS StartDate,

		-- CONVERT(VARCHAR(11),RVN.PackageStartDate,106) AS StartDate,
		
		OU.UserName AS L3Name,
		
		OU.LoginId + '@carwale.com' AS  MailTo,
		
		DAMU.UserLevel,

		D.Organization AS DealerName,
		  
		SUBSTRING((SELECT ',' +PS.Name FROM DCRM_SalesDealer AS DSDS WITH(NOLOCK)
					   INNER JOIN Packages AS PS WITH(NOLOCK) ON PS.Id=DSDS.PitchingProduct  
					   WHERE DSDS.TransactionId=DPT.TransactionId ORDER BY DSDS.Id FOR XML PATH('')),2,200) 
				      AS Product

		FROM DCRM_SalesDealer(NOLOCK) AS DSD
		INNER JOIN Dealers AS D(NOLOCK) ON D.ID=DSD.DealerId
		INNER JOIN DCRM_PaymentTransaction(NOLOCK) AS DPT ON DPT.TransactionId=DSD.TransactionId AND DPT.IsActive=1
		INNER JOIN RVN_DealerPackageFeatures(NOLOCK)AS RVN ON RVN.TransactionId = DPT.TransactionId
		LEFT JOIN DCRM_ADM_UserDealers AS DUD (NOLOCK) ON DUD.DealerId=DSD.DealerId AND DUD.RoleId = 3
		LEFT JOIN DCRM_ADM_MappedUsers AS DAMU (NOLOCK) ON DAMU.OprUserId=DUD.UserId
		LEFT JOIN OprUsers AS OU(NOLOCK) ON OU.Id=DUD.UserId
		
		WHERE 
		DSD.DealerId = @DealerId
		AND (@UserId IS NULL OR DUD.UserId=@UserId OR OU.IsActive=1)
		AND DPT.TransactionId IS NOT NULL  
		AND DPT.IsActive = 1
	    --AND DPT.CreatedOn < GETDATE() --added by ajay singh on 18 jan 2016 to show todays transaction also in pending transaction
		--AND CONVERT(DATE,DPT.CreatedOn) < CONVERT(DATE,GETDATE()) --COMMENTED BY Ajay Singh(18 jan 2016) to show all pending transaction
	)
		
	SELECT CTE.CollectionPendingProductAmount,CTE.DealerName,CTE.L2Id,CTE.L2Name,CTE.L3Name,CTE.MailTo,CTE.Product,CTE.StartDate,CTE.TransactionId,OU1.LoginId + '@carwale.com' AS MailToL2 FROM CTE
	LEFT JOIN OprUsers AS OU1 WITH(NOLOCK) ON CTE.L2Id= OU1.Id 
	WHERE CTE.CollectionPendingProductAmount>=6

	--SELECT '1200' CollectionPendingProductAmount,'AEPL' DealerName,'3' L2Id,'AMit' L2Name,'Ajya' L3Name,'ajay.singh@carwale.com' MailTo,'leads' Product,'12/1/2015' StartDate,'123344' TransactionId,'amit.yadav@carwale.com' AS MailToL2  

END