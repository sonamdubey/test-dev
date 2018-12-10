IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InsertDeliveryDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InsertDeliveryDetails]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 26th July,2013
-- Description:	Inserts New Car Delivery Details
-- Modified By: Nilesh Utture on 29th July, 2013 Added Parameter @RegNo
-- Modified By: Nilesh Utture on 6th August, 2013 Added OUTPUT Parameter @Status to to check if ChassisNo is duplicate
-- Modified By: Vivek Gupta on 22ndAug,2013	, Added TC_DispositionLogInsert SP for capturing Car Delivery logs.
-- Modified By: Manish Chourasiya on 27-08-2013 stop using the table  'TC_NewCarDeliveryDetails' and start usint 'TC_NewCarBooking'
-- Modified By Vivek on 5-9-2013, removed chasssisno check coz its being checked in retails(invoice) entries.
-- =============================================
CREATE Procedure [dbo].[TC_InsertDeliveryDetails]
	@BranchId INT,
	@UserId INT,
	@LeadOwnerId INT,
	@InqId BIGINT,
	@InqType TINYINT,
	@PanNo VARCHAR(100),
	@ChassisNo VARCHAR(100),
	@EngionNo VARCHAR(100),
	@InsuranceCoverNo VARCHAR(100),
	@InvoiceNo VARCHAR(100),
	@RegNo VARCHAR(100),
	@CarDeliveryDate DATETIME = NULL,
	@Status TINYINT = NULL OUTPUT
	
AS
BEGIN
	
	SET @Status = 1 
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @InquiriesLeadCount INT
	DECLARE @InquiryCount INT
	DECLARE @InquiriesLeadId INT
	DECLARe @LeadId INT
	
	--IF NOT EXISTS(SELECT TOP(1) TC_NewCarDeliveryDetailsId FROM TC_NewCarDeliveryDetails WHERE ChassisNumber = @ChassisNo)
	--IF NOT EXISTS(SELECT TOP(1) TC_NewCarBookingId FROM TC_NewCarBooking WHERE ChassisNumber = @ChassisNo)
	-- Modified By Vivek gupta on 5-9-2013, commented above if condition
	BEGIN
		 SET @Status = 1 -- Set status as ChassisNo( Complete VIN No. is not duplicate)
		 
		 IF(@InqId IS NOT NULL) -- Save New car delivery details
		 BEGIN
	   
			/*INSERT INTO TC_NewCarDeliveryDetails
				  (TC_NewCarInquiriesId,PanNumber,ChassisNumber,EngineNumber,InsuranceCoverNumber,InvoiceNumber,TC_UserId, RegistrationNo,DeliveryDate)
			VALUES
				  (@InqId,@PanNo,@ChassisNo,@EngionNo,@InsuranceCoverNo,@InvoiceNo,@UserId, @RegNo,ISNULL(@CarDeliveryDate,GETDATE()))-- Modified By: Nilesh Utture on 29th July, 2013*/
		 

 -- Modified By: Manish Chourasiya on 27-08-2013 stop using the table  'TC_NewCarDeliveryDetails' and start usint 'TC_NewCarBooking'
		   UPDATE TC_NewCarBooking
		   SET PanNo=@PanNo,
		   ChassisNumber=@ChassisNo,
		   EngineNumber=@EngionNo,
		   InsuranceCoverNumber=@InsuranceCoverNo,
		   InvoiceNumber=@InvoiceNo,
		   UserIdForDelivery=@UserId,
		   RegistrationNo=@RegNo,
		   DeliveryDate=ISNULL(@CarDeliveryDate,GETDATE()),
		   DeliveryEntryDate=GETDATE()
		   WHERE TC_NewCarInquiriesId = @InqId ;
		 
		 END
 
		 UPDATE TC_NewCarInquiries 
		 SET CarDeliveryDate = ISNULL(@CarDeliveryDate,GETDATE()), CarDeliveryStatus = 77 
		 WHERE TC_NewCarInquiriesId = @InqId
	     
		 -- Get nesessary details for closing the Lead
		 SELECT @InquiriesLeadId =  TC_InquiriesLeadId FROM TC_NewCarInquiries WITH (NOLOCK) WHERE TC_NewCarInquiriesId = @InqId
		 SELECT @LeadId = TC_LeadId FROM TC_InquiriesLead WITH (NOLOCK) WHERE TC_InquiriesLeadId = @InquiriesLeadId  -- AND IsActive = 1
		 --Commented Condition IsActive=1 by Manish on 27-08-2013 since this field is not using.
				
		 SELECT @InquiryCount = COUNT(TC_NewCarInquiriesId) 
		 FROM TC_NewCarInquiries WITH (NOLOCK) 
		 WHERE (TC_LeadDispositionId IS NULL OR (TC_LeadDispositionId = 4 AND ISNULL(CarDeliveryStatus, 0) <> 77) )  AND TC_InquiriesLeadId = @InquiriesLeadId 
		 
		 IF @InquiryCount = 0 -- No Inquiries present so check for InquiriesLead count
		 BEGIN
			SELECT @InquiriesLeadCount = COUNT(IL.TC_InquiriesLeadId) FROM  TC_InquiriesLead	 AS IL	 WITH(NOLOCK) 
					  LEFT JOIN  TC_NewCarInquiries AS TCNI WITH (NOLOCK) ON      IL.TC_InquiriesLeadId = TCNI.TC_InquiriesLeadId 
					   LEFT JOIN  TC_SellerInquiries AS TCSI WITH (NOLOCK) ON	   IL.TC_InquiriesLeadId = TCSI.TC_InquiriesLeadId
					   LEFT JOIN  TC_BuyerInquiries  AS TCBI WITH (NOLOCK) ON	   IL.TC_InquiriesLeadId = TCBI.TC_InquiriesLeadId																	  
					  WHERE IL.TC_LeadId = @LeadId 
							AND IL.TC_UserId = @LeadOwnerId
							AND (TCNI.TC_LeadDispositionID IS NULL OR (TCNI.TC_LeadDispositionID = 4 AND ISNULL(TCNI.CarDeliveryStatus, 0) <> 77)) AND IL.IsActive = 1
							AND  TCBI.TC_LeadDispositionId IS NULL 
							AND  TCSI.TC_LeadDispositionID IS NULL
		 END 
				
		 IF @InquiryCount = 0 AND @InquiriesLeadCount = 0 -- No inquiries as well as inquiriesLead for the Lead so close the Lead
		 BEGIN
			 EXEC TC_INQLeadClose @InquiryId = @InqId, @InquiryType = @InqType, @LeadOwnerId = @LeadOwnerId, @UserId = @UserId, @InquiriesLeadId = @InquiriesLeadId,
								 @LeadId = @LeadId
		 END 
		 
		 IF(@Status = 1)
		 BEGIN		 
			EXEC TC_DispositionLogInsert @UserId,77,@InqId,5,@LeadId --Added By Vivek Gupta on 22ndAug,2013	 
		 END
		 
	 END	 
END







/****** Object:  StoredProcedure [dbo].[TC_CaptureInvoiceDate]    Script Date: 09/05/2013 16:12:51 ******/
SET ANSI_NULLS ON
