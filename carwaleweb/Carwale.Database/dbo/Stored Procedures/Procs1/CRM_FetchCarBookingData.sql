IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_FetchCarBookingData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_FetchCarBookingData]
GO

	CREATE PROCEDURE [dbo].[CRM_FetchCarBookingData]

	@CarBasicDataId			Numeric,
	@CarBookingId			Numeric OutPut,
	@LeadId					Numeric OutPut,
	@BookingStatusId		Int OutPut,
	@BookingStatus			VarChar(150) OutPut,
	@Color					VarChar(100) OutPut,
	@RegisterPersonName		VarChar(200) OutPut,
	@UpdatedById			Numeric OutPut,
	@UpdatedByName			VarChar(100) OutPut,
	@Comments				VarChar(1000) OutPut,

	@BookingRequestDate		DateTime OutPut,
	@BookingDate			DateTime OutPut,
	@CreatedOn				DateTime OutPut,
	@UpdatedOn				DateTime OutPut,
	@NIFeedback				Bit OutPut,
	@NoFeedbackContact		Bit OutPut
				
 AS
	
BEGIN

	SELECT	
		@CarBookingId		= CBA.Id,
		@LeadId				= CBD.LeadId,
		@BookingStatusId	= (CASE WHEN CB.IsPriorBooking = 1 THEN 51 WHEN CB.IsBookingNotPossible = 1 THEN 17 WHEN CB.IsBookingCompleted = 1 THEN 16 WHEN CB.IsBookingRequested = 1 THEN 10 END),
		@BookingStatus		= CASE @BookingStatusId WHEN 10 THEN 'Car Booking Requested' WHEN 16 THEN 'Car Booked' WHEN 17 THEN 'Car Booking Not Possible' WHEN 51 THEN 'Car Booking Not Possible' END,
		@Color				= CB.Color,
		@RegisterPersonName	= CB.RegisterPersonName,
		@UpdatedById		= CB.UpdatedBy,
		@UpdatedByName		= OU.UserName,
		@Comments			= CB.Comments,

		@BookingRequestDate	= CB.BookingRequestDate,
		@BookingDate		=  CB.BookingCompleteDate,
		@CreatedOn			= CB.CreatedOn,
		@UpdatedOn			= CB.UpdatedOn,
		@NIFeedback			= CB.NIFeedback,
		@NoFeedbackContact	= CB.NoFeedbackContact

	FROM CRM_CarBookingLog AS CB WITH(NOLOCK) 
			LEFT JOIN CRM_CarBookingData CBA WITH(NOLOCK) ON CB.CBDId = CBA.CarBasicDataId
			LEFT JOIN CRM_CarBasicData AS CBD WITH(NOLOCK) ON CB.CBDID = CBD.Id
			LEFT JOIN OprUsers AS OU WITH(NOLOCK) ON CB.UpdatedBy = OU.Id
			--LEFT JOIN CRM_EventTypes AS CBA WITH(NOLOCK) ON @BookingStatusId = CBA.Id)

	WHERE CB.CBDId = @CarBasicDataId
END













