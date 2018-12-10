IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealersForCity_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealersForCity_v15]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Author: Sachin Shukla
-- Created on: 06.08.2015
-- Description: Gets all active dealers having showroom for a particular city
-- This SP is the Version OF Sp "[GetDealersForCity]" added OriginalImgPath For Image .
CREATE PROCEDURE [dbo].[GetDealersForCity_v15.8.1] @CityId INT 
AS 
   SELECT D.id AS DealerId, D.cityid, D.organization, Isnull(Sum(Si.viewcount), '0') AS TotalView, Count(Si.id) TotalCars,
			(SELECT TOP 1 thumbnail 
				FROM   showroomphotos 
				WHERE  imagecategory = 1 
					   AND isactive = 1 
					   AND ismainphoto = 1 
					   AND dealerid = D.id)   AS ShowroomImg, 
		   (SELECT TOP 1 hosturl
			FROM   showroomphotos 
			WHERE  imagecategory = 1 
				   AND isactive = 1 
				   AND ismainphoto = 1 
				   AND dealerid = D.id)   AS HostUrl,				    
			(SELECT TOP 1 OriginalImgPath 
			FROM   showroomphotos 
			WHERE  imagecategory = 1 
				   AND isactive = 1 
				   AND ismainphoto = 1 
				   AND dealerid = D.id)   AS OriginalImgPath 

	FROM	dealers AS D 
			INNER JOIN sellinquiries Si ON Si.DealerId = D.ID 	
			INNER JOIN ActiveDealers AD ON AD.DealerId = D.ID AND AD.isactive = 1 AND AD.hasshowroom = 1	
			INNER JOIN consumercreditpoints AS CCP ON CCP.ConsumerId = D.ID AND CCP.consumertype = 1 AND CCP.packagetype in (29, 19) AND (CCP.expirydate + 1) >= Getdate()		
	WHERE  D.id = Si.dealerid                      
		   AND D.status = 0 
		   --AND D.ID NOT IN (3838,4271)
		   AND D.cityid = @CityId 
	GROUP  BY D.id, 
			  organization, 
			  cityid
	ORDER  BY organization
