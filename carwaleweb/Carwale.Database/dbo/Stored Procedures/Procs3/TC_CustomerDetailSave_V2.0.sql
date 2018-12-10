IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CustomerDetailSave_V2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CustomerDetailSave_V2]
GO

	
-- Owner		:	Deepak Tripathi
-- Create date	:	4th July 2016
-- Description	:	Adding/Updating Customer
-- Modified By  :	Deepak on 4th Oct 2016 - Added concept of blocked customers

-- =============================================
CREATE PROCEDURE [dbo].[TC_CustomerDetailSave_V2.0] 
	@BranchId INT
	,@CustomerEmail		VARCHAR(100) = NULL
	,@CustomerName		VARCHAR(100)= NULL
	,@CustomerMobile	VARCHAR(15)= NULL
	,@Location			VARCHAR(50) = NULL
	,@Buytime			VARCHAR(20)= NULL
	,@Comments			VARCHAR(500)= NULL
	,@CreatedBy			INT= NULL
	,@Address			VARCHAR(150) = NULL
	,@SourceId			SMALLINT= NULL
	
	,@CustomerId		INT OUT
	,@Status			SMALLINT OUT
	,@ActiveLeadId		INT OUT
	,@CW_CustomerId		INT = NULL
	,@Salutation		VARCHAR(15) = NULL
	,@LastName			VARCHAR(100) = NULL
	,@TC_CampaignSchedulingId INT = NULL
	,@CustomerAltMob	VARCHAR(10) = NULL
	,@CampaignId		INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	
	IF dbo.TC_FNIsBlockedCustomer(@CustomerMobile) <> 1
		BEGIN
	
			SET @Status = 0
			SET @CustomerId = NULL
			SET @ActiveLeadId = NULL
			
			DECLARE @IsFake BIT = 0
			DECLARE @IsMaskingLead BIT = 0
			
			IF(@SourceId = 6) 
				SET @IsMaskingLead = 1

			--SELECT @CustomerId = Id ,@ActiveLeadId = ActiveLeadId,@IsFake = ISNULL(Isfake, 0)
			--FROM TC_CustomerDetails WITH (NOLOCK)
			--WHERE Mobile = @CustomerMobile AND BranchId = @BranchId AND IsActive = 1
			----WHERE (Mobile = @CustomerMobile or Id = @CustomerId) AND BranchId = @BranchId AND IsActive = 1

			
			SELECT @CustomerId = Id ,@ActiveLeadId = L.TC_LeadId,@IsFake = ISNULL(Isfake, 0)
			FROM TC_CustomerDetails C WITH (NOLOCK)
			LEFT JOIN TC_Lead L WITH (NOLOCK) ON C.Id = L.TC_CustomerId AND ISNULL(L.TC_LeadStageId,0) <> 3
			WHERE C.Mobile = @CustomerMobile AND C.BranchId = @BranchId AND IsActive = 1
			--WHERE (Mobile = @CustomerMobile or Id = @CustomerId) AND BranchId = @BranchId AND IsActive = 1
			

			--If Customer already Exists
			IF @@ROWCOUNT > 0
				BEGIN
					IF(@IsMaskingLead = 1) 
						BEGIN
							--Came from Mobile Masking then no need to Update Name as it is 'Unknown'
							IF (@IsFake = 1)
								BEGIN
									-- If Yes Verify the lead
									UPDATE TC_CustomerDetails SET Isfake = 0 
									WHERE Mobile = @CustomerMobile	AND BranchId = @BranchId AND IsActive = 1
									SET @IsFake = 0
								END				
						END
						--Check If Customer is fake
						IF (@IsFake = 1)
							BEGIN
								SET @Status = 1 -- Customer is fake
								RETURN - 1
							END
						ELSE
							BEGIN
								UPDATE TC_CustomerDetails 
								SET CustomerName = CASE @IsMaskingLead WHEN 1 THEN CustomerName ELSE   ISNULL(@CustomerName,CustomerName)END
									,Email = CASE @IsMaskingLead WHEN 1 THEN Email ELSE   ISNULL(@CustomerEmail,Email) END
									,Mobile =  ISNULL(@CustomerMobile,Mobile)
									,AlternateNumber =  ISNULL(@CustomerAltMob,AlternateNumber)
									,Location = ISNULL(@Location, Location)
									,Buytime =  ISNULL(@Buytime,Buytime),
									Comments =  ISNULL(@Comments,Comments),
									ModifiedBy =  ISNULL(@CreatedBy,ModifiedBy),
									ModifiedDate = GETDATE()
									,Address =  ISNULL(@Address,Address),
									CW_CustomerId =  ISNULL(@CW_CustomerId,CW_CustomerId)
									,Salutation =  ISNULL(@Salutation,Salutation),
									LastName =  ISNULL(@LastName,LastName)
									WHERE Id = @customerId	AND BranchId = @BranchId
								
								--Denormalize Tasklist table update
								UPDATE TC_TaskLists 
								SET CustomerName = CASE @IsMaskingLead WHEN 1 THEN CustomerName ELSE   ISNULL(@CustomerName,CustomerName) END,
								 CustomerMobile = CASE @IsMaskingLead WHEN 1 THEN CustomerMobile ELSE  ISNULL(@CustomerMobile,CustomerMobile) END,
									CustomerEmail = CASE @IsMaskingLead WHEN 1 THEN CustomerEmail ELSE  ISNULL(@CustomerEmail,CustomerEmail)END
								WHERE CustomerId = @customerId AND BranchId = @BranchId
								
								--Log Data
								INSERT INTO TC_CustomerDetailsLog(CustomerId, CustomerName,Mobile,AlternateNumber,Email,BranchId,Location,Buytime,Comments,ModifiedBy
											,Address,CW_CustomerId,Salutation,LastName, ModifiedDate) 
								VALUES (@CustomerId, @CustomerName,@CustomerMobile,@CustomerAltMob,@CustomerEmail,@BranchId,@Location,@Buytime,@Comments,@CreatedBy
											,@Address,@CW_CustomerId,@Salutation,@LastName, GETDATE())
								
							END
				END
			ELSE
				BEGIN
					--Its a new customer
					INSERT INTO TC_CustomerDetails(CustomerName,Mobile,AlternateNumber,Email,BranchId,Location,Buytime,Comments,CreatedBy
								,Address,TC_InquirySourceId,CW_CustomerId,Salutation,LastName,TC_CampaignSchedulingId) 
					VALUES (@CustomerName,@CustomerMobile,@CustomerAltMob,@CustomerEmail,@BranchId,@Location,@Buytime,@Comments,@CreatedBy
								,@Address,@SourceId,@CW_CustomerId,@Salutation,@LastName,@TC_CampaignSchedulingId)
					
					SET @customerId = SCOPE_IDENTITY()
					
					--Log Data	
					INSERT INTO TC_CustomerDetailsLog(CustomerId, CustomerName,Mobile,AlternateNumber,Email,BranchId,Location,Buytime,Comments,CreatedBy
								,Address,TC_InquirySourceId,CW_CustomerId,Salutation,LastName) 
					VALUES (@CustomerId, @CustomerName,@CustomerMobile,@CustomerAltMob,@CustomerEmail,@BranchId,@Location,@Buytime,@Comments,@CreatedBy
								,@Address,@SourceId,@CW_CustomerId,@Salutation,@LastName)

					
				END
		END
	END

