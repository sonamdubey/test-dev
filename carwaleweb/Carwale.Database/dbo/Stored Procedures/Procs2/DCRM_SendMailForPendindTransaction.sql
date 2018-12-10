IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_SendMailForPendindTransaction]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_SendMailForPendindTransaction]
GO

	

-- =============================================
-- Author	:	Vinay Kumar Prajapati  7th Desc 2015
-- Description: Used to get details of pending amount and send Mail to L2 AND L3(manager And Field Executive associated with dealer)
-- EXEC DCRM_SendMailForPendindTransaction
-- =============================================

CREATE PROCEDURE [dbo].[DCRM_SendMailForPendindTransaction]
	
AS
BEGIN
	
	 WITH CTE AS 
			  (SELECT DISTINCT DPT.TransactionId ,D.Organization As DealerName,
					DUD.UserId AS Executive,
					(SELECT OprUserId  FROM DCRM_ADM_MappedUsers WITH(NOLOCK) WHERE NodeRec=DAMU.NodeRec.GetAncestor(1)) AS Manager,
					DPT.TotalClosingAmount,
					DPT.DiscountAmount,
					DPT.FinalAmount,
					PT.Name AS PaymentType,
					(SELECT SUM(DPD.Amount) FROM DCRM_PaymentDetails DPD(NOLOCK) WHERE DPD.TransactionId = DPT.TransactionId) AS TotalCollectedProductAmount,
					(DPT.FinalAmount -  (SELECT SUM(DPD.Amount) FROM DCRM_PaymentDetails DPD(NOLOCK) WHERE   DPD.TransactionId = DPT.TransactionId) 
					 ) AS TotalPendingProductAmount,
					 (SELECT Top 1 DPDS.DepositedDate FROM DCRM_PaymentDetails DPDS(NOLOCK) WHERE DPDS.TransactionId = DPT.TransactionId ORDER BY DPDS.DepositedDate DESC  ) AS LastPaymentDate									

				FROM 
					DCRM_PaymentTransaction DPT(NOLOCK)
					INNER JOIN DCRM_PaymentDetails DPD(NOLOCK) ON DPT.TransactionId = DPD.TransactionId
					LEFT JOIN DCRM_PaymentType PT(NOLOCK) ON PT.PaymentTypeId = DPD.PaymentType AND PT.PaymentTypeId=3
					INNER JOIN DCRM_SalesDealer AS DSD WITH(NOLOCK) ON DSD.TransactionId=DPT.TransactionId
					INNER JOIN Dealers AS D WITH(NOLOCK) ON D.ID=DSD.DealerId
					INNER JOIN DCRM_ADM_UserDealers AS DUD WITH(NOLOCK) ON DUD.DealerId=DSD.DealerId
					--INNER JOIN OprUsers AS OU WITH(NOLOCK) ON OU.Id=DUD.UserId  --- Get  Executive Name  
					INNER JOIN DCRM_ADM_MappedUsers AS DAMU WITH(NOLOCK) ON DAMU.OprUserId=DUD.UserId
					WHERE PT.PaymentTypeId=3 
			)

	SELECT CT.TransactionId,CT.DealerName
		,CT.FinalAmount,CT.TotalCollectedProductAmount AS AmountPaid, CT.TotalPendingProductAmount AS AmountDue,CONVERT(VARCHAR(11),CT.LastPaymentDate,106) LastPaymentDate,
		 CT.Executive AS ExecutiveId,CT.Manager AS ManagerId,
		 SUBSTRING((SELECT ',' +PS.Name FROM DCRM_SalesDealer AS DSDS WITH(NOLOCK)
					   INNER JOIN Packages AS PS WITH(NOLOCK) ON PS.Id=DSDS.PitchingProduct  
					   WHERE DSDS.TransactionId=CT.TransactionId ORDER BY PS.Name FOR XML PATH('')),2,200) 
				      AS Product
	FROM CTE AS CT WITH(NOLOCK)
		INNER JOIN OprUsers AS OUManager WITH(NOLOCK) ON  OUManager.Id=CT.Manager
	WHERE CT.TotalPendingProductAmount>=1



	 ;WITH CTEUserWithManager AS (
		SELECT  (DPT.FinalAmount -  (SELECT SUM(DPD.Amount) FROM DCRM_PaymentDetails DPD(NOLOCK) WHERE   DPD.TransactionId = DPT.TransactionId) ) AS PendingAmount,
			  DUD.UserId AS ExecutiveId,
			(SELECT OprUserId  FROM DCRM_ADM_MappedUsers WITH(NOLOCK) WHERE NodeRec=DAMU.NodeRec.GetAncestor(1)) AS ManagerId
		FROM 
		DCRM_PaymentTransaction DPT(NOLOCK)
		INNER JOIN DCRM_PaymentDetails DPD(NOLOCK) ON DPT.TransactionId = DPD.TransactionId
		INNER JOIN DCRM_SalesDealer AS DSD WITH(NOLOCK) ON DSD.TransactionId=DPT.TransactionId
		INNER JOIN DCRM_ADM_UserDealers AS DUD WITH(NOLOCK) ON DUD.DealerId=DSD.DealerId
		INNER JOIN DCRM_ADM_MappedUsers AS DAMU WITH(NOLOCK) ON DAMU.OprUserId=DUD.UserId
		LEFT JOIN DCRM_PaymentType PT(NOLOCK) ON PT.PaymentTypeId = DPD.PaymentType AND PT.PaymentTypeId=3
		WHERE PT.PaymentTypeId=3 
		)

		--Mail to executive And manager when pending amount available.
		SELECT DISTINCT CUM.ExecutiveId , REPLACE(ISNULL(OUExecutive.LoginId,''), '@carwale.com' ,'')  AS  ExecutiveEmail ,OUExecutive.UserName AS ExecutiveName  ,
		  CUM.ManagerId , REPLACE(ISNULL(OUManager.LoginId,'') , '@carwale.com' ,'') AS ManagerEmail, OUManager.UserName AS ManagerName
		 FROM CTEUserWithManager  AS CUM WITH(NOLOCK) 
		LEFT JOIN OprUsers AS OUExecutive WITH(NOLOCK) ON OUExecutive.Id=CUM.ExecutiveId
		LEFT JOIN OprUsers AS OUManager WITH(NOLOCK) ON OUManager.Id=CUM.ManagerId
		WHERE CUM.PendingAmount>=1 ORDER BY CUM.ManagerId
						 
END


