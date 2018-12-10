IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Service_SaveInquiry_V2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Service_SaveInquiry_V2]
GO

	
-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <29/06/2016>
-- Description:	<Save or update Service Details>
-- Modified by : Kritika Choudhary on 27th July 2016, added ServiceDueDate parameter, commented lastservicedate and updates ServiceDueDate
-- Modified by : Kritika Choudhary on 28th July 2016, added TC_LeadDispositionId,BookComments,IsPickUpRequested,PickUpRequestedDate and BookingStatus parameters and added them in update query
-- Modified by : Kritika Choudhary on 28th July 2016, added lastservicedate again
-- Modified by : Kritika Choudhary on 29th July 2016, added comments in case of update.
-- Modified by : Nilima More On 4th Aug 2016,Added @ServiceCompletedDate to update date when service is completed.
-- Modified By : Ashwini Dhamankar on Aug 5,2016 (added @PickUpCompletedDate).
-- Modified By : Nilima More On 9th August 2016 added DropRequestedDate.
-- Modified By : Khushaboo Patil On 9th August 2016 added ServiceDeliveredDate to update date when service is delivered.
-- Modified By : Khushaboo Patil On 10th August 2016 added DropCompletedDate to update date when drop is requested.
-- =============================================
CREATE PROCEDURE [dbo].[TC_Service_SaveInquiry_V2.0]
	 @BranchId INT
	,@RegistrationNumber VARCHAR(50) = NULL
    ,@VersionId INT = NULL
    ,@Kms INT = NULL
    ,@ServiceType TINYINT = NULL
    ,@ServiceDate DATETIME = NULL
    ,@ServiceDueDate DATETIME = NULL
	,@LastServiceDate DATETIME = NULL
    ,@ModifiedBy INT
    ,@Comments VARCHAR(500) = NULL
	,@INQLeadId INT = NULL
	,@TC_LeadDispositionId INT = NULL
	,@BookComments VARCHAR(500) = NULL
	,@IsPickUpRequested BIT = NULL
	,@PickUpRequestedDate DATETIME = NULL
	,@BookingStatus INT = NULL
	,@ServiceCompletedDate  DATETIME = NULL
	,@ServiceInquiryId INT = NULL OUTPUT
	,@PickUpCompletedDate DATETIME = NULL
	,@DropRequestedDate DATETIME = NULL
	,@ServiceDeliveredDate DATETIME = NULL
	,@DropCompletedDate DATETIME = NULL
AS
BEGIN
	IF @ServiceInquiryId > 0
		BEGIN
			UPDATE TC_Service_Inquiries 
			SET LastServiceDate = ISNULL(@LastServiceDate,LastServiceDate), 
				Kms = ISNULL(@Kms,Kms),
				ServiceDate = ISNULL(@ServiceDate ,ServiceDate),
				ServiceDueDate = ISNULL(@ServiceDueDate, ServiceDueDate) ,-- Added by : Kritika Choudhary on 27th July 2016, added ServiceDueDate 
			   --Added by : Kritika Choudhary on 28th July 2016
			    TC_LeadDispositionId = ISNULL(@TC_LeadDispositionId,TC_LeadDispositionId),
				BookComments = ISNULL(@BookComments,BookComments),
			    IsPickUpRequested = ISNULL(@IsPickUpRequested ,IsPickUpRequested),
			    PickUpRequestedDate = ISNULL(@PickUpRequestedDate,PickUpRequestedDate),
				BookingStatus = ISNULL(@BookingStatus,BookingStatus),
				ModifiedBy = ISNULL(@ModifiedBy,ModifiedBy),
				ModifiedDate = GETDATE(),
				Comments = ISNULL(@Comments,Comments) ,
				ServiceCompletedDate = ISNULL(@ServiceCompletedDate,ServiceCompletedDate),
				PickUpCompletedDate = ISNULL(@PickUpCompletedDate,PickUpCompletedDate),
				DropRequestedDate =  ISNULL(@DropRequestedDate,DropRequestedDate) ,--Added By: Nilima More On 9th August 2016 added DropRequestedDate
				ServiceDeliveredDate = ISNULL(@ServiceDeliveredDate,ServiceDeliveredDate), --Added By: Khushaboo Patil On 9th August 2016 added ServiceDeliveredDate
				DropCompletedDate = ISNULL(@DropCompletedDate,DropCompletedDate) --Added By: Khushaboo Patil On 10th August 2016 added DropCompletedDate
			WHERE TC_Service_InquiriesId = @ServiceInquiryId 
		
		END
		ELSE
		BEGIN
			-- SAVE IN TC_Service_Inquiries
			INSERT INTO TC_Service_Inquiries(TC_InquiriesLeadId,RegistrationNumber,VersionId,Kms,ServiceType,ServiceDate
								,EntryDate,ModifiedBy,Comments, ServiceDueDate,LastServiceDate)
			VALUES(@INQLeadId,@RegistrationNumber,@VersionId,@Kms,@ServiceType,@ServiceDate,
							GETDATE(),@ModifiedBy,@Comments, @ServiceDueDate,@LastServiceDate)
			
			SET @ServiceInquiryId = SCOPE_IDENTITY();
		END
END
--------------------------------------
