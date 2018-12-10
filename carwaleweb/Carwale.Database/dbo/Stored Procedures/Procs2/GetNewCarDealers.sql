IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNewCarDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNewCarDealers]
GO

	-- =============================================          
-- Author:  <Vikas Jyoty>          
-- Description: <Get the car dealers>          
-- Tables Used : Dealer_NewCar,NCD_Dealers,Cities,States,CarMakes          
-- Create By: Vikas J on <01/08/2013>          
-- Modified By: Avishkar on  <01/09/2013> to use WITH(NOLOCK)     
-- Modified By:Prashant Vishe on <22/04/2013> to  add condition for retrieving top 5 records  from query   
-- Modifiled By : Raghu on <16/7/2013> to get the DealerId      
-- Modifiled By : Raghu on <3/13/2014> Sorting by Ispriority Column too    
-- =============================================          
CREATE PROCEDURE [dbo].[GetNewCarDealers]          
(          
          
 @CityId SMALLINT,          
 @MakeId SMALLINT,       
 @Choice SMALLINT = 1 --TO ELIMINATE TOP 5 ON QUERY      
           
)          
AS          
BEGIN          
          
          
 SET NOCOUNT ON;          
          
  if(@Choice=1)      
  begin             
   SELECT DNC.TcDealerId as Id,DNC.Name AS DealerName, DNC.Address, DNC.PinCode, DNC.ContactNo,          
    DNC.FaxNo, DNC.EMailId, DNC.WebSite, DNC.WorkingHours,          
    CMA.Name AS CarMake, C.Name AS City, S.Name AS State, NCD_Website          
   FROM Dealer_NewCar AS DNC WITH(NOLOCK) --Modified By: Avishkar on  <01/09/2013> to use WITH(NOLOCK)          
   LEFT JOIN NCD_Dealers Nd WITH(NOLOCK) ON DNC.Id = Nd.DealerId AND Nd.IsActive = 1          
   JOIN Cities AS C WITH(NOLOCK) ON DNC.CityId = C.ID          
   JOIN States AS S WITH(NOLOCK) ON C.StateId = S.Id          
   JOIN CarMakes AS CMA WITH(NOLOCK) ON DNC.MakeId = CMA.ID           
   WHERE DNC.CityId = @CityId           
   AND DNC.MakeId = @MakeId           
   AND DNC.IsActive = 1          
   AND C.IsDeleted = 0           
   AND S.IsDeleted = 0           
   AND CMA.IsDeleted = 0       
   ORDER BY IsPriority DESC,NCD_Website DESC, DealerName;          
 end      
else      
 begin      
   SELECT top 5 DNC.TcDealerId as Id,DNC.Name AS DealerName, DNC.Address, DNC.PinCode, DNC.ContactNo,          
    DNC.FaxNo, DNC.EMailId, DNC.WebSite, DNC.WorkingHours,          
    CMA.Name AS CarMake, C.Name AS City, S.Name AS State, NCD_Website          
   FROM Dealer_NewCar AS DNC WITH(NOLOCK) --Modified By: Avishkar on  <01/09/2013> to use WITH(NOLOCK)          
   LEFT JOIN NCD_Dealers Nd WITH(NOLOCK) ON DNC.Id = Nd.DealerId AND Nd.IsActive = 1          
   JOIN Cities AS C WITH(NOLOCK) ON DNC.CityId = C.ID          
   JOIN States AS S WITH(NOLOCK) ON C.StateId = S.Id          
   JOIN CarMakes AS CMA WITH(NOLOCK) ON DNC.MakeId = CMA.ID           
   WHERE DNC.CityId = @CityId           
   AND DNC.MakeId = @MakeId           
   AND DNC.IsActive = 1          
   AND C.IsDeleted = 0           
   AND S.IsDeleted = 0           
   AND CMA.IsDeleted = 0          
   ORDER BY NCD_Website DESC, DealerName;          
 end       
 END 

