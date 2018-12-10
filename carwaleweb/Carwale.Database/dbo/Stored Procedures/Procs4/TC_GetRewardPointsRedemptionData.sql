IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetRewardPointsRedemptionData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetRewardPointsRedemptionData]
GO

	



-- =============================================
-- Author:		<>
-- Create date: <>
-- Description:	get reward Point Redemption  
-- Modified  By : Vinay kumar prajapati  Impliment PayUMoney  (Get walletId)
-- Modified By: Komal Manajre on(01-Feb-2015)
-- Description:get package Name,amount and last payment date
-- Modified By Kartik Rathod on 28 Mar 2016,added condition for Date range,Dealerid and Request type
-- [TC_GetRewardPointsRedemptionData]  1,'2011-01-01','2016-04-01',null,3
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetRewardPointsRedemptionData] 
@ApplicationId	INT,
@DateFrom DATETIME,
@DateTo DATETIME,
@DealerId INT = NULL,
@RequestType SMALLINT= NULL
AS
BEGIN


DECLARE @TempPackage TABLE(DealerId INT,CurrentPackageName VARCHAR(300),AmountPaid INT,LastPaymentDate DATE)

INSERT INTO @TempPackage(DealerId,CurrentPackageName,AmountPaid,LastPaymentDate)
SELECT DealerId,CurrentPackageName,AmountPaid,LastPaymentDate 
FROM 
	(SELECT   D.Id AS DealerId , Pkg.Name AS  CurrentPackageName,CPR.ActualAmount AS Amountpaid,
	CASE WHEN CPR.ApprovalDate IS NOT NULL THEN CPR.ApprovalDate ELSE '' END AS LastPaymentDate
	,(ROW_NUMBER()OVER (PARTITION BY D.Id ORDER BY Cpr.ApprovalDate DESC)) AS ROWNUM 

	FROM ConsumerPackageRequests Cpr(NOLOCK)
	INNER JOIN Packages Pkg(NOLOCK)  ON Pkg.Id = Cpr.PackageId  AND Cpr.ConsumerType = 1 AND Cpr.IsActive = 1 AND Pkg.IsActive = 1
	JOIN Dealers AS D WITH(NOLOCK) ON D.id=Cpr.ConsumerId
	) T where ROWNUM = 1

	SELECT DISTINCT D.ID ,D.Organization +' ('+CONVERT(VARCHAR,D.ID)+')' Organization,RP.Denomination,RP.RedeemDate,RP.Quantity,
	--SELECT D.ID ,D.Organization,RP.Denomination,RP.RedeemDate,RP.Quantity,
	RP.RedeemedPoints,ISNULL(RP.RedeemedAmount,0) AS RedeemedAmount,RP.EmailSentOn AS EmailId ,d.MobileNo,RP.RequestType,RP.Id AS TC_RedeemedPointsID,RP.SentDate--,RL.Comment,RP.ApprovalDate,
	,ISNULL(STUFF((Select ','+  (CASE WHEN RL1.Comment <>' ' THEN (RL1.Comment + '-' + CONVERT(VARCHAR,RL1.ActionTakenOn)) ELSE ''  END)
		FROM TC_RedeemedPointsLog RL1 WITH(NOLOCK)
		WHERE RP.Id = RL1.TC_RedeemedPointsId
		ORDER BY RL1.TC_RedeemedPointsLogId 
		FOR XML PATH('')),1,1,''),'')AS COMMENTS
		,U.UserName + '-' + D.Organization AS Recipient
		,EW.Name AS RedemptionType
		--, CASE  RP.RequestType  WHEN 1 THEN 'Pending' WHEN 2 THEN 'Approved' WHEN 3 THEN 'Sent'  END  AS Status
		, RP.PayUMoneyTransactionId AS TransactionId 
		,RP.PayUMoneyMessage AS Message 
		,CASE  RP.PayUMoneyStatus  WHEN 1 THEN 'Success' ELSE 'Pending'  END  AS Status
		,ISNULL(RP.TC_EWalletsId,1) AS WalletsId -- Default 1 for flipkart Added By vinay Kumar Prajapati  11th jan 2016
		,TP.CurrentPackageName,TP.AmountPaid,TP.LastPaymentDate
		 

	FROM TC_RedeemedPoints AS  RP WITH(NOLOCK)
	LEFT JOIN TC_RedeemedPointsLog RL WITH(NOLOCK) ON RL.TC_RedeemedPointsId = RP.Id
	INNER JOIN Dealers D WITH(NOLOCK) ON RP.DealerId = D.ID
	LEFT JOIN TC_Users AS U WITH(NOLOCK) ON U.Id=RP.UserId
	LEFT JOIN TC_EWallets AS EW WITH(NOLOCK) ON EW.Id=RP.TC_EWalletsId AND EW.IsActive=1
	LEFT JOIN ConsumerCreditPoints AS CCP WITH(NOLOCK) ON CCP.ConsumerId=D.ID
	LEFT JOIN Packages AS P WITH(NOLOCK) ON P.Id=CCP.CustomerPackageId
	LEFT JOIN @TempPackage AS TP ON TP.DealerId = RP.DealerId
	WHERE D.ApplicationId = @ApplicationId AND Convert(Date,RP.RedeemDate) BETWEEN  Convert(Date,@DateFrom) AND Convert(Date,@DateTo) AND  (@RequestType IS NULL OR Rp.RequestType = @RequestType) AND (@DealerId IS NULL OR D.Id = @DealerId)
	ORDER BY RP.Id DESC
END
---------------------------------------------------------------------------------------------------------------------



