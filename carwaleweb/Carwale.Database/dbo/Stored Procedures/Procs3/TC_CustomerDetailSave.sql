IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CustomerDetailSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CustomerDetailSave]
GO

	
-- Modified By : Surendra, on 15 Feb 2013 ,Desc- Now checking input emailid null before updating it
-- Created By:	Surendra
-- Create date: 4th Jan 2013
-- Description:	Adding/Updating Customer
-- Modified by : Tejashree Patil on 10 May 2013, isFake=1 and constraint for Name updation when source is MobileMasking
-- Modified by : Tejashree Patil on 3 Jun 2013, isFake=1 and constraint for Name updation when source is MobileMasking and constaint for source 60.
-- Modified By : Tejashree Patil on 27 Aug 2013, Added parameter salutation, lastName of customer while adding inquiry.
-- Modified By : Manish on 02-09-2013 for implementing alphnumeric Unique customer id in TC_CustomerDetails table.
-- Modified By : Tejashree Patil on 11 Oct 2013, Added parameter @TC_CampaignSchedulingId while adding inquiry.
-- Modified By : Vishal Srivastava on 28 Mar 2014 , added an option for ALtername mobile number
-- Modified By : Manish Chourasiya on 15-04-2014 adding with (nolock) keyword wherever not found.
-- Modified By : Mihir A Chheda [01-08-2016] update customer name 
-- =============================================
CREATE PROCEDURE [dbo].[TC_CustomerDetailSave] @BranchId BIGINT
	,@CustomerEmail VARCHAR(100)
	,@CustomerName VARCHAR(100)
	,@CustomerMobile VARCHAR(15)
	,@Location VARCHAR(50) = NULL
	,@Buytime VARCHAR(20)
	,@Comments VARCHAR(500)
	,@CreatedBy BIGINT
	,@Address VARCHAR(150) = NULL
	,@SourceId SMALLINT
	,@CustomerId BIGINT OUT
	,@Status SMALLINT OUT
	,@ActiveLeadId BIGINT OUT
	,@CW_CustomerId BIGINT = NULL
	,@Salutation VARCHAR(15) = NULL
	,@LastName VARCHAR(100) = NULL
	,@TC_CampaignSchedulingId INT = NULL
	,-- Modified By : Tejashree Patil on 11 Oct 2013, Added parameter @TC_CampaignSchedulingId while adding inquiry.
	@CustomerAltMob VARCHAR(10) = NULL
	,--Vishal Srivastava on 28 Mar 2014 , added an option for ALtername mobile number
	@CampaignId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	SET @Status = 0
	SET @CustomerId = NULL
	SET @ActiveLeadId = NULL

	DECLARE @OriginalSourceId INT = @SourceId
	DECLARE @IsFake BIT = 0

	SELECT @CustomerId = Id
	,@ActiveLeadId = ActiveLeadId
	,@IsFake = ISNULL(Isfake, 0)
	FROM TC_CustomerDetails WITH (NOLOCK)
	WHERE Mobile = @CustomerMobile
	AND BranchId = @BranchId
	AND IsActive = 1

	-- Modified by : Tejashree Patil on 10 May 2013
	IF (
	@OriginalSourceId = 57
	OR @OriginalSourceId = 60
	) --Come from Mobile Masking then make this isFake=0
	BEGIN
	IF (@OriginalSourceId = 57)
	BEGIN
	SET @SourceId = 6 --Mobile Masking = CarWale 
	END

	IF (@OriginalSourceId = 60)
	BEGIN
	SET @SourceId = 1
	END

	IF (@IsFake = 1)
	BEGIN
	UPDATE TC_CustomerDetails
	SET Isfake = 0
	WHERE Mobile = @CustomerMobile
	AND BranchId = @BranchId
	AND IsActive = 1

	SET @IsFake = 0
	END
	END

	--Check Customer fake
	IF (@IsFake = 1)
	BEGIN
	SET @Status = 1 -- Customer is fake

	RETURN - 1
	END

	---------------Line added by Manish on 02-09-2013 for implementing alphnumeric Unique customer id-------------
	DECLARE @UniqueCustomerId VARCHAR(12)

	SET @UniqueCustomerId = SUBSTRING(REPLACE(NEWID(), '-', ''), 1, 9)

	---------------------------------------------------------------------------------------------------------------
	IF (@CustomerId IS NULL) -- Need to register Customer
	BEGIN
	--------------While condition added by Manish on 02-09-2013 for implementing alphnumeric Unique customer id-------------
	WHILE EXISTS (
	SELECT ID
	FROM TC_CustomerDetails WITH (NOLOCK)
	WHERE UniqueCustomerId = @UniqueCustomerId
	)
	BEGIN
	INSERT INTO TC_CustomerUniqueIdFailed (
	UniqueCustomerIdFailed
	,CreatedOn
	)
	VALUES (
	@UniqueCustomerId
	,GETDATE()
	)

	SET @UniqueCustomerId = SUBSTRING(REPLACE(NEWID(), '-', ''), 1, 9)
	END

	----------------------------------------------------------------------------------------------------------------------------------------	    
	INSERT INTO TC_CustomerDetails (
	CustomerName
	,Mobile
	,AlternateNumber
	,Email
	,BranchId
	,Location
	,Buytime
	,Comments
	,CreatedBy
	,Address
	,TC_InquirySourceId
	,CW_CustomerId
	,Salutation
	,LastName
	,UniqueCustomerId
	,---Column added UniqueCustomerId by Manish on 02-09-2013                     
	TC_CampaignSchedulingId
	) -- Modified By : Tejashree Patil on 11 Oct 2013, Added parameter @TC_CampaignSchedulingId while adding inquiry.
	VALUES (
	@CustomerName
	,@CustomerMobile
	,@CustomerAltMob
	,@CustomerEmail
	,@BranchId
	,@Location
	,@Buytime
	,@Comments
	,@CreatedBy
	,@Address
	,@SourceId
	,@CW_CustomerId
	,@Salutation
	,@LastName
	,@UniqueCustomerId
	,@TC_CampaignSchedulingId
	)

	SET @customerId = SCOPE_IDENTITY()
	--Vishal Srivastava on 28 Mar 2014 , added an option for ALtername mobile number
	END
	ELSE
	BEGIN -- need to update customer
	-- Modified by : Tejashree Patil on 10 May 2013
	IF (
	@OriginalSourceId = 57
	OR @OriginalSourceId = 60
	) --Come from Mobile Masking then no need to Update Name as it is 'Unknown'
	BEGIN
	UPDATE TC_CustomerDetails
	SET Mobile = @CustomerMobile
	,AlternateNumber = @CustomerAltMob
	,Email = ISNULL(@CustomerEmail, Email)
	,Location = ISNULL(@Location, Location)
	,Buytime = @Buytime
	,Comments = @Comments
	,ModifiedBy = @CreatedBy
	,ModifiedDate = GETDATE()
	,Address = @Address
	,CW_CustomerId = @CW_CustomerId
	WHERE Id = @customerId
	AND BranchId = @BranchId
	
	--By Chetan for Denormalize Tasklist table
	UPDATE TC_TaskLists SET CustomerMobile = @CustomerMobile,CustomerEmail = ISNULL(@CustomerEmail, CustomerEmail)
	WHERE CustomerId = @customerId AND BranchId = @BranchId
	
	--Vishal Srivastava on 28 Mar 2014 , added an option for ALtername mobile number
	END
	ELSE
	--Mihir A Chheda [02-08-2016]check for cutomer name is null or empty the set null or set custname with actyal value
	DECLARE @CustName VARCHAR(50);
	IF @CustomerName IS NULL OR @CustomerName = ''
	   SET @CustName=NULL
	ELSE
	   SET @CustName=@CustomerName

	BEGIN
	UPDATE TC_CustomerDetails
	SET CustomerName=ISNULL(@CustName,CustomerName)
	,Mobile = @CustomerMobile
	,AlternateNumber = @CustomerAltMob
	,Email = ISNULL(@CustomerEmail, Email)
	,Location = ISNULL(@Location, Location)
	,Buytime = @Buytime
	,Comments = @Comments
	,ModifiedBy = @CreatedBy
	,ModifiedDate = GETDATE()
	,Address = @Address
	,CW_CustomerId = @CW_CustomerId
	,Salutation = @Salutation
	,LastName = @LastName
	WHERE Id = @customerId
	AND BranchId = @BranchId


	

	--By Chetan for Denormalize Tasklist table
	UPDATE TC_TaskLists SET CustomerMobile = @CustomerMobile,CustomerEmail = ISNULL(@CustomerEmail, CustomerEmail),CustomerName=ISNULL(@CustName,CustomerName) --Mihir A Chheda [01-08-2016] update customer name 
	WHERE CustomerId = @customerId AND BranchId = @BranchId

	--Vishal Srivastava on 28 Mar 2014 , added an option for ALtername mobile number
	END
	END
END



