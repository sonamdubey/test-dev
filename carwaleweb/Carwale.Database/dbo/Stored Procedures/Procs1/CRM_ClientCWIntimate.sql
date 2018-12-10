IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ClientCWIntimate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ClientCWIntimate]
GO

	

CREATE PROCEDURE [dbo].[CRM_ClientCWIntimate]
	
	@LeadId				Numeric,
	@CBDId				Numeric,
	@EventType			VarChar(100),
	@CallType			Int
 AS
	
	DECLARE 
		@CallId Numeric,
		@CallerId AS Numeric,
		@DealerId AS Numeric,
		@NewCallId AS Numeric,
		@ScheduleDate AS DateTIme
	
BEGIN
	SET @ScheduleDate = GETDATE()
	
	SELECT @CallerId = DCId, @DealerId = CDA.DealerId 
	FROM CRM_ADM_DCDealers AS CAD WITH(NOLOCK) , CRM_CarBasicData AS CBD WITH(NOLOCK) ,
		CRM_CarDealerAssignment AS CDA WITH(NOLOCK) 
	WHERE CDA.DealerId = CAD.DealerId AND CDA.CBDId = CBD.ID AND CBD.ID = @CBDId
	
	IF @@ROWCOUNT > 0
		BEGIN
		
			SELECT TOP 1 @CallId = CallId 
			FROM CRM_CallActiveList WITH(NOLOCK) 
			WHERE LeadId = @LeadId AND CallerId = @CallerId AND CallType = @CallType AND DealerId = @DealerId
			ORDER BY CallId DESC

			IF @@ROWCOUNT <> 0
				BEGIN
					UPDATE CRM_Calls
					SET Subject = Convert(VarChar, Subject, 500) + ':' + @EventType, 
					ScheduledOn = @ScheduleDate, CallType = @CallType
					WHERE Id = @CallId
					
					UPDATE CRM_CallActiveList
					SET Subject = Convert(VarChar, Subject, 500) + ':' + @EventType, 
					ScheduledOn = @ScheduleDate, CallType = @CallType
					WHERE CallId = @CallId
				END 
			ELSE
				BEGIN
					EXEC CRM_ScheduleNewDCCall @LeadId, @CallType, 0, @CallerId, @ScheduleDate, @ScheduleDate, @EventType, @DealerId, @NewCallId
				END
		END
	ELSE
		BEGIN
			SELECT @DealerId = CDA.DealerId 
			FROM CRM_CarDealerAssignment AS CDA	WITH(NOLOCK) 
			WHERE CDA.CBDId = @CBDId
			IF @@ROWCOUNT > 0
				EXEC CRM_SaveOrphanDCLeads @CBDId,@DealerId,@CallerId, 1
		END
END

