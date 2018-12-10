IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveCarCustomerServiceRequests]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveCarCustomerServiceRequests]
GO

	
CREATE PROCEDURE [dbo].[CRM_SaveCarCustomerServiceRequests]
	
	@CCSRId				 Numeric,
	@CarBasicDataId		 Numeric,
	--@LeadId				 Numeric,
	--@VersionId			 Numeric,
	@TDLocation			 VarChar(500),

	@TDRequest			 Bit,
	@BookingRequest		 Bit,
	@InterestedInFinance Bit,
	
	@TDDate				 DateTime,
	@currentId			 Numeric OutPut
				
 AS
	 

BEGIN
	SET @currentId = -1
	IF @CCSRId = -1

		BEGIN
			-- Commented By - Deepak on 4th Jan 2013
			--SELECT TOP 1 @CarBasicDataId = Id FROM CRM_CarBasicData
			--WHERE LeadId = @LeadId AND VersionId = @VersionId
			--ORDER BY Id DESC
			
			--IF @@ROWCOUNT <> 0
				--BEGIN
					INSERT INTO CRM_CarCustomerServiceRequests
					(
						CarBasicDataId, TDLocation, TDRequest, BookingRequest, InterestedInFinance, TDDate 
					)
					VALUES
					(
						@CarBasicDataId, @TDLocation, @TDRequest, @BookingRequest, @InterestedInFinance, @TDDate
					)
					
					SET @currentId = Scope_Identity()
				--END
		END
	
	ELSE

		BEGIN
			UPDATE CRM_CarCustomerServiceRequests
			SET TDLocation = @TDLocation, TDRequest = @TDRequest, 
				BookingRequest = @BookingRequest, TDDate = @TDDate,
				InterestedInFinance = @InterestedInFinance
			WHERE Id = @CCSRId
			
			SET @currentId = @CCSRId

		END
END











