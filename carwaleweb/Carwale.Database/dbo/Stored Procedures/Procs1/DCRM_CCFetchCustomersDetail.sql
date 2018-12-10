IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_CCFetchCustomersDetail]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_CCFetchCustomersDetail]
GO

	CREATE PROCEDURE [dbo].[DCRM_CCFetchCustomersDetail]

@MONTH VARCHAR(10) ,
@YEAR VARCHAR(10) ,
@REGIONID VARCHAR(10) = NULL,
@CITYID VARCHAR(10) = NULL,
@DEALERID VARCHAR(10) = NULL

AS
BEGIN

IF @REGIONID = '0' 
	BEGIN
		SELECT Day(UCP.RequestDateTime)   AS DAY, 
		Month(UCP.RequestDateTime)        AS MONTH, 
		Year(UCP.RequestDateTime)         AS YEAR, 
		Datename(dw, UCP.RequestDateTime) AS DayName, 
		Count(DISTINCT UCP.CustomerID)    AS TOTAL, 
		Count(DISTINCT DCC.Id)            AS Forwarded 
		FROM   UsedCarPurchaseInquiries UCP (NOLOCK) 
		JOIN SellInquiries SI  ON SI.ID=UCP.SellInquiryId
		JOIN Dealers AS D  ON D.ID=SI.DealerId AND D.IsDealerActive = 1
		JOIN DCRM_ADM_RegionCities as DC  ON DC.CityId=D.CityId 
		LEFT JOIN Dcrm_CustomerCalling DCC 
		ON DCC.CustomerId = UCP.CustomerID 
		AND CONVERT(DATE, DCC.InquiryDate) = CONVERT(DATE, UCP.requestdatetime) 
		WHERE  Year(UCP.RequestDateTime) = @YEAR 
		AND Month(UCP.requestdatetime) = @MONTH     
		GROUP  BY Day(UCP.requestdatetime), 
		Month(UCP.requestdatetime), 
		Year(UCP.requestdatetime), 
		Datename(dw, UCP.requestdatetime) 
		ORDER  BY day ASC

	END
 IF @REGIONID <> '0' AND @DEALERID <> '0'
	BEGIN
		SELECT Day(UCP.RequestDateTime)   AS DAY, 
		Month(UCP.RequestDateTime)        AS MONTH, 
		Year(UCP.RequestDateTime)         AS YEAR, 
		Datename(dw, UCP.RequestDateTime) AS DayName, 
		Count(DISTINCT UCP.CustomerID)    AS TOTAL, 
		Count(DISTINCT DCC.Id)            AS Forwarded 
		FROM   UsedCarPurchaseInquiries UCP (NOLOCK) 
		JOIN SellInquiries SI  ON SI.ID=UCP.SellInquiryId
		JOIN Dealers AS D  ON D.ID=SI.DealerId AND D.IsDealerActive = 1 AND D.ID = @DEALERID
		JOIN DCRM_ADM_RegionCities as DC  ON DC.CityId=D.CityId AND DC.RegionId =  @REGIONID  
		LEFT JOIN Dcrm_CustomerCalling DCC 
		ON DCC.CustomerId = UCP.CustomerID 
		AND CONVERT(DATE, DCC.InquiryDate) = CONVERT(DATE, UCP.requestdatetime) 
		WHERE  Year(UCP.requestdatetime) = @YEAR 
		AND Month(UCP.requestdatetime) = @MONTH     
		GROUP  BY Day(UCP.requestdatetime), 
		Month(UCP.requestdatetime), 
		Year(UCP.requestdatetime), 
		Datename(dw, UCP.requestdatetime) 
		ORDER  BY day ASC

	END	
 IF @REGIONID <> '0' AND @CITYID = '0' --OR @CITYID = '-1')  
       	BEGIN
		SELECT Day(UCP.RequestDateTime)   AS DAY, 
		Month(UCP.RequestDateTime)        AS MONTH, 
		Year(UCP.RequestDateTime)         AS YEAR, 
		Datename(dw, UCP.RequestDateTime) AS DayName, 
		Count(DISTINCT UCP.CustomerID)    AS TOTAL, 
		Count(DISTINCT DCC.Id)            AS Forwarded 
		FROM   UsedCarPurchaseInquiries UCP (NOLOCK) 
		JOIN SellInquiries SI  ON SI.ID=UCP.SellInquiryId
		JOIN Dealers AS D  ON D.ID=SI.DealerId AND D.IsDealerActive = 1
		JOIN DCRM_ADM_RegionCities as DC  ON DC.CityId=D.CityId AND DC.RegionId =  @REGIONID 
		LEFT JOIN Dcrm_CustomerCalling DCC 
		ON DCC.CustomerId = UCP.CustomerID 
		AND CONVERT(DATE, DCC.InquiryDate) = CONVERT(DATE, UCP.RequestDateTime) 
		WHERE  Year(UCP.RequestDateTime) = @YEAR 
		AND Month(UCP.RequestDateTime) = @MONTH     
		GROUP  BY Day(UCP.RequestDateTime), 
		Month(UCP.RequestDateTime), 
		Year(UCP.RequestDateTime), 
		Datename(dw, UCP.RequestDateTime) 
		ORDER  BY day ASC

	END
 IF @REGIONID<> '0' AND @CITYID<>'0' AND @DEALERID = '0' 	
              	BEGIN
		SELECT Day(UCP.RequestDateTime)   AS DAY, 
		Month(UCP.RequestDateTime)        AS MONTH, 
		Year(UCP.RequestDateTime)         AS YEAR, 
		Datename(dw, UCP.RequestDateTime) AS DayName, 
		Count(DISTINCT UCP.CustomerID)    AS TOTAL, 
		Count(DISTINCT DCC.Id)            AS Forwarded 
		FROM   UsedCarPurchaseInquiries UCP (NOLOCK) 
		JOIN SellInquiries SI  ON SI.ID=UCP.SellInquiryId
		JOIN Dealers AS D  ON D.ID=SI.DealerId AND D.IsDealerActive = 1
		JOIN DCRM_ADM_RegionCities as DC  ON DC.CityId=D.CityId AND DC.RegionId = @REGIONID AND DC.CityId = @CITYID
		LEFT JOIN Dcrm_CustomerCalling DCC 
		ON DCC.CustomerId = UCP.CustomerID 
		AND CONVERT(DATE, DCC.InquiryDate) = CONVERT(DATE, UCP.RequestDateTime) 
		WHERE  Year(UCP.RequestDateTime) = @YEAR 
		AND Month(UCP.RequestDateTime) = @MONTH     
		GROUP  BY Day(UCP.RequestDateTime), 
		Month(UCP.RequestDateTime), 
		Year(UCP.RequestDateTime), 
		Datename(dw, UCP.RequestDateTime) 
		ORDER  BY day ASC

	END
 IF @REGIONID<>'0' AND @CITYID<>'0' AND @DEALERID <> '0' 	
              	BEGIN
		SELECT Day(UCP.RequestDateTime)   AS DAY, 
		Month(UCP.RequestDateTime)        AS MONTH, 
		Year(UCP.RequestDateTime)         AS YEAR, 
		Datename(dw, UCP.RequestDateTime) AS DayName, 
		Count(DISTINCT UCP.CustomerID)    AS TOTAL, 
		Count(DISTINCT DCC.Id)            AS Forwarded 
		FROM   usedcarpurchaseinquiries UCP (NOLOCK) 
		JOIN sellinquiries SI  ON SI.ID=UCP.SellInquiryId
		JOIN dealers AS D  ON D.ID=SI.DealerId AND D.IsDealerActive = 1 AND D.ID = @DEALERID 
		JOIN dcrm_adm_regioncities as DC  ON DC.CityId=D.CityId AND DC.RegionId = @REGIONID AND DC.CityId = @CITYID
		LEFT JOIN Dcrm_CustomerCalling DCC 
		ON DCC.CustomerId = UCP.CustomerID 
		AND CONVERT(DATE, DCC.InquiryDate) = CONVERT(DATE, UCP.requestdatetime) 
		WHERE  Year(UCP.requestdatetime) = @YEAR 
		AND Month(UCP.requestdatetime) = @MONTH     
		GROUP  BY Day(UCP.requestdatetime), 
		Month(UCP.requestdatetime), 
		Year(UCP.requestdatetime), 
		Datename(dw, UCP.requestdatetime) 
		ORDER  BY day ASC

	END	
END
