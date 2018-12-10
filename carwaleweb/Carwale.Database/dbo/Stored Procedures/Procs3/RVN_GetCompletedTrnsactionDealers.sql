IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RVN_GetCompletedTrnsactionDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RVN_GetCompletedTrnsactionDealers]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(24th April 2015)
-- Description	:	Get dealers name again whome transactions are created
-- Modifier : Amit Yadav(14th December 2015)
-- Purpose : Get the dealers w.r.t dealertype and city.
-- =============================================
CREATE PROCEDURE [dbo].[RVN_GetCompletedTrnsactionDealers]
	@CityId INT = NULL,
	@DealerType INT = NULL
AS
BEGIN
	SELECT 
		DISTINCT D.ID AS Value,
		D.Organization + ' ( ' +CAST(D.ID AS VARCHAR)+' ) ' AS Text 
	FROM 
		DCRM_PaymentTransaction DPT(NOLOCK)
		INNER JOIN DCRM_SalesDealer DSD(NOLOCK) ON DPT.TransactionId = DSD.TransactionId
		INNER JOIN Dealers D (NOLOCK) ON D.ID = DSD.DealerId
	WHERE
		(@CityId IS NULL OR D.CityId = @CityId )
		AND (@DealerType IS NULL OR D.TC_DealerTypeId = @DealerType )
	ORDER BY 
		Text
END



