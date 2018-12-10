IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CH_GETScheduledCalls]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CH_GETScheduledCalls]
GO

	-- PROCEDURE TO GET SCHEDULED CALLS FOR TELECALLER

CREATE PROCEDURE [dbo].[CH_GETScheduledCalls]

	@startIndex     INT,
	@userId			INT,
	@RecentCallId	Numeric = -1,
	@callId			Numeric = null
	
	
AS
	
	
BEGIN

	SET NOCOUNT ON
		DECLARE
				@sqlStatement NVARCHAR(MAX),   
				@upperBound INT,
				@lowerBound INT,
				@pageSize INT,
				@totalCalls INT 
				
				IF @startIndex  < 1 SET @startIndex = 1
				SET @pageSize = 10
				
				SET @upperBound = @startIndex * @pageSize
				SET @lowerBound=(@startIndex-1)* @pageSize
				
				Select @totalCalls= Count (SC.CallID) From CH_ScheduledCalls AS SC, CH_CallTypes AS CT,
														 CH_TBCTypes AS TBC Where  SC.TCID = @userId AND TBC.ID = SC.TBCType 
														 AND CT.TBCType = SC.TBCType AND  CT.CallId = SC.CallType 
														 AND SC.TBCDateTime <= GETDATE()
														 AND(@callId is null or SC.TBCDateTime >(SELECT TBCDateTime FROM CH_ScheduledCalls Where CallId = @callId))
														
														 
				SELECT @totalCalls AS totalCalls		
														 
				SELECT SC.CallID, SC.CallType, CT.Name AS CallTypeName, SC.TBCType, TBC.Name AS TBCTypeName,
											  SC.TBCID, SC.TBCName, SC.TBCEmailID, SC.TBCCity, Cast(Convert(VarChar(20), SC.TBCDateTime, 113) AS DateTime)
											   AS TBCDateTime,  CallPriority, SC.TCID, Sc.EventId 
				FROM (SELECT  ROW_NUMBER() OVER(ORDER BY TBCDateTime Desc,CallPriority ) AS rowNumber,SC.CallID,SC.CallType, CT.Name AS CallTypeName, SC.TBCType, 
							TBC.Name AS TBCTypeName,  SC.TBCID, SC.TBCName, SC.TBCEmailID, SC.TBCCity, 
							Cast(Convert(VarChar(20), SC.TBCDateTime, 113) AS DateTime) AS TBCDateTime, 
							CallPriority, SC.TCID, Sc.EventId
					FROM CH_ScheduledCalls AS SC, CH_CallTypes AS CT,
														 CH_TBCTypes AS TBC Where  SC.TCID = @userId AND TBC.ID = SC.TBCType 
														 AND CT.TBCType = SC.TBCType AND  CT.CallId = SC.CallType 
														 AND SC.TBCDateTime <= GETDATE()
														 AND SC.CallID <> @RecentCallId
														 AND(@callId is null or SC.TBCDateTime >(SELECT TBCDateTime FROM CH_ScheduledCalls Where CallId = @callId)))
														  AS SC,CH_CallTypes AS CT, CH_TBCTypes AS TBC
				WHERE  SC.TCID = @userId AND TBC.ID = SC.TBCType AND CT.TBCType = SC.TBCType 
									   AND  CT.CallId = SC.CallType AND SC.TBCDateTime <= GETDATE() AND
									   rowNumber > CONVERT(varchar(9), @lowerBound)  AND
								       rowNumber <=  CONVERT(varchar(9), @upperBound)
								       ORDER BY TBCDateTime Desc,CallPriority 
								       
								     
														  	
END
