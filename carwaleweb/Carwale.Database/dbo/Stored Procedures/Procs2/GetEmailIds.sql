IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetEmailIds]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetEmailIds]
GO

	CREATE PROCEDURE [dbo].[GetEmailIds]
(@number int,@price int)
as
begin

with EmailList
as
(
SELECT ROW_NUMBER() OVER(ORDER BY  CU.EMail) AS RowNumber,
 CU.EMail FROM Customers AS CU WITH(NOLOCK), 
CustomerSecurityKey AS CK WITH(NOLOCK)
WHERE CU.IsFake = 0 
AND CU.Email LIKE '%@%' 
AND CU.ReceiveNewsletters = 1 
And CU.Email NOT IN(SELECT Email FROM DoNotSendEmail WITH(NOLOCK))
AND CK.CustomerId = CU.ID
AND CU.Id IN (SELECT CustomerId FROM NewCarPurchaseInquiries WITH(NOLOCK) WHERE CarVersionId IN(SELECT CarVersionId FROM NewCarShowroomPrices WITH(NOLOCK) WHERE Price > @price))
)

SELECT Email
FROM  EmailList
WHERE RowNumber <= @number
ORDER BY Email

 --insert into EmailMarketingLog
 --(DownloadedBy,Criteria,Number)
 --select HOST_NAME() AS HostName,CAST(@price as CHAR(20)),@number


end