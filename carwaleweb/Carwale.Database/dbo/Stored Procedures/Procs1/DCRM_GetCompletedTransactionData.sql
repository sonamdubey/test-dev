IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetCompletedTransactionData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetCompletedTransactionData]
GO
	
-- =============================================
-- Author	:	Sachin Bharti(17th Dec 2014)
-- Description	:	Get transaction details for account approval 
--					and make campaign running
-- Modifier	:	Sachin Bharti(12th Jan 2015)
-- Purpose	:	Get one more column is transaction for free trial or not
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetCompletedTransactionData]
	
	@StateId	INT = NULL,
	@CityId		INT = NULL,
	@DealerId	NUMERIC(18,0) = NULL,
	@DealerName VARCHAR(200) = NULL

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT	
			DISTINCT DPT.TransactionId,
			D.ID AS DealerId,	
			D.Organization ,	
			D.StateId AS StateId,	
			D.TC_DealerTypeId ,
			TD.DealerType ,
			C.ID AS CityId,
			C.Name AS CityName,
			TD.DealerType,
			DPT.TotalClosingAmount,
			DPT.DiscountAmount,
			DPT.ProductAmount,
			DPT.TDSAmount,
			ISNULL(DPT.IsTDSGiven,0) AS IsTDS,
			DPT.ServiceTax,
			DPT.FinalAmount,
			DPT.CreatedOn AS NewCreatedOn,
			CONVERT(VARCHAR,DPT.CreatedOn,107) AS CreatedOn,
			OU.UserName AS CreatedBy,
			CASE WHEN ISNULL(DSD.IsFreeTrial,0) = 0 THEN 0 WHEN ISNULL(DSD.IsFreeTrial,0) = 1 THEN 1 END AS IsFreeTrial

	FROM	
			DCRM_PaymentTransaction DPT(NOLOCK) 
			INNER	JOIN DCRM_SalesDealer DSD(NOLOCK) ON DSD.TransactionId = DPT.TransactionId
					AND DSD.ClosingProbability = 90	AND DSD.LeadStatus = 2 --AND D.Status = 0 -- Packages having closed status and dealer is active and get converted	 
			INNER	JOIN Dealers D(NOLOCK) ON D.ID = DSD.DealerId 
			LEFT	JOIN Cities C (NOLOCK) ON C.ID = D.CityId 
			LEFT	JOIN TC_DealerType TD(NOLOCK) ON TD.TC_DealerTypeId = D.TC_DealerTypeId 
			LEFT	JOIN OprUsers OU(NOLOCK) ON OU.Id = DPT.CreatedBy
	WHERE 	
			(@CityId	IS NULL OR D.CityId = @CityId)	AND
			(@StateId	IS NULL OR D.StateId = @StateId)AND 
			(@DealerId	IS NULL OR D.ID = @DealerId)	AND
			(@DealerName IS NULL OR D.Organization LIKE '%'+@DealerName+'%')
	ORDER BY	
			DPT.CreatedOn DESC
END

