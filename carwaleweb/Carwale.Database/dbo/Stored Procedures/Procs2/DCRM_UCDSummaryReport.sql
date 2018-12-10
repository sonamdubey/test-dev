IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_UCDSummaryReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_UCDSummaryReport]
GO

	CREATE PROCEDURE [dbo].[DCRM_UCDSummaryReport]
@fromDate	DATETIME ,
@toDate		DATETIME ,
@dealerId	NUMERIC(18,0)=null

-------------------------------------------------
--Name of SP/Function                   : DCRM_UCDSummaryReport
--Applications using SP                 : DCRM
--Modules using the SP                  : UCDSummaryReport.cs
--Technical department                  : Database
--Summary								: UCDSummaryReport
--Author								: AMIT Kumar 25th June 2013
--Modification history                  : 1 
-------------------------------------------------
AS
	BEGIN
		--WITH CTE1 AS
		--(SELECT COUNT (DISTINCT UCP.CustomerID) AS TotalCustomer,D.ID  DealerIdMain,COUNT (DISTINCT CUF.CustomerID) AS TotalCustomerFeedback,
		--D.Organization AS DealerName
		--	FROM UsedCarPurchaseInquiries UCP (NOLOCK)   
		--	INNER JOIN SellInquiries SI (NOLOCK) ON SI.ID = UCP.SellInquiryId 
		--	INNER JOIN Dealers D (NOLOCK) ON D.ID = SI.DealerId 
		--	LEFT JOIN DCRM_UCDFeedback CUF (NOLOCK) ON UCP.CustomerID = CUF.CustomerId AND CUF.FBSubmitDate BETWEEN  @fromDate AND @toDate
		--	WHERE UCP.RequestDateTime BETWEEN @fromDate AND @toDate 
		--		AND (@dealerId IS NULL OR D.ID = @dealerId) 
		--	GROUP BY D.ID,D.Organization ),
		--CTE2 AS 
		--(SELECT COUNT(DISTINCT DCC.CustomerID) AS NotConnectedCustomer,SI.DealerId FROM DCRM_CustomerCalling DCC (NOLOCK)
		--	INNER JOIN UsedCarPurchaseInquiries UCP (NOLOCK) ON UCP.CustomerID = DCC.CustomerId
		--	INNER JOIN SellInquiries SI (NOLOCK) ON SI.ID = UCP.SellInquiryId 
		--	INNER JOIN Dealers D (NOLOCK) ON D.ID = SI.DealerId
		--WHERE DCC.ActionID = 2 GROUP BY SI.DealerId ) 
		
		--SELECT CTE1.DealerIdMain,CTE1.TotalCustomer,CTE1.TotalCustomerFeedback,ISNULL(CTE2.NotConnectedCustomer,'0') AS NotConnectedCustomer,
		--	CTE1.DealerName 
		--	FROM CTE1 LEFT JOIN CTE2 ON CTE1.DealerIdMain = CTE2.DealerId ORDER BY CTE1.DealerName
			
		
		SELECT DISTINCT UCP.CustomerID AS TotalCustomer,D.ID  DealerIdMain,
		D.Organization AS DealerName, DCC.ActionID, DCC.CustomerId ,DCC.IsFeedbackGiven
			FROM UsedCarPurchaseInquiries UCP (NOLOCK)   
			INNER JOIN SellInquiries SI (NOLOCK) ON SI.ID = UCP.SellInquiryId 
			INNER JOIN Dealers D (NOLOCK) ON D.ID = SI.DealerId 
			LEFT JOIN DCRM_CustomerCalling DCC (NOLOCK) ON DCC.CustomerId = UCP.CustomerID AND DCC.InquiryDate BETWEEN  @fromDate AND @toDate--BETWEEN  '2013/06/01' AND '2013/06/30' 
			WHERE UCP.RequestDateTime BETWEEN  @fromDate AND @toDate  AND (@dealerId IS NULL OR D.ID = @dealerId)--BETWEEN '2013/06/01' AND '2013/06/30' --AND D.ID = 6696 
			ORDER BY D.Organization
			
			--Pushed for Calling  DCC.CustomerId IS NOT NULL
			--Not Pushed for Calling DCC.CustomerId IS NULL
			--Feedback Taken AND DCC.ActionID = 1
			--Customer Not Connected  AND DCC.ActionID = 2
			-- In Followup, Not yet called -- AND DCC.ActionID IS NULL AND DCC.CustomerId IS NOT NULL
		
	END			
	
			