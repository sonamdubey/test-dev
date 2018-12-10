IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealersForCity_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealersForCity_v15]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Author: Sachin Shukla
-- Created on: 11.09.2015
-- Description: Gets all active dealers having showroom for a particular city
-- This SP is the Version OF Sp "[GetDealersForCity_15.9.1]" added OriginalImgPath For Image .
-- Modified by Sachin Shukla on 31-08-2015 , Added Column ActiveMaskingNo 
-- Added Total view Cars Count For respective dealers
CREATE PROCEDURE [dbo].[GetDealersForCity_v15.9.3] @CityId INT 
AS 
BEGIN
   SELECT    D.id AS DealerId, 
             D.cityid, D.organization, 
			 --Isnull(Sum(Si.viewcount), '0') AS TotalView,
			 sum(ISNULL(du.Viewcount,0)) as TotalView, ---- Modified by Sachin Shukla on 11-09-2015 , Added Column TotalView 
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
		    LEFT JOIN DealerUsedCarViews du WITH (NOLOCK) on du.InquiryID=si.ID -- Modified by Sachin Shukla on 11-09-2015   	
			--INNER JOIN ActiveDealers AD WITH (NOLOCK) ON AD.DealerId = D.ID AND AD.isactive = 1 AND AD.hasshowroom = 1	
			--INNER JOIN consumercreditpoints  AS CCP WITH (NOLOCK) ON CCP.ConsumerId = D.ID AND CCP.consumertype = 1 AND CCP.packagetype in (29, 19) AND (CCP.expirydate + 1) >= Getdate()		
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