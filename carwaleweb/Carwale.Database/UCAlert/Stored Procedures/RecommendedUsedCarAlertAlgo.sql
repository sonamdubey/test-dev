IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[RecommendedUsedCarAlertAlgo]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[RecommendedUsedCarAlertAlgo]
GO

	-- Created By:	Manish Chourasiya
-- Create date: 19-03-2014
-- Description: Return the recommended used car according to passed parameters.
-- Modified by : Manish on 17-04-2014 return no image url if front image path  is not present in livelistings
-- =============================================
--##########################  WARNING  ################################### 
--****************** This sp is using in Used Car Alert email also so any minor change in this sp also need to change in the sp [UCAlert].[RecommendCarAlertForLastDay] and [UCAlert].[RecommendCarAlertForSecondMail]
CREATE PROCEDURE [UCAlert].[RecommendedUsedCarAlertAlgo] 
@Price  INT,
@CarVersionId INT,
@CityId INT,
@NoOfRecommendCar TINYINT 
AS 
  BEGIN
         
		 DECLARE @BodyStyleNeedtoConsider TABLE (CarBodyStylesId SMALLINT)
		 DECLARE @PriceMultiplier FLOAT =.5,
		         @KmMultiplier FLOAT =.08,
				 @CarAgeMultiplier FLOAT =.4,
				 @CarPhotoMultiplier FLOAT =.02,
                 @CarSubSegmentsId SMALLINT,
                 @CarBodyStylesId SMALLINT

		SELECT @CarSubSegmentsId=CS.ID,
		       @CarBodyStylesId=CB.ID
		FROM  CarVersions  AS CV WITH (NOLOCK) 
			  JOIN CarBodyStyles AS CB WITH (NOLOCK) ON CV.BodyStyleId=CB.Id
			  JOIN CarSubSegments AS CS WITH (NOLOCK) ON CS.ID=CV.SubSegmentId
	    WHERE CV.ID=@CarVersionId


		 IF @CarBodyStylesId=3
			 BEGIN 
				INSERT INTO @BodyStyleNeedtoConsider 
				SELECT ID FROM CarBodyStyles WITH (NOLOCK) 
			 END 
		 ELSE IF @CarBodyStylesId=1
			 BEGIN 
				INSERT INTO @BodyStyleNeedtoConsider 
				SELECT ID FROM CarBodyStyles WITH (NOLOCK)  WHERE ID<>3
			 END 
		 ELSE 
		   BEGIN
		       INSERT INTO @BodyStyleNeedtoConsider 
				SELECT ID FROM CarBodyStyles WITH (NOLOCK)  WHERE ID NOT IN (3,1)
		   END

		   --------Making the link for display car details in profile id field-------------
			  SELECT TOP (@NoOfRecommendCar) LOWER(Replace(LL.MakeName,' ',''))+'-'+CM.MaskingName+'-'+LL.ProfileId AS ProfileId,
			                                 LL.MakeName+' '+LL.ModelName /*+' '+LL.VersionName*/ AS CarName,
			                                 CASE WHEN LL.HostURL IS NULL OR LL.HostURL='' THEN 'img.carwale.com'
			                                 else LL.HostURL end 
			                                 + 
											 Replace( CASE WHEN LL.FrontImagePath='' OR LL.FrontImagePath IS NULL THEN '/used/nocar.jpg'
											                ELSE  LL.FrontImagePath END ,
											          '80x60.JPG','300x225.jpg'
													 )  AS FrontImagePath,
											 LL.Inquiryid   AS InquiryId,
			                                 LL.SellerType  AS SellerType,
											 LL.Price   AS  Price,
											 LL.MakeYear AS MakeYear,
											 LL.Kilometers AS Kilometers,
											 LL.PhotoCount  AS PhotoCount,
			( 
			(CASE WHEN ABS(@Price-Price)> 120000     THEN 1
			      WHEN ABS(@Price-Price) BETWEEN 100001 AND 120000  THEN 2
			      WHEN ABS(@Price-Price) BETWEEN 50001 AND 100000  THEN 3
			      WHEN ABS(@Price-Price) BETWEEN 20001 AND 50000  THEN 4
			      WHEN ABS(@Price-Price)<= 20000  THEN 5
			      END
			* @PriceMultiplier)
			+
			(CASE WHEN ABS(DATEDIFF(MM,MakeYear,GETDATE()))/12.000>9  THEN 1
			      WHEN ABS(DATEDIFF(MM,MakeYear,GETDATE()))/12.000 BETWEEN 5.001 AND 9  THEN 2
			      WHEN ABS(DATEDIFF(MM,MakeYear,GETDATE()))/12.000 BETWEEN 3.001 AND 5 THEN 3
			      WHEN ABS(DATEDIFF(MM,MakeYear,GETDATE()))/12.000 BETWEEN 2.001 AND 3  THEN 4
			      WHEN ABS(DATEDIFF(MM,MakeYear,GETDATE()))/12.000<=2  THEN 5
			      END
			*@CarAgeMultiplier)
			+
			(CASE WHEN Kilometers>55000  THEN 1
			      WHEN Kilometers BETWEEN 45001 AND 55000  THEN 2
			      WHEN Kilometers BETWEEN 33001 AND 45000  THEN 3
			      WHEN Kilometers BETWEEN 20001 AND 33000  THEN 4
			      WHEN Kilometers<=20000 THEN 5
			      END
			*@KmMultiplier )
			+
			(CASE 
					WHEN PhotoCount=0   THEN 1
					WHEN PhotoCount BETWEEN 1 AND 5  THEN 2
					WHEN PhotoCount BETWEEN 6 AND 10 THEN 3
					WHEN PhotoCount BETWEEN 11 AND 15 THEN 4
					WHEN PhotoCount >15 THEN 5
					END   
			*@CarPhotoMultiplier)
			  )  AS  CalculatedScore
						 
			 FROM LiveListings    AS LL WITH (NOLOCK)
			  JOIN CarVersions    AS CV WITH (NOLOCK) ON CV.ID=LL.VersionId
			  JOIN CarBodyStyles  AS CB WITH (NOLOCK) ON CV.BodyStyleId=CB.Id
			  JOIN CarSubSegments AS CS WITH (NOLOCK) ON CS.ID=CV.SubSegmentId
			  JOIN @BodyStyleNeedtoConsider AS BS ON BS.CarBodyStylesId=CB.ID
			  JOIN CarModels  AS CM  WITH (NOLOCK) ON LL.ModelId=CM.ID
			  LEFT JOIN UCALERT.ModelListNotConsiderInAlgo AS ML WITH (NOLOCK) ON LL.ModelId=ML.CarModelsId
			 WHERE 
			  LL.CityId=@CityId
			  AND CS.ID BETWEEN @CarSubSegmentsId+1 AND @CarSubSegmentsId+3
			  AND ML.CarModelsId IS NULL
			  ORDER BY CalculatedScore DESC


  END 
 
