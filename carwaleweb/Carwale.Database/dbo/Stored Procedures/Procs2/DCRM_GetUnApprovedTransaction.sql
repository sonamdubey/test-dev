IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetUnApprovedTransaction]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetUnApprovedTransaction]
GO

	

-- =============================================
-- Author	:	Sachin Bharti(13th July 2015)
-- Description	:	Get transaction data those are not approved or rejected by 
--					account person created by field executives
-- Execute DCRM_GetUnApprovedTransaction
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetUnApprovedTransaction]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		SELECT 
			DISTINCT DPD.TransactionId ,
			PM.Id AS ModeId,
			PM.ModeName,
			DPT.FinalAmount,
			DPD.AmountReceived,
			CASE 
				WHEN DPD.Mode IN (2,3,8) THEN DPD.CheckDDPdcNumber --for Cheque,DD,PDC
				WHEN DPD.Mode = 4 THEN DPD.UtrTransactionId --for Online
			END AS 	ModeDetail,
			CASE 
				WHEN DPD.Mode IN (2,3,8) THEN CONVERT(VARCHAR(25),DPD.ChequeDDPdcDate,106) --for Cheque,DD,PDC
				WHEN DPD.Mode IN (1,4) THEN CONVERT(VARCHAR(25),DPD.PaymentDate,106) --for Cash and Online
			END AS 	PaymentDate,
			DPD.BankName,
			DPD.DrawerName,
			CASE 
				WHEN OU.Id IS NOT NULL THEN OU.Id
				ELSE(
						SELECT 
							TOP 1 OU.Id 
						FROM 
							DCRM_ADM_UserDealers AU(NOLOCK) 
							INNER JOIN OprUsers OU(NOLOCK) ON OU.Id = AU.UserId
						WHERE 
							AU.DealerId = DSD.DealerId AND AU.RoleId IN (3,4,5)--for sales and service field executives
					)	END AS OprUserId,
			CASE 
				WHEN OU.Id IS NOT NULL THEN OU.UserName
				ELSE(
						SELECT 
							TOP 1 OU.UserName 
						FROM 
							DCRM_ADM_UserDealers AU(NOLOCK) 
							INNER JOIN OprUsers OU(NOLOCK) ON OU.Id = AU.UserId
						WHERE 
							AU.DealerId = DSD.DealerId AND AU.RoleId IN (3,4,5)--for sales and service field executives
					)	END AS UserName,
			CASE 
				WHEN OU.Id IS NOT NULL THEN OU.LoginId
				ELSE(
						SELECT 
							TOP 1 OU.LoginId 
						FROM 
							DCRM_ADM_UserDealers AU(NOLOCK) 
							INNER JOIN OprUsers OU(NOLOCK) ON OU.Id = AU.UserId
						WHERE 
							AU.DealerId = DSD.DealerId AND AU.RoleId IN (3,4,5)--for sales and service field executives
					)	END AS LoginId,
			ISNULL(CASE	WHEN 
				OU.Id IS NOT NULL THEN OU.PhoneNo
				ELSE(
						SELECT 
							TOP 1 AU.UserId 
						FROM 
							DCRM_ADM_UserDealers AU(NOLOCK) 
							INNER JOIN OprUsers OU(NOLOCK) ON OU.Id = AU.UserId
						WHERE 
							AU.DealerId = DSD.DealerId AND AU.RoleId IN (3,4,5)--for sales and service field executives
					)	END,-1) AS MobileNo 
	FROM	
		DCRM_PaymentTransaction DPT(NOLOCK)
		INNER JOIN DCRM_PaymentDetails DPD(NOLOCK) ON DPT.TransactionId = DPD.TransactionId AND ISNULL(DPD.IsApproved,0) = 0
		INNER JOIN PaymentModes PM(NOLOCK) ON PM.Id = DPD.Mode
		INNER JOIN DCRM_SalesDealer DSD(NOLOCK) ON DSD.TransactionId = DPD.TransactionId
		LEFT JOIN OprUsers OU(NOLOCK) ON OU.Id = DPT.CreatedBy AND OU.IsActive = 1
	WHERE
		DPT.FinalAmount > 0 -- excluding free and trials
		AND DPT.CreatedBy <> 3
		AND DSD.DealerId NOT IN (535,3377)--excluding test dealers
	ORDER BY 
		DPd.TransactionId DESC

END

