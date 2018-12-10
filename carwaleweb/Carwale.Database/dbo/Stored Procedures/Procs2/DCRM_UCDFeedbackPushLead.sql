IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_UCDFeedbackPushLead]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_UCDFeedbackPushLead]
GO

	CREATE PROCEDURE [dbo].[DCRM_UCDFeedbackPushLead]    
AS    
-- CREATED By Amit kumar on 10th april 2013.    
-- Used in Automated process UCDFeedbackPushLead.cs page    
-- Summary: used to Select 75 percent of the lead.    
BEGIN    
 --DECLARE @Count INT    
 --DECLARE @LeadToBePushed INT    
     
 --SELECT @Count = COUNT(DISTINCT UCP.CustomerID)        
 --  FROM   UsedCarPurchaseInquiries UCP (NOLOCK)     
 --  JOIN SellInquiries SI (NOLOCK) ON SI.ID=UCP.SellInquiryId    
 --  JOIN Dealers AS D (NOLOCK) ON D.ID=SI.DealerId AND D.IsDealerActive = 1    
 --  JOIN DCRM_ADM_RegionCities as DC (NOLOCK) ON DC.CityId=D.CityId     
 --  LEFT JOIN Dcrm_CustomerCalling DCC (NOLOCK)    
 --  ON DCC.CustomerId = UCP.CustomerID AND DCC.ActionID IS NOT NULL AND DCC.CustomerId IS NULL    
 --  AND CONVERT(DATE, DCC.InquiryDate) = CONVERT(DATE, UCP.requestdatetime)     
 --  WHERE UCP.RequestDateTime = CONVERT(date,GETDATE()-7)  AND  DCC.CustomerId IS NULL       
 --print @Count    
     
 --SET @LeadToBePushed = (@Count *75)/100    
 --print @LeadToBePushed    
 --IF(@LeadToBePushed < 30)    
 -- BEGIN    
 --  SELECT DISTINCT UCP.CustomerID    AS CustomerID,    
 --  CONVERT(DATE,UCP.RequestDateTime) AS InquiryDate    
 --  FROM   UsedCarPurchaseInquiries UCP (NOLOCK)     
 --  JOIN SellInquiries SI (NOLOCK) ON SI.ID=UCP.SellInquiryId    
 --  JOIN Dealers AS D (NOLOCK) ON D.ID=SI.DealerId AND D.IsDealerActive = 1    
 --  JOIN DCRM_ADM_RegionCities as DC (NOLOCK) ON DC.CityId=D.CityId     
 --  LEFT JOIN Dcrm_CustomerCalling DCC (NOLOCK)    
 --  ON DCC.CustomerId = UCP.CustomerID AND DCC.ActionID IS NOT NULL AND DCC.CustomerId IS NULL    
 --  AND CONVERT(DATE, DCC.InquiryDate) = CONVERT(DATE, UCP.requestdatetime)     
 --  WHERE  CONVERT(DATE,UCP.RequestDateTime) = CONVERT(date,GETDATE()-7)      
 -- END    
 --ELSE    
 -- BEGIN    
 --  WITH CTE AS(    
 --  SELECT DISTINCT TOP (@LeadToBePushed) UCP.CustomerID AS CustomerID,    
 --  CONVERT(DATE,UCP.RequestDateTime) AS InquiryDate    
 --  FROM   UsedCarPurchaseInquiries UCP (NOLOCK)     
 --  JOIN SellInquiries SI (NOLOCK) ON SI.ID=UCP.SellInquiryId    
 --  JOIN Dealers AS D (NOLOCK) ON D.ID=SI.DealerId AND D.IsDealerActive = 1    
 --  JOIN DCRM_ADM_RegionCities as DC (NOLOCK) ON DC.CityId=D.CityId     
 --  LEFT JOIN Dcrm_CustomerCalling DCC (NOLOCK)    
 --  ON DCC.CustomerId = UCP.CustomerID AND DCC.ActionID IS NOT NULL AND DCC.CustomerId IS NULL    
 --  AND CONVERT(DATE, DCC.InquiryDate) = CONVERT(DATE, UCP.requestdatetime)     
 --  WHERE  CONVERT(DATE,UCP.RequestDateTime) = CONVERT(date,GETDATE()-7)     
 --  )    
 --  SELECT  * FROM CTE ORDER BY NEWID();    
 -- END    

   SELECT DISTINCT UCP.CustomerID AS CustomerID,    
   GETDATE()-7 AS InquiryDate    
   FROM   UsedCarPurchaseInquiries UCP (NOLOCK)     
   WHERE  UCP.RequestDateTime BETWEEN CONVERT(VARCHAR,GETDATE()-7, 101) AND DATEADD(SECOND, -1, CONVERT(VARCHAR,GETDATE()-6, 101))
    
END    