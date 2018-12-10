IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_DealerDetail]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_DealerDetail]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(3rd Dec 2014)
-- Description	:	Get Dealer details for m site based on dealerId
-- Modifier	:	Sachin Bharti(3rd July 2015)
-- Purpose	:	Remove query based daler type
-- mOdifier :KArtiK Rathod on 17 nov,fetched legalname 
-- Purpose	:	Amit Yadav (8th Feb 2016)
-- Modifier	:	To get the legalename and check if the dealer is legal or not.
-- Modified by : Sunil Yadav On 16 feb 2016 
-- description : check legal incase of UCD,UCD-NCD.
-- Modified by : Sunil Yadav On 16 feb 2016 
-- description : made dealerType allow null 
-- =============================================
CREATE PROCEDURE [dbo].[M_DealerDetail]
	
	@DealerId	INT,
	@DealerType	SMALLINT =null, -- Modified by : Sunil Yadav On 16 feb 2016 
	@UserId		INT=null
AS
BEGIN
	
	SET NOCOUNT ON;

	--get dealer details
	SELECT TOP 1
			D.Organization AS DealerName,
			D.Id AS DealerId,
			CASE WHEN ISNULL(D.Status,1) = 0 THEN 'active' WHEN ISNULL(D.Status,1) = 1 THEN 'inActive' END AS DealerStatus,
			CASE WHEN ISNULL(D.Status,1) = 0 THEN 1 WHEN ISNULL(D.Status,1) = 1 THEN 0 END AS DealerStatus,
			D.Address1 AS DealerAddress , 
			D.TC_DealerTypeId AS DealerType,
			ISNULL(DC.Id,0) AS CallId,
			D.LegalName AS LegalName,
			CASE WHEN  D.TC_DealerTypeId IN(2,3) AND (D.TanNumber IS NULL OR D.PanNumber IS NULL OR D.LegalName IS NULL)  THEN 0 -- only for NCD and NCD -UCD
			ELSE 1 END AS Legal
		FROM
			Dealers D (NOLOCK) 
			LEFT JOIN DCRM_Calls DC(NOLOCK)  ON DC.DealerId = D.ID 
				AND D.ID = @DealerId
				AND DC.ActionTakenId = 2
				AND DC.UserId = @UserId
			WHERE D.ID = @DealerId

	-- get dealer running package details
	SELECT  top 2
			CCP.ConsumerId , 
			IPC.Name,CCP.Points,
			CONVERT(CHAR,CCP.ExpiryDate,106) AS ExpiryDate ,CCP.CustomerPackageId AS PackageID
	FROM 
			ConsumerCreditPoints CCP WITH(NOLOCK) 
			INNER JOIN InquiryPointCategory IPC WITH(NOLOCK) ON CCP.PackageType = IPC.Id  
	WHERE 
			CCP.ConsumerId = @DealerId AND 
			CCP.ExpiryDate > GETDATE() AND CCP.ConsumerType = 1 --for dealer only
	ORDER BY 
			CCP.ExpiryDate DESC
END




-----------------------------------------------------------------------------------------


