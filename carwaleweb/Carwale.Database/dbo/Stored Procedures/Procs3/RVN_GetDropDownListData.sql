IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RVN_GetDropDownListData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RVN_GetDropDownListData]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(23rd July 2015)
-- Description	:	Get data for Packages , Completed Transaction Dealers and 
--					Generated Invoice Dealers 
-- EXECUTE RVN_GetDropDownListData
-- Modifier : Amit Yadav(17th Nov 2015)
-- Purpose : Get the Payment Modes and Cheque numbers.
--Modifier : Ajay Singh(on 4 jan 2016) dealerid with dealername
-- =============================================
CREATE PROCEDURE [dbo].[RVN_GetDropDownListData]

AS
BEGIN
	
	SET NOCOUNT ON;

	--get completed transaction daelers
	EXECUTE RVN_GetCompletedTrnsactionDealers

	--get invoice generated dealers
	SELECT 
		DISTINCT D.ID ,
		D.Organization + ' ( ' +CAST(D.ID AS VARCHAR)+' ) ' AS Text
	FROM
		M_GeneratedInvoice MGI(NOLOCK)
		INNER JOIN DCRM_SalesDealer DSD(NOLOCK) ON MGI.TransactionId = DSD.TransactionId
		INNER JOIN Dealers D(NOLOCK) ON D.Id = DSD.DealerId
	ORDER BY
		Text

	--get invoice status
	EXECUTE M_GetInvoiceStatus

	--Get data for States
	SELECT 
		ID, NAME AS Text 
	FROM 
		States (NOLOCK) 
	WHERE 
		IsDeleted = 0 ORDER BY Name

	--get packages
	SELECT 
		PK.Id,
		PK.Name AS Text
	FROM 
		Packages PK(NOLOCK)
	WHERE
		PK.isActive = 1 -- active 
		AND PK.ForDealer = 1 --for dealer
		ORDER BY
		Name

	--get dealer type
	EXECUTE [dbo].[DCRM_GetDealerType]

	--Get payment mode
	SELECT 
		PM.Id AS Id,
		PM.ModeName AS Name
	FROM
		PaymentModes PM (NOLOCK)
	ORDER BY
		Name

	--Get Cheque No.s
	SELECT 
		DPD.CheckDDPdcNumber AS Id,
		DPD.CheckDDPdcNumber AS Cheque
	FROM 
		DCRM_PaymentDetails DPD WITH(NOLOCK)
	WHERE
		DPD.Mode=2 --For Cheque
	ORDER BY
		Cheque 

	--Get Application
	SELECT  
		TA.ApplicationId AS Id,
		TA.ApplicationName AS Name
	FROM 
		TC_Applications AS TA WITH(NOLOCK)
END

