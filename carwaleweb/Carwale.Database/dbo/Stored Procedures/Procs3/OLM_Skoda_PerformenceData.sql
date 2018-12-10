IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_Skoda_PerformenceData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_Skoda_PerformenceData]
GO

	
-- =============================================
-- Author:		Jayant Mhatre
-- Create date: 11-1-2012
-- Description:	Geting Performence data in skoda olm panel
-- =============================================
 

CREATE PROCEDURE [dbo].[OLM_Skoda_PerformenceData]
	
	
	(
	@Type		SmallInt,
	@RegionId	int,
	@DealerId	int,
	@CityId		int,
	@ModelId	int,
	@VersionId  int,
	@FuelType   int,
    @DateFrom   DateTime,
    @DateTo		DateTime,
    @Top		Numeric = null,
	@Bottom		Numeric = null
	)
	AS
	
		BEGIN   -- table 0 will return total data of toal leads and booked leads
			IF	@Type = 1
				BEGIN
				
					SELECT COUNT(DISTINCT CL.Id) AS TotalLeads,COUNT(DISTINCT CCBD.ID) AS TotalBookings, ND.OId AS OrgId, NDO.Name AS OrgName
					FROM CRM_Leads AS CL  WITH (NOLOCK)  
						JOIN CRM_CarBasicData AS CBD  WITH (NOLOCK) ON CL.ID = CBD.LeadId
						JOIN vwMMV AS VM  WITH (NOLOCK) ON CBD.VersionId = VM.VersionId AND VM.MakeId = 15
						JOIN CRM_SkodaDealerAssignment AS CSD  WITH (NOLOCK) ON CSD.LeadId = CL.Id AND CSD.PushStatus = 'SUCCESS'
						JOIN NCS_SubDealerOrganization AS ND  WITH (NOLOCK) ON CSD.DealerId = ND.DId
						JOIN NCS_DealerOrganization AS NDO  WITH (NOLOCK) ON ND.OId = NDO.Id
						JOIN NCS_Dealers AS NDS ON NDS.Id = ND.DId
						JOIN OLM_RegionCities AS OC  WITH (NOLOCK) ON NDS.CityId = OC.CityId
						LEFT JOIN CRM_CarBookingLog AS CCBD ON CCBD.CBDId = CBD.ID AND CCBD.IsBookingCompleted = 1
					WHERE CL.CreatedOnDatePart BETWEEN @DateFrom AND @DateTo AND OC.RegionId= @RegionId
					AND(@DealerId = 0 OR  (CSD.DealerId=@DealerId))
					AND (@CityId = 0 OR  (OC.CityId=@CityId))
					AND (@VersionId = 0 OR (VM.VersionId = @VersionId))
					AND (@FuelType = -1 OR (VM.ModelId = @ModelId AND VM.CarFuelType = @FuelType))
					AND (@ModelId = -1 OR (VM.ModelId = @ModelId))
					GROUP BY  ND.OId, NDO.Name
					ORDER BY TotalBookings
				END
			ELSE
				IF @Type = 2
					BEGIN
						--table 1 will return top and bottom organization data
						SELECT  COUNT(DISTINCT CCBD.CBDId) AS TotalBookings,ND.OId AS OrgId,(CCBD.BookingCompleteDate)DateVal
						FROM CRM_Leads AS CL  WITH (NOLOCK)  
							JOIN CRM_CarBasicData AS CBD  WITH (NOLOCK) ON CL.ID = CBD.LeadId
							JOIN vwMMV AS VM  WITH (NOLOCK) ON CBD.VersionId = VM.VersionId AND VM.MakeId = 15
							JOIN CRM_SkodaDealerAssignment AS CSD  WITH (NOLOCK) ON CSD.LeadId = CL.Id AND CSD.PushStatus = 'SUCCESS'
							JOIN NCS_SubDealerOrganization AS ND  WITH (NOLOCK) ON CSD.DealerId = ND.DId
							JOIN NCS_DealerOrganization AS NDO  WITH (NOLOCK) ON ND.OId IN(@Top,@Bottom)
							JOIN CRM_CarBookingLog AS CCBD ON CCBD.CBDId = CBD.ID AND CCBD.IsBookingCompleted = 1
						WHERE CL.CreatedOnDatePart BETWEEN @DateFrom AND @DateTo 
						AND (@VersionId = 0 OR (VM.VersionId = @VersionId))
						AND (@FuelType = -1 OR (VM.ModelId = @ModelId AND VM.CarFuelType = @FuelType))
						AND (@ModelId = -1 OR (VM.ModelId = @ModelId))
						GROUP BY ND.OId, (CCBD.BookingCompleteDate)
					END
			
	    END
	    
	    
	    
		
	


