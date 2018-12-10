IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_GetOtherEvents]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_GetOtherEvents]
GO

	
-- =============================================
-- Created By : Jayant Mhatre
-- Create date: 22-06-2011
-- Description:	This procedure is used in CRM-CPA-Reports-ProjectSnap.Cs Page
-- =============================================
CREATE PROCEDURE [dbo].[CRM_GetOtherEvents]
	
	
	@MONTH		Numeric,
	@Year		Numeric,
	@Makes		NVarchar(1000),
	@Cities     NVarchar (1000),
	@Type       Numeric,
	@Duration	NVarchar(1000)
	

AS

BEGIN

		IF(@Duration='Monthly') -- For monthly data
		BEGIN
				IF(@Type=0) --For All Cities 
						BEGIN
						
						-- Will get lost data DataSet Table[0]
						SELECT COUNT(DISTINCT CDA.CBDId) AS Cnt,DAY(CL.CreatedOnDatePart)  AS DayOfEvent,CDA.Status AS Status, CL.LeadStageId,CBB.BookingStatusId AS Bstatus
						FROM CRM_Leads AS CL,CarVersions AS CV,CarModels AS MO,CRM_CarDealerAssignment AS CDA,CRM_CarBasicData AS CBD JOIN CRM_CarBookingData AS CBB ON CBD.Id= CBB.CarBasicDataId 
						WHERE CL.ID=CBD.LeadId AND CBD.ID=CDA.CBDId AND  MONTH(CL.CreatedOnDatePart) = @MONTH AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes)) 
						GROUP BY  DAY(CL.CreatedOnDatePart),CDA.Status,CL.LeadStageId ,CBB.BookingStatusId 
					
						--Will get Price Quote Data
						SELECT COUNT(PQ.Id) AS PqReq,ISNULL(PQ.IsPQCompleted,0) AS PqComp,ISNULL(PQ.IsPQNotRequired,0) AS PQNotReq,DAY(CL.CreatedOnDatePart) AS DayOfEvent 
						FROM CRM_CarPQLog As PQ,CRM_CarBasicData AS CBD,CRM_Leads AS CL,CarVersions AS CV,CarModels AS MO
						WHERE PQ.CBDId=CBD.ID AND CBD.LeadId=CL.ID AND MONTH(CL.CreatedOnDatePart) = @MONTH AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes)) 
						GROUP BY  DAY(CL.CreatedOnDatePart),PQ.IsPQCompleted,PQ.IsPQNotRequired
						
						--Will get Test Drive Data
						SELECT COUNT(TD.Id) AS tdReq,ISNULL(TD.IsTDCompleted,0) AS tdComp,ISNULL(TD.ISTDNotPossible,0) AS tdNotPos,DAY(CL.CreatedOnDatePart) AS DayOfEvent 
						FROM CRM_CarTDLog AS TD,CRM_CarBasicData AS CBD,CRM_Leads AS CL,CarVersions AS CV,CarModels AS MO
						WHERE TD.CBDId=CBD.ID AND CBD.LeadId=CL.ID AND MONTH(CL.CreatedOnDatePart) = @MONTH AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes))  
						GROUP BY  DAY(CL.CreatedOnDatePart),TD.IsTDCompleted,TD.ISTDNotPossible
				        
						--Will get Booking Data
						SELECT COUNT(CBL.Id) AS bqReq,ISNULL(CBL.IsBookingCompleted,0) AS bqComp ,ISNULL(CBL.IsBookingNotPossible,0) AS bqNotPos,ISNULL(CBL.IsPriorBooking,0) AS bqPrior,DAY(CL.CreatedOnDatePart) AS DayOfEvent 
						FROM CRM_CarBookingLog As CBL,CRM_CarBasicData AS CBD,CRM_Leads AS CL,CarVersions AS CV,CarModels AS MO
						WHERE CBL.CBDId=CBD.ID AND CBD.LeadId=CL.ID AND MONTH(CL.CreatedOnDatePart) = @MONTH AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes))  
						GROUP BY  DAY(CL.CreatedOnDatePart),CBL.IsBookingCompleted,CBL.IsBookingNotPossible,CBL.IsPriorBooking
						
						--Will get Product Data
						SELECT COUNT(CBD.Id) AS Cnt,DAY(CL.CreatedOnDatePart)  AS DayOfEvent
						FROM CRM_Leads AS CL, CRM_CarBasicData AS CBD,CarVersions AS CV,CarModels AS MO,CRM_CarDealerAssignment AS CDA
						WHERE CL.ID=CBD.LeadId AND CBD.ID=CDA.CBDId AND CBD.IsProductExplained=1 AND  MONTH(CL.CreatedOnDatePart) = @MONTH AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes)) 
						GROUP BY  DAY(CL.CreatedOnDatePart)
						
						--Will get Car Delivered Data
						SELECT COUNT(CDD.Id) AS Cnt,DAY(CL.CreatedOnDatePart) AS DayOfEvent 
						FROM CRM_CarDeliveryData AS CDD ,CRM_CarBasicData AS CBD,CRM_Leads AS CL,CarVersions AS CV,CarModels AS MO
						WHERE CDD.DeliveryStatusId=20 AND CDD.CarBasicDataId=CBD.ID AND CBD.LeadId=CL.ID AND MONTH(CL.CreatedOnDatePart) = @MONTH AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes))  
						GROUP BY DAY(CL.CreatedOnDatePart)
						
						
						END
				
				IF(@Type=1) --For Slected Cities 
						BEGIN
						
						-- Will get lost data
						SELECT COUNT(DISTINCT CL.ID) AS Cnt,DAY(CL.CreatedOnDatePart)  AS DayOfEvent,CDA.Status AS Status , CL.LeadStageId,CBB.BookingStatusId AS Bstatus
						FROM CRM_CarBasicData AS CBD,CarVersions AS CV,CarModels AS MO,CRM_CarDealerAssignment AS CDA,CRM_CarBookingData AS CBB,CRM_Leads AS CL JOIN CRM_Customers AS CC ON CC.ID=CL.CNS_CustId
						WHERE  CBD.Id= CBB.CarBasicDataId AND CL.ID=CBD.LeadId AND CBD.ID=CDA.CBDId AND  MONTH(CL.CreatedOnDatePart) = @MONTH AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes)) AND CC.CityId IN (select * from list_to_tbl(@Cities))
						GROUP BY  DAY(CL.CreatedOnDatePart),CDA.Status,CL.LeadStageId  ,CBB.BookingStatusId
					
						--Will get Price Quote Data
						SELECT COUNT(PQ.Id) AS PqReq,ISNULL(PQ.IsPQCompleted,0) AS PqComp,ISNULL(PQ.IsPQNotRequired,0) AS PQNotReq,DAY(CL.CreatedOnDatePart) AS DayOfEvent 
						FROM CRM_CarPQLog As PQ,CRM_CarBasicData AS CBD,CarVersions AS CV,CarModels AS MO,CRM_Leads AS CL JOIN CRM_Customers AS CC ON CC.ID=CL.CNS_CustId
						WHERE PQ.CBDId=CBD.ID AND CBD.LeadId=CL.ID AND MONTH(CL.CreatedOnDatePart) = @MONTH AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes)) AND CC.CityId IN (select * from list_to_tbl(@Cities)) 
						GROUP BY  DAY(CL.CreatedOnDatePart),PQ.IsPQCompleted,PQ.IsPQNotRequired
						
						--Will get Test Drive Data
						SELECT COUNT(TD.Id) AS tdReq,ISNULL(TD.IsTDCompleted,0) AS tdComp,ISNULL(TD.ISTDNotPossible,0) AS tdNotPos,DAY(CL.CreatedOnDatePart) AS DayOfEvent 
						FROM CRM_CarTDLog AS TD,CRM_CarBasicData AS CBD,CarVersions AS CV,CarModels AS MO,CRM_Leads AS CL JOIN CRM_Customers AS CC ON CC.ID=CL.CNS_CustId
						WHERE TD.CBDId=CBD.ID AND CBD.LeadId=CL.ID AND MONTH(CL.CreatedOnDatePart) = @MONTH AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes)) AND CC.CityId IN (select * from list_to_tbl(@Cities)) 
						GROUP BY  DAY(CL.CreatedOnDatePart),TD.IsTDCompleted,TD.ISTDNotPossible
				        
						--Will get Booking Data
						SELECT COUNT(CBL.Id) AS bqReq,ISNULL(CBL.IsBookingCompleted,0) AS bqComp ,ISNULL(CBL.IsBookingNotPossible,0) AS bqNotPos,ISNULL(CBL.IsPriorBooking,0) AS bqPrior,DAY(CL.CreatedOnDatePart) AS DayOfEvent 
						FROM CRM_CarBookingLog As CBL,CRM_CarBasicData AS CBD,CarVersions AS CV,CarModels AS MO,CRM_Leads AS CL JOIN CRM_Customers AS CC ON CC.ID=CL.CNS_CustId
						WHERE CBL.CBDId=CBD.ID AND CBD.LeadId=CL.ID AND MONTH(CL.CreatedOnDatePart) = @MONTH AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes)) AND CC.CityId IN (select * from list_to_tbl(@Cities)) 
						GROUP BY  DAY(CL.CreatedOnDatePart),CBL.IsBookingCompleted,CBL.IsBookingNotPossible,CBL.IsPriorBooking
						
						--Will get Product Data
						SELECT COUNT(CBD.Id) AS Cnt,DAY(CL.CreatedOnDatePart)  AS DayOfEvent
						FROM CRM_CarBasicData AS CBD,CarVersions AS CV,CarModels AS MO,CRM_CarDealerAssignment AS CDA,CRM_Leads AS CL JOIN CRM_Customers AS CC ON CC.ID=CL.CNS_CustId
						WHERE CL.ID=CBD.LeadId AND CBD.ID=CDA.CBDId AND CBD.IsProductExplained=1 AND  MONTH(CL.CreatedOnDatePart) = @MONTH AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes)) AND CC.CityId IN (select * from list_to_tbl(@Cities))
						GROUP BY  DAY(CL.CreatedOnDatePart)
						
						--Will Get Delivered Data
						SELECT COUNT(CDD.Id) AS Cnt,DAY(CL.CreatedOnDatePart) AS DayOfEvent 
						FROM CRM_CarDeliveryData AS CDD ,CRM_CarBasicData AS CBD,CarVersions AS CV,CarModels AS MO,CRM_Leads AS CL  JOIN CRM_Customers AS CC ON CC.ID=CL.CNS_CustId
						WHERE CDD.DeliveryStatusId=20 AND CDD.CarBasicDataId=CBD.ID AND CBD.LeadId=CL.ID AND MONTH(CL.CreatedOnDatePart) = @MONTH AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes))   AND CC.CityId IN (select * from list_to_tbl(@Cities))
						GROUP BY  DAY(CL.CreatedOnDatePart)
						
						
						END
				
				IF(@Type=2) -- For Other Cities
						BEGIN
						
						-- Will get lost data
						SELECT COUNT(DISTINCT CL.ID) AS Cnt,DAY(CL.CreatedOnDatePart)  AS DayOfEvent,CDA.Status AS Status , CL.LeadStageId,CBB.BookingStatusId AS Bstatus
						FROM CRM_CarBasicData AS CBD,CarVersions AS CV,CarModels AS MO,CRM_CarDealerAssignment AS CDA,CRM_CarBookingData AS CBB,CRM_Leads AS CL JOIN CRM_Customers AS CC ON CC.ID=CL.CNS_CustId
						WHERE CBD.Id= CBB.CarBasicDataId AND CL.ID=CBD.LeadId AND CBD.ID=CDA.CBDId AND  MONTH(CL.CreatedOnDatePart) = @MONTH AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes)) AND CC.CityId NOT IN (select * from list_to_tbl(@Cities))
						GROUP BY  DAY(CL.CreatedOnDatePart),CDA.Status,CL.LeadStageId ,CBB.BookingStatusId 
					
						--Will get Price Quote Data
						SELECT COUNT(PQ.Id) AS PqReq,ISNULL(PQ.IsPQCompleted,0) AS PqComp,ISNULL(PQ.IsPQNotRequired,0) AS PQNotReq,DAY(CL.CreatedOnDatePart) AS DayOfEvent 
						FROM CRM_CarPQLog As PQ,CRM_CarBasicData AS CBD,CarVersions AS CV,CarModels AS MO,CRM_Leads AS CL JOIN CRM_Customers AS CC ON CC.ID=CL.CNS_CustId
						WHERE PQ.CBDId=CBD.ID AND CBD.LeadId=CL.ID AND MONTH(CL.CreatedOnDatePart) = @MONTH AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes)) AND CC.CityId NOT IN (select * from list_to_tbl(@Cities))
						GROUP BY  DAY(CL.CreatedOnDatePart),PQ.IsPQCompleted,PQ.IsPQNotRequired
						
						--Will get Test Drive Data
						SELECT COUNT(TD.Id) AS tdReq,ISNULL(TD.IsTDCompleted,0) AS tdComp,ISNULL(TD.ISTDNotPossible,0) AS tdNotPos,DAY(CL.CreatedOnDatePart) AS DayOfEvent 
						FROM CRM_CarTDLog AS TD,CRM_CarBasicData AS CBD,CarVersions AS CV,CarModels AS MO,CRM_Leads AS CL JOIN CRM_Customers AS CC ON CC.ID=CL.CNS_CustId
						WHERE TD.CBDId=CBD.ID AND CBD.LeadId=CL.ID AND MONTH(CL.CreatedOnDatePart) = @MONTH AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes)) AND CC.CityId NOT IN (select * from list_to_tbl(@Cities)) 
						GROUP BY  DAY(CL.CreatedOnDatePart),TD.IsTDCompleted,TD.ISTDNotPossible
				        
						--Will get Booking Data
						SELECT COUNT(CBL.Id) AS bqReq,ISNULL(CBL.IsBookingCompleted,0) AS bqComp ,ISNULL(CBL.IsBookingNotPossible,0) AS bqNotPos,ISNULL(CBL.IsPriorBooking,0) AS bqPrior,DAY(CL.CreatedOnDatePart) AS DayOfEvent 
						FROM CRM_CarBookingLog As CBL,CRM_CarBasicData AS CBD,CarVersions AS CV,CarModels AS MO,CRM_Leads AS CL JOIN CRM_Customers AS CC ON CC.ID=CL.CNS_CustId
						WHERE CBL.CBDId=CBD.ID AND CBD.LeadId=CL.ID AND MONTH(CL.CreatedOnDatePart) = @MONTH AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes)) AND CC.CityId NOT IN (select * from list_to_tbl(@Cities)) 
						GROUP BY  DAY(CL.CreatedOnDatePart),CBL.IsBookingCompleted,CBL.IsBookingNotPossible,CBL.IsPriorBooking
						
						--Will get Product Data
						SELECT COUNT(CBD.Id) AS Cnt,DAY(CL.CreatedOnDatePart)  AS DayOfEvent
						FROM CRM_CarBasicData AS CBD,CarVersions AS CV,CarModels AS MO,CRM_CarDealerAssignment AS CDA,CRM_Leads AS CL JOIN CRM_Customers AS CC ON CC.ID=CL.CNS_CustId
						WHERE CL.ID=CBD.LeadId AND CBD.ID=CDA.CBDId AND CBD.IsProductExplained=1 AND  MONTH(CL.CreatedOnDatePart) = @MONTH AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes))AND CC.CityId NOT IN (select * from list_to_tbl(@Cities)) 
						GROUP BY  DAY(CL.CreatedOnDatePart)
						
						--Will Get Delivered Data
						SELECT COUNT(CDD.Id) AS Cnt,DAY(CL.CreatedOnDatePart) AS DayOfEvent 
						FROM CRM_CarDeliveryData AS CDD ,CRM_CarBasicData AS CBD,CarVersions AS CV,CarModels AS MO,CRM_Leads AS CL  JOIN CRM_Customers AS CC ON CC.ID=CL.CNS_CustId
						WHERE CDD.DeliveryStatusId=20 AND CDD.CarBasicDataId=CBD.ID AND CBD.LeadId=CL.ID AND MONTH(CL.CreatedOnDatePart) = @MONTH AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes))   AND CC.CityId NOT IN (select * from list_to_tbl(@Cities))
						GROUP BY  DAY(CL.CreatedOnDatePart)
						
						END
		END
		IF(@Duration='Yearly') -- For Yearly data
		
		BEGIN
		
			IF(@Type=0) --For All Cities
					BEGIN
					
					-- Will get lost data
					SELECT COUNT(DISTINCT CL.ID) AS Cnt,MONTH(CL.CreatedOnDatePart)  AS MonthOfEvent,CDA.Status AS Status , CL.LeadStageId,CBB.BookingStatusId AS Bstatus 
					FROM CRM_Leads AS CL,CarVersions AS CV,CarModels AS MO,CRM_CarDealerAssignment AS CDA,CRM_CarBasicData AS CBD JOIN CRM_CarBookingData AS CBB ON CBD.Id= CBB.CarBasicDataId
					WHERE CL.ID=CBD.LeadId AND CBD.ID=CDA.CBDId AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes)) 
					GROUP BY  MONTH(CL.CreatedOnDatePart),CDA.Status,CL.LeadStageId,CBB.BookingStatusId 
				
					--Will get Price Quote Data
					SELECT COUNT(PQ.Id) AS PqReq,ISNULL(PQ.IsPQCompleted,0) AS PqComp,ISNULL(PQ.IsPQNotRequired,0) AS PQNotReq,MONTH(CL.CreatedOnDatePart) AS MonthOfEvent 
					FROM CRM_CarPQLog As PQ,CRM_CarBasicData AS CBD,CRM_Leads AS CL,CarVersions AS CV,CarModels AS MO
					WHERE PQ.CBDId=CBD.ID AND CBD.LeadId=CL.ID  AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes)) 
					GROUP BY  MONTH(CL.CreatedOnDatePart),PQ.IsPQCompleted,PQ.IsPQNotRequired
					
					--Will get Test Drive Data
					SELECT COUNT(TD.Id) AS tdReq,ISNULL(TD.IsTDCompleted,0) AS tdComp,ISNULL(TD.ISTDNotPossible,0) AS tdNotPos,MONTH(CL.CreatedOnDatePart) AS MonthOfEvent
					FROM CRM_CarTDLog AS TD,CRM_CarBasicData AS CBD,CRM_Leads AS CL,CarVersions AS CV,CarModels AS MO
					WHERE TD.CBDId=CBD.ID AND CBD.LeadId=CL.ID AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes))  
					GROUP BY  MONTH(CL.CreatedOnDatePart),TD.IsTDCompleted,TD.ISTDNotPossible
			        
					--Will get Booking Data
					SELECT COUNT(CBL.Id) AS bqReq,ISNULL(CBL.IsBookingCompleted,0) AS bqComp ,ISNULL(CBL.IsBookingNotPossible,0) AS bqNotPos,ISNULL(CBL.IsPriorBooking,0) AS bqPrior,MONTH(CL.CreatedOnDatePart) AS MonthOfEvent
					FROM CRM_CarBookingLog As CBL,CRM_CarBasicData AS CBD,CRM_Leads AS CL,CarVersions AS CV,CarModels AS MO
					WHERE CBL.CBDId=CBD.ID AND CBD.LeadId=CL.ID  AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes))  
					GROUP BY  MONTH(CL.CreatedOnDatePart),CBL.IsBookingCompleted,CBL.IsBookingNotPossible,CBL.IsPriorBooking
					
					--Will get Product Data
					SELECT COUNT(CBD.Id) AS Cnt,MONTH(CL.CreatedOnDatePart) AS MonthOfEvent
					FROM CRM_Leads AS CL, CRM_CarBasicData AS CBD,CarVersions AS CV,CarModels AS MO,CRM_CarDealerAssignment AS CDA
					WHERE CL.ID=CBD.LeadId AND CBD.ID=CDA.CBDId AND CBD.IsProductExplained=1 AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes)) 
					GROUP BY  MONTH(CL.CreatedOnDatePart)
					
					--Will get Car Delivered Data
					SELECT COUNT(CDD.Id) AS Cnt,MONTH(CL.CreatedOnDatePart) AS MonthOfEvent 
					FROM CRM_CarDeliveryData AS CDD ,CRM_CarBasicData AS CBD,CRM_Leads AS CL,CarVersions AS CV,CarModels AS MO
					WHERE CDD.DeliveryStatusId=20 AND CDD.CarBasicDataId=CBD.ID AND CBD.LeadId=CL.ID AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes))  
					GROUP BY MONTH(CL.CreatedOnDatePart)
					
					END
			
			IF(@Type=1)  --For Selected Cities
					BEGIN
					
					-- Will get lost data
					SELECT COUNT(DISTINCT CL.ID) AS Cnt,MONTH(CL.CreatedOnDatePart)  AS MonthOfEvent,CDA.Status AS Status, CL.LeadStageId,CBB.BookingStatusId AS Bstatus  
					FROM CRM_CarBasicData AS CBD,CarVersions AS CV,CarModels AS MO,CRM_CarDealerAssignment AS CDA,CRM_CarBookingData AS CBB,CRM_Leads AS CL JOIN CRM_Customers AS CC ON CC.ID=CL.CNS_CustId
					WHERE CBD.Id= CBB.CarBasicDataId AND CL.ID=CBD.LeadId AND CBD.ID=CDA.CBDId  AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes)) AND CC.CityId IN (select * from list_to_tbl(@Cities))
					GROUP BY  MONTH(CL.CreatedOnDatePart),CDA.Status,CL.LeadStageId,CBB.BookingStatusId 
				
					--Will get Price Quote Data
					SELECT COUNT(PQ.Id) AS PqReq,ISNULL(PQ.IsPQCompleted,0) AS PqComp,ISNULL(PQ.IsPQNotRequired,0) AS PQNotReq,MONTH(CL.CreatedOnDatePart) AS MonthOfEvent 
					FROM CRM_CarPQLog As PQ,CRM_CarBasicData AS CBD,CarVersions AS CV,CarModels AS MO,CRM_Leads AS CL JOIN CRM_Customers AS CC ON CC.ID=CL.CNS_CustId
					WHERE PQ.CBDId=CBD.ID AND CBD.LeadId=CL.ID AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes)) AND CC.CityId IN (select * from list_to_tbl(@Cities)) 
					GROUP BY  MONTH(CL.CreatedOnDatePart),PQ.IsPQCompleted,PQ.IsPQNotRequired
					
					--Will get Test Drive Data
					SELECT COUNT(TD.Id) AS tdReq,ISNULL(TD.IsTDCompleted,0) AS tdComp,ISNULL(TD.ISTDNotPossible,0) AS tdNotPos,MONTH(CL.CreatedOnDatePart) AS MonthOfEvent
					FROM CRM_CarTDLog AS TD,CRM_CarBasicData AS CBD,CarVersions AS CV,CarModels AS MO,CRM_Leads AS CL JOIN CRM_Customers AS CC ON CC.ID=CL.CNS_CustId
					WHERE TD.CBDId=CBD.ID AND CBD.LeadId=CL.ID AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes)) AND CC.CityId IN (select * from list_to_tbl(@Cities)) 
					GROUP BY  MONTH(CL.CreatedOnDatePart),TD.IsTDCompleted,TD.ISTDNotPossible
			        
					--Will get Booking Data
					SELECT COUNT(CBL.Id) AS bqReq,ISNULL(CBL.IsBookingCompleted,0) AS bqComp ,ISNULL(CBL.IsBookingNotPossible,0) AS bqNotPos,ISNULL(CBL.IsPriorBooking,0) AS bqPrior,MONTH(CL.CreatedOnDatePart)AS MonthOfEvent
					FROM CRM_CarBookingLog As CBL,CRM_CarBasicData AS CBD,CarVersions AS CV,CarModels AS MO,CRM_Leads AS CL JOIN CRM_Customers AS CC ON CC.ID=CL.CNS_CustId
					WHERE CBL.CBDId=CBD.ID AND CBD.LeadId=CL.ID  AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes)) AND CC.CityId IN (select * from list_to_tbl(@Cities)) 
					GROUP BY  MONTH(CL.CreatedOnDatePart),CBL.IsBookingCompleted,CBL.IsBookingNotPossible,CBL.IsPriorBooking
					
					--Will get Product Data
					SELECT COUNT(CBD.Id) AS Cnt,MONTH(CL.CreatedOnDatePart)  AS MonthOfEvent
					FROM CRM_CarBasicData AS CBD,CarVersions AS CV,CarModels AS MO,CRM_CarDealerAssignment AS CDA,CRM_Leads AS CL JOIN CRM_Customers AS CC ON CC.ID=CL.CNS_CustId
					WHERE CL.ID=CBD.LeadId AND CBD.ID=CDA.CBDId AND CBD.IsProductExplained=1  AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes)) AND CC.CityId IN (select * from list_to_tbl(@Cities))
					GROUP BY  MONTH(CL.CreatedOnDatePart)
					
					--Will Get Delivered Data
					SELECT COUNT(CDD.Id) AS Cnt,MONTH(CL.CreatedOnDatePart) AS MonthOfEvent 
					FROM CRM_CarDeliveryData AS CDD ,CRM_CarBasicData AS CBD,CarVersions AS CV,CarModels AS MO,CRM_Leads AS CL  JOIN CRM_Customers AS CC ON CC.ID=CL.CNS_CustId
					WHERE CDD.DeliveryStatusId=20 AND CDD.CarBasicDataId=CBD.ID AND CBD.LeadId=CL.ID AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes))   AND CC.CityId IN (select * from list_to_tbl(@Cities))
					GROUP BY  MONTH(CL.CreatedOnDatePart)
					
					END
			
			IF(@Type=2)  --For Other Cities
					BEGIN
					
					-- Will get lost data
					SELECT COUNT(DISTINCT CL.ID) AS Cnt,MONTH(CL.CreatedOnDatePart)  AS MonthOfEvent,CDA.Status AS Status , CL.LeadStageId ,CBB.BookingStatusId AS Bstatus
					FROM CRM_CarBasicData AS CBD,CarVersions AS CV,CarModels AS MO,CRM_CarDealerAssignment AS CDA,CRM_CarBookingData AS CBB,CRM_Leads AS CL JOIN CRM_Customers AS CC ON CC.ID=CL.CNS_CustId
					WHERE CBD.Id= CBB.CarBasicDataId AND CL.ID=CBD.LeadId AND CBD.ID=CDA.CBDId AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes)) AND CC.CityId NOT IN (select * from list_to_tbl(@Cities))
					GROUP BY  MONTH(CL.CreatedOnDatePart),CDA.Status,CL.LeadStageId, CBB.BookingStatusId
				
					--Will get Price Quote Data
					SELECT COUNT(PQ.Id) AS PqReq,ISNULL(PQ.IsPQCompleted,0) AS PqComp,ISNULL(PQ.IsPQNotRequired,0) AS PQNotReq,MONTH(CL.CreatedOnDatePart)AS MonthOfEvent
					FROM CRM_CarPQLog As PQ,CRM_CarBasicData AS CBD,CarVersions AS CV,CarModels AS MO,CRM_Leads AS CL JOIN CRM_Customers AS CC ON CC.ID=CL.CNS_CustId
					WHERE PQ.CBDId=CBD.ID AND CBD.LeadId=CL.ID AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes)) AND CC.CityId NOT IN (select * from list_to_tbl(@Cities))
					GROUP BY  MONTH(CL.CreatedOnDatePart),PQ.IsPQCompleted,PQ.IsPQNotRequired
					
					--Will get Test Drive Data
					SELECT COUNT(TD.Id) AS tdReq,ISNULL(TD.IsTDCompleted,0) AS tdComp,ISNULL(TD.ISTDNotPossible,0) AS tdNotPos,MONTH(CL.CreatedOnDatePart)AS MonthOfEvent
					FROM CRM_CarTDLog AS TD,CRM_CarBasicData AS CBD,CarVersions AS CV,CarModels AS MO,CRM_Leads AS CL JOIN CRM_Customers AS CC ON CC.ID=CL.CNS_CustId
					WHERE TD.CBDId=CBD.ID AND CBD.LeadId=CL.ID  AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes)) AND CC.CityId NOT IN (select * from list_to_tbl(@Cities)) 
					GROUP BY  MONTH(CL.CreatedOnDatePart),TD.IsTDCompleted,TD.ISTDNotPossible
			        
					--Will get Booking Data
					SELECT COUNT(CBL.Id) AS bqReq,ISNULL(CBL.IsBookingCompleted,0) AS bqComp ,ISNULL(CBL.IsBookingNotPossible,0) AS bqNotPos,ISNULL(CBL.IsPriorBooking,0) AS bqPrior,MONTH(CL.CreatedOnDatePart) AS MonthOfEvent
					FROM CRM_CarBookingLog As CBL,CRM_CarBasicData AS CBD,CarVersions AS CV,CarModels AS MO,CRM_Leads AS CL JOIN CRM_Customers AS CC ON CC.ID=CL.CNS_CustId
					WHERE CBL.CBDId=CBD.ID AND CBD.LeadId=CL.ID  AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes)) AND CC.CityId NOT IN (select * from list_to_tbl(@Cities)) 
					GROUP BY  MONTH(CL.CreatedOnDatePart),CBL.IsBookingCompleted,CBL.IsBookingNotPossible,CBL.IsPriorBooking
					
					--Will get Product Data
					SELECT COUNT(CBD.Id) AS Cnt,MONTH(CL.CreatedOnDatePart) AS MonthOfEvent
					FROM CRM_CarBasicData AS CBD,CarVersions AS CV,CarModels AS MO,CRM_CarDealerAssignment AS CDA,CRM_Leads AS CL JOIN CRM_Customers AS CC ON CC.ID=CL.CNS_CustId
					WHERE CL.ID=CBD.LeadId AND CBD.ID=CDA.CBDId AND CBD.IsProductExplained=1  AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes))AND CC.CityId NOT IN (select * from list_to_tbl(@Cities)) 
					GROUP BY  MONTH(CL.CreatedOnDatePart)
					
					--Will Get Delivered Data
					SELECT COUNT(CDD.Id) AS Cnt,MONTH(CL.CreatedOnDatePart) AS MonthOfEvent 
					FROM CRM_CarDeliveryData AS CDD ,CRM_CarBasicData AS CBD,CarVersions AS CV,CarModels AS MO,CRM_Leads AS CL  JOIN CRM_Customers AS CC ON CC.ID=CL.CNS_CustId
					WHERE CDD.DeliveryStatusId=20 AND CDD.CarBasicDataId=CBD.ID AND CBD.LeadId=CL.ID AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId IN (select * from list_to_tbl(@Makes))   AND CC.CityId NOT IN (select * from list_to_tbl(@Cities))
					GROUP BY  MONTH(CL.CreatedOnDatePart)
					
					
					
					END

		END
	
END

