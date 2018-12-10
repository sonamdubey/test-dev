IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetSKODAPriceQuoteDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetSKODAPriceQuoteDetails]
GO

	-- =============================================          

-- Author:  Vikas J          

-- Create date: 10/5/2013,           

-- Description: SP to Getting Price Quote Details for particular City and Version(Used only for SKODA Dealer Websites)  

-- Modified by Vikas J on 31/5/13. For calculation of onroad price null values are replaced with 0         

-- Modified by Vikas J on 3/6/13. added a parameter withOctroi
-- Modified By Rakesh Yadav on 12 sep 2014 if CarName and city are not retrieved from 1st query then fetch from respective tables


-- =============================================          

          

CREATE PROCEDURE [dbo].[Microsite_GetSKODAPriceQuoteDetails]            

(            

 @CityId INT,  -- Parametre add by Vikas on 16-05--2013 since city come directly      

 @VersionId INT, -- Parametre add by Vikas on 16-05--2013 since version id come directly          

 @DealerId INT,        

 @Price BIGINT OUT,          

 @RTO INT OUT,          

 @Insurance INT OUT,          

 @OnRoadPrice INT OUT,          

 @City VARCHAR(50) OUT,          

 @CarName VARCHAR(100) OUT,       

 @CRTMCharges INT OUT, --CRTMCharges added by vikas         

 @DriveAssure INT OUT, --DriveAssure added by vikas

 @WithOctroi  BIT OUT --WithOctroi added by vikas     

)       

AS           

          

 --  fetching price details            

 BEGIN             

  SELECT @Price = ExShowroomPrice, @RTO = RTO, @Insurance = Insurance,@CRTMCharges=CRTMCharges,@DriveAssure=DriveAssure, @OnRoadPrice = (ISNULL(ExShowroomPrice,0)+ISNULL(RTO,0)+ISNULL(Insurance,0)+ISNULL(DriveAssure,0)+ISNULL(CRTMCharges,0)),          

   @City=C.Name, @CarName = (MMV.Make +' '+ MMV.Model +' '+ MMV.Version),@WithOctroi=N.withOctroi          

  FROM DealerWebsite_ExShowRoomPrices  AS N           

   INNER JOIN  Cities AS C ON C.Id=N.CityId          

   INNER JOIN vwMMV AS MMV ON N.CarVersionId = MMV.VersionId           

  WHERE CarVersionId=@VersionId and CityId=@CityId and DealerId=@DealerId            

  IF @CarName IS NULL OR @CarName=''
	SELECT @CarName = (Make +' '+ Model +' '+ Version) FROM vwMMV WHERE VersionId=@VersionId

  IF @City IS NULL OR @City=''
	SELECT @City= Name FROM Cities WHERE ID=@CityId

 END          

           
