IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_GetDealersData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_GetDealersData]
GO

	-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 5 Jun 2013
-- Description : Get Dealers Details 
-- Modified By:-1. Rahul Kumar on 18-10-2013  (Change column Dealer code by Saddress and put order by constraints)
-- =============================================
CREATE PROCEDURE [dbo].[OLM_GetDealersData]
    (
    @CityId NUMERIC(18,0)
	)
	AS
	
	BEGIN
	    SELECT ODD.id,FSD.DealerName,FSD.DealerCode,C.Name AS CityName ,ODD.DealerPrinciple,ODD.CMobileNoAndEmail,ODD.ContactPerson,ODD.DMobileNoAndEmail
		   FROM OLM_DealerDetails AS ODD WITH(NOLOCK)
		   LEFT JOIN FB_SkodaDealers AS FSD WITH(NOLOCK) ON FSD.Id = ODD.SkodaDealerId 
		   LEFT JOIN Cities C WITH (NOLOCK) ON ODD.CityId = C.ID  
		   WHERE  C.Id=@CityId AND (ODD.IsDeleted=0 OR ODD.IsDeleted IS NULL)
		   ORDER BY FSD.DealerName,C.Name,ODD.DealerPrinciple
       
       
		SELECT OSD.id,FSD.DealerName,OSD.SAddress,C.Name AS CityName ,OSD.SOutlet,OSD.SEmail,OSD.SContactNo
		   FROM OLM_ShowroomDetails AS OSD WITH(NOLOCK)
		   LEFT JOIN FB_SkodaDealers AS FSD WITH(NOLOCK) ON FSD.Id = OSD.SkodaDealerId 
		   LEFT JOIN Cities C WITH (NOLOCK) ON OSD.CityId = C.ID  
		   WHERE  C.Id=@CityId AND (OSD.IsDeleted=0 OR OSD.IsDeleted IS NULL)  
		   ORDER BY FSD.DealerName,C.Name,OSD.SOutlet 
 
		 SELECT OSCD.id,FSD.DealerName,OSCD.SAddress,C.Name AS CityName ,OSCD.SOutlet,OSCD.SEmail,OSCD.SContactNo
		   FROM OLM_ServiceCenterDetails AS OSCD WITH(NOLOCK)
		   LEFT JOIN FB_SkodaDealers AS FSD WITH(NOLOCK) ON FSD.Id = OSCD.SkodaDealerId 
		   LEFT JOIN Cities C WITH (NOLOCK) ON OSCD.CityId = C.ID  
		   WHERE  C.Id =@CityId AND (OSCD.IsDeleted=0 OR OSCD.IsDeleted IS NULL)
		   ORDER BY FSD.DealerName,C.Name,OSCD.SOutlet 
	END