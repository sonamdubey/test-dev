IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_FillShowroomServiceDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_FillShowroomServiceDetails]
GO

	-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 7 Jun 2013
-- Description : Get Dealers Showroom And Service  Details 
-- =============================================
CREATE PROCEDURE [dbo].[OLM_FillShowroomServiceDetails]
    (
    @Id NUMERIC(18,0),
    @Type NUMERIC(18,0)
	)
	AS
	
	BEGIN
	    IF @Type = 1
			BEGIN
		         SELECT FSD.DealerName,FSD.DealerCode,OSD.SkodaDealerId AS DId,OSD.SOutlet AS OutletName,OSD.OutletCode, OSD.CityId,OSD.SAddress AS ShowRoomAddress,
                    OSD.SContactNo, OSD.SEmail,OSD.SFax,OSD.IsRapidOutlet,OSD.SOutlet,OSD.Lattitude,OSD.Longitude,OSD.UpdatedBy,OSD.UpdatedOn 
                 FROM OLM_ShowroomDetails AS OSD WITH(NOLOCK) 
                 INNER JOIN FB_SkodaDealers AS FSD WITH(NOLOCK) ON FSD.Id=OSD.SkodaDealerId 
                 WHERE OSD.Id =@Id
		    
			END
	    ELSE IF @Type=2
	        BEGIN
	             SELECT FSD.DealerName,FSD.DealerCode,OSCD.SkodaDealerId AS DId,OSCD.SOutlet AS OutletName,OSCD.OutletCode, OSCD.CityId,OSCD.SAddress AS ShowRoomAddress,
                    OSCD.SContactNo, OSCD.SEmail,OSCD.SFax,OSCD.IsRapidOutlet,OSCD.SOutlet,OSCD.Lattitude,OSCD.Longitude,OSCD.UpdatedBy,OSCD.UpdatedOn 
                 FROM OLM_ServiceCenterDetails AS OSCD WITH(NOLOCK) 
                 INNER JOIN FB_SkodaDealers AS FSD  WITH(NOLOCK)  ON FSD.Id=OSCD.SkodaDealerId 
                 WHERE OSCD.Id =@Id
	        END
	     ELSE IF @Type=3
	        BEGIN
	            SELECT FSD.DealerName,FSD.DealerCode,ODD.SkodaDealerId AS DId, ODD.CityId,ODD.DealerPrinciple,ODD.DMobileNoAndEmail, ODD.ContactPerson,
	               ODD.CMobileNoAndEmail,ODD.UpdatedBy,ODD.UpdatedOn 
                FROM OLM_DealerDetails AS ODD WITH(NOLOCK) 
                INNER JOIN FB_SkodaDealers AS FSD  WITH(NOLOCK)  ON FSD.Id=ODD.SkodaDealerId 
                WHERE ODD.Id =@Id
	        
	        
	        END
	    
	END