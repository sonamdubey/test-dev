IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertMonthWiseNewCarInquiriesSummary]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertMonthWiseNewCarInquiriesSummary]
GO
	
---Create By   : Manish Chourasiya 
---Created on  : 08-08-2013 
-- Description : for inserting Day wise records in new car inquiries summary table
CREATE PROCEDURE [dbo].[InsertMonthWiseNewCarInquiriesSummary]
@Month TINYINT,
@Year  SMALLINT,
@Day   TINYINT
AS
BEGIN

INSERT INTO NewCarInquiriesSummary 
								   (NewCarPurchaseInquiriesId,
									CustomerId,
									CustomerName,
									CustomerEmail,
									CustomerMobile,
									CarMakeId,
									CarMakeName,
									CarModelId,
									CarModelName,
									CarVersionId,
									CarVersionName,
									Price,
									CustomerCityId,
									CustomerCityName,
									CustomerStateId,
									CustomerStateName,
									CarInquiryCityId,
									CarInquiryCityName,
									CarInquiryStateId,
									CarInquiryStateName,
									BuyTime,
									CarFuelTypesId,
									CarFuelType,
									CarBodyStylesId,
									CarBodyStyle,
									CarSegmentsId,
									CarSegment,
									CarSubSegmentsId,
									CarSubSegment,
									CarTransmissionId,
									CarTransmission ,
									RequestDateTime)
SELECT 
								NCPI.ID       AS NewCarPurchaseInquiriesId,
								C.ID          AS CustomerId,
								C.Name        AS CustomerName,
								C.email       AS CustomerEmail,
								C.Mobile      AS CustomerMobile,
								CK.ID         AS CarMakeId,
								CK.Name       AS CarMakeName,
								CM.ID         AS CarModelId,
								CM.Name       AS CarModelName,
								CV.ID         AS CarVersionId,
								CV.Name       AS CarVersionName,
								NCSP.Price    AS   Price,
								C.CityId      AS CustomerCityId,
								CT.Name       AS CustomerCityName,
								ST.ID         AS CustomerStateId,
								ST.Name       AS CustomerStateName,
								NPC.CityId    AS CarInquiryCityId,
								CT2.Name      AS CarInquiryCityName,
								ST2.ID        AS CarInquiryStateId,
								ST2.Name      AS CarInquiryStateName,
								NCPI.BuyTime  AS BuyTime,
								CFT.FuelTypeId AS CarFuelTypesId,
								CFT.FuelType  AS CarFuelType,
								CBS.ID	      AS CarBodyStylesId,
								CBS.Name      AS CarBodyStyle,
								CS.ID         AS CarSegmentsId,
								CS.Name       AS CarSegment,
								CSS.Id        AS CarSubSegmentsId,
								CSS.Name      AS CarSubSegment,
								CR.Id         AS CarTransmissionId,
								CR.Descr      AS CarTransmission ,
								NCPI.RequestDateTime AS RequestDateTime
FROM   		NewCarPurchaseInquiries AS NCPI WITH (NOLOCK)
JOIN   		NewPurchaseCities AS NPC WITH (NOLOCK) ON NCPI.Id=NPC.InquiryId
JOIN   		Customers AS C WITH (NOLOCK)  ON NCPI.CustomerId=C.Id AND C.IsFake=0
JOIN   		Cities AS CT WITH (NOLOCK) ON C.CityId=CT.id 
JOIN   		States As ST WITH (NOLOCK) ON CT.StateId=ST.Id
JOIN   		Cities AS CT2 WITH (NOLOCK) ON NPC.CityId=CT2.id 
JOIN   		States As ST2 WITH (NOLOCK) ON CT2.StateId=ST2.Id
JOIN   		CarVersions AS CV WITH (NOLOCK) ON NCPI.CarVersionId=CV.Id
JOIN   		CarModels AS CM WITH (NOLOCK)   ON CV.CarModelId=CM.Id
JOIN   		CarMakes  AS CK WITH (NOLOCK) ON CM.CarMakeId=CK.Id
LEFT JOIN   CarFuelType   AS CFT  WITH (NOLOCK) ON  CV.CarFuelType=CFT.FuelTypeId
LEFT JOIN   CarBodyStyles AS CBS WITH (NOLOCK) ON CV.BodyStyleId=CBS.ID
LEFT JOIN   CarSegments AS CS WITH (NOLOCK) ON CV.SegmentId=CS.ID
LEFT JOIN   CarSubSegments AS CSS WITH (NOLOCK) ON CV.SubSegmentId=CSS.Id
LEFT JOIN   CarTransmission AS CR WITH (NOLOCK) ON CR.Id=CV.CarTransmission
LEFT JOIN   NewCarShowroomPrices AS NCSP WITH (NOLOCK) ON NCSP.CarVersionId=NCPI.CarVersionId AND NCSP.CityId=NPC.CityId
WHERE MONTH(NCPI.RequestDateTime)=@Month
AND YEAR(NCPI.RequestDateTime)=@Year
AND DAY(NCPI.RequestDateTime)=@Day

END
