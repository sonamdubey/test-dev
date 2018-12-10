IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'Viw_JointPurchaseInq' AND
     DROP VIEW dbo.Viw_JointPurchaseInq
GO

	/*************************************************************************************************************
       Description: This view will show you joint purchase inquiries of Dealers and Individuals

       Objects Used in Operation: UsedCarPurchaseInquiries, ClassifiedRequests

       Created On: 01 Apr 2008
       Created By: Satish Sharma

       History:
       
       Edited By               		 EditedON               		 Description
       ---------                	----------               			 -----------
       Satish Sharma                	01-Apr-08 1:00 PM        		Added New Column ConsumerType
       
**********************************************************************************************************/

CREATE   View Viw_JointPurchaseInq 
AS
/*************** Purchase Inquiries of Individual ********************/
SELECT CustomerId, '1' ConsumerType, SellInquiryId, RequestDateTime
FROM   ClassifiedRequests
WHERE  IsActive = 1

UNION ALL

/********** Purchase Inquiries of Dealers **********************/
SELECT CustomerId, '0' ConsumerType, SellInquiryId, RequestDateTime
FROM UsedCarPurchaseInquiries
WHERE IsApproved = 1 AND IsFake = 0 AND StatusId = 1
