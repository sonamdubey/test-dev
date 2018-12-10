IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_GetClearBalanceData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_GetClearBalanceData]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(4th May 2015)
-- Description	:	Get trancation payment details data
-- execute [dbo].[M_GetClearBalanceData] 10952
--Modifier : Vaibhav K 10-Dec-2015
--Added new column for opr user id of the transaction creator 'CreatedById'
-- =============================================
CREATE PROCEDURE [dbo].[M_GetClearBalanceData]
	@TransactionPaymentId	INT 
AS
BEGIN
	SELECT 
		DISTINCT DPD.TransactionId ,
		PM.Id AS ModeId,
		PM.ModeName,
		DPD.AmountReceived,
		CASE 
			WHEN DPD.Mode IN (2,3,8) THEN DPD.CheckDDPdcNumber --for Cheque,DD,PDC
			WHEN DPD.Mode = 4 THEN DPD.UtrTransactionId --for Online
		END AS 	ModeDetail,
		CASE 
			WHEN DPD.Mode IN (2,3,8) THEN CONVERT(VARCHAR(25),DPD.ChequeDDPdcDate,106) --for Cheque,DD,PDC
			WHEN DPD.Mode IN (1,4) THEN DPD.UtrTransactionId --for Cash and Online
		END AS 	PaymentDate,
		DPD.BankName,
		DPD.DrawerName,
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
				)	END AS CreatedBy,
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
				)	END,-1) AS MobileNo,
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
				)	END AS CreatedById
	FROM	
		DCRM_PaymentDetails DPD(NOLOCK)
		INNER JOIN PaymentModes PM(NOLOCK) ON PM.Id = DPD.Mode
		INNER JOIN DCRM_SalesDealer DSD(NOLOCK) ON DSD.TransactionId = DPD.TransactionId
		LEFT JOIN DCRM_ADM_UserDealers DAU(NOLOCK) ON DAU.UserId = DPD.AddedBy
		LEFT JOIN OprUsers OU(NOLOCK) ON OU.Id = DAU.UserId AND OU.IsActive = 1
	WHERE
		DPD.ID = @TransactionPaymentId
END
