IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WF_NCS_DealerSubscription]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WF_NCS_DealerSubscription]
GO
	CREATE PROCEDURE [dbo].[WF_NCS_DealerSubscription]
	
	@NodeId		AS NUMERIC,
	@ValueType	AS SMALLINT

 AS

	DECLARE 
		@Value      AS DECIMAL(18,2)	

BEGIN

	SELECT @Value = ISNULL(COUNT(CCBD.Id),0) 
	FROM CRM_CarBookingData AS CCBD, CRM_CarBasicData AS CBD, CRM_Leads AS CL
	WHERE CCBD.CarBasicDataId = CBD.Id
			AND CBD.LeadId = CL.Id
			AND CL.Owner IN(SELECT DISTINCT TeamId FROM CRM_ADM_QueueTeams WHERE QueueId IN(6,10,12))
			AND DAY(BookingDate) = DAY(GETDATE()-1) 
			AND MONTH(BookingDate) = MONTH(GETDATE()-1)
			AND YEAR(BookingDate) = YEAR(GETDATE()-1) 
			AND BookingStatusId = 16

	PRINT @Value
	IF @Value <> 0 AND @Value <> 0.00
		BEGIN
			INSERT INTO WF_ActualValues(NodeId, ValueDate, Value, ValueType)
			VALUES(@NodeId, GETDATE()-1, @Value, @ValueType)
		END
		
END