IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[DCRM].[GetDealerPlanLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [DCRM].[GetDealerPlanLog]
GO

	


-- Description	:	Get Dealer ID, Name, Plan, Expirydate from Log
-- Author		:	Dilip V. 20-Mar-2012
-- Modifier		:	
CREATE PROCEDURE [DCRM].[GetDealerPlanLog]
	@From	DATETIME,
	@To		DATETIME
AS
BEGIN
	SET NOCOUNT ON			

	SELECT DDPPL.Id,D.Id DealerId, D.FirstName + ' '+ D.LastName DName,DDPPL.CreatedOn,DDPPL.ExpiryDate,P.Name PName,DDPPL.CourierNo,DDPPL.CourierDate
	FROM Dealers D
	INNER JOIN DCRM.DealerPlanPrintLog DDPPL ON D.ID = DDPPL.DealerId
	INNER JOIN Packages P ON P.Id = DDPPL.ProductType
	WHERE CONVERT(CHAR(10), DDPPL.CreatedOn, 120) BETWEEN CONVERT(CHAR(10), @From, 120) AND CONVERT(CHAR(10), @To, 120)
	
END
