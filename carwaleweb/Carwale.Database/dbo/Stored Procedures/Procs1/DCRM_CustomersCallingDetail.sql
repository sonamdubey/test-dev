IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_CustomersCallingDetail]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_CustomersCallingDetail]
GO

	
CREATE PROCEDURE [dbo].[DCRM_CustomersCallingDetail]

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
		FROM   usedcarpurchaseinquiries UCP WITH(NOLOCK) 
		JOIN sellinquiries SI WITH(NOLOCK) ON SI.ID=UCP.SellInquiryId
		JOIN dealers AS D WITH(NOLOCK) ON D.ID=SI.DealerId 
		JOIN dcrm_adm_regioncities as DC WITH(NOLOCK) ON DC.CityId=D.CityId 
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
 IF @REGIONID <> '0' AND @DEALERID <> '0'
	BEGIN
		SELECT Day(UCP.RequestDateTime)   AS DAY, 
		Month(UCP.RequestDateTime)        AS MONTH, 
		Year(UCP.RequestDateTime)         AS YEAR, 
		Datename(dw, UCP.RequestDateTime) AS DayName, 
		Count(DISTINCT UCP.CustomerID)    AS TOTAL, 
		Count(DISTINCT DCC.Id)            AS Forwarded 
		FROM   usedcarpurchaseinquiries UCP WITH(NOLOCK) 
		JOIN sellinquiries SI WITH(NOLOCK) ON SI.ID=UCP.SellInquiryId
		JOIN dealers AS D WITH(NOLOCK) ON D.ID=SI.DealerId AND D.ID = @DEALERID
		JOIN dcrm_adm_regioncities as DC WITH(NOLOCK) ON DC.CityId=D.CityId AND DC.RegionId =  @REGIONID  
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
		FROM   usedcarpurchaseinquiries UCP WITH(NOLOCK) 
		JOIN sellinquiries SI WITH(NOLOCK) ON SI.ID=UCP.SellInquiryId
		JOIN dealers AS D WITH(NOLOCK) ON D.ID=SI.DealerId 
		JOIN dcrm_adm_regioncities as DC WITH(NOLOCK) ON DC.CityId=D.CityId AND DC.RegionId =  @REGIONID 
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
 IF @REGIONID<> '0' AND @CITYID<>'0' AND @DEALERID = '0' 	
              	BEGIN
		SELECT Day(UCP.RequestDateTime)   AS DAY, 
		Month(UCP.RequestDateTime)        AS MONTH, 
		Year(UCP.RequestDateTime)         AS YEAR, 
		Datename(dw, UCP.RequestDateTime) AS DayName, 
		Count(DISTINCT UCP.CustomerID)    AS TOTAL, 
		Count(DISTINCT DCC.Id)            AS Forwarded 
		FROM   usedcarpurchaseinquiries UCP WITH(NOLOCK) 
		JOIN sellinquiries SI WITH(NOLOCK) ON SI.ID=UCP.SellInquiryId
		JOIN dealers AS D WITH(NOLOCK) ON D.ID=SI.DealerId 
		JOIN dcrm_adm_regioncities as DC WITH(NOLOCK) ON DC.CityId=D.CityId AND DC.RegionId = @REGIONID AND DC.CityId = @CITYID
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
 IF @REGIONID<>'0' AND @CITYID<>'0' AND @DEALERID <> '0' 	
              	BEGIN
		SELECT Day(UCP.RequestDateTime)   AS DAY, 
		Month(UCP.RequestDateTime)        AS MONTH, 
		Year(UCP.RequestDateTime)         AS YEAR, 
		Datename(dw, UCP.RequestDateTime) AS DayName, 
		Count(DISTINCT UCP.CustomerID)    AS TOTAL, 
		Count(DISTINCT DCC.Id)            AS Forwarded 
		FROM   usedcarpurchaseinquiries UCP WITH(NOLOCK) 
		JOIN sellinquiries SI WITH(NOLOCK) ON SI.ID=UCP.SellInquiryId
		JOIN dealers AS D WITH(NOLOCK) ON D.ID=SI.DealerId AND D.ID = @DEALERID 
		JOIN dcrm_adm_regioncities as DC WITH(NOLOCK) ON DC.CityId=D.CityId AND DC.RegionId = @REGIONID AND DC.CityId = @CITYID
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

