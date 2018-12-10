IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetIndividualResponsesForCarTrade]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetIndividualResponsesForCarTrade]
GO

	
-- =============================================
-- Author:		Navead Kazi
-- Create date: 02nd June 2016
-- Description:	Select Individual Responses to sent to CarTrade
-- exec GetIndividualResponsesForCarTradeCopy
-- =============================================
CREATE PROCEDURE [dbo].[GetIndividualResponsesForCarTrade] 	
AS
BEGIN
	
	SET NOCOUNT ON;

--DECLARE @DealerResponseDays int = -7;
DECLARE @IndividualResponseStart int = -15;
DECLARE @IndividualResponseEnd int = -30;


SELECT CR.SellInquiryId,CR.CustomerId
into #tempClassifiedRequests
FROM ClassifiedRequests CR WITH(NOLOCK)
WHERE CR.RequestDateTime >= DATEADD(mi,@IndividualResponseEnd,getdate()) and CR.RequestDateTime <= DATEADD(mi,@IndividualResponseStart,getdate())

INSERT INTO CT_IndividualResponse(
BuyerId,
BuyerName,
BuyerMobile,
BuyerEmail,
CarMakeName,
CarModelName,
CarVersionName,
CarMakeYear,
CarColor,
CarFuelType,
CarPrice,
CityName
)
SELECT
C.Id BuyerId,
C.Name BuyerName,
C.Mobile BuyerMobile,
C.email BuyerEmail,
CMA.Name CarMakeName,
CMO.Name CarModelName,
CV.Name CarVersionName,
CSI.MakeYear CarMakeYear,
CSI.Color CarColor,
CFT.FuelType CarFuelType,
CSI.Price CarPrice,
LLC.CityName CityName
FROM #tempClassifiedRequests CR WITH(NOLOCK)
INNER JOIN CustomerSellInquiries CSI WITH(NOLOCK) ON CR.SellInquiryId = CSI.ID
INNER JOIN Customers C WITH(NOLOCK) ON CR.CustomerId = C.Id
INNER JOIN CarVersions CV WITH(NOLOCK) ON CSI.CarVersionId = CV.ID
INNER JOIN CarModels CMO WITH(NOLOCK) ON CV.CarModelId=CMO.ID
INNER JOIN CarMakes CMA WITH(NOLOCK) ON CMO.CarMakeId = CMA.ID
INNER JOIN CarFuelType CFT WITH(NOLOCK) ON CFT.FuelTypeId = CV.CarFuelType
INNER JOIN LL_Cities LLC WITH(NOLOCK) ON CSI.CityId=LLC.CityId


SELECT ID,
BuyerId,
BuyerName,
BuyerMobile,
BuyerEmail,
CarMakeName,
CarModelName,
CarVersionName,
--CarMakeYear,
datepart(yyyy,CarMakeYear) CarMakeYear,
CarColor,
CarFuelType,
CarPrice,
CityName
FROM CT_IndividualResponse WITH(NOLOCK) WHERE CTStatusId IS NULL

DROP TABLE #tempClassifiedRequests
END
 