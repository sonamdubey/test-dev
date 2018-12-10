IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetEmailIdsByCityAndCar]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetEmailIdsByCityAndCar]
GO

	CREATE PROCEDURE [dbo].[GetEmailIdsByCityAndCar]  
(@number int,@price int, @city int)  
as  
begin  
  
with EmailList  
as  
(  
SELECT ROW_NUMBER() OVER(ORDER BY  CU.EMail) AS RowNumber,  
 CU.EMail FROM Customers AS CU WITH(NOLOCK),   
CustomerSecurityKey AS CK WITH(NOLOCK),
Cities as Ct  WITH(NOLOCK),
[dbo].[fnSplitCSV](@City) as f   
WHERE CU.IsFake = 0   
AND CU.Email LIKE '%@%'   
AND CU.ReceiveNewsletters = 1   
And CU.Email NOT IN(SELECT Email FROM DoNotSendEmail WITH(NOLOCK))  
AND CK.CustomerId = CU.ID  
AND CU.Id IN (SELECT CustomerId FROM NewCarPurchaseInquiries WITH(NOLOCK) WHERE CarVersionId IN(SELECT CarVersionId FROM NewCarShowroomPrices WITH(NOLOCK) WHERE Price > @price))  
AND CU.CityId=Ct.ID
AND ct.Name =f.ListMember
)  
  
SELECT Email  
FROM  EmailList  
WHERE RowNumber <= @number  
ORDER BY Email  
  
--SELECT CU.EMail FROM Customers AS CU WITH(NOLOCK),   
--CustomerSecurityKey AS CK WITH(NOLOCK),
--Cities as C  WITH(NOLOCK)
--WHERE CU.IsFake = 0   
--AND CU.Email LIKE '%@%'   
--AND CU.ReceiveNewsletters = 1   
--And CU.Email NOT IN(SELECT Email FROM DoNotSendEmail WITH(NOLOCK))  
--AND CK.CustomerId = CU.ID  
--AND CU.Id IN (SELECT CustomerId FROM NewCarPurchaseInquiries WITH(NOLOCK) WHERE CarVersionId IN(SELECT CarVersionId FROM NewCarShowroomPrices  as N WITH(NOLOCK) join vwMMV as vw on vw.VersionId=N.CarVersionId WHERE Price  between  150000 and 300000 and  vw.MakeId in (10,16)
--and vw.ModelId in (229,32)))  
--AND CU.CityId=C.ID
--AND C.ID in (2,57) 
  
  
end