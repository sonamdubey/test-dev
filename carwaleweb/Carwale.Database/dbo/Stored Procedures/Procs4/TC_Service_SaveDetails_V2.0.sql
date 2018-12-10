IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Service_SaveDetails_V2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Service_SaveDetails_V2]
GO

	
-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <29/06/2016>
-- Description:	<Save or update Service Details>
-- Modified by : Kritika Choudhary on 27th July 2016, added ServiceDueDate parameter, removed lastservicedate, ServiceDate parameters and replaced  @ServiceDate with @ServiceDueDate
-- Modified by : Kritika Choudhary on 28th July 2016, added lastservicedate
-- Modified By : Nilima More On 5th Aug 2016,update service completed date.
-- =============================================
CREATE PROCEDURE [dbo].[TC_Service_SaveDetails_V2.0]
	 @BranchId INT
	,@IsFutureInq BIT
	,@RegistrationNumber VARCHAR(50) = NULL
    ,@VersionId INT = NULL
    ,@Kms INT = NULL
    ,@CustomerId INT = NULL
    ,@ServiceType TINYINT = NULL
   -- ,@ServiceDate DATETIME = NULL
    ,@ServiceDueDate DATETIME = NULL
    ,@ModifiedBy INT
	,@CarDetails VARCHAR(250) = NULL
    ,@Comments VARCHAR(500) = NULL
	,@INQLeadId INT = NULL
	,@LeadOwnerId INT = NULL
	,@LastServiceDate DATETIME = NULL
	,@ServiceCompletedDate DATETIME = NULL
	,@ServiceRemainderId INT = NULL OUTPUT
AS
BEGIN
	
	DECLARE @RegistrationNumberSearch VARCHAR(50)
	SET @RegistrationNumberSearch = LOWER(REPLACE(@RegistrationNumber,' ' ,''))
	
	-- save data in reminder 
	IF NOT EXISTS (SELECT TC_Service_ReminderId FROM TC_Service_Reminder TSR WITH(NOLOCK) WHERE TSR.RegistrationNumberSearch = @RegistrationNumberSearch)
		BEGIN
			INSERT INTO TC_Service_Reminder(RegistrationNumber,BranchId,VersionId,Kms,CustomerId,ServiceType,ServiceDueDate, RegistrationNumberSearch,
											LeadOwnerId,Comments,CarDetails,EntryDate,ModifiedBy,LastServiceDate)
			VALUES(@RegistrationNumber,@BranchId,@VersionId,@Kms,@CustomerId,@ServiceType,@ServiceDueDate,@RegistrationNumberSearch,
											@LeadOwnerId,@Comments,@CarDetails,GETDATE(),@ModifiedBy,@LastServiceDate)
			SET @ServiceRemainderId = SCOPE_IDENTITY()
		END
	ELSE
		BEGIN
			-- if first service is done or updating service update data in reminder table
			UPDATE	TC_Service_Reminder 
			SET		ServiceDueDate = ISNULL(@ServiceDueDate,ServiceDueDate),Kms = ISNULL(@Kms,Kms),ServiceType = ISNULL(@ServiceType,ServiceType),
					LastServiceDate = ISNULL(@LastServiceDate,LastServiceDate),
					Comments = ISNULL(@Comments,Comments),ModifiedDate = GETDATE(),ModifiedBy = @ModifiedBy
					,ServiceCompletedDate = ISNULL(@ServiceCompletedDate,ServiceCompletedDate) -- Modified By : Nilima More On 5th Aug 2016,update service completed date.
			WHERE	RegistrationNumberSearch = @RegistrationNumberSearch

			SELECT @ServiceRemainderId = TC_Service_ReminderId FROM TC_Service_Reminder WITH(NOLOCK) WHERE RegistrationNumberSearch = @RegistrationNumberSearch
		END
END
