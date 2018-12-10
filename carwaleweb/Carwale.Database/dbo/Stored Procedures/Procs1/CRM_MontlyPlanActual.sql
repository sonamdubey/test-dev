IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_MontlyPlanActual]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_MontlyPlanActual]
GO

	
-- =============================================
-- Created By : Jayant Mhatre
-- Create date: 10-08-2011
-- Description:	This procedure is used in CRM-CPA-Reports-Monthly Plan Planed Vs Actual page
-- =============================================
CREATE PROCEDURE [dbo].[CRM_MontlyPlanActual]
	
	
	@Month		Numeric,
	@Year		Numeric,
	@Make		Numeric,
	@Model		NvarChar(200),
	@IsModel	int 
	
AS

BEGIN
				IF(@IsModel = 2)
					BEGIN  -- Model Based Data
					
						 -- Leads in CRM
						SELECT COUNT(DISTINCT LD.ID) AS Cnt ,Day(LD.CreatedOnDatePart)AS Day 
						FROM CRM_Leads AS LD JOIN CRM_CarBasicData AS CB ON CB.LeadId = LD.Id JOIN CarVersions AS CV ON CV.ID = CB.VersionId JOIN CarModels AS MO ON MO.ID = CV.CarModelId
						WHERE MO.CarMakeId=@Make AND MONTH(LD.CreatedOnDatePart) =@Month AND YEAR(LD.CreatedOnDatePart) =@Year AND CV.CarModelId IN (select * from list_to_tbl(@Model))
						GROUP BY Day(LD.CreatedOnDatePart)
						
						--Verrified leads
            			SELECT count(distinct CEL.id) AS Cnt, Day(CEL.EventDatePart) AS Day
						FROM CRM_EventLogs AS CEL, CRM_Leads AS CL, CRM_CarBasicData AS CB,CarVersions AS CV, CarModels AS MO
						WHERE MONTH(CEL.EventDatePart) =@Month AND YEAR(CEL.EventDatePart) =@Year AND CEL.EventType = 2 AND CL.ID = CEL.ItemId  AND CV.ID = CB.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId=@Make AND CB.LeadId = CL.ID  AND CV.CarModelId IN (select * from list_to_tbl(@Model))
           		        GROUP BY MO.CarMakeId, Day(CEL.EventDatePart) ORDER BY Day
           		        
           		        --Assigned Leads
						SELECT COUNT(Distinct CDA.CBDId)AS Cnt,DAY(CDA.CreatedOn)AS Day
					    FROM CRM_CarDealerAssignment AS CDA,CRM_CarBasicData AS CB,CarVersions AS CV, CarModels AS MO
						WHERE CDA.CBDId=CB.ID AND  CV.ID = CB.VersionId AND MO.ID = CV.CarModelId AND  MONTH(CDA.CreatedOn) =@Month AND YEAR(CDA.CreatedOn) =@Year AND MO.CarMakeId=@Make AND CV.CarModelId IN (select * from list_to_tbl(@Model))
						GROUP BY DAY(CDA.CreatedOn)
						
						--Leads To Consultant
						SELECT COUNT(Distinct CC.LeadId) AS Cnt,DAY(CC.ScheduledOn) AS Day 
						FROM CRM_Calls AS CC JOIN CRM_CarBasicData AS CB ON CB.LeadId = CC.LeadId JOIN CarVersions AS CV ON CV.ID = CB.VersionId JOIN CarModels AS MO ON MO.ID = CV.CarModelId
						WHERE CC.CallType=3 AND CC.IsTeam=1 AND MONTH(CC.ScheduledOn)=@Month AND YEAR(CC.ScheduledOn)=@Year AND MO.CarMakeId=@Make AND CV.CarModelId IN (select * from list_to_tbl(@Model))
								AND CC.LeadId NOT IN(SELECT DISTINCT LeadId FROM CRM_CallTransferLog WHERE TransferType=2)
						GROUP BY DAY(CC.ScheduledOn)
						
						--Will get Price Quote Data
						SELECT COUNT(PQ.Id) AS PqReq,ISNULL(PQ.IsPQCompleted,0) AS PqComp,ISNULL(PQ.IsPQNotRequired,0) AS PQNotReq,DAY(CL.CreatedOnDatePart) AS Day 
						FROM CRM_CarPQLog As PQ,CRM_CarBasicData AS CBD,CRM_Leads AS CL,CarVersions AS CV,CarModels AS MO
						WHERE PQ.CBDId=CBD.ID AND CBD.LeadId=CL.ID AND MONTH(CL.CreatedOnDatePart) = @Month AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId=@Make AND CV.CarModelId IN (select * from list_to_tbl(@Model))
						GROUP BY  DAY(CL.CreatedOnDatePart),PQ.IsPQCompleted,PQ.IsPQNotRequired
						
						--Will get Test Drive Data
						SELECT COUNT(TD.Id) AS tdReq,ISNULL(TD.IsTDCompleted,0) AS tdComp,ISNULL(TD.ISTDNotPossible,0) AS tdNotPos,DAY(CL.CreatedOnDatePart) AS Day 
						FROM CRM_CarTDLog AS TD,CRM_CarBasicData AS CBD,CRM_Leads AS CL,CarVersions AS CV,CarModels AS MO
						WHERE TD.CBDId=CBD.ID AND CBD.LeadId=CL.ID AND MONTH(CL.CreatedOnDatePart) = @Month AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId=@Make AND CV.CarModelId IN (select * from list_to_tbl(@Model)) 
						GROUP BY  DAY(CL.CreatedOnDatePart),TD.IsTDCompleted,TD.ISTDNotPossible
				        
						--Will get Booking Data
						SELECT COUNT(CBL.Id) AS bqReq,ISNULL(CBL.IsBookingCompleted,0) AS bqComp ,ISNULL(CBL.IsBookingNotPossible,0) AS bqNotPos,ISNULL(CBL.IsPriorBooking,0) AS bqPrior,DAY(CL.CreatedOnDatePart) AS Day 
						FROM CRM_CarBookingLog As CBL,CRM_CarBasicData AS CBD,CRM_Leads AS CL,CarVersions AS CV,CarModels AS MO
						WHERE CBL.CBDId=CBD.ID AND CBD.LeadId=CL.ID AND MONTH(CL.CreatedOnDatePart) = @Month AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId=@Make AND CV.CarModelId IN (select * from list_to_tbl(@Model))
						GROUP BY  DAY(CL.CreatedOnDatePart),CBL.IsBookingCompleted,CBL.IsBookingNotPossible,CBL.IsPriorBooking
						
				END
				ELSE IF(@IsModel = 1)
				BEGIN -- Make Based Data
				
				 -- Leads in CRM
						SELECT COUNT(DISTINCT LD.ID) AS Cnt ,Day(LD.CreatedOnDatePart)AS Day 
						FROM CRM_Leads AS LD JOIN CRM_CarBasicData AS CB ON CB.LeadId = LD.Id JOIN CarVersions AS CV ON CV.ID = CB.VersionId JOIN CarModels AS MO ON MO.ID = CV.CarModelId
						WHERE MO.CarMakeId=@Make AND MONTH(LD.CreatedOnDatePart) =@Month AND YEAR(LD.CreatedOnDatePart) =@Year 
						GROUP BY Day(LD.CreatedOnDatePart)
						
						--Verrified leads
            			SELECT count(distinct CEL.id) AS Cnt, Day(CEL.EventDatePart) AS Day
						FROM CRM_EventLogs AS CEL, CRM_Leads AS CL, CRM_CarBasicData AS CB,CarVersions AS CV, CarModels AS MO
						WHERE MONTH(CEL.EventDatePart) =@Month AND YEAR(CEL.EventDatePart) =@Year AND CEL.EventType = 2 AND CL.ID = CEL.ItemId  AND CV.ID = CB.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId=@Make AND CB.LeadId = CL.ID
           		        GROUP BY MO.CarMakeId, Day(CEL.EventDatePart) ORDER BY Day
           		        
           		        --Assigned Leads
						SELECT COUNT(Distinct CDA.CBDId)AS Cnt,DAY(CDA.CreatedOn)AS Day
					    FROM CRM_CarDealerAssignment AS CDA,CRM_CarBasicData AS CB,CarVersions AS CV, CarModels AS MO
						WHERE CDA.CBDId=CB.ID AND  CV.ID = CB.VersionId AND MO.ID = CV.CarModelId AND  MONTH(CDA.CreatedOn) =@Month AND YEAR(CDA.CreatedOn) =@Year AND MO.CarMakeId=@Make 
						GROUP BY DAY(CDA.CreatedOn)
						
						--Leads To Consultant
						SELECT COUNT(Distinct CC.LeadId) AS Cnt,DAY(CC.ScheduledOn) AS Day 
						FROM CRM_Calls AS CC JOIN CRM_CarBasicData AS CB ON CB.LeadId = CC.LeadId JOIN CarVersions AS CV ON CV.ID = CB.VersionId JOIN CarModels AS MO ON MO.ID = CV.CarModelId
						WHERE CC.CallType=3 AND CC.IsTeam=1 AND MONTH(CC.ScheduledOn)=@Month AND YEAR(CC.ScheduledOn)=@Year AND MO.CarMakeId=@Make 
								AND CC.LeadId NOT IN(SELECT DISTINCT LeadId FROM CRM_CallTransferLog WHERE TransferType=2)
						GROUP BY DAY(CC.ScheduledOn)
						
						--Will get Price Quote Data
						SELECT COUNT(PQ.Id) AS PqReq,ISNULL(PQ.IsPQCompleted,0) AS PqComp,ISNULL(PQ.IsPQNotRequired,0) AS PQNotReq,DAY(CL.CreatedOnDatePart) AS Day 
						FROM CRM_CarPQLog As PQ,CRM_CarBasicData AS CBD,CRM_Leads AS CL,CarVersions AS CV,CarModels AS MO
						WHERE PQ.CBDId=CBD.ID AND CBD.LeadId=CL.ID AND MONTH(CL.CreatedOnDatePart) = @Month AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId=@Make 
						GROUP BY  DAY(CL.CreatedOnDatePart),PQ.IsPQCompleted,PQ.IsPQNotRequired
						
						--Will get Test Drive Data
						SELECT COUNT(TD.Id) AS tdReq,ISNULL(TD.IsTDCompleted,0) AS tdComp,ISNULL(TD.ISTDNotPossible,0) AS tdNotPos,DAY(CL.CreatedOnDatePart) AS Day 
						FROM CRM_CarTDLog AS TD,CRM_CarBasicData AS CBD,CRM_Leads AS CL,CarVersions AS CV,CarModels AS MO
						WHERE TD.CBDId=CBD.ID AND CBD.LeadId=CL.ID AND MONTH(CL.CreatedOnDatePart) = @Month AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId=@Make 
						GROUP BY  DAY(CL.CreatedOnDatePart),TD.IsTDCompleted,TD.ISTDNotPossible
				        
						--Will get Booking Data
						SELECT COUNT(CBL.Id) AS bqReq,ISNULL(CBL.IsBookingCompleted,0) AS bqComp ,ISNULL(CBL.IsBookingNotPossible,0) AS bqNotPos,ISNULL(CBL.IsPriorBooking,0) AS bqPrior,DAY(CL.CreatedOnDatePart) AS Day 
						FROM CRM_CarBookingLog As CBL,CRM_CarBasicData AS CBD,CRM_Leads AS CL,CarVersions AS CV,CarModels AS MO
						WHERE CBL.CBDId=CBD.ID AND CBD.LeadId=CL.ID AND MONTH(CL.CreatedOnDatePart) = @Month AND YEAR(CL.CreatedOnDatePart) = @Year AND CV.ID = CBD.VersionId AND MO.ID = CV.CarModelId AND MO.CarMakeId=@Make
						GROUP BY  DAY(CL.CreatedOnDatePart),CBL.IsBookingCompleted,CBL.IsBookingNotPossible,CBL.IsPriorBooking
						
				
				END
				
						
						
						
END
				
			
