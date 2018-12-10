IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealersForCity_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealersForCity_v16]
GO

	
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Author: Sachin Shukla
-- Created on: 11.09.2015
-- Description: Gets all active dealers having showroom for a particular city
-- This SP is the Version OF Sp "[GetDealersForCity_15.9.1]" added OriginalImgPath For Image .
-- Modified by Sachin Shukla on 31-08-2015 , Added Column ActiveMaskingNo 
-- Added Total view Cars Count For respective dealers
-- modified by Akansha on 16.09.2015
-- Modified TotalView Count calculation 
-- Modified By - Pawan Kumar - Removed references of active dealers and consumercreditpoints and add CT_AddOnPackages table check 
CREATE PROCEDURE [dbo].[GetDealersForCity_v16.8.3] @CityId INT 
AS 
BEGIN
   SELECT    D.id AS DealerId, 
             D.cityid, D.organization, 
			 --Isnull(Sum(Si.viewcount), '0') AS TotalView,
			 sum(ISNULL(du.Viewcount,0))+Isnull(Sum(Si.viewcount), '0') as TotalView, ---- Modified by Sachin Shukla on 11-09-2015 , Added Column TotalView 
		     Count(Si.id) TotalCars,
			(SELECT TOP 1 thumbnail 
				FROM   showroomphotos WITH (NOLOCK)
				WHERE  imagecategory = 1 
					   AND isactive = 1 
					   AND ismainphoto = 1 
					   AND dealerid = D.id)   AS ShowroomImg, 
		   (SELECT TOP 1 hosturl
			FROM   showroomphotos WITH (NOLOCK)
			WHERE  imagecategory = 1 
				   AND isactive = 1 
				   AND ismainphoto = 1 
				   AND dealerid = D.id)   AS HostUrl,				    
			(SELECT TOP 1 OriginalImgPath 
			FROM   showroomphotos  WITH (NOLOCK)
			WHERE  imagecategory = 1 
				   AND isactive = 1 
				   AND ismainphoto = 1 
				   AND dealerid = D.id)   AS OriginalImgPath ,
				   D.ActiveMaskingNumber  -- Modified by Sachin Shukla on 31-08-2015 , Added Column ActiveMaskingNo 
	FROM	dealers AS D WITH (NOLOCK)
			INNER JOIN sellinquiries Si WITH (NOLOCK) ON Si.DealerId = D.ID 
			INNER JOIN CT_AddOnPackages AD WITH(NOLOCK) ON AD.CWDealerId = D.ID AND AD.IsActive = 1 AND AD.AddOnPackageId = 100	
			AND (AD.EndDate+ 1) >= Getdate()
			LEFT JOIN DealerUsedCarViews du WITH (NOLOCK) on du.InquiryID=si.ID -- Modified by Sachin Shukla on 11-09-2015.  		
	WHERE  D.id = Si.dealerid                      
		   AND D.status = 0 
		   --AND D.ID NOT IN (3838,4271)
		   AND D.cityid = @CityId 
	GROUP  BY D.id, 
			  organization, 
			  cityid,			
			  ActiveMaskingNumber
	ORDER  BY organization
END
