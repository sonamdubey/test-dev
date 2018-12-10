IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetIncompletePartPayments]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetIncompletePartPayments]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(8th June 2015)
-- Description	:	Get data for incomplete part payments 
--					to send weekly mail to field executives.
-- execute [dbo].[DCRM_GetIncompletePartPayments]
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetIncompletePartPayments]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT 
		DISTINCT DPT.TransactionId,
		DPT.FinalAmount ,
		(
			SELECT 
				TOP 1 CASE WHEN DPD1.Mode = 1 THEN CONVERT(VARCHAR(25),DPD1.PaymentDate,106) ELSE CONVERT(VARCHAR(25),DPD1.ChequeDDPdcDate,106) END 
			FROM 
				DCRM_PaymentDetails DPD1(NOLOCK) 
			WHERE 
				DPD1.TransactionId = DPT.TransactionId AND 
				DPD1.PaymentType IN (3) AND
				ISNULL(DPD1.IsApproved,1) = 1 --either approved or not approved yet
			ORDER BY
				(CASE WHEN DPD1.Mode = 1 THEN CONVERT(VARCHAR(25),DPD1.PaymentDate,106) ELSE CONVERT(VARCHAR(25),DPD1.ChequeDDPdcDate,106) END ) DESC
		)AS LastCollectionDate ,
		(
			SELECT 
				ISNULL((DPT.FinalAmount - SUM(DPD2.Amount)) ,0) 
			FROM 
				DCRM_PaymentDetails DPD2(NOLOCK) 
			WHERE 
				DPD2.TransactionId = DPT.TransactionId AND 
				DPD2.PaymentType IN (3) AND
				ISNULL(DPD2.IsApproved,1) = 1 --either approved or not approved yet
		) AS OutStandingAmount,
		ISNULL(CASE	WHEN 
				OU.Id IS NOT NULL THEN OU.Id
				ELSE(
						SELECT TOP 1 AU.UserId FROM DCRM_ADM_UserDealers AU(NOLOCK) WHERE AU.DealerId = DSD.DealerId AND AU.RoleId IN (3,4,5)--for sales and service field executives
					)	END,-1) AS OprUserId ,
		CASE	WHEN 
					OU.Id IS NOT NULL THEN OU.LoginId
				ELSE(
						SELECT 
							TOP 1 OU1.LoginId
						FROM 
							DCRM_ADM_UserDealers AU(NOLOCK)  
							INNER JOIN OprUsers OU1(NOLOCK) ON OU1.Id = AU.UserId
						WHERE AU.DealerId = DSD.DealerId AND AU.RoleId IN (3,4,5)--for sales and service field executives
					)	END AS LoginId,
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
		DCRM_PaymentTransaction DPT(NOLOCK) 
		INNER	JOIN DCRM_SalesDealer DSD(NOLOCK) ON DSD.TransactionId = DPT.TransactionId 
		INNER	JOIN DCRM_PaymentDetails DPD(NOLOCK) ON DPD.TransactionId = DPT.TransactionId AND DPD.PaymentType = 3 --For part payments
		LEFT JOIN DCRM_ADM_UserDealers DAU(NOLOCK) ON DAU.UserId = DPT.CreatedBy AND DAU.RoleId IN (3,4,5)--for sales ,service field and back office executives
		LEFT JOIN OprUsers OU(NOLOCK) ON OU.Id = DAU.UserId AND OU.IsActive = 1
	WHERE
		DSD.DealerId NOT IN (535,3377)--excluding test dealers
	GROUP BY 
		DPT.TransactionId,DPT.FinalAmount,DSD.DealerId,OU.Id,OU.LoginId,OU.UserName
	HAVING 
		(DPT.FinalAmount - (SELECT SUM(DPD.Amount) FROM DCRM_PaymentDetails DPD(NOLOCK) WHERE DPD.TransactionId = DPT.TransactionId AND DPD.PaymentType IN (3) AND ISNULL(DPD.IsApproved,1) = 1) > 1)--either approved or not approved yet
	ORDER BY
		DPT.TransactionId DESC
END

