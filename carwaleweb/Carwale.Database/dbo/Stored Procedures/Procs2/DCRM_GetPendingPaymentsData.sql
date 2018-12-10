IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetPendingPaymentsData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetPendingPaymentsData]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(7th June 2015)
-- Description	:	Get data for pending installment 3 days before 	
-- execute [dbo].[DCRM_GetPendingPaymentsData]
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetPendingPaymentsData]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--pending payment 
    SELECT 
		DISTINCT DSD.DealerId,
		D.Organization AS Dealer,
		DPD.TransactionId,
		CONVERT(VARCHAR(25),DPD.ChequeDDPdcDate,106) AS PaymentDate,
		DPD.AmountReceived,
		CASE	WHEN 
					OU.Id IS NOT NULL THEN OU.Id
				ELSE(
						SELECT 
							TOP 1 AU.UserId 
						FROM 
							DCRM_ADM_UserDealers AU(NOLOCK)  
							INNER JOIN OprUsers OU(NOLOCK) ON OU.Id = AU.UserId
						WHERE AU.DealerId = DSD.DealerId AND AU.RoleId IN (3,4,5)--for sales and service field executives
					)	END AS OprUserId ,
		CASE	WHEN 
					OU.Id IS NOT NULL THEN OU.LoginId
				ELSE(
						SELECT 
							TOP 1 OU1.LoginId
						FROM 
							DCRM_ADM_UserDealers AU(NOLOCK)  
							INNER JOIN OprUsers OU1(NOLOCK) ON OU1.Id = AU.UserId
						WHERE AU.DealerId = DSD.DealerId AND AU.RoleId IN (3,4,5)--for sales and service field executives
					)	END AS LoginId ,
		CASE	WHEN 
					OU.Id IS NOT NULL THEN OU.UserName
				ELSE(
						SELECT 
							TOP 1 OU1.UserName
						FROM 
							DCRM_ADM_UserDealers AU(NOLOCK)  
							INNER JOIN OprUsers OU1(NOLOCK) ON OU1.Id = AU.UserId
						WHERE AU.DealerId = DSD.DealerId AND AU.RoleId IN (3,4,5)--for sales and service field executives
					)	END AS UserName 
	FROM 
		DCRM_PaymentDetails DPD(NOLOCK) 
		INNER JOIN DCRM_PaymentTransaction DPT(NOLOCK) ON DPD.TransactionId = DPT.TransactionId
		INNER JOIN DCRM_SalesDealer DSD(NOLOCK) ON DSD.TransactionId = DPD.TransactionId
		INNER JOIN Dealers D(NOLOCK) ON D.ID = DSD.DealerId
		LEFT JOIN DCRM_ADM_UserDealers DAU(NOLOCK) ON DAU.UserId = DPT.CreatedBy AND DAU.RoleId IN (3,5)--for sales and service field executives
		LEFT JOIN OprUsers OU(NOLOCK) ON OU.Id = DAU.UserId AND OU.IsActive = 1
	WHERE
		DATEDIFF(DAY,GETDATE(),DPD.ChequeDDPdcDate) BETWEEN 0 AND 3 
		AND D.Id NOT IN (535,3377)--excluding test dealers
	ORDER BY 
		D.Organization

END

