IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetPriceQuoteDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetPriceQuoteDetails]
GO

	-- =============================================
-- Author:		Chetan Kane
-- Create date: 3/5/2012, 
-- Description:	SP to Getting Price Quote Details for particular City and Version
-- Modified By : Tejashree Patil on 1 Feb 2013, join with TC_InquiriesLead instead of TC_Inquiries according to new Inquiries module
-- Modified By : Manish Chourasiya on 16-5-2013, take paramete directly through dealer website no dependecy on TC.
-- EXEC [dbo].[Microsite_GetPriceQuoteDetails]  @CityId=1,@VersionId=3
-- =============================================

CREATE PROCEDURE [dbo].[Microsite_GetPriceQuoteDetails]  
	@CityId INT,  -- Parametre add by manish on 16-05--2013 since city come directly
	@VersionId INT -- Parametre add by manish on 16-05--2013 since version id come directly
	
AS 
  
-- getting verion_id and city id for particular enq  
	/*SELECT @CityId = NBI.CityId, @VersionId = NBI.VersionID 
	FROM TC_NewCarInquiries AS NBI
	INNER JOIN TC_InquiriesLead AS TCI ON NBI.TC_InquiriesLeadId = TCI.TC_InquiriesLeadId 
	WHERE NBI.TC_NewCarInquiriesId  = @TC_PriceQuoteId*/ --Commneted by manish on 16-05-2013 since city and version parametre will directly come.
 
 --  fetching price details  
 IF (@CityId IS NOT NULL AND @VersionId IS NOT NULL)  
 BEGIN 	 
	 SELECT Price AS [Price], 
	        RTO AS [RTO], 
	        Insurance AS [Insurance], 
	        (Price+RTO+Insurance) AS [OnRoadPrice],
			C.Name AS [CityName], 
		   (MMV.Make +' '+ MMV.Model +' '+ MMV.Version) AS [CarName]
	 FROM	NewCarShowroomPrices   AS N WITH (NOLOCK)
			INNER JOIN  Cities AS C WITH (NOLOCK) ON C.Id=N.CityId
			INNER JOIN vwMMV AS MMV ON N.CarVersionId = MMV.VersionId 
	 WHERE N.CarVersionId=@VersionId 
	   AND N.CityId=@CityId	 
 END
 
 
 
 



