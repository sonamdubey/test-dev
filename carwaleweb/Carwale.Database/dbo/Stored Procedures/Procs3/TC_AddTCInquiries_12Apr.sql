IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AddTCInquiries_12Apr]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AddTCInquiries_12Apr]
GO

	
-- Created By:	Surendra
-- Create date: 6 Jan 2012
-- Description: This procedure is called from many places to add all type inquiry	
-- =============================================
CREATE PROCEDURE [dbo].[TC_AddTCInquiries_12Apr]
(
-- TC_CustomerDetails's related param
@CustomerName VARCHAR(100),
@Email VARCHAR(100),
@Mobile VARCHAR(15),
@Location VARCHAR(50),
@Buytime VARCHAR(20),
@CustomerComments VARCHAR(400),
------------------
@BranchId BIGINT,
@VersionId INT,
@Comments VARCHAR(500),
@InquiryStatus SMALLINT,
@NextFollowup DATETIME,
@AssignedTo BIGINT,
@InquiryType SMALLINT,
@InquirySource SMALLINT,
----- Other Param
@UserId BIGINT,
@TC_InquiriesId BIGINT OUTPUT
)
AS
BEGIN

    DECLARE  @CarName varchar(110),@MakeId Int,@ModelId Int
	SET @TC_InquiriesId=NULL 
	BEGIN TRY
			BEGIN TRANSACTION TranB
				
				-- checking customer exsitance and getting customer id
				DECLARE @CustomerId BIGINT
				EXEC TC_Customer @BranchId,@Email,@CustomerName,@mobile,@Location,@Buytime,@CustomerComments,@UserId,@CustomerId OUTPUT
				
				-- Checking record for same customer and version and type
				SELECT @TC_InquiriesId=TC_InquiriesId FROM TC_Inquiries WHERE TC_CustomerId=@CustomerId AND VersionId=@VersionId AND IsActive=1 AND InquiryType=@InquiryType
				--IF NOT EXISTS(SELECT TC_CustomerId FROM TC_Inquiries WHERE TC_CustomerId=@CustomerId AND VersionId=@VersionId AND IsActive=1 AND InquiryType=@InquiryType)
				IF(@TC_InquiriesId IS NULL)
				BEGIN
				
				     select  @MakeId = MakeId,
				             @ModelId =ModelId,
				             @CarName = Make+' '+Model+' '+Version
				     from vwMMV
				     where VersionId=@VersionId
				     
					INSERT INTO TC_Inquiries(BranchId,TC_CustomerId,MakeId,ModelId,VersionId,CarName,SourceId,InquiryType,CreatedBy,CreatedDate)
										VALUES(@BranchId,@CustomerId,@MakeId,@ModelId,@VersionId,@CarName,@InquirySource,@InquiryType,@UserId,GETDATE())
					SET @TC_InquiriesId=SCOPE_IDENTITY()	
										
					-- Finnally inserting or updating record in TC_InquiriesLead table				
					DECLARE @InquiryCount SMALLINT,@TC_InquiriesLeadId BIGINT	
					SELECT @InquiryCount=InquiryCount FROM TC_InquiriesLead 
					WHERE TC_CustomerId=@CustomerId AND TC_InquiryTypeId=@InquiryType
					

					IF(@InquiryCount IS NULL) -- New inquiry for same customer and Inquiry type
						BEGIN
							
							-- inserting record in TC_InquiriesLead
							INSERT INTO TC_InquiriesLead
										(BranchId,TC_CustomerId,TC_UserId,InquiryCount,NextFollowUpDate,LastFollowUpDate,
										LastFollowUpComment,TC_InquiryTypeId,TC_InquiryStatusId,TC_InquiriesFollowupActionId,CreatedBy)
							VALUES
										(@BranchId,@CustomerId,@AssignedTo,1,@NextFollowup,GETDATE(),@Comments,@InquiryType,@InquiryStatus,
										NULL,@UserId)		
							SET @TC_InquiriesLeadId=SCOPE_IDENTITY()
							
							IF(@AssignedTo IS NOT NULL AND @Comments IS NOT NULL)
							BEGIN
								INSERT INTO TC_InquiriesFollowup
											(TC_InquiriesLeadId,TC_UserId,FollowUpDate,Comment,TC_InquiryStatusId,
											TC_InquiriesFollowupActionId)
								VALUES		(@TC_InquiriesLeadId,@AssignedTo,@NextFollowup,@Comments,@InquiryStatus,NULL)
							END
						END	
					ELSE -- Some inquiries are already there for same customer and Inquiry Type
						BEGIN
					
						-- checking this lead is already assigned or not, otherwise no need to re-assign it
						DECLARE @ExistingAssignedTo BIGINT
						SELECT @ExistingAssignedTo=TC_UserId FROM TC_InquiriesLead 
						WHERE TC_CustomerId=@CustomerId AND TC_InquiryTypeId=@InquiryType
						
						IF (@ExistingAssignedTo IS NULL AND @AssignedTo IS NOT NULL) 
							BEGIN
								UPDATE TC_InquiriesLead SET InquiryCount=InquiryCount+1,TC_UserId=@AssignedTo
								WHERE TC_CustomerId=@CustomerId AND TC_InquiryTypeId=@InquiryType
							END
						ELSE
							BEGIN
								UPDATE TC_InquiriesLead SET InquiryCount=InquiryCount+1
								WHERE TC_CustomerId=@CustomerId AND TC_InquiryTypeId=@InquiryType
							END
					END	
				END
			COMMIT TRANSACTION TranB
		END TRY
		
		BEGIN CATCH
			ROLLBACK TRAN
			--SELECT ERROR_NUMBER() AS ErrorNumber;
		END CATCH;
END

           

