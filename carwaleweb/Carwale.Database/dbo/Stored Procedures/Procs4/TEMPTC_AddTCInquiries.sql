IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TEMPTC_AddTCInquiries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TEMPTC_AddTCInquiries]
GO

	-- Created By:	Surendra
-- Create date: 6 Jan 2012
-- Description: This procedure is called from many places to add all type inquiry	
-- =============================================
CREATE PROCEDURE [dbo].[TEMPTC_AddTCInquiries]
(
@BranchId Numeric,          
@CarId BIGINT,
@VersionId INT,
@CustomerId BIGINT,
@Comments VARCHAR(500),
@InquiryStatus SMALLINT,
@NextFollowup DATETIME,
@AssignedTo BIGINT,
@InquiryType SMALLINT,
@InquirySource SMALLINT,
----- Other Param
@UserId BIGINT,
@CreatedDate DATETIME
)
AS
BEGIN
	
	-- Adding unique inquiry for same customer,versionid and Inquiry type
	IF NOT EXISTS(SELECT CustomerId FROM TC_Inquiries WHERE CustomerId=@CustomerId AND VersionId=@VersionId AND IsActive=1 AND InquiryType=@InquiryType)
	BEGIN
		INSERT INTO TC_Inquiries(BranchId,CustomerId,CarId,VersionId,SourceId,InquiryType,CreatedBy)
		VALUES(@BranchId,@CustomerId,@CarId,@VersionId,@InquirySource,@InquiryType,@UserId)	
	
		/*Since TC_InquiriesLead table can have only inquiry for same customer and Inquiry type hence we are checking 
		 InquiryCount if it null then no record is added till now. */
		DECLARE @InquiryCount SMALLINT,@TC_InquiriesLeadId BIGINT	
		
		SELECT @InquiryCount=InquiryCount 
		FROM TC_InquiriesLead 
		WHERE TC_CustomerId=@CustomerId 
		AND TC_InquiryTypeId=@InquiryType
		
		

		IF(@InquiryCount IS NULL) -- Fresh Inquiry
		BEGIN
			INSERT INTO TC_InquiriesLead
						(BranchId,TC_CustomerId,TC_UserId,InquiryCount,NextFollowUpDate,LastFollowUpDate,
						LastFollowUpComment,TC_InquiryTypeId,TC_InquiryStatusId,TC_InquiriesFollowupActionId,CreatedBy,CreatedDate)
			VALUES
						(@BranchId,@CustomerId,@AssignedTo,1,@NextFollowup,@CreatedDate,@Comments,@InquiryType,@InquiryStatus,
						NULL,@UserId,@CreatedDate)		
			SET @TC_InquiriesLeadId=SCOPE_IDENTITY()
			
			--IF(@AssignedTo IS NOT NULL AND @Comments IS NOT NULL)
			--BEGIN
			--	INSERT INTO TC_InquiriesFollowup
			--				(TC_InquiriesLeadId,TC_UserId,FollowUpDate,Comment,TC_InquiryStatusId,
			--				TC_InquiriesFollowupActionId)
			--	VALUES		(@TC_InquiriesLeadId,@AssignedTo,@NextFollowup,@Comments,@InquiryStatus,NULL)
			--END
		END	
		--ELSE -- Some inquiries are already there for same customer and Inquiry Type
		--BEGIN
		
			-- checking this lead is already assigned or not, otherwise no need to re-assign it
			--DECLARE @ExistingAssignedTo BIGINT
			--SELECT @ExistingAssignedTo=TC_UserId FROM TC_InquiriesLead 
			--WHERE TC_CustomerId=@CustomerId AND TC_InquiryTypeId=@InquiryType
			
			--IF (@ExistingAssignedTo IS NULL AND @AssignedTo IS NOT NULL) 
			--BEGIN
			--	UPDATE TC_InquiriesLead SET InquiryCount=InquiryCount+1,TC_UserId=@AssignedTo
			--	WHERE TC_CustomerId=@CustomerId AND TC_InquiryTypeId=@InquiryType
			--END
			--ELSE
			--BEGIN
				--UPDATE TC_InquiriesLead SET InquiryCount=InquiryCount+1,CreatedDate=@CreatedDate
				--WHERE TC_CustomerId=@CustomerId AND TC_InquiryTypeId=@InquiryType
			--END
		--END	
	END
END

           
