IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_UCDPushFreshCust]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_UCDPushFreshCust]
GO

	CREATE PROCEDURE [dbo].[DCRM_UCDPushFreshCust]
@fromDate	DATETIME ,
@toDate		DATETIME ,
@dealerId	NUMERIC(18,0)=null

-------------------------------------------------
--Name of SP/Function                   : [DCRM_UCDPushFreshCust]
--Applications using SP                 : DCRM
--Modules using the SP                  : PushCustForUCDFB.cs
--Technical department                  : Database
--Summary								: UCDPush
--Author								: AMIT Kumar 27th June 2013
--Modification history                  : 1 
-------------------------------------------------
AS
	BEGIN	
		
		SELECT * FROM 
		(SELECT DISTINCT UCP.CustomerID AS Id,D.ID  DealerIdMain,UCP.RequestDateTime AS EnquiryDate,
			 ROW_NUMBER() OVER (PARTITION BY UCP.CustomerId Order by UCP.RequestDateTime desc ) AS RowNum,
			 D.Organization AS DealerName , CU.name, CU.email, CU.mobile
		 FROM UsedCarPurchaseInquiries UCP (NOLOCK)   
			 INNER JOIN customers CU (NOLOCK) ON UCP.customerid = CU .id 
			 INNER JOIN SellInquiries SI (NOLOCK) ON SI.ID = UCP.SellInquiryId 
			 INNER JOIN Dealers D (NOLOCK) ON D.ID = SI.DealerId 
			 LEFT JOIN DCRM_CustomerCalling DCC (NOLOCK) ON DCC.CustomerId = UCP.CustomerID
			 AND DCC.InquiryDate BETWEEN @fromDate AND @toDate--BETWEEN  '2013/06/01' AND '2013/06/30' 
		 WHERE UCP.RequestDateTime BETWEEN @fromDate AND @toDate  AND (@dealerId IS NULL OR D.ID = @dealerId) AND
			 DCC.CustomerId IS NULL
		--BETWEEN '2013/06/01' AND '2013/06/30' --AND D.ID = 6696 
		)   AS FinalTable WHERE RowNum = 1 ORDER BY FinalTable.name
		
	END			