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
-- modified by Akansha on 16.09.2015
-- Modified TotalView Count calculation 
--- Modified by Manish on 23-12-2015 changed the query by split into two subqueries for optimization purpose
--- Modified by Manish on 30-12-2015 commented join in where clause since it is aready handled in Inner join condition
-- Modified by Supriya Bhide on 06-05-2016, Added packagetype 41 (Pan India package) for BBT
CREATE PROCEDURE [dbo].[GetDealersForCity_v15.9.1] @CityId INT 
AS 
       BEGIN
          
		  SET NOCOUNT ON;

             WITH CTE1 AS
	   (          SELECT       D.Id DealerId
		              ,SP.thumbnail ShowroomImg
					  ,SP.hosturl   HostUrl
					  ,SP.OriginalImgPath OriginalImgPath
		              ,ROW_NUMBER() OVER(PARTITION BY D.id Order by SP.Id DESC) RowNum
				FROM   ShowRoomPhotos AS SP WITH (NOLOCK)
				JOIN   Dealers AS D  WITH (NOLOCK) ON SP.DealerId=D.ID
				WHERE  SP.imagecategory = 1 
					   AND SP.isactive = 1 
					   AND SP.ismainphoto = 1 
					  -- AND SP.dealerid = D.id
					   AND D.CityId=@CityId
					  ),
		 CTE2 AS 
					( 
							SELECT    D.id AS DealerId, 
						 D.cityid, D.organization, 
						 --Isnull(Sum(Si.viewcount), '0') AS TotalView,
						 sum(ISNULL(du.Viewcount,0))+Isnull(Sum(Si.viewcount), '0') as TotalView, ---- Modified by Sachin Shukla on 11-09-2015 , Added Column TotalView 
						 Count(Si.id) TotalCars,
						 D.ActiveMaskingNumber  -- Modified by Sachin Shukla on 31-08-2015 , Added Column ActiveMaskingNo 
				FROM	dealers AS D WITH (NOLOCK)
						INNER JOIN sellinquiries Si WITH (NOLOCK) ON Si.DealerId = D.ID 
						INNER JOIN ActiveDealers AD WITH (NOLOCK) ON AD.DealerId = D.ID 
						                                         AND AD.isactive = 1 
																 AND AD.hasshowroom = 1	
						INNER JOIN consumercreditpoints  AS CCP WITH (NOLOCK) ON CCP.ConsumerId = D.ID 
						                                                     AND CCP.consumertype = 1 
																			 AND CCP.packagetype in (29, 19, 41) -- Modified by Supriya Bhide on 06-05-2016 
																			 AND (CCP.expirydate + 1) >= Getdate()		
						LEFT JOIN DealerUsedCarViews du WITH (NOLOCK) on du.InquiryID=si.ID -- Modified by Sachin Shukla on 11-09-2015   	
				WHERE  D.id = Si.dealerid                      
					   AND D.status = 0 
					   --AND D.ID NOT IN (3838,4271)
					   AND D.cityid = @CityId 
				GROUP  BY D.id, 
						  organization, 
						  cityid,			
						  ActiveMaskingNumber
			)SELECT CTE2.DealerId  DealerId
			       ,CTE2.cityid cityid
				   ,CTE2.organization organization
				   ,CTE2.TotalView TotalView
				   ,CTE2.TotalCars TotalCars
				   ,CTE1.ShowroomImg ShowroomImg
				   ,CTE1.HostUrl    HostUrl
				   ,CTE1.OriginalImgPath OriginalImgPath
				   ,CTE2.ActiveMaskingNumber ActiveMaskingNumber
			 FROM CTE2  WITH (NOLOCK)
			 LEFT JOIN CTE1 WITH (NOLOCK)  ON CTE1.DealerId=CTE2.DealerId
			                 AND CTE1.RowNum=1
			 ORDER BY CTE2.organization
        END