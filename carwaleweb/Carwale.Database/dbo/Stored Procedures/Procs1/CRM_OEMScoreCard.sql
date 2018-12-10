IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_OEMScoreCard]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_OEMScoreCard]
GO

	
-- =============================================
-- Author:		Vinay Kumar
-- Create date: 12 AUG 2013
-- Description:	This proc is used to get last 7 days Achieved Or Missed of OEM lead
-- =============================================
CREATE PROCEDURE [dbo].[CRM_OEMScoreCard]
AS
BEGIN 

	  DECLARE @GOAL Table( TotalAchieved NUMERIC,AchievedDay INT, TotalTarget INT)
	  DECLARE @TEMPMISSED Table( Missed NUMERIC,ScheduleDate INT)	  
	  DECLARE @TempTableDay TABLE ( Day INT )
	  
					   DECLARE @i int = 1
					   WHILE (@i <= 7)
						  BEGIN
							 INSERT INTO @TempTableDay VALUES(DAY(GETDATE()-@i))
							 SET @i = @i + 1 
						  END  												  						 						                       
	  --Goal(Achieved)	  
       INSERT INTO @GOAL
		               		                     
					SELECT COUNT(CBDId) AS TotalAchieved, DAY(CDA.CreatedOn) AS AchievedDay, ISNULL(CDT.DA_Target,0) AS TotalTarget
					FROM CRM_CarDealerAssignment CDA WITH (NOLOCK) 
					    INNER JOIN CRM_CarBasicData CBD WITH (NOLOCK)  ON CDA.CBDId = CBD.ID
					    LEFT JOIN CRM_DATarget CDT WITH (NOLOCK)  ON CONVERT(date ,CDT.Date,101)= CONVERT(date,CDA.CreatedOn,101) AND CDT.GroupType = 1
					WHERE CDA.CreatedOn BETWEEN GETDATE()-7 AND GETDATE()
						AND CBD.VersionId IN(SELECT Id FROM CarVersions WITH (NOLOCK) WHERE CarModelId IN(SELECT GMM.ModelId 
												FROM CRM_ADM_GroupModelMapping AS GMM WITH (NOLOCK) WHERE GMM.GroupType=1 ))
					GROUP BY DAY(CDA.CreatedOn), CDT.DA_Target
		               		 	  
        INSERT INTO @TEMPMISSED
					
					SELECT COUNT(CC.ID) AS Missed, DAY(CC.ScheduledOn) AS ScheduleDate 
					FROM CRM_Leads AS CL WITH (NOLOCK) 
						INNER JOIN CRM_Calls CC WITH (NOLOCK) ON CL.ID = CC.LeadId AND CC.CallType IN(1,2)
						INNER JOIN CRM_ADM_FLCGroups CAF WITH (NOLOCK) ON CAF.Id = CL.GroupId AND CAF.GroupType = 1
					WHERE   
						  DATEDIFF(dd, CC.ScheduledOn, ISNULL(CC.ActionTakenOn, GETDATE())) > 0
						  AND CC.ScheduledOn BETWEEN GETDATE()-7 AND GETDATE()-1 AND CC.CallerId <> 13
					GROUP BY DAY(CC.ScheduledOn)  
							
		-- 1st table for Last 7 Days OEM Achieved Lead AND  Missed OEM Lead 
		SELECT ISNULL(GL.TotalAchieved,0) AS TotalAchieved ,TTD.Day, ISNULL(GL.TotalTarget,0) AS TotalTarget,TM.Missed 
        FROM  @TempTableDay AS TTD 
        LEFT JOIN @TEMPMISSED AS  TM ON TM.ScheduleDate=TTD.Day 
        LEFT JOIN @GOAL AS GL ON  TTD.Day =GL.AchievedDay 
		
		 
         --2nd  table for Assign OEM Lead at current Date (today's goal)	 
        SELECT  CDT.DA_Target FROM CRM_DATarget AS CDT WITH(NOLOCK)
        WHERE CDT.GroupType=1 AND CONVERT(date ,CDT.Date,101)= CONVERT(date,GETDATE(),101)
        
        --3rd table for customer satisfaction acording to answer id 258 means 10 point(max satisfaction) and answer id 249 means 1 point (min satisfaction)
        SELECT COUNT(CGB.Id) AS Satisfaction,CGB.AnswerId  FROM CRM_Feedback AS CGB								
			   INNER JOIN CRM_CarBasicData AS CBD WITH(NOLOCK) ON CBD.Id = CGB.CBDId
		WHERE  CGB.AnswerId in(249,250,251,252,253,254,255,256,257,258) 
			   AND CBD.VersionId IN(SELECT Id FROM CarVersions WITH (NOLOCK) WHERE CarModelId IN(SELECT GMM.ModelId 
									FROM CRM_ADM_GroupModelMapping AS GMM WITH (NOLOCK) WHERE GMM.GroupType=1 )) 					
			   AND  YEAR(CGB.FBDate)= YEAR(GETDATE()) AND MONTH(CGB.FBDate)= MONTH(GETDATE()) 
		GROUP BY CGB.AnswerId	  
 END