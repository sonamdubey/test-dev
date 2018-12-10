IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AddTCInquiriesJob]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AddTCInquiriesJob]
GO

	-- Modified By:	Surendra
-- Modified date: 2nd April,2012
-- Description: new parameter @TC_LeadTypeId added
-- =============================================
-- Created By:	Surendra
-- Create date: 6 Jan 2012
-- Description: This procedure is called from many places to add all type inquiry	
-- =============================================

CREATE PROCEDURE [dbo].[TC_AddTCInquiriesJob]
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
@TC_InquiriesId BIGINT OUTPUT,
@TC_LeadTypeId TINYINT, -- new parameter,
@CreatedDate datetime
)
AS
BEGIN

    DECLARE  @CarName varchar(110),@MakeId Int,@ModelId Int,@TC_TimeDuration_Id int,@TC_CustomerSP_Id int
	SET @TC_InquiriesId=NULL 
	
	INSERT INTO TC_TimeDuration(Programme_Name,Starttime)
    VALUES('TC_AddTCInquiriesJob', GETDATE())
    
    set @TC_TimeDuration_Id= SCOPE_IDENTITY()

	BEGIN TRY
			BEGIN TRANSACTION ProcessInquiry
				
				-- checking customer exsitance and getting customer id
				DECLARE @CustomerId BIGINT	
				
                INSERT INTO TC_TimeDuration(Programme_Name,Starttime)
                VALUES('TC_Customer', GETDATE())
                
                set @TC_CustomerSP_Id= SCOPE_IDENTITY()
    
				EXEC TC_Customer @BranchId,@Email,@CustomerName,@mobile,@Location,@Buytime,@CustomerComments,@UserId,@CustomerId OUTPUT	
				
				update  TC_TimeDuration
				set Endtime=GETDATE()
				where  TC_TimeDuration_Id=@TC_CustomerSP_Id							
				
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
				     
					INSERT INTO TC_Inquiries(BranchId,TC_CustomerId,MakeId,ModelId,VersionId,CarName,SourceId,InquiryType,CreatedBy,CreatedDate,TC_LeadTypeId)
										VALUES(@BranchId,@CustomerId,@MakeId,@ModelId,@VersionId,@CarName,@InquirySource,@InquiryType,@UserId,@CreatedDate,@TC_LeadTypeId)
					SET @TC_InquiriesId=SCOPE_IDENTITY()	
										
					-- Finnally inserting or updating record in TC_InquiriesLead table				
					DECLARE @InquiryCount SMALLINT,@TC_InquiriesLeadId BIGINT	
					SELECT @InquiryCount=InquiryCount FROM TC_InquiriesLead 
					WHERE TC_CustomerId=@CustomerId AND TC_LeadTypeId=@TC_LeadTypeId
					
					DECLARE @TypeDesc VARCHAR(20)
					SELECT @TypeDesc =Abbreviation FROM TC_InquiryType WHERE TC_InquiryTypeId=@InquiryType
					

					IF(@InquiryCount IS NULL) -- New inquiry for same customer and Inquiry type
						BEGIN
							
							-- inserting record in TC_InquiriesLead
							INSERT INTO TC_InquiriesLead
										(BranchId,TC_CustomerId,TC_UserId,InquiryCount,NextFollowUpDate,LastFollowUpDate,
										LastFollowUpComment,TC_InquiryTypeId,TC_InquiryStatusId,TC_InquiriesFollowupActionId,CreatedBy,TC_LeadTypeId,InqTypeDesc)
							VALUES
										(@BranchId,@CustomerId,@AssignedTo,1,@NextFollowup,GETDATE(),@Comments,@InquiryType,@InquiryStatus,
										NULL,@UserId,@TC_LeadTypeId, @TypeDesc)		
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
								UPDATE TC_InquiriesLead SET InquiryCount=InquiryCount+1,TC_UserId=@AssignedTo,InqTypeDesc=dbo.DistinctList(InqTypeDesc + ' ' + @TypeDesc,',')
								WHERE TC_CustomerId=@CustomerId AND TC_LeadTypeId=@TC_LeadTypeId
							END
						ELSE
							BEGIN
								UPDATE TC_InquiriesLead SET InquiryCount=InquiryCount+1,InqTypeDesc=dbo.DistinctList(InqTypeDesc + ',' + @TypeDesc,',')
								WHERE TC_CustomerId=@CustomerId AND TC_LeadTypeId=@TC_LeadTypeId
							END
					END	
				END
			COMMIT TRANSACTION ProcessInquiry
		END TRY
		
		
		BEGIN CATCH
			ROLLBACK TRAN ProcessInquiry
			INSERT INTO TC_Exceptions(Programme_Name,TC_Exception,TC_Exception_Date)
         VALUES('TC_AddTCInquiries',ERROR_MESSAGE(),GETDATE())
		END CATCH;
		
		update  TC_TimeDuration
		set Endtime=GETDATE()
		where  TC_TimeDuration_Id=@TC_TimeDuration_Id
END
