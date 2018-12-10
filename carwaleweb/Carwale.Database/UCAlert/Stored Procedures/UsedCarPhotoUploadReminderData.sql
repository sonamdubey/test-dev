IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[UsedCarPhotoUploadReminderData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[UsedCarPhotoUploadReminderData]
GO

	--Created By: Shikhar on 21-06-2014
--Description: Send the email alert for the used car customers who have not uploaded the photos.
CREATE PROCEDURE [UCAlert].[UsedCarPhotoUploadReminderData]
AS
BEGIN
DECLARE  @FirstDayOfMonth DATETIME  = CONVERT(DATETIME,CONVERT(VARCHAR(10),DATEADD(MM, DATEDIFF(MM, -1, GETDATE())-1, 0),120)+ ' 00:00:00')	
DECLARE  @Currdate DATETIME =CONVERT(DATETIME,CONVERT(VARCHAR(10),GETDATE(),120)+ ' 00:00:00')

SELECT 
	CK.CustomerKey AS CustomerKey,
	C.email AS Seller_Email, 
	C.Name AS CustomerName,
	LL.Inquiryid AS ProfileID, 
	LL.MakeName + ' ' + LL.ModelName + ' ' + LL.VersionName AS CUSTOMER_CAR, 
	LL.PhotoCount AS Photocount  
FROM 
	LiveListings AS LL WITH (NOLOCK)
	INNER JOIN CustomerSellInquiries AS CS WITH (NOLOCK)
		ON CS.ID = LL.Inquiryid
	INNER JOIN Customers AS C WITH (NOLOCK)
		ON C.Id = CS.CustomerId
	INNER JOIN CustomerSecurityKey AS CK WITH (NOLOCK)
		ON CK.CustomerId = CS.CustomerId
WHERE 
	LL.SellerType=2 
AND
	LL.PhotoCount = 0
AND
	(CONVERT(DATE,  LL.EntryDate) = CONVERT(DATE, GETDATE() - 1) OR @FirstDayOfMonth=@Currdate)
ORDER BY 
	LL.PhotoCount DESC
END