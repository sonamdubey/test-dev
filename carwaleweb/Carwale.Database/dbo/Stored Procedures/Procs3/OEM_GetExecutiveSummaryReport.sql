IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OEM_GetExecutiveSummaryReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OEM_GetExecutiveSummaryReport]
GO

	        
CREATE Procedure [dbo].[OEM_GetExecutiveSummaryReport]        
@StartDate Datetime,      
@EndDate Datetime          
AS      
Begin      
          
 Declare @lmsLeadCount Int          
 Declare @dmsLeadCount Int      
 Declare @bookingCount Int      
 Declare @deliveryCount Int          
         
 if @EndDate Is Not Null        
 Begin        
  Set @lmsLeadCount = ( Select COUNT(DISTINCT LeadId) FROM           
  CRM_PrePushData WHERE Result = 'SUCCESS'           
  AND StartDate BETWEEN @StartDate AND @EndDate )               
    
  Set @dmsLeadCount = (Select COUNT(DISTINCT LeadId)   
  FROM CRM_PrePushData CPD  
  WHERE Result = 'SUCCESS' AND CPD.StartDate BETWEEN @StartDate AND @EndDate  
  AND LeadId IN(Select DISTINCT CSD.LeadId FROM CRM_SkodaDealerAssignment CSD WHERE CSD.PushStatus = 'SUCCESS'))     
        
  Set @bookingCount = ( Select COUNT(DISTINCT CCBD.CarBasicDataId)        
  FROM CRM_PrePushData AS CPD, CRM_CarBasicData AS CBD, CRM_CarBookingData AS CCBD, CarVersions CV, CarModels CMO      
  WHERE  CPD.Result = 'SUCCESS' AND CPD.LeadId = CBD.LeadId AND CBD.ID = CCBD.CarBasicDataId      
  AND CCBD.BookingStatusId IN(10,16,51,57) AND CPD.StartDate  BETWEEN @StartDate AND @EndDate     
  AND CBD.VersionId  = CV.ID AND CV.CarModelId = CMO.ID AND CMO.CarMakeId = 15  
  AND CPD.LeadId IN(Select DISTINCT CSD.LeadId FROM CRM_SkodaDealerAssignment CSD WHERE CSD.PushStatus = 'SUCCESS'))     
        
  Set @deliveryCount = ( Select COUNT(DISTINCT CCD.CarBasicDataId)        
  FROM CRM_PrePushData AS CPD, CRM_CarBasicData AS CBD, CRM_CarDeliveryData AS CCD, CarVersions CV, CarModels CMO     
  WHERE CPD.Result = 'SUCCESS' AND CPD.LeadId = CBD.LeadId AND CBD.ID = CCD.CarBasicDataId      
  AND CCD.DeliveryStatusId IN(52,38,18,19,20) AND CPD.StartDate  BETWEEN @StartDate AND @EndDate   
  AND CBD.VersionId  = CV.ID AND CV.CarModelId = CMO.ID AND CMO.CarMakeId = 15  
  AND CPD.LeadId IN(Select DISTINCT CSD.LeadId FROM CRM_SkodaDealerAssignment CSD WHERE CSD.PushStatus = 'SUCCESS'))   
        
 End        
 Else        
 Begin       
        
  Set @lmsLeadCount = ( Select COUNT(DISTINCT LeadId) FROM           
  CRM_PrePushData WHERE Result = 'SUCCESS'           
  AND CONVERT(varchar,StartDate, 103) = CONVERT(Varchar, @StartDate, 103 ) )      
    
  Set @dmsLeadCount = (Select COUNT(DISTINCT LeadId)   
  FROM CRM_PrePushData CPD  
  WHERE Result = 'SUCCESS' AND CONVERT(varchar,CPD.StartDate, 103) = CONVERT(Varchar, @StartDate, 103 )  
  AND LeadId IN(Select DISTINCT CSD.LeadId FROM CRM_SkodaDealerAssignment CSD WHERE CSD.PushStatus = 'SUCCESS')  )    
        
  Set @bookingCount = ( Select COUNT(DISTINCT CCBD.CarBasicDataId)        
  FROM CRM_PrePushData AS CPD, CRM_CarBasicData AS CBD, CRM_CarBookingData AS CCBD, CarVersions CV, CarModels CMO       
  WHERE  CPD.Result = 'SUCCESS' AND CPD.LeadId = CBD.LeadId AND CBD.ID = CCBD.CarBasicDataId      
  AND CCBD.BookingStatusId IN(10,16,51,57) AND CONVERT(VarChar,CPD.StartDate, 103) = CONVERT(VarChar, @StartDate, 103 )    
  AND CBD.VersionId  = CV.ID AND CV.CarModelId = CMO.ID AND CMO.CarMakeId = 15  
  AND CPD.LeadId IN(Select DISTINCT CSD.LeadId FROM CRM_SkodaDealerAssignment CSD WHERE CSD.PushStatus = 'SUCCESS'))         
        
  Set @deliveryCount = ( Select COUNT(DISTINCT CCD.CarBasicDataId)        
  FROM CRM_PrePushData AS CPD, CRM_CarBasicData AS CBD, CRM_CarDeliveryData AS CCD, CarVersions CV, CarModels CMO     
  WHERE CPD.Result = 'SUCCESS' AND CPD.LeadId = CBD.LeadId AND CBD.ID = CCD.CarBasicDataId      
  AND CCD.DeliveryStatusId IN(52,38,18,19,20) AND CONVERT(VarChar,CPD.StartDate, 103) = CONVERT(VarChar, @StartDate, 103 )   
  AND CBD.VersionId  = CV.ID AND CV.CarModelId = CMO.ID AND CMO.CarMakeId = 15  
  AND CPD.LeadId IN(Select DISTINCT CSD.LeadId FROM CRM_SkodaDealerAssignment CSD WHERE CSD.PushStatus = 'SUCCESS'))      
         
 End        
 Select 'LMS Leads:DMS Leads:Bookings:Deliveries' As DisplayNames, (Convert(VarChar,@lmsLeadCount)+':'+Convert(VarChar,@dmsLeadCount) + ':' + CONVERT(VarChar, @bookingCount) + ':' + CONVERT(VarChar, @deliveryCount)) As LeadsCount           
End